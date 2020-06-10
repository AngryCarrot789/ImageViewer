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
            FilePath = path;

            if (File.Exists(path))
            {
                FileName = Path.GetFileName(path);
                FileSize = new FileInfo(path).Length;
            }
        }
        public FileInformation(string path, string fileName)
        {
            FilePath = path;
            FileName = fileName;

            if (File.Exists(path))
                FileSize = new FileInfo(path).Length;
        }
    }
}
