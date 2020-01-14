using System;
using System.Linq;
using OpenQA.Selenium;
using System.Collections.ObjectModel;
using System.Threading;
using OpenQA.Selenium.Interactions;
using System.Net.Mail;
using System.Net;
using System.Collections.Generic;
using System.Net;
using System.Text;  // for class Encoding
using System.IO;

namespace CometTester
{
    class GeneralTestMethods
    {
    

        string errLocator = "none";
        string errMethod = "none";
        private int _waitIndex;   //Used by WaitForElement() method as a counter int

        public GeneralTestMethods()
        {

        }

        /// <summary>
        /// Sets the strings reported in the error email and is only used by classes other than GeneralTestMethods
        /// </summary>
        /// <param name="errLocator"></param>
        /// <param name="errMethod"></param>
        public void SetErrorStrings(string errLocator, string errMethod)
        {
            this.errLocator = errLocator;
            this.errMethod = errMethod;
        }

        public string GetErrorStrings()
        {
            return errLocator + " , " + errMethod;
        }

        /// <summary>
        /// Adds an int millisecond wait  
        /// </summary>
        /// <param name="timeWait"></param>
        public void AddDelay(int timeWait)
        {
            Thread.Sleep(timeWait);
        }

        /// <summary>
        /// Checks once if the Element defined by the locator is present on the page
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public bool IsElementPresent(By locator, BrowserDriver driver)
        {
            errLocator = locator.ToString();
            errMethod = "IsElementPresent";

            try
            {
                IWebElement element = driver.Driver.FindElement(locator);
                return true;
            }
            catch (Exception)
            {
                return false;
            }            
        }

        /// <summary>
        /// Grabs the inner HTML of a tag, so useful for checking report values
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="driver"></param>
        /// <returns></returns>
        public string FindValue(By locator, BrowserDriver driver)
        {
            errLocator = locator.ToString();
            errMethod = "FindValue";

            IWebElement elementFound = driver.Driver.FindElement(locator);
            string value = elementFound.GetAttribute("innerHTML");
            return value;
        }

        /// <summary>
        /// Finds and clicks the top left corner of the locator element with option to wait for ~30seconds
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="driver"></param>
        /// <param name="waitFor"></param>
        /// <summary>
        /// 
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="driver"></param>
        /// <param name="waitFor"></param>
        public void FindAndClick(By locator, BrowserDriver driver, bool waitFor)
        {
            errLocator = locator.ToString();
            errMethod = "FindAndClick"; 

            if (waitFor == true)
            {
                WaitForElement(locator, driver);
            }
            IWebElement elementFound = driver.Driver.FindElement(locator);
            elementFound.Click();
        }

        /// <summary>
        /// Finds and clicks the top left corner of the locator element a specified number of times with option to wait for ~30seconds
        /// </summary>
        /// <param name="input"></param>
        /// <param name="locator"></param>
        /// <param name="driver"></param>
        /// <param name="waitFor"></param>
        /// <param name="clicks"></param>
        /// <param name="delay"></param>
        public void FindAndMultiClick(string input, By locator, BrowserDriver driver, bool waitFor, int clicks, bool delay)
        {
            errLocator = locator.ToString();
            errMethod = "FindAndMultiClick";

            if (waitFor)
            {
                WaitForElement(locator, driver);
            }
            IWebElement elementFound = driver.Driver.FindElement(locator);

            for (int i = 0; i < clicks; i++)
            {
                if (delay)
                {
                    AddDelay(200); 
                }
                elementFound.Click();
            }

            // The point of the multi click input is to highlight and replace preexisting data in the input box
            if (input != "")
            {
                elementFound.SendKeys(input);
            }
        }

        /// <summary>
        /// Finds locator element and sends a key combination to the input box (like typing)
        /// </summary>
        /// <param name="input"></param>
        /// <param name="locator"></param>
        /// <param name="driver"></param>
        /// <param name="waitFor"></param>
        /// <param name="enter"></param>
        public void FindAndInput(string input, By locator, BrowserDriver driver, bool waitFor, bool enter)
        {
            errLocator = locator.ToString();
            errMethod = "FindAndInput";

            if (waitFor)
            {
                WaitForElement(locator, driver);
            }

            IWebElement elementFound = driver.Driver.FindElement(locator);
            elementFound.Click();

            elementFound.SendKeys(Keys.Backspace + input);

            // Option to hit "Enter" after key input
            if (enter)
            {
                elementFound.Submit();
            }
        }

