using System;
using OpenQA.Selenium;
using System.Linq;
using OpenQA.Selenium.Support.UI;

namespace CometTester.CFarm
{
    class CFarmXmlCroplands
    {
        GeneralTestMethods gMethods;
        BrowserDriver driver;
        CFarmXmlLister.CFarmCroplandsData xmlCroplands;
        bool harvestCreated;
        bool nitrogenCreated;
        bool tillageCreated;





        public CFarmXmlCroplands(GeneralTestMethods gMethods, BrowserDriver driver, CFarmXmlLister.CFarmCroplandsData xmlCroplands)
        {
            this.gMethods = gMethods;
            this.driver = driver;
            this.xmlCroplands = xmlCroplands;
            harvestCreated = false;
            nitrogenCreated = false;
            tillageCreated = false;

            ParcelLocationsPage();
            HistoricManagementPage();
            CurrentManagement();

            foreach (var scenario in xmlCroplands.CroplandsScenarioList)
            {
                if (scenario.Name != "Baseline")    //Creating a crop scenario isn't avialable until after the Baseline is complete
                {
                    CreateCropScenario(scenario);
                    if (scenario.CopyFromBaseline != true)    //If a future scenario copies the baseline, input data is already complete
                    {
                        foreach (var scenarioData in scenario.ScenarioDataList)
                        {
                            if (scenario.ScenarioDataList.IndexOf(scenarioData) != 0)
                            {
                                // "Ok" button for unfinished scenario
                                //gMethods.FindAndClick(By.XPath("//div[contains(@id,'messagebox')]/div[3]/div/div/em/button"), driver, true);
                            }

                            // Following methods should align with page names
                            CropAndPlant(scenarioData);
                            TillagePage(scenarioData);
                            NitrogenPage(scenarioData);
                            ManurePage(scenarioData);
                            IrrigationPage(scenarioData);
                            LimingPage(scenarioData);
                            BurningPage(scenarioData);
                            CopyCrop(scenarioData, scenario.Name);
                        }
                    }
                    else
                    {
                        WaitTillMask();
                        gMethods.AddDelay(5000);

                        // "Skip Ahead" button
                        gMethods.FindAndClick(By.XPath("//span[contains(.,'Skip Ahead >>')]"), driver, true);

                        CopyCrop(null, null);
                    }

                    EndScenario(scenario);
                }
           
            }
            EndCroplands();
        }


        public void WaitTillMask() {
        //cometfarm is a bad program
        //we make a new x-mask-loading every time we get a loading mask
        //so we want to make sure none of these are visible.

            bool Cont= false;
            while (!Cont)
            {

                var AllLoadings = driver.Driver.FindElements(By.XPath("//div[contains(@id,'loadmask-')]"));
                Cont = true;
                foreach (var element in AllLoadings)
                {

                    var b=element.GetAttribute("style").ToLower();
                    if (!b.Contains("display: none"))
                    {
                        gMethods.AddDelay(500);
                        Cont = false;
                        break;
                    }


                }
         
            }

   

        }

        public void EndCurrent()
        {


            try
            {
                WaitTillMask();
                gMethods.FindAndClick(By.Id("nextmanagementbutton"), driver, true); //here is where the error happens
                WaitTillMask();
                gMethods.FindAndClick(By.Id("noThanksContinueButton"), driver, true);//stuck here
                WaitTillMask();
                gMethods.FindAndClick(By.XPath("//span[contains(.,'No, thanks>>')]"), driver, true);
                WaitTillMask();
                gMethods.FindAndClick(By.Id("continueFuture"), driver, true);
                WaitTillMask();
            }
            catch
            {
                WaitTillMask();
                gMethods.FindAndClick(By.Id("nextmanagementbutton"), driver, true); //here is where the error happens
                WaitTillMask();
                gMethods.FindAndClick(By.Id("noThanksContinueButton"), driver, true);//stuck here
                WaitTillMask();
                gMethods.FindAndClick(By.XPath("//span[contains(.,'No, thanks>>')]"), driver, true);
                WaitTillMask();
                gMethods.FindAndClick(By.Id("continueFuture"), driver, true);
                WaitTillMask();
            }




        }

        public void CurrentManagement()
        {
            foreach (var scenario in xmlCroplands.CroplandsScenarioList)
            {
                if (scenario.Name == "Baseline")    //Creating a crop scenario isn't avialable until after the Baseline is complete
                {
                    foreach (var scenarioData in scenario.ScenarioDataList)
                    {
                        if (scenario.ScenarioDataList.IndexOf(scenarioData) != 0)
                        {
                            // "Ok" button for unfinished scenario
                            //gMethods.FindAndClick(By.XPath("//div[contains(@id,'messagebox')]/div[3]/div/div/em/button"), driver, true);
                        }

                        // Following methods should align with page names
                        CropAndPlant(scenarioData);
                        TillagePage(scenarioData);
                        NitrogenPage(scenarioData);
                        ManurePage(scenarioData);
                        IrrigationPage(scenarioData);
                        LimingPage(scenarioData);
                        BurningPage(scenarioData);
                        CopyCrop(scenarioData, scenario.Name);
                    }

          
                    EndCurrent();
                    return;
                }

            }

            }


