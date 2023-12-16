using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlDataServices
{
    public static class StringExtensions
    {
        public static string DateTimeToString = "yyyyMMdd-HHmmss";
        public static bool IsNullOrEmpty(this string str)
        {
            return str == null || str.Length == 0;
        }
    }
}
