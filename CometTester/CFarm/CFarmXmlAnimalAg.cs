using System.Collections.Generic;
using System.Linq;
using OpenQA.Selenium;

namespace CometTester.CFarm
{
    class CFarmXmlAnimalAg
    {
        GeneralTestMethods gMethods;
        BrowserDriver driver;
        private CFarmXmlLister.CFarmAnimalAgData xmlAnimalAg;
        private int _backspaces;
        private bool ABL;

        public CFarmXmlAnimalAg(GeneralTestMethods gMethods, BrowserDriver driver, CFarmXmlLister.CFarmAnimalAgData xmlAnimalAg)
        {
            //bool for multiple animaltypes
            bool skipOk = false;    // True if multiple animal types, Skips the "Ok" window with initial explanation after the Location page

            this.gMethods = gMethods;
            this.driver = driver;
            this.xmlAnimalAg = xmlAnimalAg;
            _backspaces = 0;
            ABL = false;

            PropertyLocation();

            //pass typeArray instead as parameter?
            string[] typeArray = xmlAnimalAg.TypeList.Split(',');
            AnimalTypes(typeArray);

            foreach (var scenario in xmlAnimalAg.AnimalAgScenarioList)
            {

                if (scenario.Name != "Baseline")    // True for any scenario except the first Baseline scenario
                {
                    CreateAnimalAgScenario(scenario);
                }

                if (!scenario.CopyFromFuture)
                {
                    foreach (var animalType in scenario.AnimalTypeList)
                    {
                        WaitTillMask();
                        gMethods.AddDelay(500);
                    }
                    foreach (var animalType in scenario.AnimalTypeList)
                    {
                        if (scenario.AnimalTypeList[0] != animalType)
                        {
                            gMethods.FindAndClick(By.XPath("//div[contains(@id,'currentCategoryComplete')]/div[3]/div/div/em/button"), driver, true);

                            gMethods.FindAndClick(By.XPath("//html/body"), driver, true);
                            WaitTillMask();
                            gMethods.AddDelay(500);

                            skipOk = true;
                        }
                        WaitTillMask();
                        gMethods.AddDelay(500);
                        //if the test has moved on to a second or more type skip this method
                        if (!skipOk)
                        {
                            OkButton();
                        }
                        HerdsOrDiets(animalType);
                        AnimalDetails(animalType);
                        Weight(animalType);
                        HoursOfWork(animalType);
                        FeedDetails(animalType);
                        PregnancyAndMilk(animalType);
                        TypesOfFeed(animalType);
                        FeedingSituation(animalType);
                        HousingDetails(animalType);
                        ManureSystemTypes(animalType);
                        ManureSystemDetails(animalType);
                        ManureDetails(animalType);
                    }
                }
            }
            EndAnimalAg();
        }

        /// <summary>
        /// Inputs zip code and selects measurement system
        /// </summary>
        public void PropertyLocation()
        {
            gMethods.AddDelay(1500);

            gMethods.FindAndClick(By.Id("ZipCode"), driver, true);

            gMethods.EnterKeys(By.Id("ZipCode"), driver, null, Keys.Backspace, null, 5);

            // "Zip Code" box
            gMethods.FindAndInput(xmlAnimalAg.ZipCode, By.Id("ZipCode"), driver, true, false);

            gMethods.AddDelay(1000);

            // "Unit" Select - metric
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div[2]/div[1]/div[2]/table/tbody/tr[4]/td/div[1]/select/option[contains(@value,'" + xmlAnimalAg.PreferredUnits + "')]"), driver, true);

            gMethods.AddDelay(1500);

            SaveButton();
        }

        /// <summary>
        /// Selects the checkboxes for the animal types used in the test
        /// </summary>
        /// <param name="typeArray"></param>
        public void AnimalTypes(string[] typeArray) //change parameter to a data structure capable of multiple strings instead of "string animalType"
        {
            WaitTillMask();
            gMethods.AddDelay(500);

            foreach (var animalType in typeArray)
            {
                // "Animal Type" checkbox
                gMethods.FindAndClick(By.XPath("//div[contains(@id,'" + animalType + "Check')]"), driver, true);
            }
            WaitTillMask();
            gMethods.AddDelay(500);

            SaveButton();
            WaitTillMask();
            gMethods.AddDelay(500);

        }

