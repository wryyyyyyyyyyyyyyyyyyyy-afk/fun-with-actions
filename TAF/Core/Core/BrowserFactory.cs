using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Remote;

namespace Core.Core
{
    class BrowserFactory
    {
        internal static IWebDriver GetBrowser(BrowserName browserName)
        {
            switch (browserName)
            {
                case BrowserName.Firefox:
                    return new FirefoxDriver();

                case BrowserName.InternetExplorer:
                    return new InternetExplorerDriver();

                case BrowserName.Chrome:
                    var chromeOptions = new ChromeOptions { PageLoadStrategy = PageLoadStrategy.Normal };
                    chromeOptions.AddArgument("no-sandbox");
                    return new ChromeDriver(chromeOptions);

                default:
                case BrowserName.Remote:
                    var remoteOptions = new ChromeOptions { PageLoadStrategy = PageLoadStrategy.Normal };
                    remoteOptions.AddArgument("no-sandbox");
                    return new RemoteWebDriver(remoteOptions);
            }
        }
    }
}
