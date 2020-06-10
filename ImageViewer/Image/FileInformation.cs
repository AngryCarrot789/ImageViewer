using ImageViewer.Helpers;
using ImageViewer.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageViewer.Image
{
    public class FileInformation : BaseViewModel
    {
        private string _filePath;
        private string _fileName;
        private double _fileSize;

        public string FilePath
        {
            get => _filePath;
            set => RaisePropertyChanged(ref _filePath, value);
        }

        public string FileName
        {
            get => _fileName;
            set => RaisePropertyChanged(ref _fileName, value);
        }

        public double FileSize
        {
            get => _fileSize;
            set => RaisePropertyChanged(ref _fileSize, value);
        }

        public FileInformation(string path)
        {
            UpdateValues(path);
        }

        public FileInformation(string path, string fileName)
        {
            FilePath = path;
            FileName = fileName;

            if (FilePath.IsFile())
                FileSize = new FileInfo(FilePath).Length;
        }

        public void UpdateValues(string path)
        {
            FilePath = path;

            if (FilePath.IsFile())
            {
                FileName = Path.GetFileName(FilePath);
                FileSize = new FileInfo(FilePath).Length;
            }
        }
    }
}
