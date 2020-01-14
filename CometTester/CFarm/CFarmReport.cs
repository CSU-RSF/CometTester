using System;
using System.Collections.Generic;
using System.Globalization;
using OpenQA.Selenium;
using System.Web;
using System.Web.UI;
using System.IO;

namespace CometTester.CFarm
{
    class CFarmReport
    {
        GeneralTestMethods gMethods;
        BrowserDriver driver;
        private string whichTest;

        public IDictionary<double,string> ReportValues = new Dictionary<double, string>();
        public IDictionary<double, string> PercentErrors = new Dictionary<double, string>(); 
        public IDictionary<double,double> ExpectedValues = new Dictionary<double, double>(); 
 
        string _reportString;
        private double _reportDouble;
        private double _percentError;

        public CFarmReport(GeneralTestMethods gMethods, BrowserDriver driver, string whichTest)
        {
            this.gMethods = gMethods;
            this.driver = driver;
            this.whichTest = whichTest;
            AddToExpectedValues();

            // This exists to check if the Report page finished loading
            gMethods.WaitForElement(By.XPath("/html/body/div[2]/div[3]/div/div[2]/div[1]/div/div[1]/div[3]"), driver);

            gMethods.AddDelay(2000);

            if (whichTest.Contains("Cropland"))
            {
                CroplandsReport();
            }
            else if (whichTest.Contains("AnimalAg"))
            {
                AnimalAgReport();
            }
            else if (whichTest.Contains("Agroforestry"))
            {
                AgroforestryReport();
            }
            else if (whichTest.Contains("Forestry"))
            {
                ForestryReport();
            }
            
            //gMethods.SendEmail(null, EmailDataString(), null);
        }

        public void CroplandsReport()
        {
            //Select Croplands Tab
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[1]/div[1]/div/div[1]/em/button"), driver, true);

            gMethods.AddDelay(2000);

            for (int i = 0; i < 5; i++)
            {
                string path = "/html/body/div[2]/div[3]/div/div[2]/div[1]/div/div[3]/div/div[1]/div/table/tbody/tr[3]/td/table/tbody/tr[" + (i+2) + "]/td[6]/div";
                CompareReport((By.XPath(path)), 100+i);
                gMethods.AddDelay(300);
            }
            string totalPath = "/html/body/div[2]/div[3]/div/div[2]/div[1]/div/div[3]/div/div[1]/div/table/tbody/tr[4]/td[6]/div";
            CompareReport((By.XPath(totalPath)), 105); 
        }

        public void AnimalAgReport()
        {
            //Select Animal Ag Tab
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[1]/div[1]/div/div[7]/em/button"), driver, true);

            gMethods.AddDelay(2000);

            for (int i = 0; i < 2; i++)
            {
                string path = "/html/body/div[2]/div[3]/div/div[2]/div[7]/div/div[2]/div/div[1]/div/table/tbody/tr[3]/td/table/tbody/tr[" + (i + 2) + "]/td[6]/div";
                CompareReport(By.XPath(path), 200+i);
                gMethods.AddDelay(300);
            }
            string totalPath = "/html/body/div[2]/div[3]/div/div[2]/div[7]/div/div[2]/div/div[1]/div/table/tbody/tr[4]/td[6]/div";
            CompareReport((By.XPath(totalPath)), 202); 
        }

        public void AgroforestryReport()
        {
            //Select Agroforestry Tab
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[1]/div[1]/div/div[3]/em/button"), driver, true);

            gMethods.AddDelay(2000);

            for (int i = 0; i < 5; i++)
            {
                string path = "/html/body/div[2]/div[3]/div/div[2]/div[3]/div/div[1]/div/div[1]/div/table/tbody/tr[3]/td/table/tbody/tr[" + (i + 2) + "]/td[5]/div";
                CompareReport(By.XPath(path), 300+i);
                gMethods.AddDelay(300);
            }
            string totalPath = "/html/body/div[2]/div[3]/div/div[2]/div[3]/div/div[1]/div/div[1]/div/table/tbody/tr[4]/td[5]/div";
            CompareReport((By.XPath(totalPath)), 305); 
        }

