using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlStoriesData
{
    public interface ICrawlData
    {
        Task<string> CrawDataAsync(string url = "");
        Task GotoURLAsync(string siteUrl);
    }
}
