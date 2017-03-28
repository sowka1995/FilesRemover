using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace FilesRemover
{
    internal class FilesRemover
    {
        FilesRemoverForm form;

        public FilesRemover(FilesRemoverForm form)
        {
            this.form = form;
        }

        //private void ChangeEnableControls(bool value)
        //{
        //    form.startButton.Enabled = value;
        //    deleteCopyFilesCheckBox.Enabled = value;
        //    deleteDirectoriesCheckBox.Enabled = value;
        //    numberOfWeeks.Enabled = value;
        //    sourcePathTextBox.Enabled = value;
        //    destinationPathTextBox.Enabled = value;
        //    changePaddingButton.Enabled = value;
        //    paddingValue.Enabled = value;
        //    overrideFilesCheckBox.Enabled = value;
        //}
    }
}
