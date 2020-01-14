using OpenQA.Selenium;
using System.Xml;

namespace CometTester.CFarm
{
    class CFarmCroplands
    {
        GeneralTestMethods gMethods;
        BrowserDriver driver;

        public CFarmCroplands(GeneralTestMethods gMethods, BrowserDriver driver, string whichTest)
        {
            this.gMethods = gMethods;
            this.driver = driver;           

            ParcelLocationsPage();
            HistoricManagementPage();
            CropAndPlant();
            TillagePage();
            NitrogenPage();

            // "Next" button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[8]/span/a/span[2]"), driver, true);

            IrrigationPage();
            EndCrops(false);
            CreateCropScenario(true, "Future");
            EndCrops(true);            
        }

        public void ParcelLocationsPage()
        {
            // "Address Query" box
            gMethods.FindAndInput("40.381856, -101.708146", By.XPath("//div[contains(@id,'addressText')]/div[1]/input"), driver, true, false);

            // "Go to Location" button
            gMethods.FindAndClick(By.XPath("/html/body/div[10]/div[2]/div/div[1]/div[3]/div/span/a"), driver, true);

            gMethods.AddDelay(500);

            gMethods.ClickOnPage(By.Id("OpenLayers.Layer.Vector_53_svgRoot"), driver, 400, 100, "drag", 0, 400);

            gMethods.AddDelay(500);

            //gMethods.clickOnPage(By.Id("OpenLayers.Layer.Vector_53_svgRoot"), driver, 400, 400, "drag", -400, -400);

            //gMethods.addDelay(500);

            //gMethods.clickOnPage(By.Id("OpenLayers.Layer.Vector_53_svgRoot"), driver, 400, 400, "drag", -400, -400);

            // "Add Parcel by Polygon" button
            gMethods.FindAndClick(By.Id("addPoint"), driver, true);

            gMethods.ClickOnPage(By.Id("OpenLayers.Map_5_events"), driver, 545, 315, "single", 0, 0);

            gMethods.AddDelay(1000);

            // Area Input
            gMethods.FindAndInput("129", By.XPath("//div[contains(@id,'area')]/div[1]/input"), driver, true, false);   

            // "Save" button
            gMethods.FindAndClick(By.Id("button-1073-btnEl"), driver, true);

            gMethods.AddDelay(1000);

            // "I am done defining parcels" button
            gMethods.FindAndClick(By.Id("nextparcelbutton"), driver, true);

        }

        public void HistoricManagementPage()
        {
            
            // "Pre-1980 Management"
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/form/div[2]/div[2]/div[2]/select/option[2]"), driver, true);

            // "1980-2000 Management"
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/form/div[2]/div[5]/div[2]/select/option[2]"), driver, true);

            // "1980-2000 Tillage"
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/form/div[2]/div[6]/div[2]/select/option[2]"), driver, true);                   

            // "Next" button
            gMethods.FindAndClick(By.Id("nexthistoricbutton"), driver, true);
        }

        public void CreateCropScenario(bool copy, string scenarioName)
        {
            // Scenario input
            if (gMethods.IsElementPresent(By.XPath("/html/body/div[23]/div[2]/div[1]/div/div/div[2]/div[2]/div[1]/div[1]/input"), driver))
            {
                gMethods.AddDelay(500);
                gMethods.FindAndInput(scenarioName, By.XPath("/html/body/div[23]/div[2]/div[1]/div/div/div[2]/div[2]/div[1]/div[1]/input"), driver, true, false);
            }
            else
            {
                gMethods.FindAndInput(scenarioName, By.XPath("/html/body/div[19]/div[2]/div[1]/div/div/div[2]/div[2]/div[1]/div[1]/input"), driver, true, false);
            }

            if(copy)
            {
                // "Copy" checkbox
                if (gMethods.IsElementPresent(By.XPath("/html/body/div[23]/div[2]/div[1]/div/div/div[2]/div[2]/div[2]/div[1]/input"), driver))
                {
                    gMethods.AddDelay(500);
                    gMethods.FindAndClick(By.XPath("/html/body/div[23]/div[2]/div[1]/div/div/div[2]/div[2]/div[2]/div[1]/input"), driver, true);
                }
                else
                {
                    gMethods.FindAndClick(By.XPath("/html/body/div[19]/div[2]/div[1]/div/div/div[2]/div[2]/div[2]/div[1]/input"), driver, true);
                }
            }

            gMethods.AddDelay(500);

            // "Start" button
            if (gMethods.IsElementPresent(By.XPath("/html/body/div[23]/div[2]/div[1]/div/div/div[2]/div[2]/div[3]/div/span/a/span[2]"), driver))
            {
                gMethods.AddDelay(500);
                gMethods.FindAndClick(By.XPath("/html/body/div[23]/div[2]/div[1]/div/div/div[2]/div[2]/div[3]/div/span/a/span[2]"), driver, true);
            }
            else
            {
                gMethods.FindAndClick(By.XPath("/html/body/div[19]/div[2]/div[1]/div/div/div[2]/div[2]/div[3]/div/span/a/span[2]"), driver, true);
            }
        }

