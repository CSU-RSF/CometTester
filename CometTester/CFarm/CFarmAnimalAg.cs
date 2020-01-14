using System;
using System.Collections.Generic;
using OpenQA.Selenium;

namespace CometTester.CFarm
{
    class CFarmAnimalAg
    {
        GeneralTestMethods gMethods;
        BrowserDriver driver;
        readonly string _whichTest;
        private int _dietCount = 4;

        public CFarmAnimalAg(GeneralTestMethods gMethods, BrowserDriver driver, string whichTest)
        {
            this.gMethods = gMethods;
            this.driver = driver;
            
            _whichTest = whichTest;

            PropertyLocation();
            AnimalTypes();
            //Herds();

            // "Ok" button
            gMethods.FindAndClick(By.XPath("/html/body/div[10]/div[3]/div/div/em/button"), driver, true);

            NumberOfDiets();
            AnimalDetails();
            Weight();
            FeedDetails();
            //PregnancyAndMilk();
            //TypesOfFeed();
            //FeedingSituation();
            ManureSystemTypes();
            ManureSystemDetails();
            ManureDetails();
            CreateCropScenario(true, "Future");
            EndAnimalAg();
        }

        public void PropertyLocation()
        {   
            gMethods.AddDelay(1500);

            gMethods.FindAndClick(By.Id("ZipCode"), driver, true);

            gMethods.EnterKeys(By.Id("ZipCode"), driver, Keys.Backspace, null, null, false, 5);

            // "Address Query" box
            gMethods.FindAndInput("80631", By.Id("ZipCode"), driver, true, false);

            gMethods.AddDelay(1000);

            // "Unit" Select - metric
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div[2]/div[1]/div[2]/table/tbody/tr[4]/td/div[1]/select/option[contains(@value,'Metric')]"), driver, true);

            gMethods.AddDelay(1500);

            SaveButton();
        }

        public void AnimalTypes()
        {
            gMethods.AddDelay(2500);

            // "Beef-Mature Cows" checkbox
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div[2]/div[1]/div[1]/div[3]/div/fieldset[1]/div/div[1]/div[1]/div/div/div[2]/div[contains(@id,'Feedlot CattleCheck')]/div[1]/input"), driver, true);

            SaveButton();
        }

        public void Herds()
        {
            // "Ok" button
            gMethods.FindAndClick(By.XPath("/html/body/div[10]/div[3]/div/div/em/button"), driver, true);

            SaveButton();
        }

        public void NumberOfDiets()
        {

            gMethods.FindAndMultiClick("", By.XPath("/html/body/div[2]/div[4]/div[4]/div[1]/div/div/div/div[1]/div[1]/div/div[1]"), driver, true, 3);

            SaveButton();
        }

        public void AnimalDetails()
        {
            gMethods.AddDelay(3000);

            string headCount;
            for (int i = 0; i < _dietCount; i++)
            {
                gMethods.AddDelay(1000);

                if (i == 0)
                {
                    headCount = "200"; //200
                }
                else if (i==1)
                {
                    headCount = "400"; //400
                }
                else if(i==2)
                {
                    headCount = "100"; //100
                }
                else
                {
                    headCount = "300"; //300
                }

                int j = i%2;
                gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[2]/div[contains(@id,'feedlotMonthsPanel"+(j+1)+"')]/div/div/table/tbody/tr/td[2]/div[1]/div/table/tbody/tr[1]/td[1]/div/div[1]/input"), driver, true);

                gMethods.AddDelay(100);

                // "Animal" amount
                gMethods.FindAndInput(headCount, By.XPath("/html/body/div[2]/div[4]/div[4]/div[2]/div[contains(@id,'feedlotMonthsPanel1')]/div/div/table/tbody/tr/td[2]/div[1]/div/table/tbody/tr[1]/td[1]/div/div[1]/input"), driver, true, false);

                gMethods.AddDelay(500);

                // "Copy" button
                gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[2]/div[contains(@id,'feedlotMonthsPanel1')]/div/div/table/tbody/tr/td[2]/div[1]/div/table/tbody/tr[1]/td[1]/div/label/div/a/img"), driver, true);

                if (i == 0)
                {
                    // "Copy" button
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[2]/div[contains(@id,'feedlotMonthsPanel1')]/div/div/table/tbody/tr/td[2]/div[1]/div/table/tbody/tr[1]/td[1]/div/label/div/a/img"), driver, true);
                }
                // "Animal" amount
                gMethods.FindAndInput(headCount, By.XPath("/html/body/div[2]/div[4]/div[4]/div[2]/div[contains(@id,'feedlotMonthsPanel2')]/div/div/table/tbody/tr/td[2]/div[1]/div/table/tbody/tr[1]/td[1]/div/div[1]/input"), driver, true, false);

                gMethods.AddDelay(500);

                // "Copy" button
                gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[2]/div[contains(@id,'feedlotMonthsPanel2')]/div/div/table/tbody/tr/td[2]/div[1]/div/table/tbody/tr[1]/td[1]/div/label/div/a/img"), driver, true);

                gMethods.AddDelay(1500);

                SaveButton();

                gMethods.AddDelay(500);

                if (i<3)
                {
                    //Next Diet button
                    gMethods.FindAndClick(By.XPath("/html/body/div[" + (i + 13) + "]/div[3]/div[1]/div[2]/em/button"), driver, true); 
                }                
            }

            /*
            string monthDiv;
            for (int i = 1; i < 13; i++)
            {
                monthDiv = "/html/body/div[2]/div[4]/div[4]/div[2]/div[contains(@id,'feedlotMonthsPanel1')]/div/div/table/tbody/tr/td[2]/div[1]/div/table/tbody/tr[1]/td[" + i + "]/div/div[1]/input";

                // "Animal" amount
                gMethods.FindAndInput("200", By.XPath(monthDiv), driver, true, false);
            }

            for (int i = 1; i < 13; i++)
            {
                monthDiv = "/html/body/div[2]/div[4]/div[4]/div[2]/div[contains(@id,'feedlotMonthsPanel2')]/div/div/table/tbody/tr/td[2]/div[1]/div/table/tbody/tr[1]/td[" + i + "]/div/div[1]/input";

                // "Animal" amount
                gMethods.FindAndInput("200", By.XPath(monthDiv), driver, true, false);
            }
            */

            
        }