        public void ParcelLocationsPage()
        {
            foreach (var parcel in xmlCroplands.Parcels)
            {
                // Find Location Button
                if (xmlCroplands.Parcels.IndexOf(parcel) != 0)
                {
                    gMethods.FindAndClick(By.XPath("//button[contains(@id, 'goToLoc')]"), driver, true);
                }

                // "Address Query" box
                gMethods.FindAndInput(parcel.GpsCoordinates, By.XPath("//input[contains(@id,'locationName')]"), driver, true, false);

                // "Go to Location" button
                gMethods.FindAndClick(By.XPath("//input[contains(@id,'locationGo')]"), driver, true);

                WaitTillMask();

                gMethods.AddDelay(1000);

                // Zooms in or out depending on integer sign
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

                // The assumed screen resolution is 1280x1024. The resulting parcel locations map element is 1095x715. ResAdjust accounts for differing screen sizes.
                string resString = gMethods.ElementSize(By.XPath("//div[contains(@id,'olmap')]"), driver, true);        // Grabs the visible map size in format {Width=#, Height=#}
                var resSplit = resString.Split(new string[] { "{Width=", ", Height=", "}" }, StringSplitOptions.RemoveEmptyEntries);

                int resAdjustX = (Int32.Parse(resSplit[0]) - 1055) / 2;
                int resAdjustY = (Int32.Parse(resSplit[1]) - 634) / 2;

                if (parcel.DragAdjust != "")
                {
                    string[] dragOffset = parcel.DragAdjust.Split(',');     // [xDrag], [yDrag]

                    gMethods.AddDelay(1500);


                    for (int i = 0; i < Int32.Parse(parcel.DragCount); i++)
                    {
                        gMethods.AddDelay(200);
                  //      gMethods.ClickOnPage(By.XPath("//div[contains(@class,'ol-viewport')]"), driver, Int32.Parse(dragOffset[0]) + resAdjustX, Int32.Parse(dragOffset[1]) + resAdjustY, "drag", Int32.Parse(dragOffset[2]) + resAdjustX, Int32.Parse(dragOffset[3]) + resAdjustY);
                       
                    }
                }

                gMethods.AddDelay(1000);

                // "Add Parcel by Point/Polygon" button
                if (parcel.PointAcres != "")
                {
                    gMethods.FindAndClick(By.Id("addPointButton"), driver, true);

                    string[] pointOffset = parcel.PointAdjust.Split(',');

                    gMethods.ClickOnPage(By.XPath("//div[contains(@class,'ol-viewport')]"), driver, Int32.Parse(pointOffset[0]) + resAdjustX, Int32.Parse(pointOffset[1]) + resAdjustY, "single", 0, 0);

                    gMethods.AddDelay(1000);

                    // Area Input
                    gMethods.FindAndInput(parcel.PointAcres, By.XPath("//div[contains(@id,'locationAttArea')]/input"), driver, true, false);
                }
                else
                {
                    gMethods.FindAndClick(By.Id("addPolygonButton"), driver, true);

                    string[] pointOffset = parcel.PolygonVertices.Split(',');

                    for (int i = 0; i < pointOffset.Length - 1; i += 2)
                    {
                        gMethods.ClickOnPage(By.Id("olmap"), driver, Int32.Parse(pointOffset[i]) + resAdjustX, Int32.Parse(pointOffset[i + 1]) + resAdjustY, "single", 0, 0);
                   }

                    gMethods.ClickOnPage(By.Id("olmap"), driver, Int32.Parse(pointOffset[0]) + resAdjustX, Int32.Parse(pointOffset[1]) + resAdjustY, "single", 0, 0);
                    //gMethods.ClickOnPage(By.Id("olmap"), driver, Int32.Parse(pointOffset[pointOffset.Length - 2]) + resAdjustX, Int32.Parse(pointOffset[pointOffset.Length - 1]) + resAdjustY, "double", 0, 0);
                }

                gMethods.AddDelay(1000);


                // "Save" button
                //SaveClick(1000,0);
                gMethods.FindAndClick(By.Id("locationAttSave"), driver, true);

                gMethods.AddDelay(1000);
                WaitTillMask();
            }

            // "I am done defining parcels" button
            gMethods.FindAndClick(By.Id("doneParcelsButton"), driver, true);

        }

        public void HistoricManagementPage()
        {
            foreach (var parcel in xmlCroplands.Parcels)
            {
                //delay
                gMethods.AddDelay(2000);
                WaitTillMask();
                // "Pre-1980 Management"
                gMethods.FindAndClick(By.XPath("/html/body/div[2]/form/div[2]/div[2]/div[2]/select/option[contains(.,'" + parcel.HistoricManagement.Pre1980Management + "')]"), driver, true);

                if (parcel.HistoricManagement.Crp)
                {
                    // "CRP"
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/form/div[2]/div[3]/span[2]"), driver, true);

                    // "CRP Start Year" box
                    gMethods.FindAndInput(parcel.HistoricManagement.CrpStartYear, By.XPath("/html/body/div[2]/form/div[2]/div[4]/div/input"), driver, true, false);

                    // "CRP End Year" box
                    gMethods.FindAndInput(parcel.HistoricManagement.CrpEndYear, By.XPath("/html/body/div[2]/form/div[2]/div[4]/div[2]/input"), driver, true, false);

                    // "CRP Type"
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/form/div[2]/div[4]/div[3]/div[2]/select/option[contains(.,'" + parcel.HistoricManagement.CrpType + "')]"), driver, true);

                    // "Pre-CRP Management"
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/form/div[2]/div[5]/div[2]/select/option[contains(.,'" + parcel.HistoricManagement.PreCrpManagement + "')]"), driver, true);

                    // "Pre-CRP Tillage"
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/form/div[2]/div[6]/div[2]/select/option[contains(.,'" + parcel.HistoricManagement.PreCrpTillage + "')]"), driver, true);

                    // "Post-CRP Management"
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/form/div[2]/div[7]/div[1]/div[2]/select/option[contains(.,'" + parcel.HistoricManagement.PostCrpManagement + "')]"), driver, true);

                    // "Post-CRP Tillage"
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/form/div[2]/div[7]/div[2]/div[2]/select/option[contains(.,'" + parcel.HistoricManagement.PostCrpTillage + "')]"), driver, true);
                }
                else
                {
                    // "1980-2000 Management"
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/form/div[2]/div[5]/div[2]/select/option[contains(.,'" + parcel.HistoricManagement.To2000Management + "')]"), driver, true);

                    // "1980-2000 Tillage"
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/form/div[2]/div[6]/div[2]/select/option[contains(.,'" + parcel.HistoricManagement.To2000Tillage + "')]"), driver, true);

          
                    WaitTillMask();
                    // "Next" button
                    gMethods.FindAndClick(By.Id("nexthistoricbutton"), driver, true);
                }
            }
        }

