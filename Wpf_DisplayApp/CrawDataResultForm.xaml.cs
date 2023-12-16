using CrawlDataServices.FileHelper;
using CrawlStoriesData;
using CrawlStoriesData.DataObjectCrawled;
using System;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Wpf_DisplayApp
{
    /// <summary>
    /// Interaction logic for CrawDataResultForm.xaml
    /// </summary>
    public partial class CrawDataResultForm : Window
    {
        //public string CrawlUrl { get; set; }
        private ICrawlData _crawlData { get; }
        //private QidianPageContext qidianPage = null;
        public CrawDataResultForm(ICrawlData crawlData)
        {
            InitializeComponent();
            _crawlData = crawlData;
            Loaded += async (sender, e) => {
                await Window_Loaded(sender, e);
            };
        }

        private async Task Window_Loaded(object sender, RoutedEventArgs e)
        {
            //var result = await _crawlData.CrawDataAsync(CrawlUrl);
            //var crawDataHelper = new CrawDataHelper();
            //var dataJson = crawDataHelper.GetDataObjectFromScriptTag(result);
            //qidianPage = new QidianPageContext(dataJson);
            //lbBookName.Content = qidianPage.BookName;
            //txbResult.Text = qidianPage.ChapterContent;
        }
        private async void btnSaveToFile_Click(object sender, RoutedEventArgs e)
        {
            //if(qidianPage != null)
            //{
            //    var writer = new FileWriterHelper("Qidian", QidianHelper.GenerateFileName(qidianPage));
            //    await writer.AddTextToFileAsync(qidianPage.GetChapterFullName());
            //    await writer.AddTextToFileAsync(qidianPage.ChapterContent);
            //}
        }
    }
}
