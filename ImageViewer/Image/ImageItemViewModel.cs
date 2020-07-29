using ImageViewer.Helpers;
using ImageViewer.Utilities;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ImageViewer.Image
{
    public class ImageItemViewModel : BaseViewModel
    {
        private FileInformation _fileInformation;
        private ImageSource _source;
        private string _imagePath;

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

        public string ImagePath
        {
            get => _imagePath; 
            set => RaisePropertyChanged(ref _imagePath, value);
        }

        public int ImageRotation { get; set; }

        public Action<string> AddImageCallback { get; set; }

        public Action<int> SelectImageWithIndexCallback { get; set; }

        public ImageItemViewModel(string path)
        {
            FileInformation = new FileInformation(path);
            try
            {
                LoadImage(path);
            }
            catch { }
        }

        public void LoadImage(string path)
        {
            if (path.IsFile())
            {
                ImagePath = path;
                SetImage(path);
            }
            else MessageBox.Show($"Not a file: {path}");
        }

        public void SetImage(string path)
        {
            ImagePath = path;
            Task.Run(() =>
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(path);
                image.EndInit();
                image.Freeze();
                App.Current?.Dispatcher?.Invoke(() =>
                {
                    Source = image;
                    FileInformation.UpdateValues(path);
                });
            });
        }

        public void GetAdditionalImagesAsync()
        {
            Task.Run(() =>
            {

                string parent = ImagePath.GetParentFolder();
                if (parent.IsDirectory())
                {

                    IOrderedEnumerable<string> additionalFiles =
                        Directory.GetFiles(parent).OrderBy(FileHelpers.FormatFileNumberForSort);

                    foreach (string file in additionalFiles)
                    {
                        App.Current?.Dispatcher?.Invoke(() =>
                        {
                            AddImageCallback?.Invoke(file);
                        });
                    }

                    SelectImageWithIndexCallback?.Invoke(additionalFiles.ToList().IndexOf(ImagePath));

                }
            });
        }

        public void RotateImageLeft()
        {
            //if (Source != null)
            //{
            //    if (Source is BitmapImage bitmap)
            //    {
            //        ImageRotation -= 90;

            //        if (ImageRotation == 360)
            //            ImageRotation = 0;

            //        switch (ImageRotation)
            //        {
            //            case 0: bitmap.Rotation = Rotation.Rotate0; break;
            //            case 90: bitmap.Rotation = Rotation.Rotate90; break;
            //            case 180: bitmap.Rotation = Rotation.Rotate180; break;
            //            case 270: bitmap.Rotation = Rotation.Rotate270; break;
            //        }
            //    }
            //}
        }

        public void RotateImageRight()
        {
            //if (Source != null)
            //{
            //    if (Source is BitmapImage bitmap)
            //    {
            //        BitmapImage dupe = bitmap.Clone();
            //        ImageRotation += 90;

            //        if (ImageRotation == 360)
            //            ImageRotation = 0;

            //        switch (ImageRotation)
            //        {
            //            case 0: dupe.Rotation = Rotation.Rotate0; break;
            //            case 90: dupe.Rotation = Rotation.Rotate90; break;
            //            case 180: dupe.Rotation = Rotation.Rotate180; break;
            //            case 270: dupe.Rotation = Rotation.Rotate270; break;
            //        }

            //        Source = dupe.Clone();
            //    }
            //}
        }
    }
}