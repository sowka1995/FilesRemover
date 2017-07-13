using CodeProject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace FilesRemover
{
    internal class FilesRemover
    {
        private FilesRemoverModel filesRemoverModel;
        private Form filesRemoverForm;

        private IEnumerable<FileData> allFiles;
        private IEnumerable<string> allDirectories;

        private int allDirectoriesCount;
        private int allFilesCount;
        private int maxValueProgressBar;

        ILogger logger;
        RichTextBox logBox;
        ProgressBar progressBar;

        public FilesRemover(FilesRemoverModel filesRemoverModel, ILogger logger, RichTextBox logBox, ProgressBar progressBar, Form filesRemoverForm)
        {
            this.filesRemoverModel = filesRemoverModel;
            this.filesRemoverForm = filesRemoverForm;
            this.logger = logger;
            this.logBox = logBox;
            this.progressBar = progressBar;
        }

        public void Start(Action callback)
        {
            logBox.Font = new System.Drawing.Font("Arial", 30);
            logBox.Text = "Inicjalizacja...";

            var jobsWorker = new System.ComponentModel.BackgroundWorker();
            jobsWorker.DoWork += (ss, ee) => DoJobs();
            jobsWorker.RunWorkerCompleted += (ss, ee) => { SaveLogToFile(); callback(); };

            var fileInfoWorker = new System.ComponentModel.BackgroundWorker();
            fileInfoWorker.DoWork += (ss, ee) => { CollectInfoForJobs(); };
            fileInfoWorker.RunWorkerCompleted += (ss, ee) =>
            {
                progressBar.Maximum = maxValueProgressBar;
                logBox.ResetText(); logBox.Font = new System.Drawing.Font("Arial", 7);
                jobsWorker.RunWorkerAsync();
            };

            fileInfoWorker.RunWorkerAsync();
        }

        private void DoJobs()
        {
            if (allFiles != null)
                CopyAndRemoveUnusedFiles();

            if (allDirectories != null)
                DeleteEmptyDirectories();
        }

        private void CollectFilesInfo(string path)
        {
            try
            {
                allFiles = FastDirectoryEnumerator.EnumerateFiles(path, "*", SearchOption.AllDirectories);
                allFilesCount = allFiles.Count();
                maxValueProgressBar += allFilesCount;
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
                return;
            }
        }

        private void CollectDirectoriesInfo(string path)
        {
            try
            {
                allDirectories = Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories);
                allDirectoriesCount = allDirectories.Count();
                maxValueProgressBar += allDirectoriesCount;
            }
            catch (Exception ex)
            {
                logger.Log(ex.Message);
                return;
            }
        }

        private void CopyAndRemoveUnusedFiles()
        {
            logBox.Invoke(new MethodInvoker(delegate {
                logBox.AppendText($"Liczba wszystkich plików: {allFilesCount} \n\n");
                logBox.AppendText("Pełna scieżka pliku \t Czas ostatniego dostępu do pliku \n");
            }));

            List<string> errors = new List<string>();
            int count = 0;

            foreach (FileData file in allFiles)
            {
                var lastAccessDate = file.LastAccesTime;
                if (lastAccessDate <= filesRemoverModel.BorderDate)
                {
                    string fileName = Path.GetFileName(file.Path);
                    try
                    {
                        string copiedFilePath = $@"{filesRemoverModel.DestinationPath}\{fileName}";
                        if (File.Exists(copiedFilePath))
                        {
                            copiedFilePath = copiedFilePath.Insert(copiedFilePath.LastIndexOf('.'), "_copy");
                        }
                        File.Copy(file.Path, copiedFilePath, filesRemoverModel.OverrideFiles);
                        File.SetLastAccessTime(copiedFilePath, lastAccessDate);
                        File.Delete(file.Path);

                        logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText(file.Path + " \t" + lastAccessDate + "\n"); }));

                        count++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex.Message);
                    }
                }

                Application.DoEvents();
                progressBar.Invoke(new MethodInvoker(delegate { progressBar.PerformStep(); }));
            }

            logBox.Invoke(new MethodInvoker(delegate
            {
                logBox.Text = $"Liczba skopiowanych plików: {count}\n" + logBox.Text;
                if (errors.Count != 0)
                    logBox.AppendText("\n\n " + string.Join("\n", errors.ToArray()));
            }));
        }

        private void DeleteEmptyDirectories()
        {
            List<string> errors = new List<string>();
            int count = 0;

            logBox.Invoke(new MethodInvoker(delegate
            {
                logBox.AppendText($"Liczba wszystkich katalogów: {allDirectoriesCount} \n");
            }));

            foreach (string directory in allDirectories)
            {
                if (IsDirectoryEmpty(directory))
                {
                    try
                    {
                        Directory.Delete(directory);
                        logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText(directory + " \n"); }));
                        count++;
                    }
                    catch (Exception ex)
                    {
                        errors.Add(ex.Message);
                    }
                }
                progressBar.Invoke(new MethodInvoker(delegate { progressBar.PerformStep(); }));
            }

            logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText($"\nLiczba usuniętych katalogów: {count}"); }));

            if (errors.Count != 0)
                logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText("\nBłędy: \n\n" + string.Join("\n", errors.ToArray())); }));
        }

        private void CollectInfoForJobs()
        {
            if (filesRemoverModel.CopyAndDeleteFiles)
                CollectFilesInfo(filesRemoverModel.SourcePath);
            if (filesRemoverModel.DeleteEmptyDirectories)
                CollectDirectoriesInfo(filesRemoverModel.SourcePath);
        }

        private bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private void SaveLogToFile()
        {
            DateTime dateTime = DateTime.Now;
            string logFileName = dateTime.ToString("yy-MM-dd") + "_godz_" + dateTime.ToString("HH-mm-ss");
            string path = Directory.GetCurrentDirectory() + $@"\{logFileName}.txt";

            try
            {
                logBox.SaveFile(path, RichTextBoxStreamType.PlainText);
            }
            catch (Exception ex)
            {
                logger.Log(path + "\n" + ex.Message);
            }
        }
    }
}
