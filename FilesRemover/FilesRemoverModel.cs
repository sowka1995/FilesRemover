using System;
using System.ComponentModel;

namespace FilesRemover
{
    internal class FilesRemoverModel : INotifyPropertyChanged
    {
        private bool _copyAndDeleteFiles;
        public bool CopyAndDeleteFiles
        {
            get
            {
                return _copyAndDeleteFiles;
            }

            set
            {
               _copyAndDeleteFiles = value;
            }
        }

        private bool _deleteEmptyDirectories;
        public bool DeleteEmptyDirectories
        {
            get
            {
                return _deleteEmptyDirectories;
            }

            set
            {
                _deleteEmptyDirectories = value;
            }
        }

        private bool _overrideFiles;
        public bool OverrideFiles
        {
            get
            {
                return _overrideFiles;
            }

            set
            {
                if (_overrideFiles == value) return;
                _overrideFiles = value;
                OnPropertyChanged("OverrideFiles");
            }
        }

        private string _sourcePath;
        public string SourcePath
        {
            get { return _sourcePath; }
            set
            {
                if (_sourcePath == value) return;
                _sourcePath = value;
                OnPropertyChanged("SourcePath");
            }
        }

        private string _destinationPath;
        public string DestinationPath
        {
            get { return _destinationPath; }
            set
            {
                if (_destinationPath == value) return;
                _destinationPath = value;
                OnPropertyChanged("DestinationPath");
            }
        }

        private DateTime _borderDate;
        public DateTime BorderDate
        {
            get
            {
                return _borderDate;
            }
            set
            {
                _borderDate = value;
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
