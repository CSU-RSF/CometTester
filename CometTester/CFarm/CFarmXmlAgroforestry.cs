using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace CometTester.CFarm
{
    class CFarmXmlAgroforestry
    {
        GeneralTestMethods gMethods;
        BrowserDriver driver;
        private CFarmXmlLister.CFarmAgroforestryData xmlAgroforestry;

        public CFarmXmlAgroforestry(GeneralTestMethods gMethods, BrowserDriver driver, CFarmXmlLister.CFarmAgroforestryData xmlAgroforestry)
        {
            this.gMethods = gMethods;
            this.driver = driver;
            this.xmlAgroforestry = xmlAgroforestry;

            ManagementLocation();

            foreach (var species in xmlAgroforestry.AgroforestrySpeciesList)
            {
                CurrentAgroforestry(species);
            }

            EndAgroforestry();
        }

        public void ManagementLocation()
        {
            // "State" Menu
            gMethods.FindAndClick(By.XPath("//div[contains(@id, 'stateList-triggerWrap')]"), driver, true);

            gMethods.AddDelay(2000);

            // "State"
            gMethods.FindAndClick(By.XPath("//div/ul/li[contains(.,'" + xmlAgroforestry.AgroforestryLocation.States + "')]"), driver, true);

            gMethods.AddDelay(500);

            // "County" Menu
            gMethods.FindAndClick(By.XPath("//div[contains(@id, 'countyList-triggerWrap')]"), driver, true);

            gMethods.AddDelay(500);
            // "County"
            gMethods.FindAndClick(By.XPath("//div/ul/li[contains(.,'" + xmlAgroforestry.AgroforestryLocation.County + "')]"), driver, true);

            gMethods.AddDelay(500);

            // "Next" Button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[2]/span/a/span[2]"), driver, true);
        }

        public void CurrentAgroforestry(CFarmXmlLister.AgroforestrySpecies species)
        {
            gMethods.AddDelay(500);

            // "Species Group" Menu
            gMethods.FindAndClick(By.XPath("//div[contains(@id, 'species group-triggerWrap')]"), driver, true);

            // "Maples"
            gMethods.FindAndClick(By.XPath("//div/ul/li[contains(.,'" + species.SpeciesGroup + "')]"), driver, true);

            gMethods.AddDelay(500);

            // "Species" Menu
            gMethods.FindAndClick(By.XPath("//div[contains(@id, 'species-triggerWrap')]"), driver, true);

            // "Other Maple"
            gMethods.FindAndClick(By.XPath("//div/ul/li[contains(.,'" + species.SpeciesName + "')]"), driver, true);

            gMethods.AddDelay(500);

            // "DBH"
            gMethods.FindAndInput(species.Dbh, By.XPath("/html/body/div[2]/div[3]/table[1]/tbody/tr/td[1]/div/div[1]/div/div[3]/div[1]/input"), driver, true, false);

            if (species.Dbh == "")
            {
                // "Age"
                gMethods.FindAndInput(species.Age, By.XPath("/html/body/div[2]/div[3]/table[1]/tbody/tr/td[1]/div/div[1]/div/div[4]/div[1]/input"), driver, true, false);
            }

            // "Total Number"
            gMethods.FindAndInput(species.TotalNumber, By.XPath("/html/body/div[2]/div[3]/table[1]/tbody/tr/td[1]/div/div[1]/div/div[5]/div[1]/input"), driver, true, false);

            // "Add Species" Button
            gMethods.FindAndClick(By.XPath("//div[contains(@id, 'AddSpeciesButton')]"), driver, true);
        }

        public void EndAgroforestry()
        {
            // "Next" Button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[2]/span/a/span[2]"), driver, true);

            gMethods.AddDelay(500);

            // "Continue" Button
            gMethods.FindAndClick(By.XPath("//a/span[contains(text(),'Continue to ')]"), driver, true);
        }

    }
}
