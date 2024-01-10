using CrawlDataServices;
using CrawlDataServices.FileHelper;
using CrawlStoriesData.DataObjectCrawled;
using HtmlAgilityPack;
using Newtonsoft.Json.Linq;
using System.Net.NetworkInformation;
using System.Net;
using System.Text.RegularExpressions;
using TranslateServices;

namespace CrawlStoriesData
{
    public class CrawQidianWebpageServices : ICrawWebpageServices
    {
        private readonly ILogger _logger;
        private readonly ICrawlData _crawlData;
        private readonly ITranslateService _translateServices;

        public CrawQidianWebpageServices(
            ICrawlData crawlData,
            ILogger logger,
            ITranslateService translateServices
            )
        {
            _logger = logger;
            _crawlData = crawlData;
            _translateServices = translateServices;
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
                var qidianPage = GetQidianPageContextFromJsonString(dataJson);
                if (qidianPage != null)
                {
                    var writer = new FileWriterHelper(QidianHelper.FolderName, QidianHelper.GenerateFileName(qidianPage));
                    await writer.WriteToFileAsync(qidianPage.GetChapterFullName());
                    await writer.AddTextToFileAsync(qidianPage.ChapterContent);
                    await TranslateAndSaveToFile(qidianPage);
                    return qidianPage;
                }
            }
            return null;
        }
        private async Task TranslateAndSaveToFile(QidianPageContext pageContext)
        {
            var contentTranslate = await _translateServices.TranslateAsync(pageContext.ChapterContent);
            var writer = new FileWriterHelper(QidianHelper.FolderName, QidianHelper.GenerateTranslateFileName(pageContext, LanguageEnum.English));
            await writer.WriteToFileAsync(pageContext.GetChapterFullName());
            await writer.AddTextToFileAsync(contentTranslate);
        }
        private QidianPageContext GetQidianPageContextFromJsonString(string jsonString)
        {
            var qidianPageContext = new QidianPageContext();
            JObject jsonObject = JObject.Parse(jsonString);
            IList<Exception> exceptions = new List<Exception>();
            try
            {
                string bookName = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["bookInfo"]["bookName"];
                qidianPageContext.BookName = bookName ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string chapterContent = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["content"];
                qidianPageContext.ChapterContent = chapterContent ?? string.Empty;
                qidianPageContext.UpdateContent();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string chapterId = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["chapterId"];
                qidianPageContext.ChapterId = chapterId ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string chapterNumber = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["seq"] ?? string.Empty;
                qidianPageContext.ChapterNumber = chapterNumber ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string chapterName = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["chapterName"] ?? string.Empty;
                qidianPageContext.ChapterName = chapterName ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string prevUrl = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["extra"]["preUrl"];
                qidianPageContext.PrevUrl = prevUrl ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string nextUrl = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["extra"]["nextUrl"];
                qidianPageContext.NextUrl = nextUrl ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string nextChapterId = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["next"];
                qidianPageContext.NextChapterId = nextChapterId ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string prevChapterId = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["prev"];
                qidianPageContext.PreviousChapterId = prevChapterId ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string bookId = (string)jsonObject["pageContext"]["routeParams"]["bookId"];
                qidianPageContext.BookId = bookId ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string vipStatusStr = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["vipStatus"] ?? string.Empty;
                int vipStatus = 1;
                int.TryParse(vipStatusStr, out vipStatus);
                qidianPageContext.VipStatus = vipStatus == 0 ? false : true;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            if (exceptions.Count > 0)
            {
                foreach (var exception in exceptions)
                {
                    _logger.Error(exception).Wait();
                }
            }
            return qidianPageContext;
        }
    }
}
