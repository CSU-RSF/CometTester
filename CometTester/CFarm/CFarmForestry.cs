using OpenQA.Selenium;

namespace CometTester.CFarm
{
    class CFarmForestry
    {
        GeneralTestMethods gMethods;
        BrowserDriver driver;

        public CFarmForestry(GeneralTestMethods gMethods, BrowserDriver driver)
        {
            this.gMethods = gMethods;
            this.driver = driver;

            ParcelLocationsPage();
            StandManagement();
            EndForestry();
        }

        public void ParcelLocationsPage()
        {
            // "Address Query" box
            gMethods.FindAndInput("34.351406, -89.621763", By.XPath("//div[contains(@id,'addressText')]/div[1]/input"), driver, true, false);

            // "Go to Location" button
            gMethods.FindAndClick(By.XPath("/html/body/div[10]/div[2]/div/div[1]/div[3]/div/span/a"), driver, true);

            gMethods.AddDelay(500);

            gMethods.ClickOnPage(By.Id("OpenLayers.Layer.Vector_53_svgRoot"), driver, 400, 100, "drag", 100, 100);

            gMethods.AddDelay(500);

            //gMethods.clickOnPage(By.Id("OpenLayers.Layer.Vector_53_svgRoot"), driver, 400, 400, "drag", -400, -400);

            //gMethods.addDelay(500);

            //gMethods.clickOnPage(By.Id("OpenLayers.Layer.Vector_53_svgRoot"), driver, 400, 400, "drag", -400, -400);

            // "Add Parcel by Polygon" button
            gMethods.FindAndClick(By.Id("addPolygon"), driver, true);

            gMethods.ClickOnPage(By.Id("OpenLayers.Map_5_events"), driver, 900, 130, "single", 0, 0);

            gMethods.AddDelay(700);

            gMethods.ClickOnPage(By.Id("OpenLayers.Map_5_events"), driver, 960, 400, "single", 0, 0);

            gMethods.AddDelay(700);

            gMethods.ClickOnPage(By.Id("OpenLayers.Map_5_events"), driver, 720, 480, "single", 0, 0);

            gMethods.AddDelay(700);

            gMethods.ClickOnPage(By.Id("OpenLayers.Map_5_events"), driver, 430, 345, "single", 0, 0);

            gMethods.AddDelay(700);

            gMethods.ClickOnPage(By.Id("OpenLayers.Map_5_events"), driver, 430, 105, "double", 0, 0);

            gMethods.AddDelay(700);

            // "Save" button
            gMethods.FindAndClick(By.Id("button-1073-btnEl"), driver, true);

            gMethods.AddDelay(1000);

            // "I am done defining parcels" button
            gMethods.FindAndClick(By.Id("nextparcelbutton"), driver, true);

        }

        public void StandManagement()
        {
            // "Forest Type" button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/table/tbody/tr/td/div/div/div/div[1]/div[1]/div[2]/div[1]"), driver, true);

            gMethods.AddDelay(500);

            // "Forest Type" Selection
            gMethods.FindAndClick(By.XPath("//li[contains(., 'Oak-hickory')]"), driver, true);

            // "Past Land Cover" Selection
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/table/tbody/tr/td/div/div/div/div[2]/div[1]/div/div[1]/div/div/div[1]/div[1]/div[1]/input"), driver, true);

            // Age Input box
            gMethods.FindAndInput("50", By.XPath("/html/body/div[2]/div[4]/table/tbody/tr/td/div/div/div/div[3]/div[1]/input"), driver, true, false);

            // "Prescription" Button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/table/tbody/tr/td/div/div/div/div[5]/div[1]/div[2]/div[1]"), driver, true);

            // "Prescription" Selection
            gMethods.FindAndClick(By.XPath("//li[contains(., 'Grow only, no management activity')]"), driver, true);

            // "Next" Button
            gMethods.FindAndClick(By.Id("nextlivestockbutton"), driver, true);
        }

        public void EndForestry()
        {
            gMethods.AddDelay(500);

            // "Continue to Report" Button
            gMethods.FindAndClick(By.XPath("/html/body/div[12]/div[2]/div/span[2]/a/span[2]"), driver, true);
        }
    }
}