        /// <summary>
        /// Increments the Herd or Diet counter based on the number of Herds or Diets used
        /// </summary>
        /// <param name="animalType"></param>
        public void HerdsOrDiets(CFarmXmlLister.AnimalType animalType)
        {
            var herdTypeList = new List<string>
                {
                    "Beef-Heifer Stockers",
                    "Beef-Heifer Replacements",
                    "Beef-Mature Cows/Cow-Calf",
                    "Beef-Steer Stockers",
                    "Bulls",
                    "Dairy-Heifer Replacements",
                    "Dairy-Dry Cows",
                    "Dairy-Lactating Cows",
                    "Feedlot Cattle",
                    "American Bison"
                };

            foreach (var herdType in herdTypeList)
            {
                if (animalType.TypeName == herdType)
                {
                    var herdCount = -1 + animalType.HerdOrDietList.Count();

                    if (herdCount >= 1)
                    {
                        gMethods.FindAndMultiClick("", By.XPath("/html/body/div[2]/div[4]/div[4]/div[1]/div/div/div/div[1]/div[1]/div/div[1]"), driver, true, herdCount, false);
                    }
                    WaitTillMask();
                    gMethods.AddDelay(500);

                    // gMethods.FindAndClick(By.XPath("//html/body"), driver, true);

                    //gMethods.AddDelay(16000);

                    SaveButton();
                    WaitTillMask();
                    gMethods.AddDelay(500);
                }
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
        /// Encompasses all potential inputs on the Animal Details page for all animal types
        /// </summary>
        /// <param name="animalType"></param>
        public void AnimalDetails(CFarmXmlLister.AnimalType animalType)
        {
            WaitTillMask();
            gMethods.AddDelay(500);

            int k = 0;
            foreach (var herdOrDiet in animalType.HerdOrDietList)
            {
                if (k != 0)
                {
                    HerdComplete();
                }

                gMethods.AddDelay(1000);


                //Gives Swine more time to load screen
                // if (animalType.TypeName.Contains("Pigs") || animalType.TypeName.Contains("Sows"))
                // {
                //  gMethods.AddDelay(2000);

                // gMethods.FindAndClick(By.XPath("//html/body"), driver, true);

                //  gMethods.AddDelay(1500);
                //  }


                // Population input for Dairy and Feedlot Cattle
                if (animalType.TypeName == "Feedlot Cattle" || animalType.TypeName.Contains("Dairy"))
                {

                    foreach (var aveCount in herdOrDiet.AnimalDetails.AverageMonthlyCountList)
                    {
                        gMethods.AddDelay(500);


                        // "Animal" amount click
                        gMethods.FindAndClick(By.XPath("//div[contains(@id,'Jan" + ShortName(aveCount.PopName) + "')]/div/input"), driver, true);
                        //gMethods.FindAndClick(By.XPath("//div[contains(@id,'" + ShortName(aveCount.PopName) + "MonthsPanel" + (j + 1) + "')]/div/div/table/tbody/tr/td[2]/div[1]/div/table/tbody/tr[1]/td[1]/div/div[1]/input"), driver, true);

                        gMethods.AddDelay(100);

                        // "Animal" amount input
                        gMethods.FindAndInput(aveCount.Population, By.XPath("//div[contains(@id,'Jan" + ShortName(aveCount.PopName) + "')]/div/input"), driver, true, false);

                        gMethods.AddDelay(500);

                        // "Copy" button
                        gMethods.FindAndClick(By.XPath("//div[contains(@id,'Jan" + ShortName(aveCount.PopName) + "')]/label/div/a/img"), driver, true);




                    }
                }

                    //Just for swine, adding more time and reentering data
                else if ((animalType.TypeName.Contains("Pigs")) || animalType.TypeName.Contains("Sows"))
                {
                    foreach (var aveCount in herdOrDiet.AnimalDetails.AverageMonthlyCountList)
                    {
                        WaitTillMask();
                        gMethods.AddDelay(500);

                        gMethods.FindAndClick(By.XPath("//div[contains(@id,'Jan" + ShortName(aveCount.PopName) + "')]/div/input"), driver, true);
                        WaitTillMask();
                        gMethods.AddDelay(500);

                        gMethods.FindAndInput(aveCount.Population, By.XPath("//div[contains(@id,'Jan" + ShortName(aveCount.PopName) + "')]/div/input"), driver, true, false);
                        WaitTillMask();
                        gMethods.AddDelay(500);

                        gMethods.FindAndClick(By.XPath("//div[contains(@id,'Jan" + ShortName(aveCount.PopName) + "')]/label/div/a/img"), driver, true);
                        WaitTillMask();
                        gMethods.AddDelay(500);

                        gMethods.FindAndClick(By.XPath("//div[contains(@id,'Jan" + ShortName(aveCount.PopName) + "')]/div/input"), driver, true);

                        gMethods.EnterKeys(By.XPath("//div[contains(@id,'Jan" + ShortName(aveCount.PopName) + "')]/div/input"), driver, null, Keys.Backspace, null, _backspaces + 1);
                        WaitTillMask();
                        gMethods.AddDelay(500);

                        gMethods.FindAndInput(aveCount.Population, By.XPath("//div[contains(@id,'Jan" + ShortName(aveCount.PopName) + "')]/div/input"), driver, true, false);
                    }
                }


                else
                {
                    // Population input for most animal types
                    foreach (var aveCount in herdOrDiet.AnimalDetails.AverageMonthlyCountList)
                    {

                        gMethods.FindAndClick(By.XPath("//div[contains(@id,'Jan" + ShortName(aveCount.PopName) + "')]/div/input"), driver, true);

                        gMethods.FindAndInput(aveCount.Population, By.XPath("//div[contains(@id,'Jan" + ShortName(aveCount.PopName) + "')]/div/input"), driver, true, false);

                        gMethods.FindAndClick(By.XPath("//div[contains(@id,'Jan" + ShortName(aveCount.PopName) + "')]/label/div/a/img"), driver, true);
                        WaitTillMask();
                        gMethods.AddDelay(500);
                    }
                }



                //ADFI
                var adfiTypeList = new List<string>
                {
                    "Dairy-Heifer Replacements",
                    "Dairy-Dry Cows",
                    "Dairy-Lactating Cows",
                    "Broilers",
                    "Ducks",
                    "Laying Hens",
                    "Pullets",
                    "Turkeys",
                    "Feeder Sheep",
                    "Flock Sheep",
                    "Gestating Sows",
                    "Grow-Finish Pigs",
                    "Lactating Sows",
                    "Weaning Pigs"
                };

                foreach (var type in adfiTypeList)
                {
                    if (animalType.TypeName == type)
                    {

                        gMethods.FindAndInput(herdOrDiet.AnimalDetails.Adfi, By.XPath("//div[contains(@id,'dailyFeedIntake')]/div[1]/input"), driver, true, false);
                    }
                }

                //DaysOnFeed
                var daysFeedTypeList = new List<string>
                {
                    "Broilers",
                    "Ducks",
                    "Pullets",
                    "Turkeys",
                    "Grow-Finish Pigs",
                    "Weaning Pigs"
                };

                foreach (var type in daysFeedTypeList)
                {
                    if (animalType.TypeName == type)
                    {
                        gMethods.FindAndInput(herdOrDiet.AnimalDetails.DaysOnFeed, By.XPath("//div[contains(@id,'daysOnFeed')]/div[1]/input"), driver, true, false);
                    }
                }

                //CrudeProtein
                var proteinTypeList = new List<string>
                {
                    "Broilers",
                    "Ducks",
                    "Pullets",
                    "Turkeys",
                    "Grow-Finish Pigs",
                    "Weaning Pigs",
                    "Gestating Sows",
                    "Lactating Sows",
                    "Laying Hens"
                };

                foreach (var type in proteinTypeList)
                {
                    if (animalType.TypeName == type)
                    {
                        gMethods.FindAndInput(herdOrDiet.AnimalDetails.CrudeProtein, By.XPath("//div[contains(@id,'crudeProtein')]/div[1]/input"), driver, true, false);
                    }
                }

                //DressingPercentage
                //FatFreePercenetage
                if (animalType.TypeName == "Grow-Finish Pigs")
                {
                    gMethods.FindAndInput(herdOrDiet.AnimalDetails.DressingPercentage, By.XPath("//div[contains(@id,'dressingPercentage')]/div[1]/input"), driver, true, false);
                    gMethods.FindAndInput(herdOrDiet.AnimalDetails.FatFreeLeanPercentage, By.XPath("//div[contains(@id,'fatfreePercentage')]/div[1]/input"), driver, true, false);
                }

                //InitialBodyWeight
                //FinalBodyWeight
                if (animalType.TypeName == "Grow-Finish Pigs" || animalType.TypeName == "Weaning Pigs")
                {
                    gMethods.FindAndInput(herdOrDiet.AnimalDetails.InitialBodyWeight, By.XPath("//div[contains(@id,'initialBodyWeight')]/div[1]/input"), driver, true, false);
                    gMethods.FindAndInput(herdOrDiet.AnimalDetails.FinalBodyWeight, By.XPath("//div[contains(@id,'finalBodyWeight')]/div[1]/input"), driver, true, false);
                }

                //LactationLength
                //LitterWeightBirth
                //LitterWeightWeaning
                if (animalType.TypeName == "Lactating Sows")
                {
                    gMethods.FindAndInput(herdOrDiet.AnimalDetails.LactationLength, By.XPath("//div[contains(@id,'lactationLength')]/div[1]/input"), driver, true, false);
                    gMethods.FindAndInput(herdOrDiet.AnimalDetails.LitterWeightAtBirth, By.XPath("//div[contains(@id,'litterWeightBirth')]/div[1]/input"), driver, true, false);
                    gMethods.FindAndInput(herdOrDiet.AnimalDetails.LitterWeightAtWeaning, By.XPath("//div[contains(@id,'litterWeightWeaning')]/div[1]/input"), driver, true, false);
                }

                //GestationLength
                //LitterSize
                if (animalType.TypeName == "Gestating Sows")
                {
                    gMethods.FindAndInput(herdOrDiet.AnimalDetails.GestationLength, By.XPath("//div[contains(@id,'gestationLength')]/div[1]/input"), driver, true, false);
                    gMethods.FindAndInput(herdOrDiet.AnimalDetails.LitterSize, By.XPath("//div[contains(@id,'litterSize')]/div[1]/input"), driver, true, false);

                    //random click
                    //gMethods.FindAndClick(By.XPath("//div[contains(@id,'dailyFeedIntake')]/div[1]/input"), driver, true);
                }

                //AvgEggWeight
                //EggsPerDay
                if (animalType.TypeName == "Laying Hens")
                {
                    gMethods.FindAndInput(herdOrDiet.AnimalDetails.AverageEggWeight, By.XPath("//div[contains(@id,'avgEggWeight')]/div[1]/input"), driver, true, false);
                    gMethods.FindAndInput(herdOrDiet.AnimalDetails.EggsPerDay, By.XPath("//div[contains(@id,'eggsPerDay')]/div[1]/input"), driver, true, false);
                }

                //AvgLiveWeight
                var abwTypeList = new List<string>
                {
                    "Dairy-Heifer Replacements",
                    "Dairy-Dry Cows",
                    "Dairy-Lactating Cows",
                    "Flock Sheep",
                };

                foreach (var type in abwTypeList)
                {
                    if (animalType.TypeName == type)
                    {
                        gMethods.FindAndInput(herdOrDiet.AnimalDetails.Abw, By.XPath("//div[contains(@id,'avgLiveWeight')]/div[1]/input"), driver, true, false);
                    }
                }

                //DaysInMilk
                //MilkProduction
                if (animalType.TypeName == "Dairy-Lactating Cows")
                {
                    gMethods.FindAndInput(herdOrDiet.AnimalDetails.DaysInMilk, By.XPath("//div[contains(@id,'daysInMilk')]/div[1]/input"), driver, true, false);
                    gMethods.FindAndInput(herdOrDiet.AnimalDetails.MilkPerDay, By.XPath("//div[contains(@id,'milkProduction')]/div[1]/input"), driver, true, false);
                }

                //GrowthCapacity
                if (animalType.TypeName == "Weaning Pigs")
                {
                    gMethods.FindAndClick(By.XPath("//label[contains(.,'" + herdOrDiet.AnimalDetails.GrowthCapacity + "') and contains(@id,'growthcap')]"), driver, true);
                }

                SaveButton();
                WaitTillMask();
                gMethods.AddDelay(500);



                k++;
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

        public void Weight(CFarmXmlLister.AnimalType animalType)
        {
            int i = 0;
            foreach (var herdOrDiet in animalType.HerdOrDietList)
            {
                if (i != 0)
                {
                    HerdComplete();
                }

                var abwTypeList = new List<string>
                {
                    "Beef-Heifer Stockers",
                    "Beef-Heifer Replacements",
                    "Beef-Steer Stockers",
                    "American Bison"
                };

                foreach (var type in abwTypeList)
                {
                    if (animalType.TypeName == type)
                    {
                        //ABW
                        gMethods.FindAndInput(herdOrDiet.Weight.Abw, By.XPath("//div[contains(@id,'bodyWeight-bodyEl') and not(contains(@display,'none'))]/input"), driver, true, false);

                        //ADWG
                        gMethods.FindAndInput(herdOrDiet.Weight.Adwg, By.XPath("//div[contains(@id,'dailyWeightGain-bodyEl') and not(contains(@display,'none'))]/input"), driver, true, false);

                        //AMW
                        gMethods.FindAndInput(herdOrDiet.Weight.Amw, By.XPath("//div[contains(@id, 'matureWeight-bodyEl') and not(contains(@display,'none'))]/input"), driver, true, false);

                        gMethods.AddDelay(1000);

                        DeselectClick("/html/body/div[2]");

                        SaveButton();

                        gMethods.AddDelay(1000);
                    }
                }

                if (animalType.TypeName == "Bulls" || animalType.TypeName == "Beef-Mature Cows/Cow-Calf")
                {
                    //ABW
                    gMethods.FindAndInput(herdOrDiet.Weight.Abw, By.XPath("//div[contains(@id,'bodyWeight-bodyEl') and not(contains(@display,'none'))]/input"), driver, true, false);

                    gMethods.AddDelay(1000);

                    DeselectClick("/html/body/div[2]");

                    SaveButton();

                    gMethods.AddDelay(1000);
                }



                if (animalType.TypeName == "Feedlot Cattle")
                {
                    WaitTillMask();
                    gMethods.AddDelay(500);

                    // Breed Combo box
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'primaryBreedFeedlot-triggerWrap')]"), driver, true);

                    // Breed Select
                    gMethods.FindAndClick(By.XPath("//div/ul/li[contains(.,'" + herdOrDiet.Weight.Breed + "')]"), driver, true);

                    // "TMW" box
                    //gMethods.FindAndInput("512", By.XPath("/html/body/div[2]/div[4]/div[4]/div[9]/div[1]/div/div/div[2]/div/table/tbody/tr/td[2]/div/div[1]/input"), driver, true, false);

                    for (int j = 0; j < 2; j++)
                    {
                        // "ADW" box
                        gMethods.FindAndInput(herdOrDiet.Weight.Adwg, By.XPath("/html/body/div[2]/div[4]/div[4]/div[9]/div[" + (2 + j) + "]/div[" + (2 - j) + "]/div/table/tbody/tr/td[2]/div/div/table/tbody/tr/td[1]/div/div[1]/input"), driver, true, false);

                        // ADW Copy
                        gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[9]/div[" + (2 + j) + "]/div[" + (2 - j) + "]/div/table/tbody/tr/td[1]/div/div/label/div/table/tbody/tr/td[2]/a/img"), driver, true);
                    }

                    for (int j = 0; j < 2; j++)
                    {
                        // "ALW" box
                        gMethods.FindAndInput(herdOrDiet.Weight.Amw, By.XPath("/html/body/div[2]/div[4]/div[4]/div[9]/div[" + (4 + j) + "]/div[" + (2 - j) + "]/div/table/tbody/tr/td[2]/div/div/table/tbody/tr/td[1]/div/div[1]/input"), driver, true, false);

                        // ALW Copy
                        gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[9]/div[" + (4 + j) + "]/div[" + (2 - j) + "]/div/table/tbody/tr/td[1]/div/div/label/div/table/tbody/tr/td[2]/a/img"), driver, true);
                    }

                    gMethods.AddDelay(1000);

                    DeselectClick("/html/body/div[2]");

                    SaveButton();

                    gMethods.AddDelay(1000);

                    i++;
                }
            }
        }

        public void HoursOfWork(CFarmXmlLister.AnimalType animalType)
        {
            var workTypeList = new List<string>
                {
                    "Beef-Heifer Stockers",
                    "Beef-Steer Stockers",
                    "Bulls",
                };

            foreach (var type in workTypeList)
            {
                if (animalType.TypeName == type)
                {
                    int i = 0;
                    foreach (var herdOrDiet in animalType.HerdOrDietList)
                    {
                        if (i != 0)
                        {
                            HerdComplete();
                        }

                        // Hours of Work per day
                        gMethods.FindAndInput(herdOrDiet.HoursOfWork.Hours, By.XPath("//div[contains(@id,'numHours')]/input"), driver, true, false);

                        SaveButton();

                        i++;
                    }
                }
            }
        }

        public void FeedDetails(CFarmXmlLister.AnimalType animalType)
        {
            if (animalType.TypeName == "Feedlot Cattle")
            {
                int i = 0;
                foreach (var herdOrDiet in animalType.HerdOrDietList)
                {
                    if (i != 0)
                    {
                        HerdComplete();
                    }
                    WaitTillMask();
                    gMethods.AddDelay(500);

                    //Ionophores button
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[contains(@id,'feedDetailsPanel')]/div/div/div/div/div/div/div/div/div/div[*]/div/label[contains(.,'" + herdOrDiet.FeedDetails.Ionophores + "')]"), driver, true);

                    //Fat Content button
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[contains(@id,'feedDetailsPanel')]/div/div/div[2]/div/div/div/div/div/div/div[*]/div/label[contains(.,'" + herdOrDiet.FeedDetails.FatContent + "')]"), driver, true);

                    //Grain Type button
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[contains(@id,'feedDetailsPanel')]/div/div/div[3]/div/div/div/div/div/div/div[*]/div/label[contains(.,'" + herdOrDiet.FeedDetails.GrainType + "')]"), driver, true);

                    //Concentrate Percentage button
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[contains(@id,'feedDetailsPanel')]/div/div/div[4]/div/div/div/div/div/div/div[*]/div/label[contains(.,'" + herdOrDiet.FeedDetails.ConcentratePercentage + "')]"), driver, true);

                    SaveButton();

                    gMethods.AddDelay(1000);

                    i++;
                }
            }
        }

        public void PregnancyAndMilk(CFarmXmlLister.AnimalType animalType)
        {
            if (animalType.TypeName == "Beef-Mature Cows/Cow-Calf")
            {
                int j = 0;
                foreach (var herdOrDiet in animalType.HerdOrDietList)
                {
                    if (j != 0)
                    {
                        HerdComplete();
                    }

                    var monthLabel = new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    for (int i = 0; i < 12; i++)
                    {
                        gMethods.FindAndInput(herdOrDiet.Pregnancy.Pregnant, By.XPath("//div[contains(@id, '" + monthLabel[i] + "Pregnant') and not(contains(@display,'none'))]/div/input"), driver, true, false);

                        gMethods.FindAndInput(herdOrDiet.Pregnancy.Lactating, By.XPath("//div[contains(@id, '" + monthLabel[i] + "Lactating') and not(contains(@display,'none'))]/div/input"), driver, true, false);
                    }

                    gMethods.FindAndInput(herdOrDiet.Pregnancy.MilkPerDay, By.XPath("//div[contains(@id, 'dailyMilkProduction-bodyEl')]/input"), driver, true, false);

                    gMethods.FindAndInput(herdOrDiet.Pregnancy.FatContent, By.XPath("//div[contains(@id, 'milkFatContent-bodyEl')]/input"), driver, true, false);

                    gMethods.AddDelay(500);

                    SaveButton();

                    gMethods.AddDelay(500);


                    j++;
                }
            }
        }

        public void TypesOfFeed(CFarmXmlLister.AnimalType animalType)
        {
            var feedTypeList = new List<string>
                {
                    "Beef-Mature Cows/Cow-Calf",
                    "Beef-Heifer Stockers",
                    "Beef-Heifer Replacements",
                    "Beef-Steer Stockers",
                    "Bulls",
                    "Dairy-Heifer Replacements",
                    "Dairy-Dry Cows",
                    "Dairy-Lactating Cows",
                    "Feeder Sheep",
                    "Flock Sheep",
                };



            foreach (var type in feedTypeList)
            {
                if (animalType.TypeName == type)
                {
                    foreach (var herdOrDiet in animalType.HerdOrDietList)
                    {
                        WaitTillMask();
                        gMethods.AddDelay(500);

                        // Resets Monthly table
                        gMethods.FindAndClick(By.XPath("//span[contains(.,'Remove all the dates and start over')]"), driver, true);
                        WaitTillMask();
                        gMethods.AddDelay(2500);

                        // "Month" CheckBox for feedlot cattle
                        if (animalType.TypeName == "Beef-Heifer Stockers" || animalType.TypeName == "Beef-Steer Stockers")
                        {
                            gMethods.FindAndClick(By.XPath("//div[contains(@id, 'gridcolumn-1089')]"), driver, true);
                        }

                        //Month checkbox for ND cow-calf
                        if (animalType.TypeName == "Beef-Heifer Replacements" || animalType.TypeName == "Beef-Mature Cows/Cow-Calf" || animalType.TypeName == "Bulls")
                        {
                            gMethods.FindAndClick(By.XPath("//div[contains(@id, 'gridcolumn-1087')]"), driver, true);
                        }

                        //Month checkbox for sheep
                        if (animalType.TypeName.Contains("Sheep"))
                        {
                            gMethods.FindAndClick(By.XPath("//div[contains(@id, 'gridcolumn-1079')]"), driver, true);
                        }

                        //"Month" Checkbox for Dairy
                        if (animalType.TypeName.Contains("Dairy"))
                        {
                            gMethods.FindAndClick(By.XPath("//div[contains(@id, 'gridcolumn-1088')]"), driver, true);
                        }
                        WaitTillMask();
                        gMethods.AddDelay(500);


                        foreach (var feed in herdOrDiet.TypesOfFeed.FeedList)
                        {
                            // "Feedstuff" Double click
                            gMethods.FindAndClick(By.XPath("//div[contains(@id,'typesOfFeedPanel-body')]/div/div/div/div/div[2]/div/div[2]/div/input"), driver, true);
                            gMethods.EnterKeys(By.XPath("//div[contains(@id,'typesOfFeedPanel-body')]/div/div/div/div/div[2]/div/div[2]/div/input"), driver, Keys.Control, "A", null, 1);
                            gMethods.AddDelay(500);
                            gMethods.EnterKeys(By.XPath("//div[contains(@id,'typesOfFeedPanel-body')]/div/div/div/div/div[2]/div/div[2]/div/input"), driver, null, Keys.Backspace, null, _backspaces + 1);
                            gMethods.AddDelay(500);

                            gMethods.FindAndInput(feed.FeedStuff, By.XPath("//div[contains(@id,'typesOfFeedPanel-body')]/div/div/div/div/div[2]/div/div[2]/div/input"), driver, true, false);

                            try
                            {
                                try
                                {
                                    IWebElement element = driver.Driver.FindElement(By.XPath("//div[contains(.,'" + feed.FeedStuff + "')]"));
                                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver.Driver;
                                    js.ExecuteScript("arguments[0].scrollIntoView(true);", element);

                                    gMethods.AddDelay(500);
                                }
                                catch (System.Exception e)
                                {
                                    //var F = e;

                                }

                                gMethods.FindAndClick(By.XPath("//div[contains(.,'" + feed.FeedStuff + "')]"), driver, true);
                            }
                            catch
                            {
                                try
                                {
                                    IWebElement element = driver.Driver.FindElement(By.XPath("span[contains(.,'" + feed.FeedStuff + "')]"));
                                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver.Driver;
                                    js.ExecuteScript("arguments[0].scrollIntoView(true);", element);
                                }
                                catch { }
                                gMethods.AddDelay(500);
                                gMethods.FindAndClick(By.XPath("//span[contains(.,'" + feed.FeedStuff + "')]"), driver, true);
                            }

                            if (feed.FeedStuff != "Corn")
                            {
                                // "Category"
                                gMethods.AddDelay(1000);
                                //gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[4]/div[3]/div/div/div/div[2]/div/div[1]/div[3]/div/table/tbody/tr[*]/td/div[contains(.,'" + feed.Category + "')]"), driver, true);
                                gMethods.FindAndClick(By.XPath("//table/tbody/tr[*]/td/div[contains(.,'" + feed.Category + "')]"), driver, true);
                            }
                            else
                            {
                                try
                                {
                                    //gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[4]/div[3]/div/div/div/div[2]/div/div[1]/div[3]/div/table/tbody/tr[*]/td/div[contains(.,'" + feed.Category + "')]"), driver, true);
                                    gMethods.FindAndClick(By.XPath("//table/tbody/tr[*]/td/div[contains(.,'" + feed.Category + "')]"), driver, true);

                                }
                                catch
                                {
                                    //gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[4]/div[3]/div/div/div/div[2]/div/div[1]/div[3]/div/table/tbody/tr[*]/td/div[contains(.,'Grain, High Moisture')]"), driver, true);
                                    //gMethods.FindAndClick(By.XPath("//table/tbody/tr[*]/td/div[contains(.,'Grain, High Moisture')]"), driver, true);
                                    //gMethods.FindAndClick(By.XPath("//table/tbody/tr[*]/td/div[contains(.,'Grain')]"), driver, true);

                                    gMethods.AddDelay(500);
                                    //gMethods.EnterKeys(By.XPath("/html/body"), driver, null, Keys.Down, null, 20);
                                    gMethods.EnterKeys(By.XPath("//div[contains(@id, 'gridview-1051')]"), driver, null, Keys.Down, null, 20);
                                    gMethods.AddDelay(500);
                                    //gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[4]/div[3]/div/div/div/div[2]/div/div[1]/div[3]/div/table/tbody/tr[*]/td/div[contains(.,'" + feed.Category + "')]"), driver, true);
                                    gMethods.FindAndClick(By.XPath("//table/tbody/tr[*]/td/div[contains(.,'" + feed.Category + "')]"), driver, true);

                                }
                            }

                            gMethods.AddDelay(1000);

                            // "Add" Button
                            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[4]/div[3]/div/div/div/div[3]/div/div[1]/div/div[1]/em/button"), driver, true);
                            WaitTillMask();
                            gMethods.AddDelay(500);

                            _backspaces = feed.FeedStuff.Length;

                            gMethods.AddDelay(1000);
                        }

                        // "Confirm Selections" Button
                        gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[4]/div[3]/div/div/div/div[5]/div/div[1]/em/button"), driver, true);

                        gMethods.AddDelay(500);

                        int j = 0;
                        foreach (var feed in herdOrDiet.TypesOfFeed.FeedList)
                        {
                            // "percentage eaten"
                            gMethods.FindAndInput(feed.Percent, By.XPath("//div[contains(@id,'feedList" + j + "')]/div/input"), driver, true, false);

                            j++;
                        }
                        WaitTillMask();

                        gMethods.AddDelay(500);

                        // "Done" Button
                        gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[4]/div[5]/div/div/em/button"), driver, true);
                        WaitTillMask();
                        gMethods.AddDelay(500);

                        // "Ok" Button for Successfully Saved message
                        OkButton();

                        gMethods.AddDelay(500);

                        SaveButton();
                    }
                }
            }
        }

        public void FeedingSituation(CFarmXmlLister.AnimalType animalType)
        {
            var feedTypeList = new List<string>
                {
                    "Beef-Mature Cows/Cow-Calf",
                    "Beef-Heifer Stockers",
                    "Beef-Heifer Replacements",
                    "Beef-Steer Stockers",
                    "Bulls",
                    //"Gestating Sows",
                    "American Bison"
                };

            foreach (var type in feedTypeList)
            {
                if (animalType.TypeName == type)
                {
                    int i = 0;
                    foreach (var herdOrDiet in animalType.HerdOrDietList)
                    {
                        if (i != 0)
                        {
                            HerdComplete();
                        }
                        WaitTillMask();
                        gMethods.AddDelay(500);

                        // "treatment method" menu
                        gMethods.FindAndClick(By.XPath("//div[contains(@id, 'feedingSituation-triggerWrap')]/div"), driver, true);
                        WaitTillMask();
                        gMethods.AddDelay(500);

                        // "Primary Method"
                        gMethods.FindAndClick(By.XPath("//div[not(contains(@style, 'display: none'))]/div/ul/li[contains(.,'" + herdOrDiet.FeedingSituation.Type + "')]"), driver, true);


                        gMethods.AddDelay(500);

                        SaveButton();
                        WaitTillMask();
                        gMethods.AddDelay(500);

                        i++;
                    }
                }
            }
        }

        public void HousingDetails(CFarmXmlLister.AnimalType animalType)
        {
            var feedTypeList = new List<string>
                {
                    "Dairy-Heifer Replacements",
                    "Dairy-Dry Cows",
                    "Dairy-Lactating Cows",
                    "Gestating Sows",
                    "Grow-Finish Pigs",
                    "Lactating Sows",
                    "Weaning Pigs"
                };

            foreach (var type in feedTypeList)
            {
                if (animalType.TypeName == type)
                {
                    int i = 0;
                    foreach (var herdOrDiet in animalType.HerdOrDietList)
                    {
                        if (i != 0)
                        {
                            HerdComplete();
                        }
                        WaitTillMask();
                        gMethods.AddDelay(500);

                        if (!animalType.TypeName.Contains("Dairy"))
                        {
                            // Housing Type
                            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[3]/div/div/div[1]/div[1]/div/div[1]/div/div/div[*]/div[1]/div[1]/label[contains(.,'" + herdOrDiet.HousingDetails.Type + "')]"), driver, true);
                        }

                        // Roofed Type
                        gMethods.FindAndClick(By.XPath("//label[contains(.,'" + herdOrDiet.HousingDetails.RoofedType + "') and not(contains(@display,'none'))]"), driver, true);

                        if (herdOrDiet.HousingDetails.RoofedType == "Flushed Or Scraped")
                        {
                            //Area of Barn
                            gMethods.FindAndInput(herdOrDiet.HousingDetails.AreaOfBarn, By.XPath("//div[contains(@id,'AreaOfBarn')]/div/input"), driver, true, false);
                        }
                        else
                        {
                            // DepthOrMix Type
                            gMethods.FindAndClick(By.XPath("//label[contains(.,'" + herdOrDiet.HousingDetails.DepthOrMix + "')]"), driver, true);

                            //DaysInHousing
                            gMethods.FindAndInput(herdOrDiet.HousingDetails.DaysInHousing, By.XPath("//div[contains(@id,'DaysInHousing')]/div/input"), driver, true, false);
                        }
                        WaitTillMask();
                        gMethods.AddDelay(500);

                        DeselectClick("/html/body/div[2]");

                        SaveButton();
                        WaitTillMask();
                        gMethods.AddDelay(500);

                        i++;
                    }
                }
            }
        }



        public void ManureSystemTypes(CFarmXmlLister.AnimalType animalType)
        {
            int i = 0;


            foreach (var herdOrDiet in animalType.HerdOrDietList)
            {
                if (i != 0)
                {
                    HerdComplete();
                }
                WaitTillMask();
                gMethods.AddDelay(500);

                // "treatment method" menu
                gMethods.FindAndClick(By.XPath("//div[contains(@id, 'manureTreatment')]/div/div[2]/div[1]"), driver, true);

                gMethods.AddDelay(600);

                // "Primary Method"
                gMethods.FindAndClick(By.XPath("//div[not(contains(@display, 'none'))]/ul/li[contains(.,'" + herdOrDiet.ManureSystemTypes.PrimaryMethod + "')]"), driver, false);

                //"Shared Lagoon"
                if (herdOrDiet.ManureSystemTypes.PrimaryMethod == "Anaerobic lagoon, liquid/slurry storage pond, storage tanks")
                {

                    if (ABL)
                    {
                        gMethods.AddDelay(500);
                        //Unable to click on this..needs further investigation

                        gMethods.FindAndClick(By.XPath("//div[contains(@id, 'manureTreatment-triggerWrap')]"), driver, true);
                        gMethods.FindAndClick(By.XPath("//div/ul/li[contains(.,'Anaerobic lagoon, liquid/slurry storage pond, storage tanks')]"), driver, true);
                        //gMethods.FindAndClick(By.XPath("//div[contains(@id, 'SharedLagoon_0')]"), driver, true);
                        //gMethods.FindAndClick(By.XPath("//div/label/div/div/div/div/div/div/div/div[contains(@id, 'SharedLagoon_0-bodyEl')]"), driver, true);
                        gMethods.FindAndClick(By.XPath("//div/div/div[contains(@id, 'SharedLagoon_0-bodyEl')]"), driver, true);

                        //gMethods.FindAndClick(By.XPath("//div[contains(@id, 'SharedLagoon_0')]"), driver, true);

                        gMethods.AddDelay(500);
                    }
                    ABL = true;
                }

                WaitTillMask();
                gMethods.AddDelay(500);

                SaveButton();

                gMethods.AddDelay(1000);

                i++;
            }
        }


        public void ManureSystemDetails(CFarmXmlLister.AnimalType animalType)
        {
            int i = 0;
            foreach (var herdOrDiet in animalType.HerdOrDietList)
            {
                if (i != 0)
                {
                    HerdComplete();
                }
                WaitTillMask();
                gMethods.AddDelay(500);

                if (animalType.TypeName == "Dairy-Heifer Replacements")
                {
                    //Cover Type Menu
                    gMethods.FindAndClick(By.XPath("//div[contains(@id, 'systemCoverType-triggerWrap')]"), driver, true);

                    //Cover Type Selection
                    gMethods.FindAndClick(By.XPath("/html/body/div[*]/div//ul/li[contains(.,'" + herdOrDiet.ManureSystemDetails.AnaerobicLagoon.CoverType + "')]"), driver, true);

                    //Click on Surface area click box
                    //gMethods.FindAndClick(By.XPath("//div[contains(@id, 'exposedSufaceArea-bodyEl')]"), driver, true);

                    // input value Exposed Surface Area click box
                    gMethods.FindAndInput(herdOrDiet.ManureSystemDetails.AnaerobicLagoon.Area, By.XPath("//div[contains(@id,'exposedSufaceArea')]/div/input"), driver, true, false);


                    //Click Year Dropdown
                    gMethods.FindAndClick(By.XPath("//div[contains(@id, 'storagedate-triggerWrap')]"), driver, true);

                    //Click Year Selection
                    gMethods.FindAndClick(By.XPath("/html/body/div[*]/div//ul/li[contains(.,'" + herdOrDiet.ManureSystemDetails.AnaerobicLagoon.Date + "')]"), driver, true);

                    //Length of time Dropdown
                    gMethods.FindAndClick(By.XPath("//div[contains(@id, 'storagelength-triggerWrap')]"), driver, true);

                    //Length of time Selection
                    gMethods.FindAndClick(By.XPath("/html/body/div[*]/div//ul/li[contains(.,'" + herdOrDiet.ManureSystemDetails.AnaerobicLagoon.StorageLength + "')]"), driver, true);

                    //Percent Solid Removal
                    gMethods.FindAndInput(herdOrDiet.ManureSystemDetails.AnaerobicLagoon.SolidRemoval, By.XPath("//div[contains(@id,'percentSolidRemoval')]/div/input"), driver, true, false);

                }

                if (herdOrDiet.ManureSystemTypes.PrimaryMethod == "Temporary Stack and Long-Term Stockpile")
                {
                    //Cover Type menu
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[contains(@id,'manureSystemDetail')]/div/div/div[1]/div[1]/div[2]/div[1]"), driver, true);

                    //Cover Type Selection
                    gMethods.FindAndClick(By.XPath("/html/body/div[*]/div//ul/li[contains(.,'" + herdOrDiet.ManureSystemDetails.TemporaryStackAndLongTermStockpile.CoverType + "')]"), driver, true);

                    //Storage time menu
                    gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[4]/div[contains(@id,'manureSystemDetail')]/div/div/div[2]/div[1]/div[2]/div[1]"), driver, true);

                    //Storage time selection
                    gMethods.FindAndClick(By.XPath("/html/body/div[*]/div//ul/li[contains(.,'" + herdOrDiet.ManureSystemDetails.TemporaryStackAndLongTermStockpile.StorageTerm + "')]"), driver, true);
                }

                SaveButton();
                WaitTillMask();
                gMethods.AddDelay(500);

                i++;
            }
        }

        public void ManureDetails(CFarmXmlLister.AnimalType animalType)
        {
            int i = 0;
            foreach (var herdOrDiet in animalType.HerdOrDietList)
            {
                if (i != 0)
                {
                    HerdComplete();
                }
                WaitTillMask();
                gMethods.AddDelay(500);

                //Inputs data into first month box
                gMethods.FindAndInput(herdOrDiet.ManureDetails.DryManureTotal, By.XPath("//div[contains(@id, 'JanaverageMass-bodyEl') and not(contains(@display,'none'))]/input"), driver, true, false);

                //Copies all values into Month slots
                gMethods.FindAndClick(By.XPath("//label[contains(@id, 'JanaverageMass-labelEl') and not(contains(@display,'none'))]/div/a/img"), driver, true);

                //Inputs data into nitrogen content month box
                gMethods.FindAndInput(herdOrDiet.ManureDetails.PercentNitrogen, By.XPath("//div[contains(@id, 'JannitrogenManure-bodyEl') and not(contains(@display,'none'))]/input"), driver, true, false);

                //Copies all values into month slots in nitrogen content area
                gMethods.FindAndClick(By.XPath("//label[contains(@id, 'JannitrogenManure-labelEl') and not(contains(@display,'none'))]/div/a/img"), driver, true);

                gMethods.AddDelay(1000);

                SaveButton();
                WaitTillMask();
                gMethods.AddDelay(500);

                i++;
            }
        }

        public void SaveButton()
        {
            gMethods.AddDelay(500);

            // "Save and Continue" Button
            gMethods.FindAndClick(By.Id("saveButton"), driver, true);

            gMethods.AddDelay(500);
        }

        public void CreateAnimalAgScenario(CFarmXmlLister.AnimalAgScenario scenario)
        {
            gMethods.AddDelay(500);

            try
            {

                //   gMethods.FindAndClick(By.XPath("div[contains(@id,'baselineFirstTime')]/div[2]/div[0]/div"), driver, true);
                gMethods.FindAndClick(By.XPath("div[contains(@id, 'baselineFirstTime')] / div[2] / div[0] / div"), driver, true);

            }
            catch
            {


            }
            // Scenario input
            gMethods.FindAndInput(scenario.Name, By.XPath("/html/body/div[contains(@id,'newScenarioWindow')]/div[2]/div[1]/div/div/div[2]/div/div[1]/div[1]/input"), driver, true, false);

            if (!scenario.CopyFromFuture)
            {
                // "Copy" checkbox
                gMethods.FindAndClick(By.XPath("/html/body/div[contains(@id,'newScenarioWindow')]/div[2]/div[1]/div/div/div[2]/div/div[2]/div[1]/input"), driver, true);
            }

            gMethods.AddDelay(500);

            // "Create new Scenario" button
            gMethods.FindAndClick(By.XPath("/html/body/div[contains(@id,'newScenarioWindow')]/div[2]/div[2]/div/span[2]/a/span[2]"), driver, true);

            if (scenario.CopyFromFuture)
            {
                WaitTillMask();
                gMethods.AddDelay(500);

                // "Manure Details" Link
                gMethods.FindAndClick(By.Id("manureDetailsLink"), driver, true);
                WaitTillMask();
                gMethods.AddDelay(500);

                SaveButton();

                herdClick();
            }
        }

        public void herdClick()
        {
            try
            {
                gMethods.AddDelay(800);
                HerdComplete();
                gMethods.AddDelay(800);
                SaveButton();
                gMethods.AddDelay(800);
                herdClick();
            }
            catch
            {

            }
        }

        public void EndAnimalAg()
        {
            gMethods.AddDelay(1000);

            // "Report" Button
            gMethods.FindAndClick(By.XPath("/html/body/div[contains(@id,'newScenarioWindow')]/div[2]/div[2]/div/span[1]/a"), driver, true);
        }

        public void DeselectClick(string locator)
        {
            // Body Click
            gMethods.FindAndClick(By.XPath(locator), driver, true);

            gMethods.AddDelay(500);
        }

        public void OkButton()
        {
            // "Ok" button
            gMethods.FindAndClick(By.XPath("//button[contains(.,'Ok') and not(contains(@display,'none'))]"), driver, true);
        }

        /// <summary>
        /// Used for finding the id of population input boxes
        /// </summary>
        /// <param name="typeName"></param>
        /// <returns></returns>
        public string ShortName(string typeName)
        {
            var shortName = "";
            if (typeName == "Heifers")
            {
                shortName = "heifers";
            }
            //if (typeName == "Weaning Pigs")
            // {
            //    shortName = "swine";
            // }
            if (typeName == "Roofed Facility")
            {
                shortName = "roofedfacility";
            }
            if (typeName == "Dry Lot")
            {
                shortName = "drylot";
            }
            if (typeName == "Pasture Range")
            {
                shortName = "pasturerange";
            }
            if (typeName == "Steers")
            {
                shortName = "steers";
            }
            if ((typeName == "Grow-Finish Pigs") || (typeName == "Gestating Sows") || (typeName == "Lactating Sows") || (typeName == "Weaning Pigs"))
            {
                shortName = "swine";
            }

            return shortName;
        }

        public void HerdComplete()
        {
            //Next Diet button
            gMethods.FindAndClick(By.XPath("/html/body/div[contains(@id,'herdComplete')]/div[3]/div[1]/div[2]/em/button"), driver, false);
        }
    }
}

