using Business.Business;
using Business.Data;
using Core.Core;
using Microsoft.Extensions.Configuration;
using NUnit.Framework;
using Tests.Utils;

[assembly: LevelOfParallelism(Data.LevelOfParallelism)]

namespace Tests
{
    public class TestsSetup
    {
        [SetUp]
        public void StartBrowser()
        {
            try
            {
                LoggerHolder.Logger.Info("Configure DriverSettings");

                IConfiguration config = new ConfigurationBuilder()
                    .AddJsonFile("Tests.config.json")
                    .AddEnvironmentVariables()
                    .Build();

                DriverSettings settings = config.GetRequiredSection(nameof(DriverSettings)).Get<DriverSettings>();

                LoggerHolder.Logger.Info("Start Browser");
                DriverHolder.InitDriver(settings);
                LoggerHolder.Logger.Debug(DriverHolder.Driver.ToString());

                LoggerHolder.Logger.Info("Open Home page");
                DriverHolder.Driver.Url = Data.ApplicationUrl;
                LoggerHolder.Logger.Debug($"Application Url: {Data.ApplicationUrl}");

                LoggerHolder.Logger.Info("Maximize window");
                DriverHolder.Driver.Manage().Window.Maximize();
                DriverHolder.Driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);

                var home = new HomeContext();
                LoggerHolder.Logger.Info("Accept all cookies");
                home.AcceptAllCookies();
            }
            catch (Exception ex)
            {
                LoggerHolder.Logger.Fatal(ex);
                ScreenshotMaker.TakeScreenshot();
                throw;
            }
        }

        [TearDown]
        public void CloseBrowser()
        {
            try
            {
                LoggerHolder.Logger.Info("Close Browser");
                DriverHolder.Cleanup();

                if (File.Exists(Data.FilePath))
                {
                    File.Delete(Data.FilePath);
                }

                LoggerHolder.Logger.Debug($"Test result: {TestContext.CurrentContext.Result.Outcome.Status}");
            }
            catch (Exception ex)
            {
                LoggerHolder.Logger.Fatal(ex);
                throw;
            }
        }
    }
}
