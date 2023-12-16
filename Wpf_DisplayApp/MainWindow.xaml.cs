using CrawlDataServices;
using CrawlStoriesData;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Wpf_DisplayApp.StartupHelpers;

namespace Wpf_DisplayApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly ICrawWebpageServices _crawQidianWebpageServices;

        public MainWindow(ICrawWebpageServices crawQidianWebpageServices)
        {
            _crawQidianWebpageServices = crawQidianWebpageServices;
            InitializeComponent();
            Closed += ExitProgram;
        }

        private void ExitProgram(object? sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private async void btnGetData_Click(object sender, RoutedEventArgs e)
        {
            var url = txbUrl.Text;
            txbUrl.Text = string.Empty;
            btnGetData.IsEnabled = false;
            btnGetData.Content = "Downloading, please wait ...";
            await _crawQidianWebpageServices.CrawlAndSaveAllChaptersOfStoryAsync(url);
            btnGetData.IsEnabled = true;
            btnGetData.Content = "Download Text Files";
            MessageBox.Show("Download finish with url: " + url);
            OpenFolder(CustomSettings.SaveFileUrl);
        }
        private void OpenFolder(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    Arguments = folderPath,
                    FileName = "explorer.exe"
                };

                Process.Start(startInfo);
            }
            else
            {
                MessageBox.Show(string.Format("{0} Directory does not exist!", folderPath));
            }
        }
    }
}