        /// <summary>
        /// Waits for locator element to appear on the page for a max of ~30seconds before throwing an exception
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="driver"></param>
        public void WaitForElement(By locator, BrowserDriver driver)
        {
            try
            {
                driver.Driver.FindElement(locator);
                _waitIndex = 0;
            }
            catch
            {
                if (_waitIndex < 60)
                {
                    Thread.Sleep(500);
                    _waitIndex++;
                    WaitForElement(locator, driver);
                }               
            }
        }

        public void Wait30Minutes(By locator, BrowserDriver driver)
        {

            try
            {
                driver.Driver.FindElement(locator);
          
            }
            catch
            {
                if (_waitIndex < 3600)
                {
                    Thread.Sleep(500);
                    _waitIndex++;
                    Wait30Minutes(locator, driver);
                }
            }
        }

        public void WaitLongTime(By locator, BrowserDriver driver)
        {
            try
            {
                driver.Driver.FindElement(locator);
                _waitIndex = 0;
            }
            catch
            {
                if (_waitIndex < 600)
                {
                    Thread.Sleep(500);
                    _waitIndex++;
                    WaitLongTime(locator, driver);
                }
            }
        }

        public string ElementSize(By locator, BrowserDriver driver, bool waitFor)
        {
            errLocator = locator.ToString();
            errMethod = "ElementSize";

            if (waitFor)
            {
                WaitForElement(locator, driver);
            }

            IWebElement marker = driver.Driver.FindElement(locator);
            var size = marker.Size.ToString();



            return size;
        }

        /// <summary>
        /// Finds an element and clicks within the element. Options are to single/double click and click and drag
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="driver"></param>
        /// <param name="xOffset"></param>  Defines the location on the element to be clicked
        /// <param name="yOffset"></param>
        /// <param name="clickHow"></param> What kind of click. Options are "single", "double", and "drag"
        /// <param name="xOffset2"></param> Defines the movement vector to drag the mouse to after click. Only used on "drag" option
        /// <param name="yOffset2"></param>
        public void ClickOnPage(By locator, BrowserDriver driver, int xOffset, int yOffset, string clickHow, int xOffset2, int yOffset2)
        {
            errLocator = locator.ToString();
            errMethod = "ClickOnPage";

            IWebElement marker = driver.Driver.FindElement(locator);
            Actions builder = new Actions(driver.Driver);

            if (clickHow == "drag")
            {
                builder.MoveToElement(marker, xOffset, yOffset).ClickAndHold().MoveByOffset(1, 1).MoveByOffset(xOffset2, yOffset2);
            }
            else if (clickHow == "double")
            {
                builder.MoveToElement(marker, xOffset, yOffset).KeyDown(Keys.Shift).Click().KeyUp(Keys.Shift);
            }
            else
            {
                builder.MoveToElement(marker, xOffset, yOffset).Click();
            }
            
            builder.Build().Perform();
            
            if (clickHow == "drag")
            {
                AddDelay(200);
                builder.Release();
                builder.Build().Perform();
            }
        }

