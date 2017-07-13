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

        private int maxValueProgressBar;

        private FilesRemoverModel filesRemoverModel;
        private FilesRemover filesRemover;

        public FilesRemoverForm()
        {
            InitializeComponent();

            // Logi do MessageBox-a
            messageBoxLogger = new MessageBoxLogger();

            filesRemoverModel = new FilesRemoverModel();
     
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
                filesRemover = new FilesRemover(filesRemoverModel, messageBoxLogger, logBox, progressBar, this);

                UseWaitCursor = true;
                Application.DoEvents();

                StartTimer();
                filesRemover.Start(() =>
                {
                    StopTimer();
                    UseWaitCursor = false;
                    ChangeEnableControls(true);
                });
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

        public void ChangeEnableControls(bool value)
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

        private void InitializeProgressBar()
        {
            maxValueProgressBar = 1;

            progressBar.Value = 0;
            progressBar.Step = 1;
            progressBar.Maximum = maxValueProgressBar;
            progressBar.ForeColor = System.Drawing.Color.Yellow;
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