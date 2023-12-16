using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrawlStoriesData
{
    public class CrawDataHelper
    {
        public string GetMainTab(string fullContent)
        {
            string pattern = @"<main[^>]*>(.*?)<\/main>";
            Match match = Regex.Match(fullContent, pattern, RegexOptions.Singleline);
            string mainContent = match.Groups[1].Value;
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }
        public string GetDataObjectFromScriptTag(string pageContent)
        {
            string pattern = @"<script\s+id=""vite-plugin-ssr_pageContext""\s+type=""application/json"">\s*({[\s\S]*?})\s*</script>";
            Match match = Regex.Match(pageContent, pattern, RegexOptions.Singleline);

            if (match.Success)
            {
                string jsonContent = match.Groups[1].Value;
                return jsonContent;
            }
            return string.Empty;
        }
        public string GetChapterContentList(string mainTab)
        {
            string pattern = @"[\u4e00-\u9fa5]+";
            MatchCollection matches = Regex.Matches(mainTab, pattern);
            return string.Join("\n\r", matches.Select(x => x.Value));
        }
    }
}