        public void CreateCropScenario(CFarmXmlLister.CroplandsScenario scenario)
        {
            if (xmlCroplands.CroplandsScenarioList.IndexOf(scenario) > 1)
            {
                // Scenario input for future scenarios after Future 1
                gMethods.FindAndInput(scenario.Name, By.XPath("/html/body/div[contains(@id,'nextScenarioWindow')]/div[2]/div[1]/div/div/div[2]/div[2]/div[1]/div[1]/input"), driver, true, false);

                if (scenario.CopyFromBaseline)
                {
                    // "Copy" checkbox
                    gMethods.FindAndClick(By.XPath("/html/body/div[contains(@id,'nextScenarioWindow')]/div[2]/div[1]/div/div/div[2]/div[2]/div[2]/div[1]/input"), driver, true);
                }

                WaitTillMask();
     
                // "Start" button
                gMethods.FindAndClick(By.XPath("//span[contains(.,'Start >>')]"), driver, true);

               
                WaitTillMask();

            }
            else
            {
                // Scenario input
                gMethods.FindAndInput(scenario.Name, By.XPath("/html/body/div[contains(@id,'newScenarioWindow')]/div[2]/div[1]/div/div/div[2]/div[2]/div[1]/div[1]/input"), driver, true, false);

                if (scenario.CopyFromBaseline)
                {
                    // "Copy" checkbox
                    gMethods.FindAndClick(By.XPath("/html/body/div[contains(@id,'newScenarioWindow')]/div[2]/div[1]/div/div/div[2]/div[2]/div[2]/div[1]/input"), driver, true);
                }

                WaitTillMask();

                // "Start" button
                gMethods.FindAndClick(By.XPath("//span[contains(.,'Start >>')]"), driver, true);

                WaitTillMask();
            }
        }

        public void CropAndPlant(CFarmXmlLister.ScenarioData scenarioData)
        {

            gMethods.AddDelay(500);
            WaitTillMask();


            //Clicks crop rotation when applicable
            try
                {
                    var promptWindow = driver.Driver.FindElement(By.XPath("//a[contains(@id, 'getBurnButton')]"));
                    if (promptWindow.Displayed)
                    {
                    WaitTillMask();
                    gMethods.FindAndClick(By.XPath("//a[contains(@id, 'getBurnButton')]"), driver, true);
                    }
                }
                catch
                {}

            // Tries to click the "ok" button after continuing onto another parcel
            try
            {
                var sequenceElement = driver.Driver.FindElement(By.XPath("//div[contains(@id,'managementComplete')]/div[2]/div/div[2]/span/a/span[2]"));
                if (sequenceElement.Displayed)
                {
                    WaitTillMask();
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'managementComplete')]/div[2]/div/div[2]/span/a/span[2]"), driver, true);
                }
            }
            catch
            {}

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

            // "Crop" Selection
            gMethods.FindAndClick(By.XPath("/html/body/div[contains(@id,'boundlist')]/div/ul/li[contains(.,'" + scenarioData.CropAndPlant.CropType + "')]"), driver, true);

            gMethods.AddDelay(500);

            // "Plant Date" Calender Open
            gMethods.FindAndClick(By.XPath("//div[@id='plantingDate-triggerWrap']"), driver, true);

            // To Month
            gMethods.EnterKeys(By.XPath("/html/body"), driver, Keys.Control, Keys.Right, Keys.Left, MonthClicks(scenarioData.CropAndPlant.PlantingDate, false)[0] - 2);

            // To Day
            //gMethods.FindAndClick(By.XPath("/html/body/div[19]/div/table/tbody/tr[*]/td[contains(@title,'" + scenarioData.CropAndPlant.PlantingDate + "')]/a/em/span"), driver, true);
            DateClick(scenarioData.CropAndPlant.PlantingDate, 0);

