using ImageViewer.Animations;
using ImageViewer.Helpers;
using ImageViewer.Image;
using ImageViewer.Themes;
using ImageViewer.ViewModels;
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

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainViewModel ViewModel { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            ViewModel.AnimateFadeIn = AnimateFadeIn;
            DataContext = ViewModel;
            System.Windows.Controls.Image ee;
        }

        public MainWindow(string[] startupArgs)
        {
            InitializeComponent();
            ViewModel = new MainViewModel();
            ViewModel.AnimateFadeIn = AnimateFadeIn;
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
            switch (int.Parse(((Button)sender).Uid))
            {
                case 0: ThemesController.SetTheme(ThemeTypes.Light); break;
                case 1: ThemesController.SetTheme(ThemeTypes.ColourfulLight); break;
                case 2: ThemesController.SetTheme(ThemeTypes.Dark); break;
                case 3: ThemesController.SetTheme(ThemeTypes.ColourfulDark); break;
            }
            e.Handled = true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            imgMovable.ResetMargin();
        }
    }
}
