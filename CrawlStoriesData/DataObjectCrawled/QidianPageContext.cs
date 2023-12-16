using CrawlDataServices;
using Newtonsoft.Json.Linq;

namespace CrawlStoriesData.DataObjectCrawled
{
    public class QidianPageContext
    {
        public string NextUrl { get; set; }
        public string PrevUrl { get; set; }
        public string ChapterId { get; set; }
        public string ChapterNumber { get; set; }
        public string ChapterName { get; set; }
        public string ChapterContent { get; set; }
        public string BookName { get;set; }
        public string BookId { get; set; }
        public string NextChapterId { get; set; }
        public string PreviousChapterId { get; set; }
        public bool VipStatus { get; set; }
        public bool NextVipStatus { get; set; }
        public bool PrevVipStatus { get; set; }

        public QidianPageContext()
        {
            
        }
        public QidianPageContext(string jsonString)
        {
            MappingDataFromJsonString(jsonString);
        }
        private void MappingDataFromJsonString(string jsonString)
        {
            JObject jsonObject = JObject.Parse(jsonString);
            IList<Exception> exceptions = new List<Exception>();
            try
            {
                string bookName = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["bookInfo"]["bookName"];
                BookName = bookName ?? string.Empty;
            }catch(Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string chapterContent = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["content"];
                ChapterContent = chapterContent ?? string.Empty;
                UpdateContent();
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string chapterId = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["chapterId"];
                ChapterId = chapterId ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string chapterNumber = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["seq"] ?? string.Empty;
                ChapterNumber = chapterNumber ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string chapterName = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["chapterName"] ?? string.Empty;
                ChapterName = chapterName ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string prevUrl = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["extra"]["preUrl"];
                PrevUrl = prevUrl ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string nextUrl = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["extra"]["nextUrl"];
                NextUrl = nextUrl ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string nextChapterId = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["next"];
                NextChapterId = nextChapterId ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string prevChapterId = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["prev"];
                PreviousChapterId = prevChapterId ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string bookId = (string)jsonObject["pageContext"]["routeParams"]["bookId"];
                BookId = bookId ?? string.Empty;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            try
            {
                string vipStatusStr = (string)jsonObject["pageContext"]["pageProps"]["pageData"]["chapterInfo"]["vipStatus"]??string.Empty;
                int vipStatus = 1;
                int.TryParse(vipStatusStr, out vipStatus);
                VipStatus = vipStatus == 0 ? false : true;
            }
            catch (Exception ex)
            {
                exceptions.Add(ex);
            }
            if(exceptions.Count > 0)
            {
                var logger = new Logger();
                foreach (var exception in exceptions)
                {
                    logger.Error(exception).Wait();
                }
            }
        }
        public string GetChapterFullName()
        {
            return string.Format("Chapter {0}: {1}", ChapterNumber, ChapterName);
        }
        private void UpdateContent()
        {
            this.ChapterContent = this.ChapterContent.Replace("<p>", "\n\r");
        }
    }
}
