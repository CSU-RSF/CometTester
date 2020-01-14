using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using CometTester.CFarm;
using FluentScheduler;
using System.Windows;
using Button = System.Windows.Controls.Button;
using RichTextBox = System.Windows.Controls.RichTextBox;
using System.IO;
using System.Threading;
using System.Windows.Documents;
using System.Windows.Controls.Primitives;
using System.Drawing;
using System.Diagnostics;
using Bytescout.Spreadsheet;

namespace CometTester
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        static string _whichBrowser = "";
        static string _whichTest = "";
        private static List<string> _xmlPathList;
        private static List<Button> _browseButtonList; 
        private static List<RichTextBox> _textBoxList;
        private static int _failCount;

        private static string _url = "https://cometfarm.nrel.colostate.edu/";  // "http://localhost/cfarmweb/"; //"http://cfarm-dev1.nrel.colostate.edu/"; //"http://cfarm-dev1.nrel.colostate.edu/dev"; 
        

        private static string[] CroplandProjects = {
            "CFarmCroplands-ARRice",
      "CFarmCroplands-COGrass",
       "CFarmCroplands-NECorn"
        };

        private static string[] AnimalAgProjects = {
        "CFarmAnimalAg-80631-FeedlotCattle",
       "CFarmAnimalAg-CADairy",
       "CFarmAnimalAg-CAGoats",
       "CFarmAnimalAg-COFeedlotSheep",
       "CFarmAnimalAg-IA-SwineFinishing",
        "CFarmAnimalAg-IALayingHens",
       "CFarmAnimalAg-IASwineNursery",
       "CFarmAnimalAg-IASwineSowOp",
       "CFarmAnimalAg-NDCowCalf",
        "CFarmAnimalAg-NMSheep",
        "CFarmAnimalAg-SCBroilers",
       "CFarmAnimalAg-TXBeefStockerOp" };

        private static string[] AgroForestryProject = {
        "CFarmAgroforestry-Maples-Iowa"
        };


        private static string[] ForestryProjects = {
        "CFarmForestry-Oak-Hickory",
     
        };

        private static string[] ComprehensiveProjects = {
      
        "CFarm-Croplands-AnimalAg-Agroforestry-Forestry-Scenario-1"
        };

        private static string[] BrowserList = {
            "Firefox",
            "Microsoft Edge",
            "Chrome"

        };


        public MainWindow()
        {
            _xmlPathList = new List<string>{"","","","","",""};
            _failCount = 0;
           

        }

        /// <summary>
        /// Sets whichbrowser string to selected browser combo box item
        /// Also, creates browse buttons and path text boxes for file selection
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WebBrowser_Click(object sender, RoutedEventArgs e)
        {
            SiteAddress.Document.Blocks.Clear();
            SiteAddress.Document.Blocks.Add(new Paragraph(new Run(_url)));
            var whichBrowse = (ComboBox) sender;
            _whichBrowser = whichBrowse.SelectedItem.ToString();
            _browseButtonList = new List<Button>{Browse0};
            _textBoxList = new List<RichTextBox>{XmlPathText0};
            FileCanvas.Opacity = 1;
            FileCanvas.IsEnabled = true;

            for (int i = 1; i < 6; i++)
            {
                Button tb = new Button();
                tb.Content = "Browse";
                tb.Name = "browse" + i;
                tb.Click += BrowseFile_Click;
                tb.Width = 120;
                tb.Height = 24;
                Canvas.SetLeft(tb, 488);
                Canvas.SetTop(tb, (i*30)+84);
                tb.Opacity = 0;
                tb.IsEnabled = false;
                FileCanvas.Children.Add(tb);
                _browseButtonList.Add(tb);

                RichTextBox rtb = new RichTextBox();
                rtb.Name = "XmlPathText" + i;
                rtb.MaxWidth = 463;
                rtb.Height = 24;
                Canvas.SetLeft(rtb, 10);
                Canvas.SetTop(rtb, (i * 30) + 84);
                rtb.Opacity = 0;
                rtb.IsEnabled = false;
                rtb.IsReadOnly = true;
                FileCanvas.Children.Add(rtb);
                _textBoxList.Add(rtb);
            }
            Canvas.SetZIndex(AddXmlFile,100);
            Canvas.SetZIndex(RemoveXmlFile, 100);
        }

        /// <summary>
        /// Function for selecting Premade Tests
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void WhichTest_Click(object sender, RoutedEventArgs e)
        {
            var whichTest = (ComboBoxItem)sender;
            _whichTest = whichTest.Content.ToString();


            
            RunCanvas.Opacity = 1;
            RunCanvas.IsEnabled = true;
        }

        private void BrowseFile_Click(object sender, RoutedEventArgs e)
        {
            var button = (Button)e.OriginalSource;
            var whichButton = button.Name;
            whichButton = whichButton.Remove(0, 6);
            var thisButton = Int16.Parse(whichButton);

            var fb = new FileBrowser();
            _xmlPathList[thisButton] = fb.GetPath();
            _textBoxList[thisButton].ClearValue(ContentProperty);
            _textBoxList[thisButton].AppendText(_xmlPathList[thisButton]);            
            _textBoxList[thisButton].ScrollToEnd();

            RunCanvas.Opacity = 1;
            RunCanvas.IsEnabled = true;
        }

        public void RunTest_Click(object sender, RoutedEventArgs e)
        {
            RunScheduledTest();


            ExportButton.Visibility = Visibility.Visible;
          
        }
        public void Export_Click(object sender, RoutedEventArgs e)
        {

            OpenWorkSheet();
            ExportButton.Visibility = Visibility.Collapsed;
           
        }

        public static String getTestLoc()
        {
            String url = _url;
            return CFarmXmlReport.testDevOrProd(url);
        }

        public void OpenWorkSheet()
        {
            Spreadsheet document = new Spreadsheet();
        
            document.LoadFromFile("Copy of validation_oct_18_MJE.xlsx");
           if (_whichTest != "")
           {
               _xmlPathList = new List<String>();
               if (_whichTest == "CFarmCroplands" || _whichTest == "CFarmComprehensive" || _whichTest == "All Cfarm Tests")
               {

                   Worksheet worksheetAccount = document.Workbook.Worksheets.ByName("Account");
                   worksheetAccount.Cell(1, 1).Value = "Not Verified";
                   worksheetAccount.Cell(2, 1).Value = "Not Verified";
                   worksheetAccount.Cell(3, 1).Value = "Verified";
                   worksheetAccount.Cell(4, 1).Value = "Not Verified";
                   worksheetAccount.Cell(5, 1).Value = "Not Verified";
                   Worksheet worksheetActivity = document.Workbook.Worksheets.ByName("Activity");
                   worksheetActivity.Cell(1, 1).Value = "Verified";
                   worksheetActivity.Cell(2, 1).Value = "Not Verified";
                   worksheetActivity.Cell(3, 1).Value = "Verified";
                   worksheetActivity.Cell(4, 1).Value = "Not Verified";
                   worksheetActivity.Cell(5, 1).Value = "Not Verified";
                   worksheetActivity.Cell(6, 1).Value = "Verified";
                   worksheetActivity.Cell(7, 1).Value = "Verified";
                   Worksheet worksheetField = document.Workbook.Worksheets.ByName("Field");
                   worksheetField.Cell(1, 1).Value = "Verified";
                   worksheetField.Cell(2, 1).Value = "Verified";
                   worksheetField.Cell(3, 1).Value = "Verified";
                   worksheetField.Cell(4, 1).Value = "Not Verified";
                   worksheetField.Cell(5, 1).Value = "Not Verified";
                   worksheetField.Cell(6, 1).Value = "Not Verified";
                   worksheetField.Cell(7, 1).Value = "NotVerified";
                   worksheetField.Cell(8, 1).Value = "Not Verified";
                   worksheetField.Cell(9, 1).Value = "Verified";
                   worksheetField.Cell(10, 1).Value = "Not Verified";
                   worksheetField.Cell(11, 1).Value = "Not Verified";
                   worksheetField.Cell(12, 1).Value = "Not Verified";
                   worksheetField.Cell(13, 1).Value = "Not Verified";
                   worksheetField.Cell(14, 1).Value = "Not Verified";
                   worksheetField.Cell(15, 1).Value = "Not Verified";

                   Worksheet worksheetHistoric = document.Workbook.Worksheets.ByName("Historic");
                   worksheetHistoric.Cell(1, 1).Value = "Verified";
                   worksheetHistoric.Cell(2, 1).Value = "Not Verified";
                   worksheetHistoric.Cell(3, 1).Value = "Verified";
                   worksheetHistoric.Cell(4, 1).Value = "Verified";
                   worksheetHistoric.Cell(5, 1).Value = "Not Verified";

                   Worksheet worksheetCurrentFuture = document.Workbook.Worksheets.ByName("Current and Future");
                   worksheetCurrentFuture.Cell(1, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(2, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(3, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(4, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(5, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(6, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(7, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(8, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(9, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(10, 1).Value = "Not Verified";
                   worksheetCurrentFuture.Cell(11, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(12, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(13, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(14, 1).Value = "Not Verified";
                   worksheetCurrentFuture.Cell(15, 1).Value = "Not Verified";
                   worksheetCurrentFuture.Cell(16, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(17, 1).Value = "Not Verified";
                   worksheetCurrentFuture.Cell(18, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(19, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(20, 1).Value = "Verified";
                   worksheetCurrentFuture.Cell(21, 1).Value = "Not Verified";

                   Worksheet worksheetCroplandReports = document.Workbook.Worksheets.ByName("Reports");
                   worksheetCroplandReports.Cell(1, 1).Value = "Verified";
                   worksheetCroplandReports.Cell(5, 1).Value = "Verified";

               }

               if (_whichTest == "CFarmAnimalAg" || _whichTest == "CFarmComprehensive" || _whichTest == "All Cfarm Tests")
               {
                   Worksheet worksheetAnimalCat = document.Workbook.Worksheets.ByName("Animal Location and Cate");
                   worksheetAnimalCat.Cell(1, 1).Value = "Verified";
                   worksheetAnimalCat.Cell(2, 1).Value = "Verified";
                   worksheetAnimalCat.Cell(3, 1).Value = "Verified";
                   worksheetAnimalCat.Cell(4, 1).Value = "Verified";
                   worksheetAnimalCat.Cell(5, 1).Value = "Verified";
                   worksheetAnimalCat.Cell(6, 1).Value = "Verified";
                   worksheetAnimalCat.Cell(7, 1).Value = "Verified";
                   Worksheet worksheetAnimalChar = document.Workbook.Worksheets.ByName("Animal Char");
                   worksheetAnimalChar.Cell(1, 1).Value = "Verified";
                   worksheetAnimalChar.Cell(2, 1).Value = "Verified";
                   worksheetAnimalChar.Cell(3, 1).Value = "Verified";
                   worksheetAnimalChar.Cell(4, 1).Value = "Verified";
                   worksheetAnimalChar.Cell(5, 1).Value = "Verified";
                   worksheetAnimalChar.Cell(6, 1).Value = "Verified";
                   worksheetAnimalChar.Cell(7, 1).Value = "Verified";
                   worksheetAnimalChar.Cell(8, 1).Value = "Verified";
                   worksheetAnimalChar.Cell(9, 1).Value = "Verified";
                   worksheetAnimalChar.Cell(10, 1).Value = "Verified";
                   worksheetAnimalChar.Cell(11, 1).Value = "Verified";
                   worksheetAnimalChar.Cell(12, 1).Value = "Not Verified";
                   Worksheet worksheetAnimalAgReports = document.Workbook.Worksheets.ByName("Reports");
                   worksheetAnimalAgReports.Cell(2, 1).Value = "Verified";
                   worksheetAnimalAgReports.Cell(6, 1).Value = "Verified";

               }

               if (_whichTest == "CFarmAgroforestry" || _whichTest == "CFarmComprehensive" || _whichTest == "All Cfarm Tests")
               {


                   Worksheet worksheetAgroForestry = document.Workbook.Worksheets.ByName("Agroforestry");
                   worksheetAgroForestry.Cell(1, 1).Value = "Verified";
                   worksheetAgroForestry.Cell(2, 1).Value = "Verified";
                   worksheetAgroForestry.Cell(3, 1).Value = "Verified";
                   worksheetAgroForestry.Cell(4, 1).Value = "Verified";
                   worksheetAgroForestry.Cell(5, 1).Value = "Verified";
                   worksheetAgroForestry.Cell(6, 1).Value = "Verified";
                   worksheetAgroForestry.Cell(7, 1).Value = "Verified";
                   worksheetAgroForestry.Cell(8, 1).Value = "Verified";
                   worksheetAgroForestry.Cell(9, 1).Value = "Verified";
                   worksheetAgroForestry.Cell(10, 1).Value = "Verified";
                   Worksheet worksheetAgroReports = document.Workbook.Worksheets.ByName("Reports");
                   worksheetAgroReports.Cell(3, 1).Value = "Verified";
                   worksheetAgroReports.Cell(7, 1).Value = "Verified";


               }

               if (_whichTest == "CFarmForestry" || _whichTest == "CFarmComprehensive" || _whichTest == "All Cfarm Tests")
               {
                   Worksheet worksheetForestry = document.Workbook.Worksheets.ByName("Forestry");
                   worksheetForestry.Cell(1, 1).Value = "Verified";
                   worksheetForestry.Cell(2, 1).Value = "Verified";
                   worksheetForestry.Cell(3, 1).Value = "Verified";
                   worksheetForestry.Cell(4, 1).Value = "Verified";
                   worksheetForestry.Cell(5, 1).Value = "Verified";
                   worksheetForestry.Cell(6, 1).Value = "Verified";
                   worksheetForestry.Cell(7, 1).Value = "Verified";
                   worksheetForestry.Cell(8, 1).Value = "Verified";
                   worksheetForestry.Cell(9, 1).Value = "Verified";
                   worksheetForestry.Cell(10, 1).Value = "Verified";
                   Worksheet worksheetForestryReports = document.Workbook.Worksheets.ByName("Reports");
                   worksheetForestryReports.Cell(4, 1).Value = "Verified";
                   worksheetForestryReports.Cell(8, 1).Value = "Verified";

               }
           }

       if (System.IO.File.Exists(Path.GetTempPath() + "VerifiedTest.xlsx"))
              {
                  System.IO.File.Delete(Path.GetTempPath() + "VerifiedTest.xlsx");
              }

              document.SaveAs(Path.GetTempPath() + "VerifiedTest.xlsx");

              // Close Spreadsheet
              document.Close();
          
        }
        public void ScheduleTest_Click(object sender, RoutedEventArgs e)
        {



         

            if (Int16.Parse(ScheduleHour.Text) < 0 || Int16.Parse(ScheduleHour.Text) > 23)
            {
                MessageBox.Show("Hour input must be 0-23");
            }
            else if (Int16.Parse(ScheduleMin.Text) < 0 || Int16.Parse(ScheduleMin.Text) > 59)
            {
                MessageBox.Show("Minute input must be 0-59");
            }
            else
            {
                JobManager.Initialize(new ScheduledTasksRegistry(ScheduleHour.Text, ScheduleMin.Text,this));
            }         
        }

        public  void RunScheduledTest()
        {
            
            TextRange textRange = new TextRange(
        // TextPointer to the start of content in the RichTextBox.
        SiteAddress.Document.ContentStart,
        // TextPointer to the end of content in the RichTextBox.
        SiteAddress.Document.ContentEnd
        );

            // The Text property on a TextRange object returns a string
            // representing the plain text content of the TextRange.
            var InputBrowser = textRange.Text.Replace("\r\n","");

            _url = InputBrowser;

            var url = _url;
            var baseDir = System.IO.Directory.GetCurrentDirectory();
            var Path1 = "";
            if (_whichTest != "")
            {
                _xmlPathList = new List<String>();
                switch (_whichTest) {
                    case "CFarmCroplands":
                        Path1= Path.Combine(baseDir, "CFarm\\CroplandsScenarios");
                        foreach (var FileName in CroplandProjects) {
                            _xmlPathList.Add(Path.Combine(Path1, FileName+".xml"));
                        }

                  

                        break;

                    case "CFarmAnimalAg":
                        Path1 = Path.Combine(baseDir, "CFarm\\AnimalAgScenarios");
                        foreach (var FileName in AnimalAgProjects)
                        {
                            _xmlPathList.Add(Path.Combine(Path1, FileName + ".xml"));
                        }
                    
                        
                        break;

                    case "CFarmAgroforestry":
                        Path1 = Path.Combine(baseDir, "CFarm\\AgroforestryScenarios");
                        foreach (var FileName in AgroForestryProject)
                        {
                            _xmlPathList.Add(Path.Combine(Path1, FileName + ".xml"));
                        }
                        break;


                    case "CFarmForestry":
                        Path1 = Path.Combine(baseDir, "CFarm\\ForestryScenarios");
                        foreach (var FileName in ForestryProjects)
                        {
                            _xmlPathList.Add(Path.Combine(Path1, FileName + ".xml"));
                        }

                        break;


                    case "CFarmComprehensive":
                        Path1 = Path.Combine(baseDir, "CFarm\\ComprehensiveScenarios");
                        foreach (var FileName in ComprehensiveProjects)
                        {
                            _xmlPathList.Add(Path.Combine(Path1, FileName + ".xml"));
                        }
                        break;

                    case "BfuelComprehensive":

                        break;


                    case "All Cfarm Tests":

                        Path1 = Path.Combine(baseDir, "CFarm\\CroplandsScenarios");
                        foreach (var FileName in CroplandProjects)
                        {
                            _xmlPathList.Add(Path.Combine(Path1, FileName + ".xml"));
                        }

                        Path1 = Path.Combine(baseDir, "CFarm\\AnimalAgScenarios");
                        foreach (var FileName in AnimalAgProjects)
                        {
                            _xmlPathList.Add(Path.Combine(Path1, FileName + ".xml"));
                        }

                        Path1 = Path.Combine(baseDir, "CFarm\\AgroforestryScenarios");
                        foreach (var FileName in AgroForestryProject)
                        {
                            _xmlPathList.Add(Path.Combine(Path1, FileName + ".xml"));
                        }
                        Path1 = Path.Combine(baseDir, "CFarm\\ForestryScenarios");
                        foreach (var FileName in ForestryProjects)
                        {
                            _xmlPathList.Add(Path.Combine(Path1, FileName + ".xml"));
                        }
                        Path1 = Path.Combine(baseDir, "CFarm\\ComprehensiveScenarios");
                        foreach (var FileName in ComprehensiveProjects)
                        {
                            _xmlPathList.Add(Path.Combine(Path1, FileName + ".xml"));
                        }
                        break;


                }
            }


            if (_whichBrowser.Contains("All")) //one email per browser
            {
                foreach (string BrowserString in BrowserList)
                {
                    Thread t = new Thread(() => ThreadedSpinoff(_xmlPathList, BrowserString,url));
                    t.Start();
            
                }


            }
            else
            {
                var gMethods = new GeneralTestMethods();

                CFarmEmail cFarmEmail = new CFarmEmail(gMethods);
                foreach (var path in _xmlPathList)
                {
                    cFarmEmail = CreateTestObjects(path, cFarmEmail, gMethods, _whichBrowser, url);
                }




                //


                cFarmEmail.SetSubject(_whichBrowser);

                if (cFarmEmail.GetEmailList() != null)
                {
                    gMethods.SendEmail(cFarmEmail.GetSubject(), cFarmEmail.GetData(), cFarmEmail.GetEmailList());
                }
                gMethods.PostResults(cFarmEmail.GetJson(), url);
            }
          
        }
 

        private static void ThreadedSpinoff(List<string> _pathList, string browser, string url) {


            if (browser.Contains("Firefox"))
            {
                browser = "Firefox";
            }
            else if (browser.Contains("Microsoft"))
            {
                browser = "Microsoft Edge";
            }
            else if (browser.Contains("Chrome"))
            {
                browser = "Chrome";
            }

           
            var gethods = new GeneralTestMethods();

            CFarmEmail cFarmEmail = new CFarmEmail(gethods);
            foreach (var path in _xmlPathList)
            {
                cFarmEmail = CreateTestObjects(path, cFarmEmail, gethods, browser, url);

               
           
        }

            cFarmEmail.SetSubject(browser);

            if (cFarmEmail.GetEmailList() != null)
            {
                gethods.SendEmail(cFarmEmail.GetSubject(), cFarmEmail.GetData(), cFarmEmail.GetEmailList());
            }
            gethods.PostResults(cFarmEmail.GetJson(),url);

        }



        private static CFarmEmail CreateTestObjects(string path, CFarmEmail cFarmEmail, GeneralTestMethods gMethods, String browser,string url)
        {
            if (path != "")
            {
             
                {
                    var driver = new BrowserDriver(browser);

                    try
                    {
                        if (path.Contains("CFarm") && path.Contains("xml"))
                        {
                            var cFarmXml = new CFarmXmlLister(path);
                            var cFarmXmlTest = new CFarmXmlTest(driver, gMethods, cFarmXml,"Tester",url);

                            cFarmEmail.SetSuccessData(driver, cFarmXml, cFarmXml._xmlDoc.CFarmReportData);
                        }
                        _failCount = 0;
                        cFarmEmail.IncPassCount();
                        cFarmEmail.IncTestCount();

                        driver.Driver.Quit();
                    }

                    catch (Exception e)
                    {
                        driver.Driver.Quit();
                        if (_failCount < 2)

                        {
                            _failCount++;
                            CreateTestObjects(path, cFarmEmail, gMethods, browser, url);
                        }
                        else
                        {
                            cFarmEmail.IncTestCount();
                            cFarmEmail.SetFailData(path, e);
                        }


                    }

                }

            
            }
            
            return cFarmEmail;
        }

        private void AddXmlFile_Click(object sender, RoutedEventArgs e)
        {
            var button = AddXmlFile;
            var buttonPosY = (double)button.GetValue(TopProperty);
            var buttonNum = ((int)buttonPosY - 84) / 30;

            Canvas.SetTop(AddXmlFile, buttonPosY + 30);
            Canvas.SetTop(RemoveXmlFile, buttonPosY + 60);

            _browseButtonList[buttonNum].Opacity = 1;
            _browseButtonList[buttonNum].IsEnabled = true;
            _textBoxList[buttonNum].Opacity = 1;
            _textBoxList[buttonNum].IsEnabled = true;

            if (buttonNum == 1)
            {
                RemoveXmlFile.Opacity = 1;
                RemoveXmlFile.IsEnabled = true;
            }

            if (buttonNum == 5)
            {
                AddXmlFile.Opacity = .3;
                AddXmlFile.IsEnabled = false;
            }
        }

        private void RemoveXmlFile_Click(object sender, RoutedEventArgs e)
        {
            var button = RemoveXmlFile;
            var buttonPosY = (double)button.GetValue(TopProperty);

            var buttonNum = ((int)buttonPosY - 144)/30;

            _browseButtonList[buttonNum].Opacity = 0;
            _browseButtonList[buttonNum].IsEnabled = false;
            _textBoxList[buttonNum].Opacity = 0;
            _textBoxList[buttonNum].IsEnabled = false;

            Canvas.SetTop(AddXmlFile, buttonPosY - 60);
            Canvas.SetTop(RemoveXmlFile, buttonPosY - 30);

            _textBoxList[buttonNum].Document.Blocks.Clear();
            _xmlPathList[buttonNum] = "";

            if (buttonNum == 1)
            {
                RemoveXmlFile.Opacity = .3;
                RemoveXmlFile.IsEnabled = false;
            }

            if (buttonNum == 5)
            {
                AddXmlFile.Opacity = 1;
                AddXmlFile.IsEnabled = true;
            }
        }

        private static bool IsTextAllowed(string text)
        {
            Regex regex = new Regex("[^0-9.-]+"); //regex that matches disallowed text
            return !regex.IsMatch(text);
        }

        private void BfuelComprehensive_Click(object sender, RoutedEventArgs e)
        {

        }

        private void PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            e.Handled = !IsTextAllowed(e.Text);
        }

        /* public string SelectFolder()
         {
             string selectedFolder = string.Empty;
             FolderBrowserDialog selectFolderDialog = new FolderBrowserDialog();
             selectFolderDialog.ShowNewFolderButton = true;
             if (selectFolderDialog.ShowDialog() == DialogResult.OK)
             {
                 selectedFolder = selectFolderDialog.SelectedPath;
             }
             return selectedFolder;
         }*/


    }
}