        public void Weight()
        {
            string adw;
            string alw;
            for (int i = 0; i < _dietCount; i++)
            {
                gMethods.AddDelay(2000);

                if (i == 0)
                {
                    adw = "1.5";
                    alw = "300";
                }
                else if (i == 1)
                {
                    adw = "1.5";
                    alw = "400";
                }
                else if (i == 2)
                {
                    adw = "1.5";
                    alw = "450";
                }
                else
                {
                    adw = "1.5";
                    alw = "500";
                }

                // Breed Combo box
                gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[9]/div[1]/div/div/div[2]/div/table/tbody/tr/td[1]/div/div[1]/div[2]/div[1]"), driver, true);

                // Breed Select
                gMethods.FindAndClick(By.XPath("/html/body/div[16]/div/ul/li[contains(.,'Angus')]"), driver, true);

                // "TMW" box
                //gMethods.FindAndInput("512", By.XPath("/html/body/div[2]/div[4]/div[4]/div[9]/div[1]/div/div/div[2]/div/table/tbody/tr/td[2]/div/div[1]/input"), driver, true, false);

                for (int j = 0; j < 2; j++)
                {
                    // "ADW" box
                    gMethods.FindAndInput(adw, By.XPath("/html/body/div[2]/div[4]/div[4]/div[9]/div[" + (2 + j) + "]/div[" + (2 - j) + "]/div/table/tbody/tr/td[2]/div/div/table/tbody/tr/td[1]/div/div[1]/input"), driver, true, false);

                    // ADW Copy
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[9]/div[" + (2 + j) + "]/div[" + (2 - j) + "]/div/table/tbody/tr/td[1]/div/div/label/div/table/tbody/tr/td[2]/a/img"), driver, true);
                }

                for (int j = 0; j < 2; j++)
                {
                    // "ALW" box
                    gMethods.FindAndInput(alw, By.XPath("/html/body/div[2]/div[4]/div[4]/div[9]/div[" + (4 + j) + "]/div[" + (2 - j) + "]/div/table/tbody/tr/td[2]/div/div/table/tbody/tr/td[1]/div/div[1]/input"), driver, true, false);

                    // ALW Copy
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[9]/div[" + (4 + j) + "]/div[" + (2 - j) + "]/div/table/tbody/tr/td[1]/div/div/label/div/table/tbody/tr/td[2]/a/img"), driver, true);
                }

                gMethods.AddDelay(500);

                DeselectClick("/html/body/div[2]");

                SaveButton(); 

                gMethods.AddDelay(1000);

                if (i<3)
                {
                    //Next Diet button
                    gMethods.FindAndClick(By.XPath("/html/body/div[" + ((i*2) + 18) + "]/div[3]/div[1]/div[2]/em/button"), driver, true); 
                } 
            }
        }

