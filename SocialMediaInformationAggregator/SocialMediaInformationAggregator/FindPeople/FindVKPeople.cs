using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;
using System.Windows.Media.Imaging;
using System.Windows.Controls;

namespace SocialMediaInformationAggregator.FindPeople
{
    public partial class FindPeople //: IFindPeople
    {
        private List<PersonInformation> _peopleFromVK = new List<PersonInformation>();

        public List<PersonInformation> PeopleFromVK { get => _peopleFromVK; private set => _peopleFromVK = value; }

        public void FindPeopleOnVK(IWebDriver webDriver, SearchOptions searchOptions)
        {
            webDriver.Navigate().GoToUrl("https://vk.com/search?c%5Bper_page%5D=40&c%5Bphoto%5D=1&c%5Bsection%5D=people");

            //Thread.Sleep(500);
            InputCountry(webDriver);
            
            //Решить проблему с задержкой перед городом
            Thread.Sleep(500);
            InputCity(webDriver, searchOptions);
            InputSchool(webDriver, searchOptions);
            InputUniversitet(webDriver, searchOptions);
            InputYearBirthFrom(webDriver, searchOptions);
            InputYearBirthTo(webDriver, searchOptions);

            IWebElement querName = webDriver.FindElement(By.Id("search_query"));
            querName.SendKeys(searchOptions.Name + " " + searchOptions.LastName + "\n");

            Thread.Sleep(1000);

            if(!IsElementExist(By.XPath("//*[@id='results']/div[1]/div[3]/div[1]/a"),webDriver))
            {
                webDriver.Navigate().Refresh();
                querName.Clear();
                querName.SendKeys(searchOptions.Name + " " + searchOptions.LastName + "\n");
            }

            //try
            //{
            for (int i = 1; i < 6; i++)
            {
                Thread.Sleep(1000);
                if (IsElementExist(By.XPath("//*[@id='results']/div[" + i + "]/div[3]/div[1]/a"), webDriver))
                {
                    Thread.Sleep(1000);
                    //IWebElement people = webDriver.FindElement(By.XPath("//*[@id='results']/div[" + i + "]/div[3]/div[1]/a"));
                    webDriver.FindElement(By.XPath("//*[@id='results']/div[" + i + "]/div[3]/div[1]/a")).Click();
                    //Thread.Sleep(500);
                    PersonInformation personInformation = new PersonInformation();
                    //Thread.Sleep(500);

                    NameAndLastName(webDriver, out string name, out string lName);

                    personInformation.Name = name;
                    //Thread.Sleep(500);
                    personInformation.LastName = lName;
                    //Thread.Sleep(500);
                    personInformation.YearOfBirth = YearBirth(webDriver);
                    //Thread.Sleep(500);
                    personInformation.Cities = new List<string>() { LiveCity(webDriver) };
                    Thread.Sleep(500);
                    personInformation.Education = Educations(webDriver);
                    personInformation.SocialNetwork = SocialNetwork.VK;
                    //personInformation.Photo = Photo(webDriver, i);
                    Thread.Sleep(500);
                    personInformation.ProfileLink = webDriver.Url;

                    PeopleFromVK.Add(personInformation);

                    webDriver.Navigate().Back();
                }
            }
            //}
            //catch
            //{

            //}
        }

        //+
        private void NameAndLastName(IWebDriver webDriver, out string name, out string lName)
        {
            name = "";
            lName = "";
            //new WebDriverWait(webDriver, TimeSpan.FromSeconds(5)).Until(driver => driver.FindElement(By.Id("page_info_wrap")));
            Thread.Sleep(500);
            if (IsElementExist(By.Id("page_info_wrap"), webDriver))
            {
                string str = webDriver.FindElement(By.XPath("//*[@id='page_info_wrap']/div[1]/h2")).Text;
                name = str.Substring(0, str.IndexOf(' '));
                lName = str.Substring(str.IndexOf(' ') + 1);
            }
        }

