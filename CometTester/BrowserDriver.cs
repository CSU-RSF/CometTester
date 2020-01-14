using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.IE;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Edge;

namespace CometTester
{
    /// <summary>
    /// Driver object
    /// </summary>
    public class BrowserDriver
    {
        public IWebDriver Driver;
        public string whichBrowser;
  

        public BrowserDriver(string whichBrowser)
        {

            this.whichBrowser = whichBrowser;

            if (whichBrowser.Contains("Firefox"))
            {
    
                Driver = new FirefoxDriver();
                Driver.Manage().Window.Maximize();
            }
            if (whichBrowser.Contains("Microsoft Edge"))
            {
                Driver = new EdgeDriver();
            }
            if (whichBrowser.Contains("Chrome"))
            {
                ChromeOptions options = new ChromeOptions();
                options.AddArgument("--start-maximized");
                Driver = new ChromeDriver(options);
            } 
        }

    }
}
