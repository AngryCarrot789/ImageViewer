using System.Reflection;
using System.Windows;

namespace ImageViewer
{
    /// <summary>
    /// Interaction logic for AboutWindow.xaml
    /// </summary>
    public partial class AboutWindow : Window
    {
        public AboutWindow()
        {
            InitializeComponent();
            description.Text = AssemblyDescription;
            Closing += AboutWindow_Closing;
        }
        bool forceClose;
        private void AboutWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!forceClose)
            {
                e.Cancel = true;
                this.Hide();
            }
        }

        public void ForceClose()
        {
            forceClose = true;
            Close();
        }

        public string AssemblyDescription
        {
            get
            {
                object[] attributes = 
                    Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(AssemblyDescriptionAttribute), false);
                return attributes.Length != 0
                    ? ((AssemblyDescriptionAttribute)attributes[0]).Description :
                    "Image Viewer";
            }
        }
    }
}