        //+
        private void InputUniversitet(IWebDriver webDriver, SearchOptions searchOptions)
        {
            if (searchOptions.Education != null && searchOptions.City!= null)
            {
                new WebDriverWait(webDriver, TimeSpan.FromSeconds(5)).Until(driver => driver.FindElement(By.Id("cUniversity")));
                if (IsElementExist(By.Id("cUniversity"), webDriver) && !webDriver.FindElement(By.Id("container2")).Text.Contains("Выбор города"))
                {
                    webDriver.FindElement(By.Id("cUniversity")).Click();
                    Thread.Sleep(500);
                    bool elemExist = true;
                    int i = 0;
                    try
                    {
                        while (elemExist && i < 100)
                        {
                            if (IsElementExist(By.XPath("//*[@id='option_list_options_container_12_" + i + "']"), webDriver))
                            {
                                IWebElement universitet = webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_12_" + i + "']"));
                                i++;
                                if (universitet.Text.Contains(searchOptions.Education))
                                {
                                    universitet.Click();
                                    elemExist = false;
                                }
                            }
                            else
                                i++;
                        }
                        webDriver.FindElement(By.Id("cUniversity")).Click();
                    }
                    catch
                    {

                    }
                }

            }

        }
        
        //+
        private void InputSchool(IWebDriver webDriver, SearchOptions searchOptions)
        {
            if(searchOptions.Schools != null && searchOptions.City!=null)
            {
                new WebDriverWait(webDriver, TimeSpan.FromSeconds(5)).Until(driver => driver.FindElement(By.Id("cSchool")));

                if (IsElementExist(By.Id("cSchool"), webDriver) && !webDriver.FindElement(By.Id("container2")).Text.Contains("Выбор города"))
                {
                    webDriver.FindElement(By.Id("cSchool")).Click();
                    Thread.Sleep(500);
                    bool elemExist = true;
                    int i = 0;
                    try
                    {
                        while (elemExist && i < 5000)
                        {
                            //if (webDriver is OpenQA.Selenium.Firefox.FirefoxDriver)
                            if (IsElementExist(By.XPath("//*[@id='option_list_options_container_6_" + i + "']"), webDriver))
                            {
                                IWebElement city = webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_6_" + i + "']"));
                                i++;
                                if (city.Text.Contains(searchOptions.Schools))
                                {
                                    city.Click();
                                    elemExist = false;
                                }
                            }
                            else
                                i++;
                        }
                    }
                    catch
                    {

                    }
                }
            }

        }
        
        //+
        private void InputYearBirthFrom(IWebDriver webDriver, SearchOptions searchOptions)
        {
            if (searchOptions.ForThisYear!=null)
            {
                new WebDriverWait(webDriver, TimeSpan.FromSeconds(5)).Until(driver => driver.FindElement(By.Id("container13")));

                if (IsElementExist(By.Id("container13"), webDriver))
                {
                    IWebElement querYearFrom = webDriver.FindElement(By.Id("container13"));
                    querYearFrom.Click();
                    Thread.Sleep(500);
                    bool elemExist = true;
                    int i = 0;
                    try
                    {
                        while (elemExist)
                        {
                            //if (webDriver is OpenQA.Selenium.Firefox.FirefoxDriver)
                            if (IsElementExist(By.XPath("//*[@id='option_list_options_container_13_" + i + "']"), webDriver))
                            {
                                IWebElement yearFrom = webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_13_" + i + "']"));
                                i++;
                                if (yearFrom.Text.Contains((DateTime.Now.Year - searchOptions.ForThisYear).ToString()))
                                {
                                    yearFrom.Click();
                                    elemExist = false;
                                }
                            }
                            else
                                i++;
                        }
                    }
                    catch
                    {

                    }
                }
            }


        }
        
