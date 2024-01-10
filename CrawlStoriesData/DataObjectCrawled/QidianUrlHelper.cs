using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrawlDataServices;
using TranslateServices;

namespace CrawlStoriesData.DataObjectCrawled
{
    public static class QidianHelper
    {
        public static string Url { get; } = "https://www.qidian.com/";
        public static string FolderName { get; } = "Qidian";
        public static string FolderPath { get; } = CustomSettings.SaveFileUrl + FolderName;
        public static string BookUrl(string bookId)
        {
            return $"{Url}/book/{bookId}";
        }
        public static string ChapterUrl(string bookId, string chapterId)
        {
            return $"{Url}/chapter/{bookId}/{chapterId}/";
        }
        public static string GenerateFileName(QidianPageContext qidianPageContext)
        {
            if(qidianPageContext == null || qidianPageContext.BookId.IsNullOrEmpty() || qidianPageContext.ChapterContent.IsNullOrEmpty())
            {
                return string.Empty;
            }
            return $"{qidianPageContext.BookId}\\Chinese\\{qidianPageContext.BookId}-{qidianPageContext.ChapterId}-Chapter{qidianPageContext.ChapterNumber + (qidianPageContext.VipStatus ? "_Vip" : "")}.txt";
        }
        public static string GenerateTranslateFileName(QidianPageContext qidianPageContext, LanguageEnum language)
        {
            if (qidianPageContext == null || qidianPageContext.BookId.IsNullOrEmpty() || qidianPageContext.ChapterContent.IsNullOrEmpty())
            {
                return string.Empty;
            }
            return $"{qidianPageContext.BookId}\\{language.GetLanguageFullName()}\\{qidianPageContext.BookId}-{qidianPageContext.ChapterId}-Chapter{qidianPageContext.ChapterNumber + (qidianPageContext.VipStatus ? "_Vip" : "")}.txt";
        }
        public static bool IsValidUrl(this string url)
        {
            if (Uri.TryCreate(url, UriKind.Absolute, out _))
            {
                return true; // URL is valid
            }
            else
            {
                return false; // URL is not valid
            }
        }
        public static string GetNextChapterUrl(this QidianPageContext qidianPageContext)
        {
            if(qidianPageContext != null && !qidianPageContext.BookId.IsNullOrEmpty() && !qidianPageContext.NextChapterId.IsNullOrEmpty())
            {
                return string.Format("https://www.qidian.com/chapter/{0}/{1}/", qidianPageContext.BookId, qidianPageContext.NextChapterId);
            }
            return string.Empty;
        }
    }
}
