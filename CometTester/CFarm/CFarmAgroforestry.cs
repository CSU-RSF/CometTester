using OpenQA.Selenium;

namespace CometTester.CFarm
{
    class CFarmAgroforestry
    {
        GeneralTestMethods gMethods;
        BrowserDriver driver;

        public CFarmAgroforestry(GeneralTestMethods gMethods, BrowserDriver driver, string whichTest)
        {
            this.gMethods = gMethods;
            this.driver = driver;

            ManagementLocation();
            CurrentAgroforestry();
            EndAgroforestry();
        }

        public void ManagementLocation()
        {
            // "State" Menu
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div[2]/div/div/div[1]/div[1]/div[2]/div[1]"), driver, true);            

            // "Iowa"
            gMethods.FindAndClick(By.XPath("/html/body/div[8]/div/ul/li[14]"), driver, true);

            gMethods.AddDelay(500);

            // "County" Menu
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div[2]/div/div/div[2]/div[1]/div[2]/div[1]"), driver, true);

            // "Adair County"
            gMethods.FindAndClick(By.XPath("/html/body/div[9]/div/ul/li[1]"), driver, true);

            gMethods.AddDelay(500);

            // "Next" Button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[2]/span/a/span[2]"), driver, true);
        }

        public void CurrentAgroforestry()
        {
            // "Species Group" Menu
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/table[1]/tbody/tr/td[1]/div/div[1]/div/div[1]/div[1]/div[2]/div[1]"), driver, true);

            // "Maples"
            gMethods.FindAndClick(By.XPath("/html/body/div[11]/div/ul/li[2]"), driver, true);

            gMethods.AddDelay(500);

            // "Species" Menu
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/table[1]/tbody/tr/td[1]/div/div[1]/div/div[2]/div[1]/div[2]/div[1]"), driver, true);

            // "Other Maple"
            gMethods.FindAndClick(By.XPath("/html/body/div[12]/div/ul/li[3]"), driver, true);

            gMethods.AddDelay(500);

            // "Address Query" box
            gMethods.FindAndInput("30", By.XPath("/html/body/div[2]/div[3]/table[1]/tbody/tr/td[1]/div/div[1]/div/div[3]/div[1]/input"), driver, true, false);

            // "Address Query" box
            gMethods.FindAndInput("500", By.XPath("/html/body/div[2]/div[3]/table[1]/tbody/tr/td[1]/div/div[1]/div/div[5]/div[1]/input"), driver, true, false);

            // "Add Species" Button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/table[1]/tbody/tr/td[1]/div/div[1]/div/div[6]/em/button"), driver, true);

            // "Next" Button
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[4]/div[2]/span/a/span[2]"), driver, true);
        }

        public void EndAgroforestry()
        {
            gMethods.AddDelay(500);

            // "Continue" Button
            gMethods.FindAndClick(By.XPath("/html/body/div[13]/div[2]/div/span[2]/a/span[2]"), driver, true);
        }

    }
}
