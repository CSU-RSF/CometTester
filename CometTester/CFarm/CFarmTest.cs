using System;
using OpenQA.Selenium;
using System.Xml;

namespace CometTester.CFarm
{
    /// <summary>
    /// cfarm test
    /// </summary>
    public class CFarmTest
    {
        BrowserDriver driver;
        GeneralTestMethods gMethods;
        readonly string _whichTest;
        private readonly string _whichBrowser;
        
        public CFarmTest(string whichBrowser, string whichTest)
        {            

            driver = new BrowserDriver(whichBrowser);
            gMethods = new GeneralTestMethods();
            _whichTest = whichTest;
            _whichBrowser = whichBrowser;

            gMethods.SetErrorStrings("none","CFarmTestConstructor");

            try
            {
                CFarmIntro();
                ActivitiesSetup();
                if (whichTest == "CFarmCroplands")
                {
                    CFarmCroplands CFarmCroplands = new CFarmCroplands(gMethods, driver, _whichTest);
                }
                if (whichTest == "CFarmCroplandsDemo")
                {
                    CFarmCroplandsDemo();
                }
                if (whichTest == "CFarmAnimalAg")
                {
                    CFarmAnimalAg CFarmAnimalAg = new CFarmAnimalAg(gMethods, driver, _whichTest);
                }
                if (whichTest == "CFarmAgroforestry")
                {
                    CFarmAgroforestry CFarmAgroforestry = new CFarmAgroforestry(gMethods, driver, _whichTest);
                }
                if (whichTest == "CFarmForestry")
                {
                    CFarmForestry CFarmForestry = new CFarmForestry(gMethods, driver);
                }
                if (whichTest == "CFarmComprehensive")
                {
                    CFarmCroplands CFarmCroplands = new CFarmCroplands(gMethods, driver, whichTest);
                    CFarmAnimalAg CFarmAnimalAg = new CFarmAnimalAg(gMethods, driver, whichTest);
                    CFarmAgroforestry CFarmAgroforestry = new CFarmAgroforestry(gMethods, driver, whichTest);
                    CFarmForestry CFarmForesty = new CFarmForestry(gMethods, driver);                   
                }
                CFarmReport CFarmReport = new CFarmReport(gMethods, driver, _whichTest);

                // This exists to check if the Report page finished loading
                //gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[1]/div/div[1]/div[3]"), driver, true);
                
            }
            catch(Exception e)
            {
                //gMethods.SendEmail(null,null,null);
            }
            driver.Driver.Quit();
        }

        public void CFarmIntro()
        {
            driver.Driver.Navigate().GoToUrl("http://cometfarm.nrel.colostate.edu/");
            //driver.Driver.Navigate().GoToUrl("http://cfarm-dev1.nrel.colostate.edu/Home");
            if (_whichBrowser != "Internet Explorer")
            {
                driver.Driver.Manage().Window.Maximize();
                
            }
            // "Close" button
            gMethods.FindAndClick(By.XPath("//button[contains(.,'Close')]"), driver, true);

            // "Start" button
            gMethods.FindAndClick(By.Id("getstartedbutton"), driver, true);

            // Username
            gMethods.FindAndInput("Tester", By.Id("username"), driver, true, false);

            // Password
            gMethods.FindAndInput("skeebop", By.Id("password"), driver, true, false);

            // "Sign In" button
            gMethods.FindAndClick(By.Id("signInButton"), driver, true);

            gMethods.AddDelay(1000);
        }

        public void ActivitiesSetup()
        {
            // delete Project
            gMethods.FindAndClick(By.Id("deleteProjectLink"), driver, false);

            // delete Project
            gMethods.FindAndClick(By.XPath("/html/body/div[7]/div[2]/div[1]/div/span[1]/a/span[2]"), driver, true);

            gMethods.AddDelay(2500);
            
            if(_whichTest=="CFarmCroplands" || _whichTest=="CFarmComprehensive")
            {
                // Cropland Checkbox
                gMethods.FindAndClick(By.Id("10"), driver, false);
            }
            if (_whichTest == "CFarmAnimalAg" || _whichTest == "CFarmComprehensive")
            {
                // Animal Ag Checkbox
                gMethods.FindAndClick(By.Id("212320"), driver, false);
            }
            if (_whichTest == "CFarmAgroforestry" || _whichTest == "CFarmComprehensive")
            {
                // Agroforestry Checkbox
                gMethods.FindAndClick(By.Id("13"), driver, false);
            }

            if (_whichTest == "CFarmForestry" || _whichTest == "CFarmComprehensive")
            {
                // Agroforestry Checkbox
                gMethods.FindAndClick(By.Id("14"), driver, false);
            }

            gMethods.AddDelay(2000);

            // "Define Activities" button
            gMethods.FindAndClick(By.Id("definebutton"), driver, true);
        }

        public void CFarmCroplandsDemo()
        {
            // Cropland Checkbox
            gMethods.FindAndClick(By.Id("10"), driver, false);

            // "Create Demo Project" link
            gMethods.FindAndClick(By.ClassName("subtleLinkLarge"), driver, false);

            // demo "Create" button
            gMethods.FindAndClick(By.Id("createDemoButton"), driver, false);

            // demo Project Link
            gMethods.FindAndClick(By.Id("Croplands Demo ProjectProject"), driver, true);

            // "Define Activities" button
            gMethods.FindAndClick(By.Id("definebutton"), driver, true);

            gMethods.FindAndClick(By.Id("cancelClickedButton"), driver, true);

            // "I am done defining parcels" button
            gMethods.FindAndClick(By.Id("nextparcelbutton"), driver, true);

            gMethods.FindAndClick(By.Id("cancelClickedButton"), driver, true);

            CropManagement();

            // "Continue to Future Management" button
            gMethods.FindAndClick(By.Id("continueFuture"), driver, true);

            CropManagement();

            // "Continue to Report" button
            gMethods.FindAndClick(By.Id("nextStep"), driver, true);

            gMethods.FindAndClick(By.Id("cancelClickedButton"), driver, true);
        }

        public void CropManagement()
        {
            gMethods.FindAndClick(By.Id("cancelClickedButton"), driver, true);

            // "Skip Ahead" button
            gMethods.FindAndClick(By.Id("nextmanagementbutton"), driver, false);

            // "No Thanks, Continue" button
            gMethods.FindAndClick(By.Id("getBurnButton"), driver, false);

            // "Copy" button
            gMethods.FindAndClick(By.ClassName("nextStep"), driver, true);

        }

        public void CFarmAnimalAg()
        {
            // demo AnimalAg Checkbox
            gMethods.FindAndClick(By.Id("212320"), driver, false);

            // "Create Demo Project" link
            gMethods.FindAndClick(By.Id("subtleLinkLarge"), driver, false);

            // Animal Ag Demo Select
            gMethods.FindAndClick(By.XPath("/html/body/div[7]/div[2]/div[1]/div/div[1]/div[1]/div/div/div/div[2]/div[1]/input"), driver, false);            

            // demo "Create" button
            gMethods.FindAndClick(By.Id("createDemoButton"), driver, false);

            gMethods.FindAndClick(By.Id("AnimalAg Demo ProjectProject"), driver, false);

            // "Define Activities" button
            gMethods.FindAndClick(By.Id("definebutton"), driver, false);

            gMethods.FindAndClick(By.Id("cancelClickedButton"), driver, false);
        }
    }
}
