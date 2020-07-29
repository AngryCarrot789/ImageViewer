using ImageViewer.Image;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for ImageWindow.xaml
    /// </summary>
    public partial class ImageWindow : Window
    {
        public ImageItemViewModel Model
        {
            get => this.DataContext as ImageItemViewModel;
            set => this.DataContext = value;
        }

        public ImageWindow()
        {
            InitializeComponent();
            Closing += ImageWindow_Closing;
            PreviewKeyDown += ImageWindow_PreviewKeyDown;
        }

        private void ImageWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void ImageWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                UndoFullscreen();
            if (e.Key == Key.Escape)
                UndoFullscreen();
            if (e.Key == Key.F11)
                UndoFullscreen();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            imgMovable.ResetMargin();
        }

        public void SetFullscreen(ImageItemViewModel image, bool setFullOrNot)
        {
            if (image != null)
            {
                if (setFullOrNot)
                {
                    Model = image;
                    this.Show();
                    this.Visibility = Visibility.Collapsed;
                    this.Topmost = true;
                    this.WindowStyle = WindowStyle.None;
                    this.ResizeMode = ResizeMode.NoResize;
                    WindowState = WindowState.Maximized;
                    // re-show the window after changing style
                    this.Visibility = Visibility.Visible;
                }
                else
                {
                    UndoFullscreen();
                }
            }
        }

        public void UndoFullscreen()
        {
            WindowState = WindowState.Minimized;
            WindowStyle = WindowStyle.SingleBorderWindow;
            ResizeMode = ResizeMode.CanResize;
            this.Hide();
        }
    }
}
