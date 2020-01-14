using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace CometTester.CFarm
{
   class CFarmEmail
   {
       private GeneralTestMethods gMethods;
       private string subject;
       private string data;
       private List<string> emailList;
       private int[] testPassCount;
        private List<string> IndividualResults = new List<string>();
        private string _Browser;

        public CFarmEmail(GeneralTestMethods gMethods)
        {
            this.gMethods = gMethods;

            
            emailList = new List<string>();
            //emailList.Add("mclayer@colostate.edu");
            emailList.Add("mattrandrus@gmail.com");
            //emailList.Add("crystal.toureene@colostate.edu");
            //emailList.Add("nycole.echeverria@colostate.edu");
            //emailList.Add("mark.easter@colostate.edu");
            //emailList.Add("geoffpietz@gmail.com");
            //emailList.Add("kevin.dwayne.brown@colostate.edu");
            //emailList.Add("keith.paustian@colostate.edu");
            //emailList.Add("appnrel@colostate.edu");
            //emailList.Add("geoffreypietz@hotmail.com");

            // 0  is tests done, 1 is tests passed
            testPassCount = new int[2];
        }
        public void SetSubject(String Browser)
        {
            _Browser = Browser;

            string passColor;

            if (testPassCount[0] > 0 && testPassCount[1] / testPassCount[0] == 1)      // Pass / Test Counts
            {
                subject = Browser+" Successful CFarm Test";
                passColor = "#04B404";
            }
            else if (testPassCount[0] > 0 && testPassCount[1] % testPassCount[0] > 0)        // Any Passed tests are > 0
            {
                subject = Browser+" Partially Successful CFarm Test";
                passColor = "#AEB404";
            }
            else
            {
                subject = Browser+" Failed CFarm Test";
                passColor = "#B40404";
            }



            //data = "<p style=\"font-size:14pt\">Test Summary</p><br>" + "<p style=\"margin:0\">Passed Tests: </p>" + "<p style=\"color:" + passColor + "; margin:0\">" + testPassCount[1].ToString() + " out of " + testPassCount[0].ToString() + "<br><br>" + data;

            data = "<p style=\"font-size:14pt\">Test Summary</p><br>" + "<p style=\"margin:0\">Passed Tests: </p>" + "<p style=\"color:" + passColor + "; margin:0\">" + testPassCount[1].ToString() + " out of " + testPassCount[0].ToString() + "<br><br>" + data;
        }


        public void SetSubject()
       {
            SetSubject("");
      }

       public string GetSubject()
       {
           return subject;
       }

        public string GetJson()
        {
            var ret = new
            {
                browser = _Browser,
                results = IndividualResults

            };
        
         return new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(ret);

        }

       public string GetData()
       {
           return data;
       }

       public List<string> GetEmailList()
       {
           return emailList;
       }

       public void IncTestCount()
       {
           testPassCount[0]++;
       }

       public void IncPassCount()
       {
           testPassCount[1]++;
       }

       public void SetSuccessData(BrowserDriver driver, CFarmXmlLister cFarmXml, CFarmXmlLister.CFarmReportData reportData)
       {
           var report = new CFarmXmlReport(gMethods, driver, cFarmXml, reportData);
           data += "<div style='height:200px; border:thick solid green; padding:2px; overflow-y:auto;'><b>Successful Test:</b> " + cFarmXml._path + "<br>";
           data += report.EmailDataString() + "</div>";
           data += "<br><Separator/>";
            IndividualResults.Add(MakeResJson(cFarmXml._path, report));

        }
        public string MakeResJson(String whichTest, CFarmXmlReport report)
        {
            var ret = new
            {
                name = whichTest,
                error = "",
                result = report.ResultObj()

            };


            return new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(ret);

        }

        public string MakeResJson(String whichTest, String Error)
        {
            var ret = new
            {
                name = whichTest,
                error = Error,
                result=new { }
            };


            return new JavaScriptSerializer { MaxJsonLength = Int32.MaxValue }.Serialize(ret);
        }
       public void SetFailData(string whichTest, Exception e)
       {
           data += "<div style='height:200px; max-height:200px; border:thick solid red; padding:2px; overflow-y:auto'><b>Failed Test:</b> " + whichTest + "<br><br>";
           data += "Failed at [locator],[method]: " + gMethods.GetErrorStrings() + "<br><br>" + e + "<br></div>";
           data += "<br><Separator/>";
            IndividualResults.Add(MakeResJson(whichTest, gMethods.GetErrorStrings()));

       }
   }
}
