using ImageViewer.RecycleBin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageViewer.Image
{
    /// <summary>
    /// Interaction logic for ImageItem.xaml
    /// </summary>
    public partial class ImageItem : UserControl
    {
        private Point _initialMousePos;

        public ImageItemViewModel ViewModel { get => this.DataContext as ImageItemViewModel; }

        public Action<ImageItem> Close { get; set; }
        public Action<ImageItem> OpenInFileExplorer { get; set; }

        public ImageItem(ImageItemViewModel imageView)
        {
            InitializeComponent();
            DataContext = imageView;
        }
        public ImageItem()
        {
            InitializeComponent();
        }

        public void DoDragDrop()
        {

        }

        public void SetDraggingStatus(bool isDragging)
        {
            if (isDragging)
                BorderThickness = new Thickness(2);
            else
                BorderThickness = new Thickness(0);
        }

        private void gripLeftMouseButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
                _initialMousePos = e.GetPosition(null);
        }

        private void gripMouseMove(object sender, MouseEventArgs e)
        {
            if (_initialMousePos != e.GetPosition(null) && e.LeftButton == MouseButtonState.Pressed)
            {
                if (ViewModel != null)
                {
                    try
                    {
                        SetDraggingStatus(true);

                        DragDrop.DoDragDrop(this, 
                            new DataObject(
                                typeof(ImageSource), 
                                ViewModel.Source), 
                            DragDropEffects.Copy);

                        SetDraggingStatus(false);
                    }
                    catch { }
                }
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Close?.Invoke(this);
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            switch (int.Parse(((MenuItem)sender).Uid))
            {
                case 1: Close?.Invoke(this); break;
                case 2: OpenInFileExplorer?.Invoke(this); break;
                case 3: DeleteFile(); break;
            }
        }

        private void DeleteFile()
        {
            string fileName = ViewModel.FileInformation.FilePath;
            if (File.Exists(fileName))
                Task.Run(() => RecyclingBin.SilentSend(fileName));
            Close?.Invoke(this);
        }
    }
}
