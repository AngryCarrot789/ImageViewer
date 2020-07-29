using ImageViewer.Animations;
using ImageViewer.Helpers;
using ImageViewer.Image;
using ImageViewer.Themes;
using ImageViewer.ViewModels;
using System;
using System.Security.Policy;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public AboutWindow About { get; set; } = new AboutWindow();
        public MainViewModel ViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel
            {
                AnimateFadeIn = AnimateFadeIn,
                ResetImagePosition = ResetImagePosition
            };
            DataContext = ViewModel;

            Closing += MainWindow_Closing;
        }

        private void ResetImagePosition()
        {
            imgMovable.ResetMargin();
        }

        private void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            About.ForceClose();
            About = null;
            Application.Current.Shutdown();
        }

        public MainWindow(string[] startupArgs)
        {
            InitializeComponent();
            ViewModel = new MainViewModel
            {
                AnimateFadeIn = AnimateFadeIn
            };
            DataContext = ViewModel;

            foreach(string path in startupArgs)
            {
                if (path.IsFile())
                {
                    ViewModel.OpenImage(path);
                }
            }
        }

        public void AnimateFadeIn(ImageItem item)
        {
            AnimationLibrary.OpacityControl(item, 0, 1, 0.2);
            AnimationLibrary.MoveToTargetX(item, 0, -ActualWidth, 0.2);
        }

        public void ImageDropped(DragEventArgs e)
        {
            if (ViewModel != null && e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                if (e.Data.GetData(DataFormats.FileDrop) is string[] droppedData)
                {
                    foreach (string filePath in droppedData)
                    {
                        ViewModel.OpenImage(filePath);
                    }
                }
            }
        }

        private void ListBox_Drop(object sender, DragEventArgs e)
        {
            ImageDropped(e);
        }

        private void ChangeTheme(object sender, RoutedEventArgs e)
        {
            switch (int.Parse(((FrameworkElement)sender).Uid))
            {
                case 1: ThemesController.SetTheme(ThemeTypes.Light); break;
                case 0: ThemesController.SetTheme(ThemeTypes.ColourfulDark); break;
            }
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            imgMovable.ResetMargin();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            About.Show();
        }

        private void CopyClipboardClick(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetImage(ViewModel?.Image?.Source as BitmapSource);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to set clipboard as image --> {ex.Message}");
            }
        }

        public bool IsImageFullscreen { get; set; }

        // got lazy. its 3am
        private void Window_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.F11)
            {
                IsImageFullscreen = !IsImageFullscreen;
                ViewModel.SetFullscreen(IsImageFullscreen);
            }
        }
    }
}
