namespace FilesRemover
{
    partial class FilesRemoverForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FilesRemoverForm));
            this.startButton = new System.Windows.Forms.Button();
            this.logBox = new System.Windows.Forms.RichTextBox();
            this.sourcePathTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.destinationPathTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.changePaddingButton = new System.Windows.Forms.Button();
            this.paddingValue = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numberOfWeeks = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.borderDateLabel = new System.Windows.Forms.Label();
            this.deleteDirectoriesCheckBox = new System.Windows.Forms.CheckBox();
            this.deleteCopyFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.overrideFilesCheckBox = new System.Windows.Forms.CheckBox();
            this.startTimeLabel = new System.Windows.Forms.Label();
            this.endTimeLabel = new System.Windows.Forms.Label();
            this.durationLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.paddingValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfWeeks)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // startButton
            // 
            this.startButton.Enabled = false;
            this.startButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.startButton.Location = new System.Drawing.Point(12, 24);
            this.startButton.Name = "startButton";
            this.startButton.Size = new System.Drawing.Size(75, 52);
            this.startButton.TabIndex = 0;
            this.startButton.Text = "Start";
            this.startButton.UseVisualStyleBackColor = true;
            this.startButton.Click += new System.EventHandler(this.startButton_Click);
            // 
            // logBox
            // 
            this.logBox.Location = new System.Drawing.Point(12, 159);
            this.logBox.Name = "logBox";
            this.logBox.ReadOnly = true;
            this.logBox.Size = new System.Drawing.Size(1089, 491);
            this.logBox.TabIndex = 1;
            this.logBox.Text = "";
            // 
            // sourcePathTextBox
            // 
            this.sourcePathTextBox.Location = new System.Drawing.Point(93, 27);
            this.sourcePathTextBox.Name = "sourcePathTextBox";
            this.sourcePathTextBox.Size = new System.Drawing.Size(456, 20);
            this.sourcePathTextBox.TabIndex = 2;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label1.Location = new System.Drawing.Point(90, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(132, 16);
            this.label1.TabIndex = 3;
            this.label1.Text = "ścieżka początkowa:";
            // 
            // progressBar
            // 
            this.progressBar.Location = new System.Drawing.Point(93, 57);
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(1011, 23);
            this.progressBar.TabIndex = 4;
            // 
            // destinationPathTextBox
            // 
            this.destinationPathTextBox.Location = new System.Drawing.Point(598, 27);
            this.destinationPathTextBox.Name = "destinationPathTextBox";
            this.destinationPathTextBox.Size = new System.Drawing.Size(506, 20);
            this.destinationPathTextBox.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label2.Location = new System.Drawing.Point(595, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(189, 16);
            this.label2.TabIndex = 6;
            this.label2.Text = "ścieżka do kopiowania plików:";
            // 
            // changePaddingButton
            // 
            this.changePaddingButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.changePaddingButton.Location = new System.Drawing.Point(896, 129);
            this.changePaddingButton.Name = "changePaddingButton";
            this.changePaddingButton.Size = new System.Drawing.Size(95, 25);
            this.changePaddingButton.TabIndex = 7;
            this.changePaddingButton.Text = "Zmień padding";
            this.changePaddingButton.UseVisualStyleBackColor = true;
            this.changePaddingButton.Click += new System.EventHandler(this.changePaddingButton_Click_1);
            // 
            // paddingValue
            // 
            this.paddingValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.paddingValue.Location = new System.Drawing.Point(997, 131);
            this.paddingValue.Maximum = new decimal(new int[] {
            1200,
            0,
            0,
            0});
            this.paddingValue.Minimum = new decimal(new int[] {
            400,
            0,
            0,
            0});
            this.paddingValue.Name = "paddingValue";
            this.paddingValue.Size = new System.Drawing.Size(95, 22);
            this.paddingValue.TabIndex = 8;
            this.paddingValue.Value = new decimal(new int[] {
            800,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label4.Location = new System.Drawing.Point(994, 112);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 16);
            this.label4.TabIndex = 12;
            this.label4.Text = "Padding:";
            // 
            // numberOfWeeks
            // 
            this.numberOfWeeks.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.numberOfWeeks.Location = new System.Drawing.Point(504, 86);
            this.numberOfWeeks.Maximum = new decimal(new int[] {
            9999,
            0,
            0,
            0});
            this.numberOfWeeks.Name = "numberOfWeeks";
            this.numberOfWeeks.Size = new System.Drawing.Size(73, 22);
            this.numberOfWeeks.TabIndex = 14;
            this.numberOfWeeks.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            this.numberOfWeeks.ValueChanged += new System.EventHandler(this.numberOfWeeks_ValueChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label5.Location = new System.Drawing.Point(356, 88);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(142, 16);
            this.label5.TabIndex = 15;
            this.label5.Text = "Czyść pliki starsze niż: ";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.label6.Location = new System.Drawing.Point(583, 88);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(29, 16);
            this.label6.TabIndex = 16;
            this.label6.Text = "tyg.";
            // 
            // borderDateLabel
            // 
            this.borderDateLabel.AutoSize = true;
            this.borderDateLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.borderDateLabel.Location = new System.Drawing.Point(501, 111);
            this.borderDateLabel.Name = "borderDateLabel";
            this.borderDateLabel.Size = new System.Drawing.Size(111, 16);
            this.borderDateLabel.TabIndex = 17;
            this.borderDateLabel.Text = "borderDateLabel";
            // 
            // deleteDirectoriesCheckBox
            // 
            this.deleteDirectoriesCheckBox.AutoSize = true;
            this.deleteDirectoriesCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.deleteDirectoriesCheckBox.Location = new System.Drawing.Point(160, 107);
            this.deleteDirectoriesCheckBox.Name = "deleteDirectoriesCheckBox";
            this.deleteDirectoriesCheckBox.Size = new System.Drawing.Size(142, 20);
            this.deleteDirectoriesCheckBox.TabIndex = 19;
            this.deleteDirectoriesCheckBox.Text = "usuń puste katalogi";
            this.deleteDirectoriesCheckBox.UseVisualStyleBackColor = true;
            this.deleteDirectoriesCheckBox.CheckedChanged += new System.EventHandler(this.deleteDirectoriesCheckBox_CheckedChanged);
            // 
            // deleteCopyFilesCheckBox
            // 
            this.deleteCopyFilesCheckBox.AutoSize = true;
            this.deleteCopyFilesCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.deleteCopyFilesCheckBox.Location = new System.Drawing.Point(160, 81);
            this.deleteCopyFilesCheckBox.Name = "deleteCopyFilesCheckBox";
            this.deleteCopyFilesCheckBox.Size = new System.Drawing.Size(134, 20);
            this.deleteCopyFilesCheckBox.TabIndex = 20;
            this.deleteCopyFilesCheckBox.Text = "skopiuj i usuń pliki";
            this.deleteCopyFilesCheckBox.UseVisualStyleBackColor = true;
            this.deleteCopyFilesCheckBox.CheckedChanged += new System.EventHandler(this.deleteCopyFilesCheckBox_CheckedChanged);
            // 
            // overrideFilesCheckBox
            // 
            this.overrideFilesCheckBox.AutoSize = true;
            this.overrideFilesCheckBox.Checked = true;
            this.overrideFilesCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
            this.overrideFilesCheckBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.overrideFilesCheckBox.Location = new System.Drawing.Point(160, 131);
            this.overrideFilesCheckBox.Name = "overrideFilesCheckBox";
            this.overrideFilesCheckBox.Size = new System.Drawing.Size(204, 20);
            this.overrideFilesCheckBox.TabIndex = 21;
            this.overrideFilesCheckBox.Text = "nadpisuj pliki przy kopiowaniu";
            this.overrideFilesCheckBox.UseVisualStyleBackColor = true;
            // 
            // startTimeLabel
            // 
            this.startTimeLabel.AutoSize = true;
            this.startTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.startTimeLabel.Location = new System.Drawing.Point(633, 83);
            this.startTimeLabel.Name = "startTimeLabel";
            this.startTimeLabel.Size = new System.Drawing.Size(120, 16);
            this.startTimeLabel.TabIndex = 23;
            this.startTimeLabel.Text = "Czas rozpoczęcia :";
            this.startTimeLabel.Visible = false;
            // 
            // endTimeLabel
            // 
            this.endTimeLabel.AutoSize = true;
            this.endTimeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.endTimeLabel.Location = new System.Drawing.Point(633, 105);
            this.endTimeLabel.Name = "endTimeLabel";
            this.endTimeLabel.Size = new System.Drawing.Size(115, 16);
            this.endTimeLabel.TabIndex = 24;
            this.endTimeLabel.Text = "Czas ukończenia: ";
            this.endTimeLabel.Visible = false;
            // 
            // durationLabel
            // 
            this.durationLabel.AutoSize = true;
            this.durationLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.durationLabel.Location = new System.Drawing.Point(633, 128);
            this.durationLabel.Name = "durationLabel";
            this.durationLabel.Size = new System.Drawing.Size(89, 16);
            this.durationLabel.TabIndex = 25;
            this.durationLabel.Text = "Czas trwania: ";
            this.durationLabel.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::FilesRemover.Properties.Resources.cdpLogo;
            this.pictureBox1.Location = new System.Drawing.Point(12, 95);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(142, 50);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 26;
            this.pictureBox1.TabStop = false;
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button1.Location = new System.Drawing.Point(521, 24);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(28, 24);
            this.button1.TabIndex = 27;
            this.button1.Text = "...";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(238)));
            this.button2.Location = new System.Drawing.Point(1076, 24);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(28, 24);
            this.button2.TabIndex = 28;
            this.button2.Text = "...";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(555, 16);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(37, 35);
            this.button3.TabIndex = 29;
            this.button3.Text = ">>";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // FilesRemoverForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1113, 662);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.durationLabel);
            this.Controls.Add(this.endTimeLabel);
            this.Controls.Add(this.startTimeLabel);
            this.Controls.Add(this.overrideFilesCheckBox);
            this.Controls.Add(this.deleteCopyFilesCheckBox);
            this.Controls.Add(this.deleteDirectoriesCheckBox);
            this.Controls.Add(this.borderDateLabel);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.numberOfWeeks);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.paddingValue);
            this.Controls.Add(this.changePaddingButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.destinationPathTextBox);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.sourcePathTextBox);
            this.Controls.Add(this.logBox);
            this.Controls.Add(this.startButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1129, 701);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(1129, 701);
            this.Name = "FilesRemoverForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Czyszczenie starych plików";
            ((System.ComponentModel.ISupportInitialize)(this.paddingValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numberOfWeeks)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button startButton;
        private System.Windows.Forms.RichTextBox logBox;
        private System.Windows.Forms.TextBox sourcePathTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.TextBox destinationPathTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button changePaddingButton;
        private System.Windows.Forms.NumericUpDown paddingValue;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numberOfWeeks;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label borderDateLabel;
        private System.Windows.Forms.CheckBox deleteDirectoriesCheckBox;
        private System.Windows.Forms.CheckBox deleteCopyFilesCheckBox;
        private System.Windows.Forms.CheckBox overrideFilesCheckBox;
        private System.Windows.Forms.Label startTimeLabel;
        private System.Windows.Forms.Label endTimeLabel;
        private System.Windows.Forms.Label durationLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
    }
}

