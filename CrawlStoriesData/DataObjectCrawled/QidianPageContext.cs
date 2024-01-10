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
        public string GetChapterFullName()
        {
            return string.Format("Chapter {0}: {1}", ChapterNumber, ChapterName);
        }
        public void UpdateContent()
        {
            this.ChapterContent = this.ChapterContent.Replace("<p>", "\n\r");
        }
    }
}