        //+
        private void InputYearBirthTo(IWebDriver webDriver, SearchOptions searchOptions)
        {
            if (searchOptions.YearOfBirth!=null)
            {
                new WebDriverWait(webDriver, TimeSpan.FromSeconds(5)).Until(driver => driver.FindElement(By.Id("container14")));

                if (IsElementExist(By.Id("container14"), webDriver))
                {
                    IWebElement querYearFrom = webDriver.FindElement(By.Id("container14"));
                    querYearFrom.Click();
                    Thread.Sleep(500);
                    bool elemExist = true;
                    int i = 0;
                    try
                    {
                        while (elemExist)
                        {
                            //if (webDriver is OpenQA.Selenium.Firefox.FirefoxDriver)
                            if (IsElementExist(By.XPath("//*[@id='option_list_options_container_14_" + i + "']"), webDriver))
                            {
                                IWebElement yearFrom = webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_14_" + i + "']"));
                                i++;
                                if (yearFrom.Text.Contains((DateTime.Now.Year - searchOptions.YearOfBirth).ToString()))
                                {
                                    yearFrom.Click();
                                    elemExist = false;
                                }
                            }
                            else
                                i++;
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }
        
        //+
        private void InputCity(IWebDriver webDriver, SearchOptions searchOptions)
        {
            if(searchOptions.City!=null)
            {
                new WebDriverWait(webDriver, TimeSpan.FromSeconds(10)).Until(driver => driver.FindElement(By.Id("container2")));
                if (IsElementExist(By.Id("container2"), webDriver))
                {
                    webDriver.FindElement(By.Id("container2")).Click();
                    Thread.Sleep(500);
                    bool elemExist = true;
                    int i = 0;
                    try
                    {
                        while (elemExist)
                        {
                            if (IsElementExist(By.XPath("//*[@id='option_list_options_container_2_" + i + "']"), webDriver))
                            {
                                IWebElement city = webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_2_" + i + "']"));
                                i++;
                                if (city.Text.Equals(searchOptions.City, StringComparison.CurrentCultureIgnoreCase))
                                {
                                    city.Click();
                                    elemExist = false;
                                }
                                if (city.Text.Equals("Другой город"))
                                {
                                    city.Click();
                                    webDriver.FindElement(By.XPath("//*[@id='container2']/table/tbody/tr/td/input[1]")).SendKeys(searchOptions.City);
                                    Thread.Sleep(500);
                                    webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_2_1']")).Click();
                                    elemExist = false;
                                }
                            }
                            else
                                i++;                      
                        }
                    }
                    catch
                    {

                    }
                }
            }
        }

        //+
        private void InputCountry(IWebDriver webDriver)
        {
            new WebDriverWait(webDriver, TimeSpan.FromSeconds(20)).Until(driver => driver.FindElement(By.Id("container3")));
            if (IsElementExist(By.Id("container3"), webDriver))
            {
                IWebElement querCounntry = webDriver.FindElement(By.Id("container3"));
                querCounntry.Click();
                Thread.Sleep(500);
                bool elemExist = true;
                int i = 0;
                try
                {
                    while (elemExist)
                    {
                        if (IsElementExist(By.XPath("//*[@id='option_list_options_container_3_" + i + "']"), webDriver))
                        {
                            IWebElement country = webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_3_" + i + "']"));
                            i++;
                            if (country.Text == "Россия")
                            {
                                elemExist = false;
                                country.Click();
                            }
                        }
                        else
                            i++;
                    }
                }
                catch
                {

                }
            }
        }

        //?
        private int? YearBirth(IWebDriver webDriver)
        {
            Thread.Sleep(500);
            int? bith=null;
            try
            {
                for (int i = 1; i < 10; i++)
                {
                    if (IsElementExist(By.XPath("//*[@id='profile_short']/div[" + i + "]"), webDriver))
                    {
                        IWebElement querBirth = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[" + i + "]"));
                        if (querBirth.Text.Contains("День рождения:"))
                        {
                            bith = Convert.ToInt32(webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[" + i + "]/div[2]/a[2]")).Text.Substring(0, 4));
                            return bith;
                        }
                    }
                }
                return bith;
            }
            catch
            {
                return bith;
            }  
        }

        //?
        private string LiveCity(IWebDriver webDriver)
        {
            string city;
            try
            {
                for(int i=0;i<10;i++)
                {
                    if(IsElementExist(By.XPath("//*[@id='profile_short']/div["+i+"]"),webDriver))
                    {
                        IWebElement querCity = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[" + i + "]"));
                        if(querCity.Text.Contains("Город:"))
                        {
                            city = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[" + i + "]/div[2]/a")).Text;
                            return city;
                        }
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        //break
        //+
        private List<string> Educations(IWebDriver webDriver)
        {
            List<string> education = new List<string>();
            List<IWebElement> blocksDiv= BlocksDiv(webDriver);
            for (int i=0; i< blocksDiv.Count;i++)
            {
                if(blocksDiv[i].Text.StartsWith("Образование"))
                {
                    List<IWebElement> blocksEducation = BlocksEducation(webDriver,i); 
                    for(int j=0;j<blocksEducation.Count;j++)
                    {
                        if(blocksEducation[j].Text.StartsWith("Вуз")||blocksEducation[j].Text.StartsWith("Школа"))
                        {
                            string educat = blocksEducation[j].FindElement(By.XPath(".//a[1]")).Text;
                            education.Add(educat);
                        }
                    }
                }
            }
            return education;
        }

        private List<IWebElement> BlocksDiv(IWebDriver webDriver)
        {
            List<IWebElement> blocksDiv = new List<IWebElement>();
            Thread.Sleep(500);
            for(int i=0;i<10;i++)
            {
                if(IsElementExist(By.XPath("//*[@id='profile_short']/div[" + i + "]"),webDriver))
                {
                    IWebElement querEducation = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[" + i + "]"));
                    if (querEducation.Text.Contains("Показать подробную информацию"))
                    {
                        querEducation.Click();
                        for (int j = 0; j < 20; j++)
                        {
                            if (IsElementExist(By.XPath("//*[@id='profile_full']/div[" + j + "]"), webDriver))
                            {
                                IWebElement block = webDriver.FindElement(By.XPath("//*[@id='profile_full']/div[" + j + "]"));
                                blocksDiv.Add(block);
                            }
                        }
                    }
                }
            }
            return blocksDiv;
        }

        private List<IWebElement> BlocksEducation(IWebDriver webDriver, int j)
        {
            Thread.Sleep(500);
            List<IWebElement> blocksEducation = new List<IWebElement>();
            for (int i = 0; i < 20; i++)
            {
                if (IsElementExist(By.XPath("//*[@id='profile_full']/div[" + (j + 1) + "]/div[2]/div[" + i + "]"), webDriver))
                {
                    blocksEducation.Add(webDriver.FindElement(By.XPath("//*[@id='profile_full']/div[" + (j + 1) + "]/div[2]/div[" + i + "]")));
                }
            }
            return blocksEducation;
        }

        /*
        private Image Photo(IWebDriver webDriver, int i)
        {
            Thread.Sleep(1000);
            IWebElement img = webDriver.FindElement(By.XPath("//*[@id='profile_photo_link']/img"));
            Screenshot screenshot = ((ITakesScreenshot)webDriver.FindElement(By.XPath("//*[@id='profile_photo_link']/img"))).GetScreenshot();// .FindElement(By.XPath("")).
            string im = @"C:\Users\User\OneDrive\Social-media-information-aggregator-BranchToShow\SocialMediaInformationAggregator\SocialMediaInformationAggregator\Assets\" + i.ToString() + ".png";
            screenshot.SaveAsFile(im);
            Image image = new Image();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(@"pack://application:,,,/Assets/"+i+".png");
            image.Source = bitmapImage;
            return image;
        }*/

        private bool IsElementExist(By by, IWebDriver webDriver)
        {
            try
            {
                webDriver.FindElement(by);
                return true;
            }
            catch (NoSuchElementException)
            {
                return false;
            }
        }
    }
}