        public void CropAndPlant()
        {
            gMethods.AddDelay(8000);

            //Crop drop down
            //gMethods.FindAndClick(By.XPath("//div[@id='crop-triggerWrap']/div[1]"), driver, true);
            if (gMethods.IsElementPresent(By.XPath("//div[@id='crop-triggerWrap']/div[1]"), driver))
            {
                gMethods.AddDelay(500);
                gMethods.FindAndClick(By.XPath("//div[@id='crop-triggerWrap']/div[1]"), driver, true);
            }
            else
            {
                gMethods.FindAndClick(By.XPath("//div[@id='crop-triggerWrap']"), driver, true);
            }
            // /html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[1]/div/div[1]/div/div/div/div/div[1]/div/div[2]/div[1]/div[2]/div[1]

            gMethods.AddDelay(500);

            // "Corn"
            gMethods.FindAndClick(By.XPath("/html/body/div[contains(@id,'boundlist')]/div/ul/li[4]"), driver, true);

            gMethods.AddDelay(500);

            // "Plant Date"
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[1]/div/div[1]/div/div/div/div/div[1]/div/div[5]/div[1]/div/div[1]"), driver, true);

            // To Month
            gMethods.EnterKeys(By.XPath("/html/body"), driver, Keys.Control, Keys.Right, Keys.Left, true, 3);

            // To Day
            gMethods.FindAndClick(By.LinkText("3"), driver, true);
            
            // "Add New Harvest" button
            gMethods.FindAndClick(By.Id("button-1028-btnEl"), driver, true);

            // "Harvest Date" Box
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[1]/div/div[1]/div/div/div/div/div[2]/div/div[1]/div/div[1]/div/div[1]/div[4]/div/table/tbody/tr[2]/td[1]/div"), driver, true);

            // "Harvest Date" Menu
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[1]/div/div[1]/div/div/div/div/div[2]/div/div[1]/div/div[1]/div/div[1]/div[4]/div[2]/div/div[1]/div/div[1]"), driver, true);

            // To Harvest Date
            gMethods.EnterKeys(By.XPath("/html/body"), driver, Keys.Control, Keys.Right, Keys.Left, true, 3);

            // "15"
            gMethods.FindAndClick(By.LinkText("15"), driver, true);

            // "Yield" Click
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[1]/div/div[1]/div/div/div/div/div[2]/div/div[1]/div/div[1]/div/div[1]/div[4]/div[1]/table/tbody/tr[2]/td[3]/div"), driver, true);

            // "Yield" Input
            gMethods.FindAndInput("235", By.XPath("//input[contains(@name,'HarvestYield')]"), driver, true, false);

            // "Straw %" Click
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[1]/div/div[1]/div/div/div/div/div[2]/div/div[1]/div/div[1]/div/div[1]/div[4]/div[1]/table/tbody/tr[2]/td[4]/div"), driver, true);

            // "Straw %" Input
            gMethods.FindAndInput("70", By.XPath("//input[contains(@name,'StrawStoverHayPct')]"), driver, true, false);

            // Random click to save
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[1]/div/div[1]/div/div/div/div/div[2]/div/div[1]/div/div[1]/div/div[1]/div[4]/div/table/tbody/tr[2]/td[1]/div"), driver, true);

            // "Next" button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[8]/span/a"), driver, true);
           
        }