        public void FeedDetails()
        {
            var feedList = new List<Dictionary<string, string>>
                {
                    new Dictionary<string, string>
                        {
                            {"0", "1"},
                            {"1", "1"},
                            {"2", "2"},
                            {"3", "2"}
                        },
                    new Dictionary<string, string>
                        {
                            {"0", "1"},
                            {"1", "2"},
                            {"2", "1"},
                            {"3", "1"}
                        },
                    new Dictionary<string, string>
                        {
                            {"0", "1"},
                            {"1", "2"},
                            {"2", "1"},
                            {"3", "1"}
                        },
                    new Dictionary<string, string>
                        {
                            {"0", "1"},
                            {"1", "3"},
                            {"2", "1"},
                            {"3", "1"}
                        }
                };
            string value;
            for (int i = 0; i < _dietCount; i++)
            {  
                gMethods.AddDelay(1500);

                //Ionophores button
                feedList[i].TryGetValue("0", out value);
                gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[10]/div/div/div/div/div/div/div/div/div/div[" + value + "]/div/input"), driver, true);

                //Fat Content button
                feedList[i].TryGetValue("1", out value);
                gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[10]/div/div/div[2]/div/div/div/div/div/div/div[" + value + "]/div/input"), driver, true);

                //Grain Type button
                feedList[i].TryGetValue("2", out value);
                gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[10]/div/div/div[3]/div/div/div/div/div/div/div[" + value + "]/div/input"), driver, true);

                //Concentrate Percentage button
                feedList[i].TryGetValue("3", out value);
                gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[10]/div/div/div[4]/div/div/div/div/div/div/div[" + value + "]/div/input"), driver, true);

                SaveButton();

                gMethods.AddDelay(1000);

                if (i<3)
                {
                    //Next Diet button
                    gMethods.FindAndClick(By.XPath("/html/body/div[" + ((i*2) + 25) + "]/div[3]/div[1]/div[2]/em/button"), driver, true); 
                } 
            }
        }

        public void PregnancyAndMilk()
        {
            gMethods.AddDelay(500);

            string monthDiv;
            int divTwo;
            for (int i = 1; i < 13; i++)
            {
                if ((i % 2) == 0)
                    divTwo = 2;
                else
                    divTwo = 1;

                monthDiv = "/html/body/div[2]/div[4]/div[4]/div[7]/div[1]/div/div/div/div[1]/div/table/tbody/tr[" + ((i + 1) / 2).ToString() + "]/td[" + divTwo + "]/div/div[1]/input";

                // "pregnant" monthly
                gMethods.FindAndInput("50", By.XPath(monthDiv), driver, true, false);
            }

            for (int i = 1; i < 13; i++)
            {
                if ((i % 2) == 0)
                    divTwo = 2;
                else
                    divTwo = 1;

                monthDiv = "/html/body/div[2]/div[4]/div[4]/div[7]/div[2]/div/div/div/div[1]/div/table/tbody/tr[" + ((i + 1) / 2).ToString() + "]/td[" + divTwo + "]/div/div[1]/input";

                // "lactating" monthly
                gMethods.FindAndInput("5", By.XPath(monthDiv), driver, true, false);
            }

            // "milk production" box
            gMethods.FindAndInput("50", By.XPath("/html/body/div[2]/div[4]/div[4]/div[7]/div[3]/div/div/div[1]/div[1]/input"), driver, true, false);

            // "fat content" box
            gMethods.FindAndInput("10", By.XPath("/html/body/div[2]/div[4]/div[4]/div[7]/div[3]/div/div/div[2]/div[1]/input"), driver, true, false);

            SaveButton();
        }

        public void TypesOfFeed()
        {
            gMethods.AddDelay(1000);

            // "Month" CheckBox
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[4]/div[2]/div/div/div/div[1]/div/div[1]/div[2]/div/div[1]/div/span"), driver, true);

            gMethods.AddDelay(2500);

            // "Alfalfa"
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[4]/div[3]/div/div/div/div[1]/div/div[1]/div[4]/div/table/tbody/tr[2]/td"), driver, true);

            gMethods.AddDelay(500);

            // "Cubes"
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[4]/div[3]/div/div/div/div[2]/div/div[1]/div[3]/div/table/tbody/tr[2]/td/div"), driver, true);

            gMethods.AddDelay(500);

            // "Add" Button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[4]/div[3]/div/div/div/div[3]/div/div[1]/div/div[1]/em/button"), driver, true);

            gMethods.AddDelay(500);

            // "Confirm Selections" Button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[4]/div[3]/div/div/div/div[5]/div/div[1]/em/button"), driver, true);

            gMethods.AddDelay(500);

            // "percentage eaten"
            gMethods.FindAndInput("100", By.XPath("/html/body/div[2]/div[4]/div[4]/div[4]/div[4]/div[2]/div[2]/div/table/tbody/tr/td/div/div/div[1]/div[1]/input"), driver, true, false);

            gMethods.AddDelay(3500);

            // "Done" Button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[4]/div[5]/div/div/em/button"), driver, true);

            gMethods.AddDelay(2500);

            // "Ok" Button
            gMethods.FindAndClick(By.XPath("/html/body/div[20]/div[3]/div/div/em/button"), driver, true);

            SaveButton();
        }

