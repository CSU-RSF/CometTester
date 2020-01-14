using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace CometTester.CFarm
{
    class CFarmXmlForestry
    {
        GeneralTestMethods gMethods;
        BrowserDriver driver;
        private CFarmXmlLister.CFarmForestryData xmlForestry;

        public CFarmXmlForestry(GeneralTestMethods gMethods, BrowserDriver driver, CFarmXmlLister.CFarmForestryData xmlForestry)
        {
            this.gMethods = gMethods;
            this.driver = driver;
            this.xmlForestry = xmlForestry;

            ParcelLocationsPage();
            StandManagement();
            EndForestry();
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

        public void ParcelLocationsPage()
        {
            int parcelCount = 1;
            foreach (var parcel in xmlForestry.ForestryParcelList)
            {
                // Find Location Button
                if (parcelCount != 1)
                {
                    gMethods.FindAndClick(By.XPath("//button[contains(@id, 'goToLoc')]"), driver, true);
                }
                parcelCount++;

                // "Address Query" box
                gMethods.FindAndInput(parcel.GpsCoordinates, By.XPath("//input[contains(@id,'locationName')]"), driver, true, false);

                // "Go to Location" button
                gMethods.FindAndClick(By.XPath("//input[contains(@id,'locationGo')]"), driver, true);

                WaitTillMask();

                gMethods.AddDelay(1000);

                if (parcel.Scroll != "")
                {
                    if (Int16.Parse(parcel.Scroll) > 0)
                    {
                        for (int i = 0; i < Int16.Parse(parcel.Scroll); i++)
                        {
                            gMethods.FindAndClick(By.XPath("//button[contains(@class,'ol-zoom-in')]"), driver, true);

                            gMethods.AddDelay(500);
                        }
                    }
                    if (Int16.Parse(parcel.Scroll) < 0)
                    {
                        for (int i = 0; i > Int16.Parse(parcel.Scroll); i--)
                        {
                            gMethods.FindAndClick(By.XPath("//button[contains(@class,'ol-zoom-out')]"), driver, true);

                            gMethods.AddDelay(500);
                        }
                    }
                }

                string resString = gMethods.ElementSize(By.XPath("//div[contains(@id,'olmap')]"), driver, true);
                var resSplit = resString.Split(new string[] { "{Width=", ", Height=", "}" }, StringSplitOptions.RemoveEmptyEntries);

                int resAdjustX = (Int32.Parse(resSplit[0]) - 1095) / 2;
                int resAdjustY = (Int32.Parse(resSplit[1]) - 715) / 2;

                if (parcel.DragAdjust != "")
                {
                    string[] dragOffset = parcel.DragAdjust.Split(',');

                    for (int i = 0; i < Int32.Parse(parcel.DragCount); i++)
                    {
                        gMethods.ClickOnPage(By.XPath("//div[contains(@class,'ol-viewport')]"), driver, Int32.Parse(dragOffset[0]) + resAdjustX, Int32.Parse(dragOffset[1]) + resAdjustY, "drag", Int32.Parse(dragOffset[2]), Int32.Parse(dragOffset[3]));
                    }
                }

                gMethods.AddDelay(500);

                // "Add Parcel by Point/Polygon" button

                if (parcel.PointAcres != "")
                {
                    gMethods.FindAndClick(By.Id("addPointButton"), driver, true);

                    string[] pointOffset = parcel.PointAdjust.Split(',');

                    gMethods.ClickOnPage(By.Id("OpenLayers.Map_5_events"), driver, Int32.Parse(pointOffset[0]) + resAdjustX, Int32.Parse(pointOffset[1]) + resAdjustY, "single", 0, 0);

                    gMethods.AddDelay(1000);

                    // Area Input
                    gMethods.FindAndInput(parcel.PointAcres, By.XPath("//div[contains(@id,'area')]/div[1]/input"), driver, true, false);
                }
                else
                {
                    gMethods.FindAndClick(By.Id("addPolygonStandButton"), driver, true);

                    string[] pointOffset = parcel.PolygonVertices.Split(',');

                    for (int i = 0; i < pointOffset.Length - 2; i += 2)
                    {
                        gMethods.ClickOnPage(By.Id("olmap"), driver, Int32.Parse(pointOffset[i]) + resAdjustX, Int32.Parse(pointOffset[i + 1]) + resAdjustY, "single", 0, 0);
                    }

                    gMethods.ClickOnPage(By.Id("olmap"), driver, Int32.Parse(pointOffset[0]) + resAdjustX, Int32.Parse(pointOffset[1]) + resAdjustY, "single", 0, 0);
                    //gMethods.ClickOnPage(By.Id("olmap"), driver, Int32.Parse(pointOffset[pointOffset.Length - 2]) + resAdjustX, Int32.Parse(pointOffset[pointOffset.Length - 1]) + resAdjustY, "double", 0, 0);
                }

                // "Save" button
                gMethods.FindAndClick(By.Id("locationAttSave"), driver, true);

                gMethods.AddDelay(10000);
            }

            // "I am done defining parcels" button
            gMethods.FindAndClick(By.Id("doneStandsButton"), driver, true);

        }

        public void StandManagement()
        {
            foreach (var parcel in xmlForestry.ForestryParcelList)
            {
                // "Forest Type" button
                //gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/table/tbody/tr/td/div/div/div/div[1]/div[1]/div[2]/div[1]"), driver, true);
                //gMethods.FindAndClick(By.Id("management_pre_1980"), driver, true);

                //   gMethods.AddDelay(500);

                // "Forest Type" Selection
                gMethods.FindAndClick(By.Id("ForestryTypes"), driver, true);
                gMethods.AddDelay(500);

                //Commented out for Comprehensive Testing
                //CHANGE BACK FOR OTHER TESTS
                gMethods.FindAndClick(By.XPath("//li[contains(., '" + parcel.ForestryManagement.ForestType + "')]"), driver, true);

                // "Past Land Cover" Selection
                gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/table/tbody/tr/td/div/div/div/div[2]/div[1]/div/div[1]/div/div/div[1]/div[1]/div[1]/input"), driver, true);

                // Age Input box
                gMethods.FindAndInput(parcel.ForestryManagement.Age, By.XPath("/html/body/div[2]/div[4]/table/tbody/tr/td/div/div/div/div[3]/div[1]/input"), driver, true, false);

                if (parcel.ForestryManagement.Age == "")
                {
                    // Volume Input box
                    gMethods.FindAndInput(parcel.ForestryManagement.Volume, By.XPath("/html/body/div[2]/div[4]/table/tbody/tr/td/div/div/div/div[4]/div[1]/input"), driver, true, false);
                }

                // "Prescription" Button
                //gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/table/tbody/tr/td/div/div/div/div[5]/div[1]/div[2]/div[1]"), driver, true);
                gMethods.FindAndClick(By.Id("prescription-triggerWrap"), driver, true);

                // "Prescription" Selection
                gMethods.FindAndClick(By.XPath("//li[contains(., 'Grow only, no management activity')]"), driver, true);

                // "Next" Button
                gMethods.FindAndClick(By.Id("nextlivestockbutton"), driver, true);
            }
        }

        public void EndForestry()
        {
            gMethods.AddDelay(500);

            // "Continue to Report" Button
            gMethods.FindAndClick(By.XPath("//a/span[contains(text(), 'Continue to Report')]"), driver, true);
        }
    }
}
