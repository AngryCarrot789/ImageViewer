using ImageViewer.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageViewer.Image
{
    public class ImageItemViewModel : BaseViewModel
    {
        private FileInformation _fileInformation;
        private ImageSource _source;

        public FileInformation FileInformation
        {
            get => _fileInformation;
            set => RaisePropertyChanged(ref _fileInformation, value);
        }

        public ImageSource Source
        {
            get => _source;
            set => RaisePropertyChanged(ref _source, value);
        }

        public ImageItemViewModel(string path)
        {
            FileInformation = new FileInformation(path);
            if (File.Exists(path))
            {
                try
                {
                    Source = new BitmapImage(new Uri(path));
                }
                catch { }
            }
        }
    }
}