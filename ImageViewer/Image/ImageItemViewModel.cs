using ImageViewer.Helpers;
using ImageViewer.Utilities;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
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
        private int _curImgIndex;
        public FileInformation FileInformation
        {
            get => _fileInformation;
            set => RaisePropertyChanged(ref _fileInformation, value);
        }

        public ObservableCollection<string> AdditionalImagePaths { get; set; }

        public ImageSource Source
        {
            get => _source;
            set => RaisePropertyChanged(ref _source, value);
        }

        public int CurrentImageIndex
        {
            get => _curImgIndex;
            set => RaisePropertyChanged(ref _curImgIndex, value);
        }

        public ImageItemViewModel(string path)
        {
            FileInformation = new FileInformation(path);
            AdditionalImagePaths = new ObservableCollection<string>();
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
                SetImage(path);
                Task.Run(() =>
                {

                    string parent = path.GetParentFolder();
                    if (parent.IsDirectory())
                    {

                        IOrderedEnumerable<string> additionalFiles =
                            Directory.GetFiles(parent).OrderBy(FileHelpers.FormatFileNumberForSort);

                        foreach (string file in additionalFiles)
                        {
                            if (ImageVerification.IsValidImage(file))
                            {
                                AdditionalImagePaths.Add(file);
                            }
                        }

                        CurrentImageIndex = additionalFiles.ToList().IndexOf(path);
                        if (CurrentImageIndex == -1)
                        {
                            ClearAdditionalPaths();
                            MessageBox.Show("Failed to fetch additional images");
                        }
                    }
                });
            }
            else
                MessageBox.Show($"Not a file: {path}");
        }

        public void SetImage(string path)
        {
            Task.Run(() =>
            {
                BitmapImage image = new BitmapImage();
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.UriSource = new Uri(path);
                image.EndInit();
                image.Freeze();
                if (App.Current != null)
                {
                    App.Current.Dispatcher.Invoke(() =>
                    {
                        Source = image;
                        FileInformation.UpdateValues(path);
                    });
                }
            });
        }

        public void ClearAdditionalPaths()
        {
            AdditionalImagePaths.Clear();
        }

        public void MoveLeft()
        {
            string img = GetPreviousAdditionalPath();
            if (img.IsFile())
                SetImage(img);
        }

        public void MoveRight()
        {
            string img = GetNextAdditionalPath();
            if (img.IsFile())
                SetImage(img);
        }

        public string GetPreviousAdditionalPath()
        {
            try
            {
                if (AdditionalImagePaths.Count > 0 && CurrentImageIndex > 0)
                {
                    CurrentImageIndex--;
                    return AdditionalImagePaths[CurrentImageIndex];
                }
                return null;
            }
            catch { }
            return null;
        }

        public string GetNextAdditionalPath()
        {
            try
            {
                if (AdditionalImagePaths.Count > 0 && CurrentImageIndex <= AdditionalImagePaths.Count - 1)
                {
                    CurrentImageIndex++;
                    return AdditionalImagePaths[CurrentImageIndex];
                }
                return null;
            }
            catch { }
            return null;
        }
    }
}