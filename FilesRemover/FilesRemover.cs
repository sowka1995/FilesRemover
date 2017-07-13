using CodeProject;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace FilesRemover
{
    internal class FilesRemover
    {
        private FilesRemoverModel filesRemoverModel;

        private IEnumerable<FileData> allFiles;

        private int allDirectoriesCount;
        private int allFilesCount;
        private int maxValueProgressBar;
        private int deletedDirectoriesCount = 0;

        private List<string> errorsDuringDeleteDirectories = new List<string>();

        ILogger logger;
        RichTextBox logBox;
        ProgressBar progressBar;

        public FilesRemover(FilesRemoverModel filesRemoverModel, ILogger logger, RichTextBox logBox, ProgressBar progressBar)
        {
            this.filesRemoverModel = filesRemoverModel;
            this.logger = logger;
            this.logBox = logBox;
            this.progressBar = progressBar;
            allDirectoriesCount = -1;
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

            if (allDirectoriesCount != -1)
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
                allDirectoriesCount = Directory.EnumerateDirectories(path, "*", SearchOption.AllDirectories).Count();
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
                        Label: 
                        if (File.Exists(copiedFilePath))
                        {
                            copiedFilePath = copiedFilePath.Insert(copiedFilePath.LastIndexOf('.'), "_copy");
                            goto Label;
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
            logBox.Invoke(new MethodInvoker(delegate
            {
                logBox.AppendText($"Liczba wszystkich katalogów: {allDirectoriesCount} \n \n");
                logBox.AppendText($"Lista usuniętych katalogów: \n");
            }));

            DeleteDirectoriesRecursive(filesRemoverModel.SourcePath);

            logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText($"\nLiczba usuniętych katalogów: {deletedDirectoriesCount}"); }));

            if (errorsDuringDeleteDirectories.Count != 0)
                logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText("\nBłędy: \n\n" + string.Join("\n", errorsDuringDeleteDirectories.ToArray())); }));
        }

        private void DeleteDirectoriesRecursive(string startLocation)
        {
            foreach (var directory in Directory.EnumerateDirectories(startLocation))
            {
                DeleteDirectoriesRecursive(directory);
                if (IsDirectoryEmpty(directory))
                {
                    try
                    {
                        Directory.Delete(directory, false);
                        logBox.Invoke(new MethodInvoker(delegate { logBox.AppendText(directory + " \n"); }));
                        deletedDirectoriesCount++;
                    }
                    catch (Exception ex)
                    {
                        errorsDuringDeleteDirectories.Add(ex.Message);
                        return;
                    }
                }

                progressBar.Invoke(new MethodInvoker(delegate { progressBar.PerformStep(); }));
            }
        }

        private void CollectInfoForJobs()
        {
            if (filesRemoverModel.CopyAndDeleteFiles)
                CollectFilesInfo(filesRemoverModel.SourcePath);
            if (filesRemoverModel.DeleteEmptyDirectories)
                CollectDirectoriesInfo(filesRemoverModel.SourcePath); // tylko dla progres bara
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
