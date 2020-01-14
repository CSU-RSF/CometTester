using System;
using System.Collections.Generic;
using OpenQA.Selenium;
using System.Web.UI;
using System.IO;
using System.Text;

namespace CometTester.CFarm
{
    /// <summary>
    /// Manages and compares the Report values and the Expected values
    /// Forms html tables to display report value comparison
    /// </summary>
    class CFarmXmlReport
    {
        GeneralTestMethods gMethods;
        BrowserDriver driver;
        private string whichTest;
        private CFarmXmlLister.CFarmReportData xmlReport;
        private List<CroplandValues> CroplandReports;
        private List<AnimalAgValues> AnimalAgReports;
        private List<AgroforestryValues> AgroforestryReports;
        private List<ForestryValues> ForestryReports;

        public CFarmXmlReport(GeneralTestMethods gMethods, BrowserDriver driver, CFarmXmlLister cFarmXml, CFarmXmlLister.CFarmReportData xmlReport)
        {
            this.gMethods = gMethods;
            this.driver = driver;
            //this.whichTest = whichTest;
            this.xmlReport = xmlReport;
            CroplandReports = new List<CroplandValues>();
            AnimalAgReports = new List<AnimalAgValues>();
            AgroforestryReports = new List<AgroforestryValues>();
            ForestryReports = new List<ForestryValues>();

            gMethods.AddDelay(2000);

          
        
            // If the xml filename contains these strings
            if (cFarmXml._xmlDoc.CFarmCroplandsData != null)//(whichTest.Contains("Cropland"))
            {

                gMethods.Wait30Minutes(By.XPath("//span[contains(.,'100% Complete')]"), driver);
                CroplandsReport();
            }
            if (cFarmXml._xmlDoc.CFarmAnimalAgData != null)//(whichTest.Contains("AnimalAg"))
            {
                gMethods.WaitLongTime(By.XPath("//span[contains(.,'Source') and not(contains(@display,'none'))]"), driver);

                AnimalAgReport();
            }
            if (cFarmXml._xmlDoc.CFarmAgroforestryData != null) //(whichTest.Contains("Agroforestry"))
            {
                gMethods.WaitLongTime(By.XPath("//span[contains(.,'Source') and not(contains(@display,'none'))]"), driver);

                AgroforestryReport();
            }
            if (cFarmXml._xmlDoc.CFarmForestryData != null) //(whichTest.Contains("Forestry"))
            {
                gMethods.WaitLongTime(By.XPath("//span[contains(.,'Source') and not(contains(@display,'none'))]"), driver);

                ForestryReport();
            }
        }

