using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using System.Xml;

namespace CometTester
{
    class CGlobalTest
    {
        BrowserDriver driver;
        GeneralTestMethods gMethods;
        readonly string _whichTest;
        private readonly string _whichBrowser;

        public CGlobalTest (string whichBrowser, string whichTest)
        {
            driver = new BrowserDriver(whichBrowser);
            gMethods = new GeneralTestMethods();
            _whichTest = whichTest;
            _whichBrowser = whichBrowser;

            gMethods.SetErrorStrings("none", "CGlobalTestConstructor");

            try
            {

            }
            catch (Exception)
            {
                {
                    
                }
                throw;
            }
        }

        public void CGlobalIntro()
        {
            driver.Driver.Navigate().GoToUrl("http://localhost/cgloweb/"); //Driver is used to create a browser depending on the browser that's being used

            if (_whichBrowser != "Internet Explorer")
            {
                driver.Driver.Manage().Window.Maximize(); //maximize window size
            }

            string signInPath = "/html/body/div/div[1]/div[3]/a[1]"; //XPath for sign in
            gMethods.FindAndClick(By.XPath(signInPath), driver, true);  //locate sign in button and click it

            string continuePath = "/html/body/div/div[2]/div[2]/div[2]/a/input";
            gMethods.FindAndClick(By.XPath(continuePath), driver, true);

            gMethods.AddDelay(1000);
        }

        public void ActivitiesSetup()
        {
            
        }
    }
}
