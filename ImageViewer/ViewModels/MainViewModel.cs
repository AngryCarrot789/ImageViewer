using ImageViewer.Helpers;
using ImageViewer.Image;
using ImageViewer.Utilities;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace ImageViewer.ViewModels
{
    public class MainViewModel : BaseViewModel
    {
        private ImageItemViewModel _image;
        private ImageItem _selectedImage;
        private int _selectedIndex;
        private bool _imageSelected;

        public ObservableCollection<ImageItem> ImageItems { get; set; }

        public ImageItemViewModel Image
        {
            get => _image;
            set => RaisePropertyChanged(ref _image, value);
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set => RaisePropertyChanged(ref _selectedIndex, value);
        }

        public ImageItem SelectedImage
        {
            get => _selectedImage;
            set => RaisePropertyChanged(ref _selectedImage, value, SetImage);
        }

        public bool ImageSelected
        {
            get => _imageSelected;
            set => RaisePropertyChanged(ref _imageSelected, value);
        }

        public ICommand OpenImageCommand { get; private set; }
        public ICommand OpenImagesInDirectoryCommand { get; private set; }
        public ICommand CloseSelectedImageCommand { get; private set; }
        public ICommand CloseAllImagesCommand { get; private set; }
        //public ICommand PrintImageCommand { get; private set; }

        public Action<ImageItem> AnimateFadeIn { get; set; }

        public MainViewModel()
        {
            ImageItems = new ObservableCollection<ImageItem>();
            OpenImageCommand = new Command(OpenImageFromExplorer);
            OpenImagesInDirectoryCommand = new Command(OpenImagesInFolder);
            CloseSelectedImageCommand = new Command(CloseSelectedImage);
            CloseAllImagesCommand = new Command(CloseAllImages);
        }

        public void SetImage(ImageItem item)
        {
            if (item != null && item.ViewModel != null)
            {
                Image = item.ViewModel;
                ImageSelected = true;
            }
            else
                ImageSelected = false;

        }

        #region Helpers

        public bool IsImageNull
        {
            get => Image == null || Image.Source == null;
        }

        #endregion

        #region Adding images to list

        public void AddImage(ImageItem item)
        {
            ImageItems.Add(item);
            AnimateFadeIn?.Invoke(item);
        }

        public void AddImage(ImageItemViewModel imageView)
        {
            ImageItem image = new ImageItem(imageView);
            image.Close = CloseImage;
            image.OpenInFileExplorer = OpenImageInFileExplorer;

            AddImage(image);
        }

        #endregion

        #region Closing/Removing images from list

        public void CloseImage(ImageItem item)
        {
            ImageItems.Remove(item);
        }

        public void CloseSelectedImage()
        {
            if (!IsImageNull)
                CloseImage(SelectedImage);
        }

        public void CloseAllImages()
        {
            foreach(ImageItem item in ImageItems)
            {
                CloseImage(item);
            }
        }

        #endregion

        #region Open image

        public void OpenImagesInFolder()
        {
            System.Windows.Forms.FolderBrowserDialog ofd =
                new System.Windows.Forms.FolderBrowserDialog();
            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                if (ofd.SelectedPath.IsDirectory())
                {
                    MessageBoxResult a =
                        MessageBox.Show(
                            "Open all files in this directory?",
                            "Open entire directory",
                            MessageBoxButton.YesNo);
                    if (a == MessageBoxResult.Yes)
                    {
                        try
                        {
                            foreach(string imagePath in Directory.GetFiles(ofd.SelectedPath))
                            {
                                OpenImage(imagePath);
                            }
                        }
                        catch (Exception e)
                        {
                            MessageBox.Show(
                                $"Error opening all images in a directory:  {e.Message}",
                                "Error opening a directory");
                        }
                    }
                }
            }
        }

        public void OpenImageFromExplorer()
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                Title = "Select File(s) to open",
                Multiselect = true,
                Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp|All Files|*.*"
            };
            if (ofd.ShowDialog() == true)
            {
                foreach(string imagePath in ofd.FileNames)
                {
                    OpenImage(imagePath);
                }
            }
        }

        public void OpenImage(string path)
        {
            if (File.Exists(path))
            {
                if (ImageVerification.IsValidImage(path))
                {
                    ImageItemViewModel image = new ImageItemViewModel(path);
                    AddImage(image);
                }
            }
        }

        public void OpenImageInFileExplorer(ImageItem image)
        {
            if (image != null && image.ViewModel != null)
            {
                string folderPath = image.ViewModel.FileInformation.FilePath;
                if (File.Exists(folderPath))
                {
                    ProcessStartInfo info = new ProcessStartInfo()
                    {
                        FileName = "explorer.exe",
                        Arguments = string.Format("/e, /select, \"{0}\"", folderPath)
                    };
                    Process.Start(info);
                }
            }
        }

        #endregion

        #region Transforming Images

        #endregion
    }
}
