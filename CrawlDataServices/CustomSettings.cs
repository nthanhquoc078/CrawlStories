using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlDataServices
{
    public static class CustomSettings
    {
        private static string CurrentUserDocmentPath { get; } = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string SaveFileUrl = CurrentUserDocmentPath + "\\ConvertStories\\FileSaver\\";
    }
}
