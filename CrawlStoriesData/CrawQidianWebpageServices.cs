using CrawlDataServices;
using CrawlDataServices.FileHelper;
using CrawlStoriesData.DataObjectCrawled;
using HtmlAgilityPack;
using System.Text.RegularExpressions;

namespace CrawlStoriesData
{
    public class CrawQidianWebpageServices : ICrawWebpageServices
    {
        private readonly ILogger _logger;
        private readonly ICrawlData _crawlData;

        public CrawQidianWebpageServices(
            ICrawlData crawlData
            )
        {
            _logger = new Logger();
            _crawlData = crawlData;
        }
        public async Task CrawlAndSaveAllChaptersOfStoryAsync(string url)
        {
            if (url.Contains("/book/"))
            {
                var htmlResult = await _crawlData.CrawDataAsync(url);
                if(htmlResult.IsNullOrEmpty())
                {
                    return;
                }
                string pattern = @"<a\b[^>]*\sid\s*=\s*""bookImg""[^>]*>(.*?)</a>"; //get a tag from the page
                Match match = Regex.Match(htmlResult, pattern, RegexOptions.Singleline);

                if (match.Success)
                {
                    string tagResult = match.Groups[0].Value;
                    HtmlDocument doc = new HtmlDocument();
                    doc.LoadHtml(tagResult);
                    HtmlNode bookImgNode = doc.DocumentNode.SelectSingleNode("//a[@id='bookImg']");
                    if (bookImgNode != null)
                    {
                        // Get the value of the href attribute
                        string hrefValue = bookImgNode.GetAttributeValue("href", "");
                        if(!hrefValue.IsNullOrEmpty() && hrefValue.IsValidUrl())
                        {
                            await this.CrawlAndSaveAllChapterAsync(hrefValue);
                        }
                    }
                }
            }
            else
            {
                await this.CrawlAndSaveAllChapterAsync(url);
            }
        }
        private async Task CrawlAndSaveAllChapterAsync(string fromChapterUrl)
        {
            QidianPageContext qidianChapterContext = await CrawAndSaveChapterAsync(fromChapterUrl);
            while(qidianChapterContext != null && !qidianChapterContext.NextUrl.IsNullOrEmpty())
            {
                var nextUrl = qidianChapterContext.GetNextChapterUrl();
                if(nextUrl.IsNullOrEmpty())
                {
                    break;
                }
                var nextchapter = await this.CrawAndSaveChapterAsync(nextUrl);
                if(nextchapter != null)
                {
                    qidianChapterContext = nextchapter;
                }
                else
                {
                    break;
                }
            }

        }
        /// <summary>
        /// Craw and save chapter to file, then return next chapter page context
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private async Task<QidianPageContext> CrawAndSaveChapterAsync(string url)
        {
            var htmlResult = await _crawlData.CrawDataAsync(url);
            if (!htmlResult.IsNullOrEmpty())
            {
                var crawDataHelper = new CrawDataHelper();
                var dataJson = crawDataHelper.GetDataObjectFromScriptTag(htmlResult);
                var qidianPage = new QidianPageContext(dataJson);
                if (qidianPage != null)
                {
                    var writer = new FileWriterHelper(QidianHelper.FolderName, QidianHelper.GenerateFileName(qidianPage));
                    await writer.WriteToFileAsync(qidianPage.GetChapterFullName());
                    await writer.AddTextToFileAsync(qidianPage.ChapterContent);
                    return qidianPage;
                }
            }
            return null;
        }
    }
}
