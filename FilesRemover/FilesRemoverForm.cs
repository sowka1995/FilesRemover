using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using CodeProject;

namespace FilesRemover
{
    public partial class FilesRemoverForm : Form
    {
        ILogger messageBoxLogger;

        private Stopwatch _stopwatch;

        private IEnumerable<FileData> allFiles;
        private IEnumerable<string> allDirectories;

        private int maxValueProgressBar;

        private int allDirectoriesCount;
        private int allFilesCount;

        private FilesRemoverModel filesRemoverModel;

        public FilesRemoverForm()
        {
            InitializeComponent();

            filesRemoverModel = new FilesRemoverModel();

            // Logi do MessageBox-a
            messageBoxLogger = new MessageBoxLogger();

            // padding okienka wyświetlającego logi
            logBox.SelectionTabs = new[] {0, 800};
        }

        #region Events

        private void sourcePathDialog_Click(object sender, EventArgs e)
        {
            filesRemoverModel.SourcePath = ShowFolderDialog(filesRemoverModel.SourcePath);
        }

        private void destinationPathDialog_Click(object sender, EventArgs e)
        {
            filesRemoverModel.DestinationPath = ShowFolderDialog(filesRemoverModel.DestinationPath);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            //czyszczenie pola z logiem operacji i aktualizacja daty granicznej
            ChangeEnableControls(false);
            logBox.ResetText();

            InitializeProgressBar();

            if (IsOptionsValid())
            {
                StartTimer();
                UseWaitCursor = true;
                Application.DoEvents();

                logBox.Font = new System.Drawing.Font("Arial", 30);
                logBox.Text = "Inicjalizacja...";

                var jobsWorker = new System.ComponentModel.BackgroundWorker();
                jobsWorker.DoWork += (ss, ee) => DoJobs();
                jobsWorker.RunWorkerCompleted += (ss, ee) => { StopTimer(); SaveLogToFile(); ChangeEnableControls(true); allFiles = null; allDirectories = null; };

                var fileInfoWorker = new System.ComponentModel.BackgroundWorker();
                fileInfoWorker.DoWork += (ss, ee) => { CollectInfoForJobs(); };
                fileInfoWorker.RunWorkerCompleted += (ss, ee) =>
                {
                    progressBar.Maximum = maxValueProgressBar;
                    UseWaitCursor = false;
                    logBox.ResetText(); logBox.Font = new System.Drawing.Font("Arial", 7);
                    jobsWorker.RunWorkerAsync();
                };

                fileInfoWorker.RunWorkerAsync();
            }
            else
                ChangeEnableControls(true);
        }

        private void changePaddingButton_Click_1(object sender, EventArgs e)
        {
            int padding = int.Parse(paddingValue.Text);
            string text = logBox.Text;

            logBox.Clear();
            logBox.SelectionTabs = new[] {0, padding};
            logBox.AppendText(text);
        }

        private void numberOfWeeks_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown numberOfWeeks = sender as NumericUpDown;

            UpdateBorderDate((int)numberOfWeeks.Value);
        }