        public void ForestryReport()
        {
            //Select Forestry Tab
            gMethods.FindAndClick(By.XPath("/html/body/div[2]/div[3]/div/div[1]/div[1]/div/div[5]/em/button"), driver, true);

            gMethods.AddDelay(2000);

            for (int i = 0; i < 8; i++)
            {
                string path = "/html/body/div[2]/div[3]/div/div[2]/div[5]/div/div[1]/div/div[1]/div/table/tbody/tr[3]/td/table/tbody/tr[" + (i + 2) + "]/td[5]/div";
                CompareReport(By.XPath(path), 400+i);
                gMethods.AddDelay(300);
            }
            string totalPath = "/html/body/div[2]/div[3]/div/div[2]/div[5]/div/div[1]/div/div[1]/div/table/tbody/tr[4]/td[5]/div";
            CompareReport((By.XPath(totalPath)), 408); 
        }

        public void AddToExpectedValues()
        {
            if (whichTest == "CFarmCroplands" || whichTest == "CFarmComprehensive")
            {
                //Cropland Values
                ExpectedValues.Add(100, .4);        //C
                ExpectedValues.Add(101, 18.7);        //CO2
                ExpectedValues.Add(102, 0);        //CO
                ExpectedValues.Add(103, 63.8);        //N2O
                ExpectedValues.Add(104, 0);        //CH4
                ExpectedValues.Add(105, 82.9);        //Total 
            }

            if (whichTest == "CFarmAnimalAg" || whichTest == "CFarmComprehensive")
            {
                //Animal Ag Values
                ExpectedValues.Add(200, 1764.2);        //Methane
                ExpectedValues.Add(201, 0);        //Nitrous Oxide
                ExpectedValues.Add(202, 0);        //Total 
            }

            if (whichTest == "CFarmAgroforestry" || whichTest == "CFarmComprehensive")
            {
                //Agroforestry Values
                ExpectedValues.Add(300, 0);        //Live Trees
                ExpectedValues.Add(301, 0);        //Downed Dead Wood
                ExpectedValues.Add(302, 0);        //Forest Floor
                ExpectedValues.Add(303, 0);        //Standing Dead Trees
                ExpectedValues.Add(304, 0);        //Understory
                ExpectedValues.Add(305, 0);        //Total 
            }

            if (whichTest == "CFarmForestry" || whichTest == "CFarmComprehensive")
            {
                //Forestry Values
                ExpectedValues.Add(400, 0);        //Live Trees
                ExpectedValues.Add(401, 0);        //Standing Dead
                ExpectedValues.Add(402, 0);        //Forest Floor
                ExpectedValues.Add(403, 0);        //Understory
                ExpectedValues.Add(404, 0);        //Downed Dead Wood
                ExpectedValues.Add(405, 0);        //Soil Organic
                ExpectedValues.Add(406, 0);        //Products In Use
                ExpectedValues.Add(407, 0);        //In Landfills
                ExpectedValues.Add(408, 0);        //Total 
            }
        }

        public void CompareReport(By locator, double key)
        {
            _reportString = gMethods.FindValue(locator, driver);
            _reportDouble = Convert.ToDouble(_reportString);
            _percentError = ((ExpectedValues[key] - _reportDouble) / ExpectedValues[key]) * 100; //Percent error of report value compared to expected value
            
            _percentError = Math.Round(_percentError);

            ReportValues.Add(key,_reportDouble.ToString());
            PercentErrors.Add(key,_percentError.ToString());
        }