        public void CroplandsReport()
        {
            string reportValue;
            int scenarioCount = 0;

            foreach (var scenario in xmlReport.CroplandsReportScenarioList)
            {
                int parcelCount = 1;
                foreach (var report in scenario.CroplandsReportList)
                {
                    var cValues = new CroplandValues();
                    cValues.ScenarioName = scenario.Name;
                    cValues.ParcelName = report.Name;
                    cValues.SetExpected(report);

                    //Select Croplands Tab
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'reportTabs')]/div/div/div/div[*]/em/button/span[contains(.,'Cropland, Pasture, Range')]"), driver, true);

                    // gMethods.AddDelay(50000);  //OK for one, we already waited until th ecropland report hit 100%

                    // Grabs the values off the report table and compares to expected values
                    int tdval = 0;
                    if (scenarioCount == 0)
                    {
                        tdval = 6;
                    }
                    else
                    {
                        tdval = 8+((scenarioCount-1)*4);
                    }
                    

                    for (int i = 0; i < 5; i++)
                    {
                        reportValue = gMethods.FindValue(By.XPath("//div[contains(@id,'fieldReportContainer')]/div/div/div/table/tbody/tr[" + (parcelCount * 3) + "]/td/table/tbody/tr[" + (i + 2) + "]/td[" + tdval + "]/div"), driver);

                        cValues.SetActual(i, reportValue);
                    }
                   
                    reportValue = gMethods.FindValue(By.XPath("//div[contains(@id,'fieldReportContainer')]/div/div/div/table/tbody/tr[" + ((parcelCount * 3)+1) + "]/td[" + tdval + "]/div"), driver);

                       //reportValue = gMethods.FindValue(By.XPath("//div[contains(@id,'reportTabContainer')]/div/div[2]/div[1]/div/div[3]/div/div[1]/div/table/tbody/tr[" + (parcelCount * 3 + 1) + "]/td[" + (scenarioCount + 6) + "]/div"), driver);

                    cValues.SetActual(5, reportValue);

                    cValues.CalculatePYield();

                    CroplandReports.Add(cValues);

                    parcelCount++;
                }
                scenarioCount++;
                
            }
        }

        public void AnimalAgReport()
        {
            string reportValue;
            int scenarioCount = 0;

            foreach (var scenario in xmlReport.AnimalAgReportScenarioList)
            {
                int typeCount = 1;
                foreach (var report in scenario.AnimalAgReportList)
                {
                    var aValues = new AnimalAgValues();
                    
                    aValues.SetExpected(report);

                    //Select AnimalAg Tab
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'reportTabs')]/div/div/div/div[*]/em/button/span[contains(.,'Animal Agriculture')]"), driver, true);

                    gMethods.AddDelay(2000);

                    // Grabs the values off the report table and compares to expected values
                    for (int i = 0; i < 2; i++)
                    {
                        reportValue = gMethods.FindValue(By.XPath("//div[contains(@id,'animalReportContainer')]/div/div[1]/div/table/tbody/tr[" + (typeCount * 3) + "]/td/table/tbody/tr[" + (i + 2) + "]/td[" + (scenarioCount + 6) + "]/div"), driver);

                        gMethods.AddDelay(300);

                        aValues.SetActual(i, reportValue);
                    }

                    reportValue = gMethods.FindValue(By.XPath("//div[contains(@id,'animalReportContainer')]/div/div[1]/div/table/tbody/tr[" + (typeCount * 3 + 1) + "]/td[" + (scenarioCount + 6) + "]/div"), driver);

                    aValues.SetActual(2, reportValue);

                    aValues.CalculatePYield();

                    AnimalAgReports.Add(aValues);

                    typeCount++;
                }
                scenarioCount++;
                if (scenario.Name != "Baseline") scenarioCount++;
            }
        }

        public void AgroforestryReport()
        {


            string reportValue;
            int scenarioCount = 0;

            foreach (var scenario in xmlReport.AgroforestryReportScenarioList)
            {
                int parcelCount = 0;
                foreach (var report in scenario.AgroforestryReportList)
                {
                    var aValues = new AgroforestryValues();

                    aValues.SetExpected(report);

                    //Select Croplands Tab
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'reportTabs')]/div/div/div/div[*]/em/button/span[contains(.,'Agroforestry')]"), driver, true);

                    gMethods.AddDelay(2000);

                    // Grabs the values off the report table and compares to expected values
                    for (int i = 0; i < 5; i++)
                    {
                        //reportValue = gMethods.FindValue(By.XPath("//div[contains(@id,'reportTabContainer')]/div/div[2]/div[1]/div/div[3]/div/div[1]/div/table/tbody/tr[" + (parcelCount * 3) + "]/td/table/tbody/tr[" + (i + 2) + "]/td[" + (scenarioCount + 6) + "]/div"), driver);
                        reportValue = gMethods.FindValue(By.XPath("//div[contains(@id,'13_AGRO-body')]/div/div/div/div/table/tbody/tr[3]/td/table/tbody/tr[" + (2 + i) + "]/td[5]/div"), driver);

                        gMethods.AddDelay(300);

                        aValues.SetActual(i, reportValue);
                    }

                    //reportValue = gMethods.FindValue(By.XPath("//div[contains(@id,'reportTabContainer')]/div/div[2]/div[1]/div/div[3]/div/div[1]/div/table/tbody/tr[" + (parcelCount * 3 + 1) + "]/td[" + (scenarioCount + 6) + "]/div"), driver);
                    reportValue = gMethods.FindValue(By.XPath("//div[contains(@id,'13_AGRO-body')]/div/div/div/div/table/tbody/tr[4]/td[5]/div"), driver);

                    aValues.SetActual(5, reportValue);

                    aValues.CalculatePYield();

                    AgroforestryReports.Add(aValues);

                    parcelCount++;
                }
                scenarioCount++;
                if (scenario.Name != "Baseline") scenarioCount++;
            }
        }

        public void ForestryReport()
        {

            string reportValue;
            int scenarioCount = 0;

            foreach (var scenario in xmlReport.ForestryReportScenarioList)
            {
                int parcelCount = 0;
                foreach (var report in scenario.ForestryReportList)
                {
                    var fValues = new ForestryValues();

                    fValues.SetExpected(report);

                    //Select Croplands Tab
                    gMethods.FindAndClick(By.XPath("//div[contains(@id,'reportTabs')]/div/div/div/div[*]/em/button/span[contains(.,'Forestry')]"), driver, true);

                    gMethods.AddDelay(2000);

                    // Grabs the values off the report table and compares to expected values
                    for (int i = 0; i < 2; i++)
                    {
                        //reportValue = gMethods.FindValue(By.XPath("//div[contains(@id,'forestryReportContainer')]/div/div[2]/div[1]/div/div/div[1]/div/table/tbody/tr[" + (parcelCount * 3) + "]/td/table/tbody/tr[" + (i + 2) + "]/td[" + (scenarioCount + 5) + "]/div"), driver);
                        reportValue = gMethods.FindValue(By.XPath("//div[contains(@id,'forestryReportContainer')]/div/div/div/table/tbody/tr[3]/td/table/tbody/tr[" + (i + 2) + "]/td[6]/div"), driver);
                        //reportValue = gMethods.FindValue(By.XPath("//div[contains(@id,'forestryReportContainer')]/div/div/div/table/tbody/tr[3]/td/table/tbody/tr[" + (i + 2) + "]/td[6]/div"), driver);


                        gMethods.AddDelay(300);

                        fValues.SetActual(i, reportValue);
                    }

                    //reportTabContainer
                    //reportValue = gMethods.FindValue(By.XPath("//div[contains(@id,'forestryReportContainer')]/div/div[2]/div[1]/div/div/div[1]/div/table/tbody/tr[" + (parcelCount * 3 + 1) + "]/td[" + (scenarioCount + 5) + "]/div"), driver);
                    reportValue = gMethods.FindValue(By.XPath("//div[contains(@id,'forestryReportContainer')]/div/div/div/table/tbody/tr[4]/td[6]/div"), driver);

                    fValues.SetActual(2, reportValue);

                    fValues.CalculatePYield();

                    ForestryReports.Add(fValues);

                    parcelCount++;
                }
                scenarioCount++;
                if (scenario.Name != "Baseline") scenarioCount++;
            }
        }

        ///Makes an anonymous objet for later jsonification for direct transmission to the cfarm site
        public object ResultObj()
        {
            var res = new {
                croplandsResult = new List<object>(),
            animalAgResult = new List<object>(),
                agroForestryResult = new List<object>(),
                forestryResult = new List<object>()


            };

            foreach (var values in CroplandReports)
            {
                res.croplandsResult.Add(createCropReport(values));
                

            }
            foreach (var values in AnimalAgReports)
            {
                res.animalAgResult.Add(createAnimalAgReport(values));
             

            }

            // Agorforestry Tables
            foreach (var values in AgroforestryReports)
            {
                res.agroForestryResult.Add(createAgroForestryReport(values));
                

            }

            // Forestry Tables
            foreach (var values in ForestryReports)
            {
                res.forestryResult.Add(createForestryReport(values));
               

            }


            return res;
        }

        // Forms a table for each xmlreport tag
        public string EmailDataString()
        {
            StringBuilder SB = new StringBuilder();

            using (var otherWriter = new StringWriter(SB))
            {
                using (var writer = new HtmlTextWriter(otherWriter))
                {
                    // Croplands Tables
                    foreach (var values in CroplandReports)
                    {

                            CreateCroplandsTable(writer, values);
                        
                    }

                    // AnimalAg Tables
                    foreach (var values in AnimalAgReports)
                    {

                            CreateAnimalAgTable(writer, values);
                        
                    }

                    // Agorforestry Tables
                    foreach (var values in AgroforestryReports)
                    {

                            CreateAgroforestryTable(writer, values);
                        
                    }

                    // Forestry Tables
                    foreach (var values in ForestryReports)
                    {

                            CreateForestryTable(writer, values);
                        
                    }
                }
                return SB.ToString();
                //return otherWriter.ToString();
            }
        }

        //return if test is being run on dev, dev-dev, or production
        public static String testDevOrProd(string _url)
        {
            if (_url == "https://cometfarm.nrel.colostate.edu/")
            {
                return "Testing on Production";
            }

            else if (_url == "http://cfarm-dev1.nrel.colostate.edu/")
            {
                return "Testing on Dev";
            }

            else if (_url == "http://cfarm-dev1.nrel.colostate.edu/dev")
            {
                return "Testing on Dev Dev";
            }

            return "";
        }

        public object MakeReportObj(String expected, string actual, string yeild) {

            return new { expected = expected, actual = actual, yield = yeild };
        }

        public object createCropReport(CroplandValues values)
        {

            var Expected = values.GetExpected();
            var Actual = values.GetActual();
            var PYield = values.GetPYield();

            var cReport = new
            {
                scenario = values.ScenarioName,
                location = values.ParcelName,
                C = MakeReportObj(Expected[0].ToString(), Actual[0], PYield[0]),
                CO2 = MakeReportObj(Expected[1].ToString(), Actual[1], PYield[1]),
                CO = MakeReportObj(Expected[2].ToString(), Actual[2], PYield[2]),
                N2O = MakeReportObj(Expected[3].ToString(), Actual[3], PYield[3]),
                CH4 = MakeReportObj(Expected[4].ToString(), Actual[4], PYield[4]),
                Total = MakeReportObj(Expected[5].ToString(), Actual[5], PYield[5]),
            };


            return cReport;

        }
        public void CreateCroplandsTable(HtmlTextWriter writer, CroplandValues values)
        {
            var Expected = values.GetExpected();
            var Actual = values.GetActual();
            var PYield = values.GetPYield();
            

            //Cropland Table
            writer.RenderBeginTag("table style='width:100%' border='2' border-collapse: 'separate'");
            CreateHeaderRow(writer, values.ScenarioName, values.ParcelName, "", "");
            CreateHeaderRow(writer, "Cropland", "Expected", "Reported", "Percent Yield");
            CreateNormalRow(writer, "C", Expected[0].ToString(), Actual[0], PYield[0]);
            CreateNormalRow(writer, "CO2", Expected[1].ToString(), Actual[1], PYield[1]);
            CreateNormalRow(writer, "CO", Expected[2].ToString(), Actual[2], PYield[2]);
            CreateNormalRow(writer, "N2O", Expected[3].ToString(), Actual[3], PYield[3]);
            CreateNormalRow(writer, "CH4", Expected[4].ToString(), Actual[4], PYield[4]);
            CreateNormalRow(writer, "Total", Expected[5].ToString(), Actual[5], PYield[5]);
            CreateNormalRow(writer, "Site", "", "", MainWindow.getTestLoc());
            writer.RenderEndTag();
            writer.Write("<br>");
            
              
        }
        public object createAnimalAgReport(AnimalAgValues values)
        {

            var Expected = values.GetExpected();
            var Actual = values.GetActual();
            var PYield = values.GetPYield();

            var aReport = new
            {
                scenario = values.ScenarioName,
                type = values.TypeName,
                CH4 = MakeReportObj(Expected[0].ToString(), Actual[0], PYield[0]),
                N2O = MakeReportObj(Expected[1].ToString(), Actual[1], PYield[1]),
                Total = MakeReportObj(Expected[2].ToString(), Actual[2], PYield[2]),
       
            };


            return aReport;

        }
        public void CreateAnimalAgTable(HtmlTextWriter writer, AnimalAgValues values)
        {
            var Expected = values.GetExpected();
            var Actual = values.GetActual();
            var PYield = values.GetPYield();


            //Animal Ag Table
            writer.RenderBeginTag("table style='width:100%' border='2' border-collapse: 'collapse'");
            CreateHeaderRow(writer, values.ScenarioName, values.TypeName, "", "");
            CreateHeaderRow(writer, "Animal Ag", "Expected", "Reported", "Percent Yield");
            CreateNormalRow(writer, "Methane", Expected[0].ToString(), Actual[0], PYield[0]);
            CreateNormalRow(writer, "Nitrous Oxide", Expected[1].ToString(), Actual[1], PYield[1]);
            CreateNormalRow(writer, "Total", Expected[2].ToString(), Actual[2], PYield[2]);
            CreateNormalRow(writer, "Site", "", "", MainWindow.getTestLoc());
            writer.RenderEndTag();
            writer.Write("<br>");
        }
        public object createAgroForestryReport(AgroforestryValues values)
        {

            var Expected = values.GetExpected();
            var Actual = values.GetActual();
            var PYield = values.GetPYield();

            var aReport = new
            {
              
                Live_Trees = MakeReportObj(Expected[0].ToString(), Actual[0], PYield[0]),
                Downed_Dead_Wood = MakeReportObj(Expected[1].ToString(), Actual[1], PYield[1]),
                Forest_Floor = MakeReportObj(Expected[2].ToString(), Actual[2], PYield[2]),
                Standing_Trees = MakeReportObj(Expected[3].ToString(), Actual[3], PYield[3]),
                Understory = MakeReportObj(Expected[4].ToString(), Actual[4], PYield[4]),
                Total = MakeReportObj(Expected[5].ToString(), Actual[5], PYield[5]),

            };


            return aReport;

        }

        public void CreateAgroforestryTable(HtmlTextWriter writer, AgroforestryValues values)
        {
            var Expected = values.GetExpected();
            var Actual = values.GetActual();
            var PYield = values.GetPYield();


            //Agroforestry Table
            writer.RenderBeginTag("table style='width:100%' border='2' border-collapse: 'collapse'");
            CreateHeaderRow(writer, "Agroforestry", "Expected", "Reported", "Percent Yield");
            CreateNormalRow(writer, "Live Trees", Expected[0].ToString(), Actual[0], PYield[0]);
            CreateNormalRow(writer, "Downed Dead Wood", Expected[1].ToString(), Actual[1], PYield[1]);
            CreateNormalRow(writer, "Forest Floor", Expected[2].ToString(), Actual[2], PYield[2]);
            CreateNormalRow(writer, "Standing Trees", Expected[3].ToString(), Actual[3], PYield[3]);
            CreateNormalRow(writer, "Understory", Expected[4].ToString(), Actual[4], PYield[4]);
            CreateNormalRow(writer, "Total", Expected[5].ToString(), Actual[5], PYield[5]);
            CreateNormalRow(writer, "Site", "", "", MainWindow.getTestLoc());
            writer.RenderEndTag();
            writer.Write("<br>");
        }
        public object createForestryReport(ForestryValues values)
        {

            var Expected = values.GetExpected();
            var Actual = values.GetActual();
            var PYield = values.GetPYield();

            var fReport = new
            {
                scenario = values.ScenarioName,
                location = values.ParcelName,
                //Live_Trees = MakeReportObj(Expected[0].ToString(), Actual[0], PYield[0]),
                //Standing_Dead = MakeReportObj(Expected[1].ToString(), Actual[1], PYield[1]),
                //Forest_Floor = MakeReportObj(Expected[2].ToString(), Actual[2], PYield[2]),
                //Understory = MakeReportObj(Expected[3].ToString(), Actual[3], PYield[3]),
                //Downed_Dead_Wood = MakeReportObj(Expected[4].ToString(), Actual[4], PYield[4]),
                //Soil_Organic = MakeReportObj(Expected[5].ToString(), Actual[5], PYield[5]),
                //Products_In_Use = MakeReportObj(Expected[6].ToString(), Actual[6], PYield[6]),
                //In_Landfills = MakeReportObj(Expected[7].ToString(), Actual[7], PYield[7]),
                Total_Stand_Carbon = MakeReportObj(Expected[0].ToString(), Actual[0], PYield[0]),
                Harvested_Carbon = MakeReportObj(Expected[1].ToString(), Actual[1], PYield[1]),
                Total = MakeReportObj(Expected[2].ToString(), Actual[2], PYield[2]),
            };


            return fReport;

        }
        public void CreateForestryTable(HtmlTextWriter writer, ForestryValues values)
        {
            var Expected = values.GetExpected();
            var Actual = values.GetActual();
            var PYield = values.GetPYield();

            //Forestry Table
            writer.RenderBeginTag("table style='width:100%' border='2' border-collapse: collapse");
            CreateHeaderRow(writer, values.ScenarioName, values.ParcelName, "", "");
            CreateHeaderRow(writer, "Forestry", "Expected", "Reported", "Percent Yield");
            //CreateNormalRow(writer, "Live Trees", Expected[0].ToString(), Actual[0], PYield[0]);
            //CreateNormalRow(writer, "Standing Dead", Expected[1].ToString(), Actual[1], PYield[1]);
            //CreateNormalRow(writer, "Forest Floor", Expected[2].ToString(), Actual[2], PYield[2]);
            //CreateNormalRow(writer, "Understory", Expected[3].ToString(), Actual[3], PYield[3]);
            //CreateNormalRow(writer, "Downed Dead Wood", Expected[4].ToString(), Actual[4], PYield[4]);
            //CreateNormalRow(writer, "Soil Organic", Expected[5].ToString(), Actual[5], PYield[5]);
            //CreateNormalRow(writer, "Products In Use", Expected[6].ToString(), Actual[6], PYield[6]);
            //CreateNormalRow(writer, "In Landfills", Expected[7].ToString(), Actual[7], PYield[7]);
            //CreateNormalRow(writer, "Total", Expected[8].ToString(), Actual[8], PYield[8]);

            CreateNormalRow(writer, "Total Stand Carbon", Expected[0].ToString(), Actual[0], PYield[0]);
            CreateNormalRow(writer, "Harvested Carbon", Expected[1].ToString(), Actual[1], PYield[1]);
            CreateNormalRow(writer, "Total", Expected[2].ToString(), Actual[2], PYield[2]);
            CreateNormalRow(writer, "Site", "", "", MainWindow.getTestLoc());
            writer.RenderEndTag();
        }

        public void CreateHeaderRow(HtmlTextWriter writer, string col1, string col2, string col3, string col4)
        {
            writer.RenderBeginTag("tr");
            writer.RenderBeginTag("th style='width:25%'");
            writer.Write(col1);             //Category
            writer.RenderEndTag();
            writer.RenderBeginTag("th style='width:25%'");
            writer.Write(col2);             //Expected Value
            writer.RenderEndTag();
            writer.RenderBeginTag("th style='width:25%'");
            writer.Write(col3);             //Reported Value
            writer.RenderEndTag();
            writer.RenderBeginTag("th style='width:25%'");
            writer.Write(col4);             //Percent Error
            writer.RenderEndTag();
            writer.RenderEndTag();
        }

        public void CreateNormalRow(HtmlTextWriter writer, string col1, string col2, string col3, string col4)
        {
            writer.RenderBeginTag("tr");
            writer.RenderBeginTag("td");
            writer.Write(col1);             //Category
            writer.RenderEndTag();
            writer.RenderBeginTag("td");
            writer.Write(col2);             //Expected Value
            writer.RenderEndTag();
            writer.RenderBeginTag("td");
            writer.Write(col3);             //Reported Value
            writer.RenderEndTag();
            writer.RenderBeginTag("td");
            writer.Write(col4);             //Percent Error
            writer.RenderEndTag();
            writer.RenderEndTag();
        }
    }
}