        public void TillagePage()
        {
            // "Add New Tillage" button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[2]/div/div/div/div[2]/div/div/div[2]/div/div/em/button"), driver, true);
            // /html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[2]/div/div[1]/div/div[2]/div/div[1]/div[2]/div/div/em/button

            gMethods.AddDelay(500);

            // "Tillage Date" Box
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[2]/div/div[1]/div/div[2]/div/div[1]/div[4]/div/table/tbody/tr[2]/td[1]/div"), driver, true);

            // "Tillage Date" Menu
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[2]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[2]/div/div[1]/div/div[1]"), driver, true);

            gMethods.AddDelay(1000);

            // To Month
            if (gMethods.IsElementPresent(By.XPath("/html/body/div[21]/div/table"), driver))
            {
                gMethods.AddDelay(500);
                gMethods.EnterKeys(By.XPath("/html/body/div[21]/div/table"), driver, Keys.Control, Keys.Right, Keys.Left, true, 3);
            }
            else
            {
                gMethods.EnterKeys(By.XPath("/html/body/div[25]/div/table"), driver, Keys.Control, Keys.Right, Keys.Left, true, 3);
            }

            // "Day"
            if (gMethods.IsElementPresent(By.XPath("/html/body/div[21]/div/table/tbody/tr[*]/td[contains(@title,'April 30, 2000')]/a/em/span"), driver))
            {
                gMethods.FindAndClick(By.XPath("/html/body/div[21]/div/table/tbody/tr[*]/td[contains(@title,'April 30, 2000')]/a/em/span"), driver, true);
            }
            else
            {
                gMethods.FindAndClick(By.XPath("/html/body/div[25]/div/table/tbody/tr[*]/td[contains(@title,'April 30, 2000')]/a/em/span"), driver, true);
            }            

            // "Tillage Type"
            //gMethods.findAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[2]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[1]/table/tbody/tr[2]/td[2]/div"), driver, true);

            // "Tillage Type"
            //gMethods.findAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[2]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[3]/div/div[1]/div[2]/div[1]"), driver, true);

            // "Tillage Type"
            //gMethods.findAndClick(By.XPath("/html/body/div[23]/div/ul/li[2]"), driver, true);

            // Random click to save
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[2]/div/div[1]/div/div[2]/div/div[1]/div[4]/div/table/tbody/tr[2]/td[2]/div"), driver, true);

            // "Next" button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[8]/span/a/span[2]"), driver, true);
        }

