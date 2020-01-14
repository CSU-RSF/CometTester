using System;
using OpenQA.Selenium;

namespace CometTester.CFarm
{
    class CFarmXmlScratch
    {
        GeneralTestMethods gMethods;
        BrowserDriver driver;
        ///CFarmXmlLister.CFarmCroplandsData xmlCroplands;

        ///constructor
        public CFarmXmlScratch(GeneralTestMethods gMethods, BrowserDriver driver/*place holder for CFarmXmlLister*/)
        {
            this.gMethods = gMethods;
            this.driver = driver;
            ///this.lister = passed lister
            
            ///add methods to be called in constructor here
            

            /*
            foreach (var scenario in *CFarmXmlLister*.CroplandsScenarioList)
            {
                if (scenario.Name != "Baseline")    //Creating a crop scenario isn't avialable until after the Baseline is complete
                {
                    CreateCropScenario(scenario);
                }
                if (scenario.CopyFromBaseline != true)    //If a future scenario copies the baseline, input data is already complete
                {
                    foreach (var scenarioData in scenario.ScenarioDataList)
                    {
                        if (scenario.ScenarioDataList.IndexOf(scenarioData) != 0)
                        {
                            // "Ok" button for unfinished scenario
                            gMethods.FindAndClick(By.XPath("//div[contains(@id,'messagebox')]/div[3]/div/div/em/button"), driver, true);
                        }

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
                    gMethods.AddDelay(6000);

                    // "Skip Ahead" button
                    gMethods.FindAndClick(By.XPath("//span[contains(.,'Skip Ahead >>')]"), driver, true);

                    CopyCrop(null, null);
                }
                EndScenario(scenario);
            }
            EndCroplands();
            */
        }
    }
}
