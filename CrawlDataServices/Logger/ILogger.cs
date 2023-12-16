namespace CrawlDataServices
{
    public interface ILogger
    {
        Task Infomation(Exception ex);
        Task Infomation(string info);
        Task Error(Exception ex);
        Task Error(string info);
        Task Warning(Exception ex);
        Task Warning(string info);
    }
}