            int lastMonth = MonthClicks(scenarioData.CropAndPlant.PlantingDate, true)[0] + 2;
            int lastDay = MonthClicks(scenarioData.CropAndPlant.PlantingDate, true)[1];
            int harvestCount = 0;
            foreach (var harvest in scenarioData.CropAndPlant.HarvestList)
            {
                // "Add New Harvest" button
                gMethods.FindAndClick(By.Id("addHarvest"), driver, true);

                // "Harvest Date" Box
                // gMethods.FindAndMultiClick("", By.XPath("//div[contains(@id,'row" + harvestCount + "jqxGridHarvest')]/div[1]/div"), driver, true, 2, false);

                if (!harvestCreated)
                {
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + harvestCount + "jqxGridHarvest')]/div[1]/div"), driver, true);
                    harvestCreated = true;
                }

                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + harvestCount + "jqxGridHarvest')]/div[1]/div"), driver, true);


                // "Harvest Date" Menu
                //gMethods.FindAndClick(By.XPath("//div[contains(@id,'datetimeeditorjqxGridHarvestShowDate')]/div/div/div"), driver, true);

                // To Month
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Left, null, 2);

                // Enter Harvest Month
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(harvest.Date, false)[0] - lastMonth);    // # of Clicks: (harvest month #) - (planting month #) - 2

                // To Day
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Right, null, 1);

                // Enter Harvest Day
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(harvest.Date, false)[1] - lastDay);    // # of Clicks: (harvest month #) - (planting month #) - 2

                // "Day"
                //gMethods.FindAndClick(By.XPath("/html/body/div[20]/div/table/tbody/tr[*]/td[contains(@title,'" + harvest.Date + "')]/a/em/span"), driver, true);
                //DateClick(harvest.Date, 0);

                if (!harvest.Grain)
                {
                    // "Grain" Click
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + harvestCount + "jqxGridHarvest')]/div[2]/div/div/div"), driver, true); 
                }

                // "Yield" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + harvestCount + "jqxGridHarvest')]/div[3]/div"), driver, true);

                // "Yield" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + harvestCount + "jqxGridHarvest')]/div[3]/div"), driver, true);

                // "Yield" Input
                gMethods.FindAndInput(harvest.Yield, By.XPath("//input[contains(@id,'textboxeditorjqxGridHarvestHarvestYield')]"), driver, true, false);

                // "Straw %" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + harvestCount + "jqxGridHarvest')]/div[4]/div"), driver, true);

                // "Straw %" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + harvestCount + "jqxGridHarvest')]/div[4]/div"), driver, true);

                // "Straw %" Input
                gMethods.FindAndInput(harvest.Straw, By.XPath("//input[contains(@id,'textboxeditorjqxGridHarvestStrawStoverHayPct')]"), driver, true, false);

                // Random click to save
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + harvestCount + "jqxGridHarvest')]/div[1]"), driver, true);

                lastMonth = MonthClicks(harvest.Date, true)[0];
                lastDay = MonthClicks(harvest.Date, true)[1];
                harvestCount++;
            }

            lastMonth = MonthClicks(scenarioData.CropAndPlant.PlantingDate, true)[0] + 2;
            lastDay = MonthClicks(scenarioData.CropAndPlant.PlantingDate, true)[1];
            int grazingCount = 0;
            foreach (var grazing in scenarioData.CropAndPlant.GrazingList)
            {
                // "Add New Grazing" button
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'grazingGrid')]/div[2]/div/div/em/button"), driver, true);

                // "Grazing StartDate" Box
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'grazingGrid-body')]/div/table/tbody/tr[" + (grazingCount + 2) + "]/td[contains(@class,'GrazingStart')]/div"), driver, true);

                // "Grazing StartDate" Menu
                //gMethods.FindAndClick(By.XPath("//div[contains(@id,'grazingGrid-body')]/div[2]/div/div/div/div"), driver, true);

                // To Grazing StartDate
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(grazing.StartDate, false)[0] - lastMonth);    // # of Clicks: (harvest month #) - (planting month #) - 2

                // To Day
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Right, null, 1);

                // To Grazing StartDate
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(grazing.StartDate, false)[0] - lastDay);    // # of Clicks: (harvest month #) - (planting month #) - 2

                // "Grazing StartDate Day"
                //gMethods.FindAndClick(By.XPath("/html/body/div[20]/div/table/tbody/tr[*]/td[contains(@title,'" + harvest.Date + "')]/a/em/span"), driver, true);
                //DateClick(grazing.StartDate, 0);

                // "Grazing EndDate" Box
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'grazingGrid-body')]/div/table/tbody/tr[" + (+2) + "]/td[contains(@class,'GrazingEnd')]/div"), driver, true);

                // "Grazing EndDate" Menu
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'grazingGrid-body')]/div[3]/div/div/div/div"), driver, true);

                // To Grazing EndDate
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(grazing.EndDate, false)[0] - lastMonth);    // # of Clicks: (harvest month #) - (planting month #) - 2

                // To Day
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Right, null, 1);

                // To Grazing EndDate
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(grazing.EndDate, false)[1] - lastDay);    // # of Clicks: (harvest month #) - (planting month #) - 2

                // "Grazing EndDate Day"
                //gMethods.FindAndClick(By.XPath("/html/body/div[20]/div/table/tbody/tr[*]/td[contains(@title,'" + harvest.Date + "')]/a/em/span"), driver, true);
                DateClick(grazing.EndDate, 0);

                // "RestPeriod" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'grazingGrid-body')]/div/table/tbody/tr[" + (grazingCount + 2) + "]/td[contains(@class,'RestPeriod')]/div"), driver, true);

                // "RestPeriod" Input
                gMethods.FindAndInput(grazing.RestPeriod, By.XPath("//div[contains(@id,'grazingGrid-body')]/div[4]/div/div/input"), driver, true, false);

                // "Utilization %" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'grazingGrid-body')]/div/table/tbody/tr[" + (grazingCount + 2) + "]/td[contains(@class,'UtilizationPct')]/div"), driver, true);

                // "Utilization %" Input
                gMethods.FindAndInput(grazing.Utilization, By.XPath("//div[contains(@id,'grazingGrid-body')]/div[5]/div/div/input"), driver, true, false);

                // Random click to save
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'grazingGrid-body')]"), driver, true);

                lastMonth = MonthClicks(grazing.EndDate, true)[0];
                lastDay = MonthClicks(grazing.EndDate, true)[1];
                grazingCount++;
            }

            NextButton();
        }

        public void TillagePage(CFarmXmlLister.ScenarioData scenarioData)
        {
            int lastMonth = 1;
            int lastDay = 1;
            int tillageCount = 0;
            foreach (var tillage in scenarioData.TillageList)
            {
                // "Add New Tillage" button
                gMethods.FindAndClick(By.Id("addTillage"), driver, true);

                gMethods.AddDelay(500);
                if (!tillageCreated)
                {
                    // "Tillage Date" Box
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + tillageCount + "jqxGridTillage')]/div/div"), driver, true);

                    tillageCreated = true;
                }
               
                // "Tillage Date" Box
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + tillageCount + "jqxGridTillage')]/div/div"), driver, true);

                // "Tillage Date" Menu
                //gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[4]/div/div[2]/div[2]/div/div[1]/div/div[2]/div/div[1]/div[4]/div[2]/div/div[1]/div/div[1]"), driver, true);

                WaitTillMask();
                // To Month
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Left, null, 2);

                // To Month
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(tillage.Date, false)[0] - lastMonth);

                // To Day
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Right, null, 1);

                // To Day
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(tillage.Date, false)[1] - lastDay);

                // "Day"
                //gMethods.FindAndClick(By.XPath("/html/body/div[21]/div/table/tbody/tr[*]/td[contains(@title,'" + tillage.Date + "')]/a/em/span"), driver, true);
                //DateClick(tillage.Date, 0);

                // Tillage Type
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + tillageCount + "jqxGridTillage')]/div[2]/div"), driver, true);

                // Tillage Type
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + tillageCount + "jqxGridTillage')]/div[2]/div"), driver, true);

                // Tillage drop down arrow
                gMethods.FindAndClick(By.Id("dropdownlistArrowdropdownlisteditorjqxGridTillageTillage"), driver, true);

                gMethods.AddDelay(200);

                // Tillage Type selection
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'listBoxContentinnerListBoxdropdownlisteditorjqxGridTillageTillage')]/div/div[*]/span[contains(.,'" + tillage.Type + "')]"), driver, true);

                // Random click to save
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + tillageCount + "jqxGridTillage')]/div/div"), driver, true);

                lastMonth = MonthClicks(tillage.Date, true)[0];
                lastDay = MonthClicks(tillage.Date, true)[1] + 1;
                tillageCount++;
            }

            NextButton();
        }

        public void NitrogenPage(CFarmXmlLister.ScenarioData scenarioData)
        {
            gMethods.AddDelay(500);

            int lastMonth = 1;
            int lastDay = 1;
            int nitrogenCount = 0;
            foreach (var nitrogen in scenarioData.NitrogenApplicationList)
            {
                // "Add New Nitrogen" button
                gMethods.FindAndClick(By.Id("addNitrogen"), driver, true);

                gMethods.AddDelay(500);

                // "Nitrogen Date" Box
                if (!nitrogenCreated)
                {
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + nitrogenCount + "jqxGridNitrogen')]/div/div"), driver, true);
                    nitrogenCreated = true;
                }
                // "Nitrogen Date" Menu
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + nitrogenCount + "jqxGridNitrogen')]/div/div"), driver, true);

                gMethods.AddDelay(500);

                // To Month
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Left, null, 2);

                // To Month
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(nitrogen.Date, false)[0] - lastMonth); // (irrigation month #) - (previous irrigation month #)

                // To Day
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Right, null, 1);

                // To Month
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(nitrogen.Date, false)[1] - lastDay);

                // "Day"
                //gMethods.FindAndClick(By.XPath("/html/body/div[22]/div/table/tbody/tr[*]/td[contains(@title,'" + nitrogen.Date + "')]/a/em/span"), driver, true);
                //DateClick(nitrogen.Date, 0);

                // "Fertilizer" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + nitrogenCount + "jqxGridNitrogen')]/div[2]/div"), driver, true);

                // "Fertilizer" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + nitrogenCount + "jqxGridNitrogen')]/div[2]/div"), driver, true);

                // "Fertilizer" arrow Click
                gMethods.FindAndClick(By.Id("dropdownlistWrapperdropdownlisteditorjqxGridNitrogenFertilizerType"), driver, true);

                gMethods.AddDelay(200);

                if (nitrogen.Type == "Urea (46-00-00)")
                {
                    gMethods.EnterKeys(By.XPath("//div[contains(@id, 'listBoxContentinnerListBoxdropdownlisteditorjqxGridNitrogenFertilizerType')]"), driver, null, Keys.Down, null, 25);
                }

                // "Fertilizer" Input
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'listBoxContentinnerListBoxdropdownlisteditorjqxGridNitrogenFertilizerType')]/div/div[contains(.,'" + nitrogen.Type + "')]"), driver, true);

                // "Total Nitrogen" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + nitrogenCount + "jqxGridNitrogen')]/div[3]/div"), driver, true);

                // "Total Nitrogen" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + nitrogenCount + "jqxGridNitrogen')]/div[3]/div"), driver, true);

                // "Total Nitrogen" Input
                gMethods.FindAndInput(nitrogen.TotalApplied, By.Id("textboxeditorjqxGridNitrogenTotalApplied"), driver, true, false);

                // "Application Method" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + nitrogenCount + "jqxGridNitrogen')]/div[4]/div"), driver, true);

                // "Application Method" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + nitrogenCount + "jqxGridNitrogen')]/div[4]/div"), driver, true);

                // "Application Method" arrow Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'dropdownlistArrowdropdownlisteditorjqxGridNitrogenApplicationMethod')]/div"), driver, true);

                gMethods.AddDelay(200);

                // "Application Method" Input
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'listBoxContentinnerListBoxdropdownlisteditorjqxGridNitrogenApplicationMethod')]/div/div[*]/span[contains(.,'" + nitrogen.Method + "')]"), driver, true);

                if (nitrogen.Eep != "")
                {
                    // "EEP" Click
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + nitrogenCount + "jqxGridNitrogen')]/div[5]/div"), driver, true);

                    // "EEP" Click
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + nitrogenCount + "jqxGridNitrogen')]/div[5]/div"), driver, true);

                    // "EEP" arrow Click
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'dropdownlistArrowdropdownlisteditorjqxGridNitrogenEnhancedEfficiencyProduct')]/div"), driver, true);

                    gMethods.AddDelay(200);

                    // "EEP" Input
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'listBoxContentinnerListBoxdropdownlisteditorjqxGridNitrogenEnhancedEfficiencyProduct')]/div/div[*]/span[contains(.,'" + nitrogen.Eep + "')]"), driver, true);
                }

                // Random Click to save
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + nitrogenCount + "jqxGridNitrogen')]/div[1]/div"), driver, true);

                lastMonth = MonthClicks(nitrogen.Date, false)[0];
                lastDay = MonthClicks(nitrogen.Date, false)[1];

                nitrogenCount++;
            }

            NextButton();
        }

        public void ManurePage(CFarmXmlLister.ScenarioData scenarioData)
        {
            int lastMonth = 1;
            int lastDay = 1;
            int manCount = 0;
            foreach (var manure in scenarioData.ManureApplicationList)
            {
                // "Add Manure" button
                gMethods.FindAndClick(By.Id("addManure"), driver, true);

                gMethods.AddDelay(500);

                // "Manure Date" Box
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + manCount + "jqxGridManure')]/div/div"), driver, true);

                // "Manure Date" Menu
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + manCount + "jqxGridManure')]/div/div"), driver, true);

                gMethods.AddDelay(500);

                // To Month
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Left, null, 2);

                // To Month
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(manure.Date, false)[0] - lastMonth);

                // To Day
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Right, null, 1);

                // To Month
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(manure.Date, false)[1] - lastDay);

                //DateClick(manure.Date, 0);

                // "ManureType" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + manCount + "jqxGridManure')]/div[2]/div"), driver, true);

                // "ManureType" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + manCount + "jqxGridManure')]/div[2]/div"), driver, true);

                // "ManureType" Menu
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'manureGrid-body')]/div[3]/div/div/div[2]/div[1]"), driver, true);

                // "ManureType" Select
                gMethods.FindAndClick(By.XPath("//ul/li[contains(.,'" + manure.Type + "')]"), driver, true);

                // "Amount" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + manCount + "jqxGridManure')]/div[3]/div"), driver, true);

                // "Amount"
                gMethods.FindAndInput(manure.AmountApplied, By.XPath("//div[contains(@id,'manureGrid-body')]/div[4]/div/div/input"), driver, true, false);

                // "Percent N" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + manCount + "jqxGridManure')]/div[4]/div"), driver, true);

                // Removes Percent N values
                gMethods.EnterKeys(By.XPath("//div[contains(@id,'manureGrid-body')]/div/table/tbody/tr[" + (2 + manCount) + "]/td[4]/div"), driver, null, Keys.Backspace, null, 3);
                gMethods.EnterKeys(By.XPath("//div[contains(@id,'manureGrid-body')]/div/table/tbody/tr[" + (2 + manCount) + "]/td[4]/div"), driver, null, Keys.Delete, null, 3);

                gMethods.AddDelay(200);

                // "Percent N"
                gMethods.FindAndInput(manure.PercentN, By.XPath("//div[contains(@id,'manureGrid-body')]/div[5]/div/div/input"), driver, true, false);

                // "CToN Ratio" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + manCount + "jqxGridManure')]/div[5]/div"), driver, true);

                // Removes CToN values
                gMethods.EnterKeys(By.XPath("//div[contains(@id,'manureGrid-body')]/div/table/tbody/tr[" + (2 + manCount) + "]/td[5]/div"), driver, null, Keys.Backspace, null, 3);
                gMethods.EnterKeys(By.XPath("//div[contains(@id,'manureGrid-body')]/div/table/tbody/tr[" + (2 + manCount) + "]/td[5]/div"), driver, null, Keys.Delete, null, 3);

                gMethods.AddDelay(200);

                // "CToN Ratio"
                gMethods.FindAndInput(manure.CToNRatio, By.XPath("//div[contains(@id,'manureGrid-body')]/div[6]/div/div/input"), driver, true, false);

                lastMonth = MonthClicks(manure.Date, true)[0];
                lastDay = MonthClicks(manure.Date, true)[1] + 1;

                manCount++;
            }

            NextButton();
        }

        public void IrrigationPage(CFarmXmlLister.ScenarioData scenarioData)
        {
            int lastMonth = 1;
            int lastDay = 1;
            int irrCount = 0;
            foreach (var irrigation in scenarioData.IrrigationList)
            {
                // "Add New Irrigation" button
                gMethods.FindAndClick(By.Id("addIrrigation"), driver, true);

                // "Irrigation Date" Box
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + irrCount + "jqxGridIrrigation')]/div/div"), driver, true);

                // "Irrigation Date" Box
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + irrCount + "jqxGridIrrigation')]/div/div"), driver, true);

                // "Irrigation Date" Menu
                //gMethods.FindAndClick(By.XPath("//div[contains(@id,'irrigationGrid')]/div[4]/div[2]/div/div[1]/div/div[1]"), driver, true);

                gMethods.AddDelay(500);

                // To Month
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Left, null, 2);

                // To Month
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(irrigation.Date, false)[0] - lastMonth);

                // To Day
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Right, null, 1);

                // To Month
                gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(irrigation.Date, false)[1] - lastDay);

                // "Day"
                //gMethods.FindAndClick(By.XPath("html/body/div[30]/div/table/tbody/tr[*]/td[contains(@title,'" + irrigation.Date + "')]/a/em/span"), driver, true);
                //DateClick(irrigation.Date, 0);

                if (scenarioData.CropAndPlant.CropType == "Rice - Flooded")
                {
                    // "No. of Applications" Click
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + irrCount + "jqxGridIrrigation')]/div[2]/div"), driver, true);

                    // "No. of Applications" Click
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + irrCount + "jqxGridIrrigation')]/div[2]/div"), driver, true);

                    // "No. of Applications" Input
                    gMethods.FindAndInput(irrigation.Applications, By.Id("textboxeditorjqxGridIrrigationNumApplications"), driver, true, false); 
                }

                // "Inches" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + irrCount + "jqxGridIrrigation')]/div[3]/div"), driver, true);

                // "Inches" Click
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + irrCount + "jqxGridIrrigation')]/div[3]/div"), driver, true);

                gMethods.AddDelay(100);

                // "Inches" Input
                gMethods.FindAndInput(irrigation.Inches, By.Id("textboxeditorjqxGridIrrigationTotalApplied"), driver, true, false);

                if (scenarioData.CropAndPlant.CropType == "Rice - Flooded")
                {
                    // To Day
                    gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Right, null, 1);

                    // "Irrigation EndDate" Box
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + irrCount + "jqxGridIrrigation')]/div[4]/div"), driver, true);

                    // "Irrigation EndDate" Menu
                    //gMethods.FindAndClick(By.XPath("//div[contains(@id,'irrigationGrid-body')]/div[4]/div/div[1]/div/div[1]"), driver, true);

                    gMethods.AddDelay(500);

                    // To Month
                    gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Left, null, 2);

                    // To Month
                    gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(irrigation.EndDate, false)[0] - lastMonth);

                    // To Day
                    gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Right, null, 1);

                    // To Day
                    gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Up, Keys.Down, MonthClicks(irrigation.EndDate, false)[1] - lastDay);

                    // "Day"
                    //gMethods.FindAndClick(By.XPath("html/body/div[30]/div/table/tbody/tr[*]/td[contains(@title,'" + irrigation.Date + "')]/a/em/span"), driver, true);
                    //DateClick(irrigation.EndDate, 0);

                    if (irrigation.Aerated)
                    {
                        // To Day
                        gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Right, null, 2);

                        gMethods.AddDelay(200);

                        // "Aerated" Click
                        gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + irrCount + "jqxGridIrrigation')]/div[5]/div"), driver, true);

                        // "Irrigation EndDate" Box - Necessary to move back to column 1 - selects table element
                        gMethods.FindAndClick(By.XPath("//div[contains(@id,'row" + irrCount + "jqxGridIrrigation')]/div[4]/div"), driver, true);

                        // To Day
                        gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Left, null, 4);
                    }
                    else
                    {
                        // To Day
                        gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Left, null, 4);
                    }
                }
                lastMonth = MonthClicks(irrigation.Date, true)[0];
                lastDay = MonthClicks(irrigation.Date, true)[1] + 1;

                irrCount++;
            }

            NextButton();
        }

        public void LimingPage(CFarmXmlLister.ScenarioData scenarioData)
        {
            foreach (var liming in scenarioData.LimingList)
            {
                // Liming Material Menu
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'limingPanelInner-body')]/div[2]/div/div/div[2]/div/div/div/div[2]/div"), driver, true);

                gMethods.AddDelay(500);

                // Liming Material
                gMethods.FindAndClick(By.XPath("//div[*]/div/ul/li[contains(.,'" + liming.Material + "')]"), driver, true);

                // "Liming Date" Menu
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'limingPanelInner-body')]/div[2]/div/div/div[1]/div/div/div/div/div[1]"), driver, true);

                gMethods.AddDelay(500);

                // "Day"
                //gMethods.FindAndClick(By.XPath("html/body/div[30]/div/table/tbody/tr[*]/td[contains(@title,'" + irrigation.Date + "')]/a/em/span"), driver, true);
                DateClick(liming.Date, 0);

                gMethods.FindAndInput(liming.AmountApplied, By.XPath("//div[contains(@id,'limingPanelInner-body')]/div[2]/div/div/div[3]/div/div/div/input"), driver, true, false);
            }

            NextButton();
        }

        public void BurningPage(CFarmXmlLister.ScenarioData scenarioData)
        {
            foreach (var burning in scenarioData.BurningList)
            {
                // Burn Menu
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'burnPanelInner-body')]/div[2]/div/div/div[1]/div/div[2]/div"), driver, true);

                // Burn
                gMethods.FindAndClick(By.XPath("//ul[*]/li[contains(.,'" + burning.Burn + "')]"), driver, true);
            }

            NextButton();
        }

        public void CopyCrop(CFarmXmlLister.ScenarioData scenarioData, string scenarioName)
        {
            // "No Thanks, Continue" button  (the one on the Add Additional Crop? window
      
              gMethods.FindAndClick(By.Id("noThanksContinueButton"), driver, true);

            if (scenarioData != null)
            {
                var yearsToCopy = new string[19];
                if (scenarioData.Years == "All")
                {
                    if (scenarioName == "Baseline")
                    {
                        for (int i = 0; i < 19; i++)
                        {
                            yearsToCopy[i] = (2000 + i).ToString();
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 10; i++)
                        {
                            yearsToCopy[i] = (2018 + i).ToString();
                        }
                    }
                }
                else
                {
                    yearsToCopy = scenarioData.Years.Split(',');
                }

                string selectString;
                for (int i = 1; i < yearsToCopy.Length; i++)
                {
                    if (yearsToCopy[i] != null)
                    {
                        selectString = "copyRotation" + scenarioData.ParcelName + yearsToCopy[i];

                        // Yearly checkboxes
                        gMethods.FindAndClick(By.Id(selectString), driver, true);
                    }
                }
            }


            // "Copy & Continue" button
            gMethods.FindAndClick(By.Id("copyContinueButton"), driver, true);

            WaitTillMask();

            // For Perennial Crops "Ok" Button
            try
            {
                var sequenceElement = driver.Driver.FindElement(By.XPath("//div[contains(@id,'perennialMsg-body')]/div/div/span/a"));
                if (sequenceElement.Displayed)
                {
                    gMethods.AddDelay(200);
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'perennialMsg-body')]/div/div/span/a"), driver, true);
                
                    
                }
            }
            catch
            {}
            WaitTillMask();
            gMethods.AddDelay(2000);


        }

        public void noPerennial(CFarmXmlLister.ScenarioData scenarioData)
        {

            if (scenarioData.CropAndPlant.CropType == "Rice - Flooded")
            {
                gMethods.FindAndClick(By.Id("nextManagementButton"), driver, true);

                gMethods.FindAndClick(By.Id("noThanksContinueButton"), driver, true);

                gMethods.FindAndClick(By.Id("copyContinueButton"), driver, true);

                WaitTillMask();
            }


        }

        public void EndScenario(CFarmXmlLister.CroplandsScenario scenario)
        {
            //noPerennial();

            if (scenario.Name == "Baseline")
            {
                // "Continue to Future Management" button
                gMethods.FindAndClick(By.Id("continueFuture"), driver, true);
            }
        }

        public void EndCroplands()
        {
            WaitTillMask();

            // "Continue to Report" button
            gMethods.FindAndClick(By.Id("nextStep"), driver, true);
        }

        public void NextButton()
        {
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[8]/span/a/span[2]"), driver, true);
        }

        public void DateClick(string dateToClick, int i)
        {
            try
            {
                IWebElement day = driver.Driver.FindElement(By.XPath("html/body/div[" + i + "]/div/table/tbody/tr[*]/td[contains(@title,'" + dateToClick + "')]/a/em/span"));
                if (i < 80)
                {
                    if (day.Displayed)
                    {
                        // "Day"
                        gMethods.FindAndClick(By.XPath("html/body/div[" + i + "]/div/table/tbody/tr[*]/td[contains(@title,'" + dateToClick + "')]/a/em/span"), driver, false);
                    }
                    else
                    {
                        i++;
                        DateClick(dateToClick, i);
                    }
                }
            }
            catch
            {
                i++;
                DateClick(dateToClick, i);
            }
        }

        public void SaveClick(int buttonNum, int i)
        {
            // Click Save button
            gMethods.FindAndClick(By.XPath("//div[contains(@id,'window') and not(contains(@display,'none'))]/div[3]/div/div/em/button/span[contains(.,'Save')]"), driver, true);
            
            /*try
            {
                
                if (i < 100)
                {
                    IWebElement day = driver.Driver.FindElement(By.XPath("//div[contains(@id,'button-" + (buttonNum + i) + "')]/em/button/span[contains(.,'Save')]"));
                    if (day.Displayed)
                    {
                        // "Day"
                        gMethods.FindAndClick(By.XPath("//div[contains(@id,'button-" + (buttonNum + i) + "')]/em/button/span[contains(.,'Save')]"), driver, false);
                    }
                    else
                    {
                        i++;
                        SaveClick(buttonNum, i);
                    }
                }
                else
                {
                    SaveClick(buttonNum+100,0);
                }
            }
            catch
            {
                i++;
                SaveClick(buttonNum, i);
            }   */
        }



        /// <summary>
        /// Used to determine the number of presses needed to navigate to the correct month using gMethods.EnterKeys().
        /// When the date string contains the last day of the month when finding "lastMonth", the month # is incremented 
        /// to account for the calender's automatic advancement to the next month.
        /// </summary>
        /// <param name="month"></param> Date string in the format "Month DD". ex: "January 08"
        /// <param name="last"></param> Determines if this is a "lastMonth" int and the last day of the month is accounted for
        /// <returns></returns>
        public int[] MonthClicks(string month, bool last)
        {
            var monthDay = month.Split(' ');
            var monthNum = 0;
            var dayNum = Int16.Parse(monthDay[1]);
            if (monthDay[0] == "January")
            {
                monthNum = 1;                
            }
            if (monthDay[0] == "February")
            {
                monthNum = 2;
            }
            if (monthDay[0] == "March")
            {
                monthNum = 3;
            }
            if (monthDay[0] == "April")
            {
                monthNum = 4;
            }
            if (monthDay[0] == "May")
            {
                monthNum = 5;
            }
            if (monthDay[0] == "June")
            {
                monthNum = 6;
            }
            if (monthDay[0] == "July")
            {
                monthNum = 7; 
            }
            if (monthDay[0] == "August")
            {
                monthNum = 8;
            }
            if (monthDay[0] == "September")
            {
                monthNum = 9;
            }
            if (monthDay[0] == "October")
            {
                monthNum = 10;
            }
            if (monthDay[0] == "November")
            {
                monthNum = 11;
            }
            if (monthDay[0] == "December")
            {
                monthNum = 12;
            }

            if (monthDay[0] == "January" || monthDay[0] == "March" || monthDay[0] == "May" || monthDay[0] == "July" || monthDay[0] == "August" || monthDay[0] == "October" || monthDay[0] == "December")
            {
                if (last && month.Contains("31"))
                {
                    monthNum++;
                    dayNum =  1;
                }
            }

            if (monthDay[0] == "April" || monthDay[0] == "June" || monthDay[0] == "September" || monthDay[0] == "November")
            {
                if (last && month.Contains("30"))
                {
                    monthNum++;
                    dayNum = 1;
                }
            }

            if (monthDay[0] == "February")
            {
                if (last && month.Contains("29"))
                {
                    monthNum++;
                    dayNum = 1;
                }
            }

            var dateArray = new int[] {monthNum, dayNum};
            return dateArray;
        }


    }
}