        public void NitrogenPage()
        {
            gMethods.AddDelay(1000);

            // "Add New Nitrogen" button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[4]/div/div[1]/div/div[2]/div/div[1]/div[2]/div/div/em/button"), driver, true);

            gMethods.AddDelay(500);

            // "Nitrogen Date" Box
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[4]/div/div[1]/div/div[2]/div/div[1]/div[4]/div/table/tbody/tr[2]/td[1]/div"), driver, true);

            // "Nitrogen Date" Menu
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[4]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[2]/div/div[1]/div/div[1]"), driver, true);

            gMethods.AddDelay(500);

            // To Month
            gMethods.EnterKeys(By.XPath("/html/body"), driver, Keys.Control, Keys.Right, Keys.Left, true, 4);

            // "Day"
            if (gMethods.IsElementPresent(By.XPath("/html/body/div[26]/div/table/tbody/tr[*]/td[contains(@title,'May 01, 2000')]/a/em/span"), driver))
            {
                gMethods.FindAndClick(By.XPath("/html/body/div[26]/div/table/tbody/tr[*]/td[contains(@title,'May 01, 2000')]/a/em/span"), driver, true);
            }
            else
            {
                gMethods.FindAndClick(By.XPath("/html/body/div[22]/div/table/tbody/tr[*]/td[contains(@title,'May 01, 2000')]/a/em/span"), driver, true);
            }  

            // "Fertilizer" Click
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[4]/div/div[1]/div/div[2]/div/div[1]/div[4]/div/table/tbody/tr[2]/td[2]/div"), driver, true);

            // "Fertilizer" arrow Click
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[4]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[3]/div/div[1]/div[2]/div[1]"), driver, true);

            // "Fertilizer" Input
            gMethods.FindAndClick(By.XPath("//li[contains(.,'Urea')]"), driver, true);

            // "Total Nitrogen" Click
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[4]/div/div[1]/div/div[2]/div/div[1]/div[4]/div/table/tbody/tr[2]/td[3]/div"), driver, true);

            // "Total Nitrogen" Input
            gMethods.FindAndInput("200", By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[4]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[4]/div/div[1]/input"), driver, true, false);

            // "Application Method" Click
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[4]/div/div[1]/div/div[2]/div/div[1]/div[4]/div/table/tbody/tr[2]/td[4]/div"), driver, true);

            // "Application Method" arrow Click
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[4]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[5]/div/div[1]/div[2]/div[1]"), driver, true);

            // "Application Method" Input
            gMethods.FindAndClick(By.XPath("//li[contains(.,'Incorporate / Inject')]"), driver, true);

            // Random Click to save
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[4]/div/div[1]/div/div[2]/div/div[1]/div[4]/div/table/tbody/tr[2]/td[1]/div"), driver, true);

            // "Next" button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[8]/span/a/span[2]"), driver, true);
        }

        public void ManurePage()
        {

        }

        public void IrrigationPage()
        {
            // "Add New Irrigation" button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[2]/div/div/em/button"), driver, true);

            // "Irrigation Date" Box
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[4]/div/table/tbody/tr[2]/td[1]/div"), driver, true);

            // "Irrigation Date" Menu
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[2]/div/div[1]/div/div[1]"), driver, true);

            gMethods.AddDelay(500);

            // To Month
            gMethods.EnterKeys(By.XPath("/html/body"), driver, Keys.Control, Keys.Right, Keys.Left, true, 4);

            // "Day"
            if (gMethods.IsElementPresent(By.XPath("/html/body/div[34]/div/table/tbody/tr[*]/td[contains(@title,'May 01, 2000')]/a/em/span"), driver))
            {
                gMethods.FindAndClick(By.XPath("/html/body/div[34]/div/table/tbody/tr[*]/td[contains(@title,'May 01, 2000')]/a/em/span"), driver, true);
            }
            else
            {
                gMethods.FindAndClick(By.XPath("/html/body/div[30]/div/table/tbody/tr[*]/td[contains(@title,'May 01, 2000')]/a/em/span"), driver, true);
            }  
            // "Add New Irrigation" button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[2]/div/div/em/button"), driver, true);

            // "Irrigation Date" Box
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[1]/table/tbody/tr[3]/td[1]/div"), driver, true);

            // "Irrigation Date" Menu
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[2]/div/div[1]/div/div[1]"), driver, true);

            gMethods.AddDelay(500);

            // To Month
            gMethods.EnterKeys(By.XPath("/html/body"), driver, Keys.Control, Keys.Right, Keys.Left, true, 5);

            // "Day"
            gMethods.FindAndClick(By.XPath("//td[contains(@title,'June 30, 2000')]/a/em/span"), driver, true);

            // "Add New Irrigation" button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[2]/div/div/em/button"), driver, true);

            // "Irrigation Date" Box
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[1]/table/tbody/tr[4]/td[1]/div"), driver, true);

            // "Irrigation Date" Menu
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[2]/div/div[1]/div/div[1]"), driver, true);

            gMethods.AddDelay(500);

            // To Month
            gMethods.EnterKeys(By.XPath("/html/body"), driver, Keys.Control, Keys.Right, Keys.Left, true, 6);

            // "Day"
            gMethods.FindAndClick(By.XPath("//td[contains(@title,'July 31, 2000')]/a/em/span"), driver, true);

            // "Add New Irrigation" button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[2]/div/div/em/button"), driver, true);

            // "Irrigation Date" Box
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[1]/table/tbody/tr[5]/td[1]/div"), driver, true);

            // "Irrigation Date" Menu
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[2]/div/div[1]/div/div[1]"), driver, true);

            gMethods.AddDelay(500);

            // To Month
            gMethods.EnterKeys(By.XPath("/html/body"), driver, Keys.Control, Keys.Right, Keys.Left, true, 7);

            // "Day"
            gMethods.FindAndClick(By.XPath("//td[contains(@title,'August 31, 2000')]/a/em/span"), driver, true);

            // "Add New Irrigation" button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[2]/div/div/em/button"), driver, true);

            // "Irrigation Date" Box
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[1]/table/tbody/tr[6]/td[1]/div"), driver, true);

            // "Irrigation Date" Menu
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[2]/div/div[1]/div/div[1]"), driver, true);

            gMethods.AddDelay(500);

            // To Month
            gMethods.EnterKeys(By.XPath("/html/body"), driver, Keys.Control, Keys.Right, Keys.Left, true, 8);

            // "Day"
            gMethods.FindAndClick(By.XPath("//td[contains(@title,'September 30, 2000')]/a/em/span"), driver, true);

            // Random Click to save
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[3]/div/div[1]/div/div[2]/div/div[1]/div[4]/div/table/tbody/tr[2]/td[1]/div"), driver, true);

            // "Next" button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[8]/span/a/span[2]"), driver, true);
        }

        public void LimingPage()
        {

        }

        public void BurningPage()
        {

        }

        public void EndCrops(bool future)
        {
            if (future == true)
            {
                gMethods.AddDelay(8000);
                
            }

            // "Skip Ahead" button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[7]/span/a/span[2]"), driver, true);

            // "No Thanks, Continue" button
            gMethods.FindAndClick(By.Id("noThanksContinueButton"), driver, true);

            if (future != true)
            {
                int selectInt;
                string selectString;
                for (int i = 1; i < 2; i++) // For each parcel
                {
                    for (int j = 1; j < 16; j++) // For each year
                    {
                        selectInt = 2000 + j;
                        selectString = "copyRotationF" + i.ToString() + selectInt.ToString();

                        // Yearly checkboxes
                        gMethods.FindAndClick(By.Id(selectString), driver, true);
                    }
                }
            }

            gMethods.AddDelay(500);

            // "Copy & Continue" button
            gMethods.FindAndClick(By.Id("copyContinueButton"), driver, true);

            gMethods.AddDelay(500);

            if (future == true)
            {
                gMethods.AddDelay(1000);
                // "Continue to Report" button
                gMethods.FindAndClick(By.Id("nextStep"), driver, true);
            }

            else
            {
                // "Continue to Future Management" button
                gMethods.FindAndClick(By.Id("continueFuture"), driver, true);
            }
        }

    }
}