        public void FeedingSituation()
        {
            gMethods.AddDelay(1000);

            SaveButton();
        }

        public void ManureSystemTypes()
        {
            DeselectClick("/html/body/div[2]");

            var manureTypeList = new List<String>
                {
                    {"Temporary Stack and Long-Term Stockpile"},
                    {"Temporary Stack and Long-Term Stockpile"},
                    {"Temporary Stack and Long-Term Stockpile"},
                    {"Temporary Stack and Long-Term Stockpile"}
                };

            for (int i = 0; i < _dietCount; i++)
            {
                gMethods.AddDelay(1500);

                // "treatment method" menu
                gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[18]/div/div/div[5]/div[1]/div[2]/div[1]"), driver, true);

                // "Aerobic Lagoon"
                gMethods.FindAndClick(By.XPath("/html/body/div[" + (31) + "]/div/ul/li[contains(.,'" + manureTypeList[i] + "')]"), driver, true);

                gMethods.AddDelay(1500);

                SaveButton();

                gMethods.AddDelay(1000);

                if (i < 3)
                {
                    //Next Diet button
                    gMethods.FindAndClick(By.XPath("/html/body/div[" + ((i * 2) + 33) + "]/div[3]/div[1]/div[2]/em/button"), driver, true);
                } 
            }
        }

        public void ManureSystemDetails()
        {
            for (int i = 0; i < _dietCount; i++)
            {
                gMethods.AddDelay(1500);

                // "volume" box
                //gMethods.FindAndInput("100", By.XPath("/html/body/div[2]/div[4]/div[4]/div[19]/div/div/div[6]/div[1]/input"), driver, true, false);            

                SaveButton();

                gMethods.AddDelay(1000);

                if (i < 3)
                {
                    //Next Diet button
                    gMethods.FindAndClick(By.XPath("/html/body/div[" + ((i * 2) + 40) + "]/div[3]/div[1]/div[2]/em/button"), driver, true);
                } 
            } 
        }

        public void ManureDetails()
        {
            for (int i = 0; i < _dietCount; i++)
            {
                gMethods.AddDelay(2000);

                SaveButton();

                gMethods.AddDelay(1000);

                if (i < 3)
                {
                    //Next Diet button
                    gMethods.FindAndClick(By.XPath("/html/body/div[" + ((i * 2) + 47) + "]/div[3]/div[1]/div[2]/em/button"), driver, true);
                } 
            }
        }

        public void SaveButton()
        {
            gMethods.AddDelay(500);

            // "Save and Continue" Button
            gMethods.FindAndClick(By.Id("saveButton"), driver, true);

            gMethods.AddDelay(500);
        }

        public void CreateCropScenario(bool copy, string scenarioName)
        {
            gMethods.AddDelay(500);

            // Scenario input
            gMethods.FindAndInput(scenarioName, By.XPath("/html/body/div[53]/div[2]/div[1]/div/div/div[2]/div/div[1]/div[1]/input"), driver, true, false);

            if (copy != true)
            {
                // "Copy" checkbox
                gMethods.FindAndClick(By.XPath("/html/body/div[28]/div[2]/div[1]/div/div/div[2]/div/div[2]/div[1]/input"), driver, true);
            }

            gMethods.AddDelay(500);

            // "Create new Scenario" button
            gMethods.FindAndClick(By.XPath("/html/body/div[53]/div[2]/div[2]/div/span[2]/a/span[2]"), driver, true);
        }

        public void EndAnimalAg()
        {
            gMethods.AddDelay(5000);

            // "Manure Details" Link
            gMethods.FindAndClick(By.Id("manureDetailsLink"), driver, true);

            for (int i = 0; i < _dietCount; i++)
            {
                SaveButton();

                gMethods.AddDelay(1500);

                if (i < 3)
                {
                    //Next Diet button
                    gMethods.FindAndClick(By.XPath("/html/body/div[" + (i + 55) + "]/div[3]/div[1]/div[2]/em/button"), driver, true);
                } 
            }

            // "Report" Button
            gMethods.FindAndClick(By.XPath("/html/body/div[contains(@id,'newScenarioWindow1')]/div[2]/div[2]/div/span[1]/a"), driver, true);
        }

        public void DeselectClick(string locator)
        {
            // Body Click
            gMethods.FindAndClick(By.XPath(locator), driver, true);

            gMethods.AddDelay(500);
        }
    }
}