        public string EmailDataString()
        {

            using (var otherWriter = new StringWriter())
            {
                using (var writer = new HtmlTextWriter(otherWriter))
                {
                    writer.RenderBeginTag("p style='font-size:16pt'");
                    writer.Write("<b>Report Summary</b><br><br>");
                    writer.RenderEndTag();

                    if (whichTest=="CFarmCroplands"||whichTest=="CFarmComprehensive")
                    {
                        //Cropland Table
                        writer.RenderBeginTag("table style='width:100%' border='2' border-collapse: collapse");
                        CreateHeaderRow(writer, "Cropland", "Expected", "Reported", "Percent Error");
                        CreateNormalRow(writer, "C", ExpectedValues[100].ToString(), ReportValues[100], PercentErrors[100]);
                        CreateNormalRow(writer, "CO2", ExpectedValues[101].ToString(), ReportValues[101], PercentErrors[101]);
                        CreateNormalRow(writer, "CO", ExpectedValues[102].ToString(), ReportValues[102], PercentErrors[102]);
                        CreateNormalRow(writer, "N2O", ExpectedValues[103].ToString(), ReportValues[103], PercentErrors[103]);
                        CreateNormalRow(writer, "CH4", ExpectedValues[104].ToString(), ReportValues[104], PercentErrors[104]);
                        CreateNormalRow(writer, "Total", ExpectedValues[105].ToString(), ReportValues[105], PercentErrors[105]);
                        writer.RenderEndTag();
                        writer.Write("<br><br>"); 
                    }

                    if (whichTest=="CFarmAnimalAg"||whichTest=="CFarmComprehensive")
                    {
                        //Animal Ag Table
                        writer.RenderBeginTag("table style='width:100%' border='2' border-collapse: collapse");
                        CreateHeaderRow(writer, "Animal Ag", "Expected", "Reported", "Percent Error");
                        CreateNormalRow(writer, "C", ExpectedValues[200].ToString(), ReportValues[200], PercentErrors[200]);
                        CreateNormalRow(writer, "CO2", ExpectedValues[201].ToString(), ReportValues[201], PercentErrors[201]);
                        writer.RenderEndTag();
                        writer.Write("<br><br>"); 
                    }

                    if (whichTest=="CFarmAgroforestry"||whichTest=="CFarmComprehensive")
                    {
                        //Agroforestry Table
                        writer.RenderBeginTag("table style='width:100%' border='2' border-collapse: collapse");
                        CreateHeaderRow(writer, "Agroforestry", "Expected", "Reported", "Percent Error");
                        CreateNormalRow(writer, "Live Trees", ExpectedValues[300].ToString(), ReportValues[300], PercentErrors[300]);
                        CreateNormalRow(writer, "Downed Dead Wood", ExpectedValues[301].ToString(), ReportValues[301], PercentErrors[301]);
                        CreateNormalRow(writer, "Forest Floor", ExpectedValues[302].ToString(), ReportValues[302], PercentErrors[302]);
                        CreateNormalRow(writer, "Standing Trees", ExpectedValues[303].ToString(), ReportValues[303], PercentErrors[303]);
                        CreateNormalRow(writer, "Understory", ExpectedValues[304].ToString(), ReportValues[304], PercentErrors[304]);
                        CreateNormalRow(writer, "Total", ExpectedValues[305].ToString(), ReportValues[305], PercentErrors[305]);
                        writer.RenderEndTag();
                        writer.Write("<br><br>"); 
                    }

                    if (whichTest=="CFarmForestry"||whichTest=="CFarmComprehensive")
                    {
                        //Forestry Table
                        writer.RenderBeginTag("table style='width:100%' border='2' border-collapse: collapse");
                        CreateHeaderRow(writer, "Forestry", "Expected", "Reported", "Percent Error");
                        CreateNormalRow(writer, "Live Trees", ExpectedValues[400].ToString(), ReportValues[400], PercentErrors[400]);
                        CreateNormalRow(writer, "Standing Dead", ExpectedValues[401].ToString(), ReportValues[401], PercentErrors[401]);
                        CreateNormalRow(writer, "Forest Floor", ExpectedValues[402].ToString(), ReportValues[402], PercentErrors[402]);
                        CreateNormalRow(writer, "Understory", ExpectedValues[403].ToString(), ReportValues[403], PercentErrors[403]);
                        CreateNormalRow(writer, "Downed Dead Wood", ExpectedValues[404].ToString(), ReportValues[404], PercentErrors[404]);
                        CreateNormalRow(writer, "Soil Organic", ExpectedValues[405].ToString(), ReportValues[405], PercentErrors[405]);
                        CreateNormalRow(writer, "Products In Use", ExpectedValues[406].ToString(), ReportValues[406], PercentErrors[406]);
                        CreateNormalRow(writer, "In Landfills", ExpectedValues[407].ToString(), ReportValues[407], PercentErrors[407]);
                        CreateNormalRow(writer, "Total", ExpectedValues[408].ToString(), ReportValues[408], PercentErrors[408]);
                        writer.RenderEndTag(); 
                    }
                    
                }
                return otherWriter.ToString();
            }        
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