        private void deleteCopyFilesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filesRemoverModel.CopyAndDeleteFiles = (sender as CheckBox).Checked;
            AnyJobSelected();
        }

        private void deleteEmptyDirectoriesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            filesRemoverModel.DeleteEmptyDirectories = (sender as CheckBox).Checked;
            AnyJobSelected();
        }

        private void FilesRemoverForm_Load(object sender, EventArgs e)
        {
            // Bind textboxes - source and destination path
            sourcePathTextBox.DataBindings.Add("Text", filesRemoverModel, "SourcePath", true, DataSourceUpdateMode.OnPropertyChanged);
            destinationPathTextBox.DataBindings.Add("Text", filesRemoverModel, "DestinationPath", true, DataSourceUpdateMode.OnPropertyChanged);

            // Bind checkboxes
            deleteCopyFilesCheckBox.DataBindings.Add("Checked", filesRemoverModel, "CopyAndDeleteFiles", true, DataSourceUpdateMode.OnPropertyChanged);
            deleteEmptyDirectoriesCheckBox.DataBindings.Add("Checked", filesRemoverModel, "DeleteEmptyDirectories", true, DataSourceUpdateMode.OnPropertyChanged);
            overrideFilesCheckBox.DataBindings.Add("Checked", filesRemoverModel, "OverrideFiles", true, DataSourceUpdateMode.OnPropertyChanged);

            UpdateBorderDate((int)numberOfWeeks.Value);
        }

        private void swapPathButton_Click(object sender, EventArgs e)
        {
            string tmp = filesRemoverModel.SourcePath;
            filesRemoverModel.SourcePath = filesRemoverModel.DestinationPath;
            filesRemoverModel.DestinationPath = tmp;
        }

        #endregion

        #region HelperMethods

        private void AnyJobSelected()
        {
            if (!filesRemoverModel.CopyAndDeleteFiles && !filesRemoverModel.DeleteEmptyDirectories)
                startButton.Enabled = false;
            else
                startButton.Enabled = true;
        }

        private void ChangeEnableControls(bool value)
        {
            startButton.Enabled = value;
            deleteCopyFilesCheckBox.Enabled = value;
            deleteEmptyDirectoriesCheckBox.Enabled = value;
            numberOfWeeks.Enabled = value;
            sourcePathTextBox.Enabled = value;
            destinationPathTextBox.Enabled = value;
            changePaddingButton.Enabled = value;
            paddingValue.Enabled = value;
            overrideFilesCheckBox.Enabled = value;
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
                messageBoxLogger.Log(ex.Message);
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
                messageBoxLogger.Log(ex.Message);
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

        private void DoJobs()
        {      
            if (allFiles != null)
                CopyAndRemoveUnusedFiles();

            if (allDirectories != null)
                DeleteEmptyDirectories();

            progressBar.Invoke(new MethodInvoker(delegate { progressBar.PerformStep(); }));
        }

        private void CollectInfoForJobs()
        {
            if (filesRemoverModel.CopyAndDeleteFiles)
                CollectFilesInfo(filesRemoverModel.SourcePath);
            if (filesRemoverModel.DeleteEmptyDirectories)
                CollectDirectoriesInfo(filesRemoverModel.SourcePath);
        }

        private void InitializeProgressBar()
        {
            maxValueProgressBar = 1;

            progressBar.Value = 0;
            progressBar.Step = 1;
            progressBar.Maximum = maxValueProgressBar;
            progressBar.ForeColor = System.Drawing.Color.Yellow;
        }

        private bool IsDirectoryEmpty(string path)
        {
            return !Directory.EnumerateFileSystemEntries(path).Any();
        }

        private bool IsOptionsValid()
        {
            //sprawdzanie poprawności scieżki początkowej i końcowej
            if (!Directory.Exists(filesRemoverModel.SourcePath))
            {
                messageBoxLogger.Log("Podana ścieżka początkowa nie istnieje!");    
                return false;
            }

            if (filesRemoverModel.CopyAndDeleteFiles && !Directory.Exists(filesRemoverModel.DestinationPath))
            {
                messageBoxLogger.Log("Podana ścieżka końcowa nie istnieje!");
                return false;
            }

            if (filesRemoverModel.CopyAndDeleteFiles && Path.GetFullPath(filesRemoverModel.SourcePath) == Path.GetFullPath(filesRemoverModel.DestinationPath))
            {
                messageBoxLogger.Log("Ścieżka początkowa i końcowa nie może być taka sama!");
                return false;
            }

            return true;
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
                messageBoxLogger.Log(path + "\n" + ex.Message);
            }
        }

        private string ShowFolderDialog(string currentPath)
        {
            string resultPath = currentPath;
            var folderDialog = new FolderBrowserDialog();
            
            folderDialog.Reset();
            folderDialog.SelectedPath = currentPath;
            folderDialog.ShowNewFolderButton = true;

            DialogResult dr = folderDialog.ShowDialog();
            if (dr == DialogResult.OK || dr == DialogResult.Yes)
            {
                resultPath = folderDialog.SelectedPath;
            }

            return resultPath;
        }

        private void StartTimer()
        {
            _stopwatch = new Stopwatch();

            startTimeLabel.Visible = true;
            endTimeLabel.Visible = false;
            durationLabel.Visible = false;

            _stopwatch.Start();

            startTimeLabel.Text = "Czas rozpoczęcia: " + DateTime.Now.ToString("HH:mm:ss");
            Application.DoEvents();
        }

        private void StopTimer()
        {
            _stopwatch.Stop();

            endTimeLabel.Text = "Czas ukończenia: " + DateTime.Now.ToString("HH:mm:ss");
            endTimeLabel.Visible = true;

            durationLabel.Text = "Czas trwania: " + _stopwatch.Elapsed.Seconds + " sek";
            durationLabel.Visible = true;
        }

        private void UpdateBorderDate(int numberOfWeeksBack)
        {
            int days = numberOfWeeksBack * 7;
            filesRemoverModel.BorderDate = DateTime.Now;
            filesRemoverModel.BorderDate = filesRemoverModel.BorderDate.Subtract(TimeSpan.FromDays(days));

            borderDateLabel.Text = filesRemoverModel.BorderDate.ToShortDateString();
        }

        #endregion 
    }
}