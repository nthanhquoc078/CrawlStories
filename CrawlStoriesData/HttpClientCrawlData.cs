using CrawlDataServices;
using CrawlStoriesData.DataObjectCrawled;
using System.Net;

namespace CrawlStoriesData
{
    public class HttpClientCrawlData : ICrawlData
    {
        HttpClient httpClient;
        HttpClientHandler httpHandler;
        CookieContainer cookie;

        public HttpClientCrawlData()
        {
            cookie = new CookieContainer();
            httpHandler = new HttpClientHandler()
            {
               CookieContainer = cookie,
               ClientCertificateOptions = ClientCertificateOption.Automatic,
               AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip,
               AllowAutoRedirect = true,
               UseDefaultCredentials = false,
            };
            httpClient = new HttpClient(httpHandler);
            SetRequestHeader();
        }
        private void SetRequestHeader()
        {
            if (httpClient == null)
                return;
            httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,image/apng,*/*;q=0.8,application/signed-exchange;v=b3;q=0.7");
            httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
            httpClient.DefaultRequestHeaders.Add("Sec-Ch-Ua", "\"Not_A Brand\";v=\"8\", \"Chromium\";v=\"120\", \"Google Chrome\";v=\"120\"");
            httpClient.DefaultRequestHeaders.Add("Accept-Language", "en");
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Site", "same-origin");
            httpClient.DefaultRequestHeaders.Add("Sec-Fetch-User", "?1");
            httpClient.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
            httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
        }
        public async Task<string> CrawDataAsync(string url = "")
        {
            if (url.IsNullOrEmpty())
            {
                url = QidianHelper.Url;
            }
            if (url.StartsWith("//"))
            {
                url = "https:" + url;
            }
            var uri = new Uri(url);
            var responseMessage = await httpClient.GetAsync(uri);
            if(responseMessage.StatusCode == HttpStatusCode.OK)
            {
                return await responseMessage.Content.ReadAsStringAsync();
            }
            return string.Empty;
        }

        public Task GotoURLAsync(string siteUrl)
        {
            throw new NotImplementedException();
        }
    }
}