        /// <summary>
        /// Types keys into locator element. Useful for general page hotkeys.
        /// </summary>
        /// <param name="locator"></param>
        /// <param name="driver"></param>
        /// <param name="key1"></param>     Pressed and Held Key
        /// <param name="key2"></param>     Positive pressed Key
        /// <param name="key3"></param>     Used in conjuction with negative "presses" values
        /// <param name="holdKey1"></param> True if key1 needs held down while key2 pressed
        /// <param name="presses"></param>  How many times does this key combo need pressed?
        public void EnterKeys(By locator, BrowserDriver driver, string key1, string key2, string key3, int presses)
        {
            errLocator = locator+key1+key2;
            errMethod = "EnterKeys";
          
            if (presses != 0)
            {
                IWebElement marker = driver.Driver.FindElement(locator);
                Actions builder = new Actions(driver.Driver);

                if (key1 != null)
                {
                    if (presses < 0)
                    {
                        builder.MoveToElement(marker).KeyDown(key1).SendKeys(key3).KeyUp(key1); 
                    }
                    else
                    {
                        builder.MoveToElement(marker).KeyDown(key1).SendKeys(key2).KeyUp(key1); 
                    }
                }
                else
                {
                    if (presses < 0)
                    {
                        builder.MoveToElement(marker).SendKeys(key3);
                    }
                    else
                    {
                        builder.MoveToElement(marker).SendKeys(key2);
                    }
                }
                IAction pressNextElement = builder.Build();
                for (int i = 0; i < Math.Abs(presses); i++)
                {
                    AddDelay(20);
                    pressNextElement.Perform();
                } 
            }

        }

        /// <summary>
        /// Used to close popup windows. Closes all windows except the current(hopefully main) window
        /// </summary>
        /// <param name="driver"></param>
        public void ClosePopUp(BrowserDriver driver)
        {
            errLocator = "";
            errMethod = "ClosePopUp";

            //get the current window handles 
            string popupHandle = string.Empty;
            ReadOnlyCollection<string> windowHandles = driver.Driver.WindowHandles;

            string currentWindowHandle = windowHandles.ElementAt(0);

            foreach (string handle in windowHandles)
            {
                if (handle != currentWindowHandle)
                {
                    popupHandle = handle;

                    //switch to new window 
                    driver.Driver.SwitchTo().Window(popupHandle);

                    //close the new window to navigate to the previous one
                    driver.Driver.Close();

                    break;
                }
            }

            //switch back to original window 
            driver.Driver.SwitchTo().Window(currentWindowHandle);

        }

        /// <summary>
        /// Checks if the locator element is visible on the screen. Similar to IsElementPresent()
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="locator"></param>
        /// <returns></returns>
        public static bool IsElementDisplayed(BrowserDriver driver, By locator)
        {
            try
            {
                return driver.Driver.FindElement(locator).Displayed;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }





        public void PostResults(string Gdata, string url)
        {
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(url + "TesterResults/Add");

                var postData = "authorization=ItsMeYouDummmy";
                postData += "&results=" + Gdata;// Uri.EscapeDataString(Gdata);  
                var data = Encoding.ASCII.GetBytes(postData);

                request.Method = "POST";
                request.ContentType = "application/x-www-form-urlencoded";
                request.ContentLength = data.Length;

                using (var stream = request.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }

                var response = (HttpWebResponse)request.GetResponse();

                var responseString = new StreamReader(response.GetResponseStream()).ReadToEnd();
            }
            catch(Exception e){

                int a = 1;
            }

        }

        /// <summary>
        /// Used to send success and error emails
        /// </summary>
        /// <param name="summary"></param>      
        /// <param name="successData"></param>
        /// <param name="e"></param>
        public void SendEmail(string subject, string data, List<string> emailList)
        {
            String user = "appnrel";
            String userPassword = "Swib99cRl062913";
            
            SmtpClient smtpClient = new SmtpClient();
            smtpClient.Host = "smtp.colostate.edu";
            smtpClient.Port = 587;
            NetworkCredential credential = new NetworkCredential(user, userPassword);
            smtpClient.EnableSsl = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.UseDefaultCredentials = false;
            smtpClient.Timeout = 5000;
            smtpClient.Credentials = credential;

            DateTime date1 = DateTime.Now;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("appnrel@colostate.edu");
            mail.Subject = subject;
            mail.Body = data;
            
            foreach (string email in emailList)
            {
                mail.To.Add(email);
                AlternateView htmlview = default(AlternateView);
                htmlview = AlternateView.CreateAlternateViewFromString(data, null, "text/html");
                mail.AlternateViews.Add(htmlview);        
            }
            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception t)
            {
                Console.WriteLine(t.Message);
            }
        }
    }
}
