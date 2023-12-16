using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrawlDataServices.FileHelper
{
    public interface IFileWriterHelper
    {
        Task WriteToFileAsync(string content);
        Task AddTextToFileAsync(string content);
    }
}
