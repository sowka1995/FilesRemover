using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace FilesRemover
{
    public partial class FilesRemoverForm : Form
    {
        ILogger logger;

        private Stopwatch _stopwatch;
        private FilesRemoverModel _filesRemoverModel;
        private FilesRemover _filesRemover;

        public FilesRemoverForm()
        {
            InitializeComponent();

            // Logi do MessageBox-a
            logger = new MessageBoxLogger();

            // Ustawiane wszystkie opcje
            _filesRemoverModel = new FilesRemoverModel();
     
            // padding okienka wyświetlającego logi
            logBox.SelectionTabs = new[] {0, 800};
        }

        private void FilesRemoverForm_Load(object sender, EventArgs e)
        {
            // Bind textboxes - source and destination path
            sourcePathTextBox.DataBindings.Add("Text", _filesRemoverModel, "SourcePath", true, DataSourceUpdateMode.OnPropertyChanged);
            destinationPathTextBox.DataBindings.Add("Text", _filesRemoverModel, "DestinationPath", true, DataSourceUpdateMode.OnPropertyChanged);

            // Bind checkbox - overrideFiles
            overrideFilesCheckBox.DataBindings.Add("Checked", _filesRemoverModel, "OverrideFiles", true, DataSourceUpdateMode.OnPropertyChanged);

            // Aktualizacja daty granicznej
            UpdateBorderDate((int)numberOfWeeks.Value);
        }

        #region Events

        private void sourcePathDialog_Click(object sender, EventArgs e)
        {
            _filesRemoverModel.SourcePath = ShowFolderDialog(_filesRemoverModel.SourcePath);
        }

        private void destinationPathDialog_Click(object sender, EventArgs e)
        {
            _filesRemoverModel.DestinationPath = ShowFolderDialog(_filesRemoverModel.DestinationPath);
        }

        private void startButton_Click(object sender, EventArgs e)
        {
            //czyszczenie pola z logiem operacji i blokowanie kontrolek
            ChangeEnableControls(false);
            logBox.ResetText();

            InitializeProgressBar();

            if (IsOptionsValid())
            {
                _filesRemover = new FilesRemover(_filesRemoverModel, logger, logBox, progressBar);

                UseWaitCursor = true;
                Application.DoEvents();

                StartTimer();
                _filesRemover.Start(() =>
                {
                    StopTimer();
                    UseWaitCursor = false;
                    ChangeEnableControls(true);
                    logger.Log("Zakończono!");
                });
            }
            else
            {
                ChangeEnableControls(true);
            }          
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
            _filesRemoverModel.CopyAndDeleteFiles = (sender as CheckBox).Checked;
            AnyJobSelected();
        }

        private void deleteEmptyDirectoriesCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            _filesRemoverModel.DeleteEmptyDirectories = (sender as CheckBox).Checked;
            AnyJobSelected();
        }
       
        private void swapPathButton_Click(object sender, EventArgs e)
        {
            string tmp = _filesRemoverModel.SourcePath;
            _filesRemoverModel.SourcePath = _filesRemoverModel.DestinationPath;
            _filesRemoverModel.DestinationPath = tmp;
        }

        private void aboutProgram_Click(object sender, EventArgs e)
        {
            Form aboutForm = AboutProgram.GetInstance();

            if (!aboutForm.Visible)
            {
                aboutForm.Show();
            }
            else
            {
                aboutForm.BringToFront();
            }
        }

        #endregion

        #region HelperMethods

        private void AnyJobSelected()
        {
            if (!_filesRemoverModel.CopyAndDeleteFiles && !_filesRemoverModel.DeleteEmptyDirectories)
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
            progressBar.Value = 0;
            progressBar.Step = 1;
            progressBar.Maximum = 1;
            progressBar.ForeColor = System.Drawing.Color.Yellow;
        }

        private bool IsOptionsValid()
        {
            //sprawdzanie poprawności scieżki początkowej i końcowej
            if (!Directory.Exists(_filesRemoverModel.SourcePath))
            {
                logger.Log("Podana ścieżka początkowa nie istnieje!");    
                return false;
            }

            if (_filesRemoverModel.CopyAndDeleteFiles && !Directory.Exists(_filesRemoverModel.DestinationPath))
            {
                logger.Log("Podana ścieżka końcowa nie istnieje!");
                return false;
            }

            if (_filesRemoverModel.CopyAndDeleteFiles && Path.GetFullPath(_filesRemoverModel.SourcePath) == Path.GetFullPath(_filesRemoverModel.DestinationPath))
            {
                logger.Log("Ścieżka początkowa i końcowa nie może być taka sama!");
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
            _filesRemoverModel.BorderDate = DateTime.Now;
            _filesRemoverModel.BorderDate = _filesRemoverModel.BorderDate.Subtract(TimeSpan.FromDays(days));

            borderDateLabel.Text = _filesRemoverModel.BorderDate.ToShortDateString();
        }

        #endregion
    }
}