using CrawlDataServices.FileHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlDataServices
{
    public class Logger : ILogger
    {
        private IFileWriterHelper _writerHelper;
        private string ErrorStr = "Error";
        private string InforStr = "Info";
        private string WarningStr = "Warning";
        private string ContentFormat = "{0} -- {1}: {2}";
        public Logger()
        {
            _writerHelper = new FileWriterHelper("Logger", "Log");
        }
        public async Task Error(Exception ex)
        {
            await this.Error(ex.ToString());
        }

        public async Task Error(string info)
        {
            string content = string.Format(ContentFormat, DateTime.Now.ToString(StringExtensions.DateTimeToString), this.ErrorStr, info);
            await _writerHelper.AddTextToFileAsync(content);
        }

        public async Task Infomation(Exception ex)
        {
            await this.Infomation(ex.ToString());
        }

        public async Task Infomation(string info)
        {
            string content = string.Format(ContentFormat, DateTime.Now.ToString(StringExtensions.DateTimeToString), this.InforStr, info);
            await _writerHelper.AddTextToFileAsync(content);
        }

        public async Task Warning(Exception ex)
        {
            await this.Warning(ex.ToString());
        }

        public async Task Warning(string info)
        {
            string content = string.Format(ContentFormat, DateTime.Now.ToString(StringExtensions.DateTimeToString), this.WarningStr, info);
            await _writerHelper.AddTextToFileAsync(content);
        }
    }
}
