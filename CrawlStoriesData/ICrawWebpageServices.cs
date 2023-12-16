namespace CrawlStoriesData
{
    public interface ICrawWebpageServices
    {
        Task CrawlAndSaveAllChaptersOfStoryAsync(string url);
    }
}