using OpenQA.Selenium;

namespace CometTester.CFarm
{
    /// <summary>
    /// Runs through initial cfarm setup: sign in, activities selection
    /// Instantiates CFarm module objects based on xml file name
    /// </summary>
    class CFarmXmlTest
    {
        BrowserDriver driver;
        GeneralTestMethods gMethods;
       // readonly string _whichTest;
        
        public CFarmXmlTest(BrowserDriver driver, GeneralTestMethods gMethods, CFarmXmlLister cFarmXml, string username, string url)
        {
            //don't use _whichTest, look at which nodes are present in the xml.
          
         //  _whichTest = whichTest;
            this.driver = driver;
            this.gMethods = gMethods;



            gMethods.SetErrorStrings("none","CFarmXmlTestConstructor");

                CFarmIntro(username, url);
                ActivitiesSetup(cFarmXml);
                if (cFarmXml._xmlDoc.CFarmCroplandsData!=null)//(whichTest.Contains("Croplands"))
                {
                    var CFarmXmlCroplands = new CFarmXmlCroplands(gMethods, driver, cFarmXml._xmlDoc.CFarmCroplandsData);
                }
                if (cFarmXml._xmlDoc.CFarmAnimalAgData != null)//(whichTest.Contains("AnimalAg"))
            {
                    var CFarmXmlAnimalAg = new CFarmXmlAnimalAg(gMethods, driver, cFarmXml._xmlDoc.CFarmAnimalAgData);
                }
            if (cFarmXml._xmlDoc.CFarmAgroforestryData != null)//(whichTest.Contains("Agroforestry"))
            {
                    var CFarmXmlAgroforestry = new CFarmXmlAgroforestry(gMethods, driver, cFarmXml._xmlDoc.CFarmAgroforestryData);
                }
            if (cFarmXml._xmlDoc.CFarmForestryData != null)//(whichTest.Contains("Forestry"))
            {
                    var CFarmXmlForestry = new CFarmXmlForestry(gMethods, driver, cFarmXml._xmlDoc.CFarmForestryData);
                }
        }



        public void WaitTillMask()
        {
            //cometfarm is a bad program
            //we make a new x-mask-loading every time we get a loading mask
            //so we want to make sure none of these are visible.

            bool Cont = false;
            while (!Cont)
            {

                var AllLoadings = driver.Driver.FindElements(By.XPath("//div[contains(@id,'loadmask-')]"));
                Cont = true;
                foreach (var element in AllLoadings)
                {

                    var b = element.GetAttribute("style").ToLower();
                    if (!b.Contains("display: none"))
                    {
                        gMethods.AddDelay(500);
                        Cont = false;
                        break;
                    }


                }

            }




        }

        /// <summary>
        /// Goes through Home Page and Sign in
        /// </summary>
        public void CFarmIntro(string username,string url)
        {
            driver.Driver.Navigate().GoToUrl(url);
            //driver.Driver.Navigate().GoToUrl("http://cfarm-dev1.nrel.colostate.edu/Home");
            //driver.Driver.Manage().Window.Maximize();
            
                            
            // "Close" button
            gMethods.FindAndClick(By.XPath("//button[contains(.,'Close')]"), driver, true);

            // "Start" button
            gMethods.FindAndClick(By.Id("getstartedbutton"), driver, true);

            // Username
            //different username per browser
            gMethods.FindAndInput(username, By.Id("username"), driver, true, false);

            // Password
            gMethods.FindAndInput("skeebop", By.Id("password"), driver, true, false);

            // "Sign In" button
            gMethods.FindAndClick(By.Id("signInButton"), driver, true);

            gMethods.AddDelay(1000);
        }

        /// <summary>
        /// Deletes previous project and selects activities check boxes
        /// </summary>
        public void ActivitiesSetup(CFarmXmlLister cFarmXml)
        {
          //  gMethods.AddDelay(500);
            WaitTillMask();
            // delete Project
            try
            {
                gMethods.FindAndClick(By.Id("deleteProjectLink"), driver, false);
            }
            catch {
                gMethods.AddDelay(5000);
                try
                {
                    gMethods.FindAndClick(By.Id("deleteProjectLink"), driver, false);
                }
                catch { }


                } //no project to delete
            // delete Project
            gMethods.FindAndClick(By.XPath("//span[contains(@class,'cfarmButton') and contains(.,'Delete')]"), driver, true);

            gMethods.AddDelay(3000);
            WaitTillMask();
          

            if ((cFarmXml._xmlDoc.CFarmCroplandsData != null))//(_whichTest.Contains("Croplands") || _whichTest.Contains("Comprehensive"))
            {
                // Cropland Checkbox
                //gMethods.FindAndClick(By.Id("Croplands Demo Project_liProject"), driver, true);
                gMethods.FindAndClick(By.XPath("//label[contains(.,'Cropland, Pasture, Range') and not(contains(@display,'none'))]"), driver, false);
            }
            if ((cFarmXml._xmlDoc.CFarmAnimalAgData != null))//(_whichTest.Contains("AnimalAg") || _whichTest.Contains("Comprehensive"))
            {
                // Animal Ag Checkbox
                gMethods.FindAndClick(By.XPath("//label[contains(.,'Animal Agriculture') and not(contains(@display,'none'))]"), driver, false);
            }
            if(cFarmXml._xmlDoc.CFarmAgroforestryData != null) // (_whichTest.Contains("Agroforestry") || _whichTest.Contains("Comprehensive"))
            {
                // Agroforestry Checkbox
                gMethods.FindAndClick(By.XPath("//label[contains(.,'Agroforestry') and not(contains(@display,'none'))]"), driver, false);
            }

            if (cFarmXml._xmlDoc.CFarmForestryData != null)//(_whichTest.Contains("Forestry") || _whichTest.Contains("Comprehensive"))
            {
                // Agroforestry Checkbox
                gMethods.FindAndClick(By.XPath("//label[contains(.,'Forestry') and not(contains(@display,'none'))]"), driver, false);
            }

            //    gMethods.AddDelay(2000);
            WaitTillMask();
            // "Define Activities" button
            gMethods.FindAndClick(By.Id("definebutton"), driver, true);
        }
    }
}

