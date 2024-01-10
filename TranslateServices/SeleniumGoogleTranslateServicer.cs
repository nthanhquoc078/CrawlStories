using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Tokens;
using CrawlDataServices;

namespace TranslateServices
{
    public class SeleniumGoogleTranslateServicer : ITranslateService, IDisposable
    {
        IWebDriver _driver = null;

        private readonly ILogger _logger;

        public SeleniumGoogleTranslateServicer(ILogger logger)
        {
            _logger = logger;
            //GoToUrl();
        }
        private void GoToUrl(string url = "")
        {
            if(_driver == null)
            {
                var options = new ChromeOptions();
                options.AddArgument("--window-size=200,200");
                var chromeDriverService = ChromeDriverService.CreateDefaultService();
                chromeDriverService.HideCommandPromptWindow = false;
                //options.AddArgument("--no-startup-window");
                _driver = new ChromeDriver(chromeDriverService, options);
            }
            if (url.IsNullOrEmpty())
            {
                url = GetUrl();
            }
            _driver.Navigate().GoToUrl(url);
        }
        public async Task<string> TranslateAsync(string input, LanguageEnum from = LanguageEnum.Auto, LanguageEnum to = LanguageEnum.English)
        {
            int delayTimes = 0;
            if (_driver == null)
            {
                GoToUrl(GetUrl(from, to));
                await Task.Delay(2000);
            }
            delayTimes = input.Split().Length;

            IWebElement sourceInput = _driver.FindElement(By.XPath("//textarea[@aria-label='Source text']"));

            //sourceInput.Click();
            sourceInput.Clear();
            sourceInput.SendKeys(input);
            sourceInput.SendKeys(Keys.Enter);

            if(delayTimes < 1000)
            {
                delayTimes = 1000;
            }
            await Task.Delay(delayTimes);
            //_driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromMilliseconds(delayTimes);
            IList< IWebElement> resultOutputs = new List<IWebElement>();
            int count = 0;
            do
            {
                try
                {
                    resultOutputs = _driver.FindElements(By.CssSelector("span.ryNqvb"));
                    count = 10;
                }
                catch (Exception ex)
                {
                    count++;
                    await Task.Delay(1000);
                    await _logger.Error(ex);
                }
            } while (count < 10);
            var resultString = string.Join("\n", resultOutputs.Select(x => x.Text)) ?? string.Empty;
            return resultString;
        }
        private string GetUrl(LanguageEnum from = LanguageEnum.Auto, LanguageEnum to = LanguageEnum.English)
        {
            var url = string.Format("https://translate.google.com/?hl=en&sl={0}&tl={1}&op=translate", from.GetLanguageShortName(true), to.GetLanguageShortName());
            return url;
        }

        public void Dispose()
        {
            if(_driver != null )
            {
                _driver.Close();
                _driver.Quit();
            }
        }
    }
}
