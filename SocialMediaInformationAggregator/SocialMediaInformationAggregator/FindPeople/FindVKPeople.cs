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

            InputCountry(webDriver);
            InputCity(webDriver, searchOptions);
            InputYearBirthFrom(webDriver, searchOptions);
            InputYearBirthTo(webDriver, searchOptions);


            //*[@id="school_header"]
            Thread.Sleep(1000);
            webDriver.FindElement(By.XPath("//*[@id='school_header']")).Click();
            Thread.Sleep(500);
            if (IsElementExist(By.Id("container6"), webDriver))
            {
                IWebElement querEducation = webDriver.FindElement(By.Id("container6"));
                querEducation.Click();
                //querEducation.SendKeys("ш");
                /*Thread.Sleep(500);
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
                                elemExist = false;
                                yearFrom.Click();
                            }
                        }
                        else
                            i++;
                    }
                }
                catch
                {

                } */
            }

            //Ищет человека в поисковике по имени
            IWebElement querName = webDriver.FindElement(By.Id("search_query"));
            querName.SendKeys(searchOptions.Name + " " + searchOptions.LastName + "\n");

            Thread.Sleep(1000);
            //try
            //{
            for (int i = 1; i < 6; i++)
            {
                Thread.Sleep(500);
                if (IsElementExist(By.XPath("//*[@id='results']/div[" + i + "]/div[3]/div[1]/a"), webDriver))
                {
                    IWebElement people = webDriver.FindElement(By.XPath("//*[@id='results']/div[" + i + "]/div[3]/div[1]/a"));
                    webDriver.FindElement(By.XPath("//*[@id='results']/div[" + i + "]/div[3]/div[1]/a")).Click();

                    PersonInformation personInformation = new PersonInformation();
                    personInformation.Name = searchOptions.Name;
                    personInformation.LastName = searchOptions.LastName;
                    personInformation.YearOfBirth = YearBirth(webDriver);
                    personInformation.Cities = new List<string>() { LiveCity(webDriver) };
                    personInformation.Education = Educations(webDriver);
                    personInformation.SocialNetwork = SocialNetwork.VK;
                    //personInformation.Photo = Photo(webDriver, i);

                    PeopleFromVK.Add(personInformation);

                    webDriver.Navigate().Back();
                }
            }
            //}
            //catch
            //{

            //}
        }

        private void InputYearBirthFrom(IWebDriver webDriver, SearchOptions searchOptions)
        {
            Thread.Sleep(1000);
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
                                elemExist = false;
                                yearFrom.Click();
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

        private void InputYearBirthTo(IWebDriver webDriver, SearchOptions searchOptions)
        {
            Thread.Sleep(1000);
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
                                elemExist = false;
                                yearFrom.Click();
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

        private void InputCity(IWebDriver webDriver, SearchOptions searchOptions)
        {
            Thread.Sleep(500);
            if (IsElementExist(By.Id("container2"), webDriver))
            {
                IWebElement querCity = webDriver.FindElement(By.Id("container2"));
                querCity.Click();
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
                                elemExist = false;
                                city.Click();
                            }
                            if (city.Text.Equals("Другой город"))
                            {
                                if(searchOptions.City!=null)
                                {
                                    elemExist = false;
                                    city.Click();
                                    webDriver.FindElement(By.XPath("//*[@id='container2']/table/tbody/tr/td/input[1]")).SendKeys(searchOptions.City);
                                    Thread.Sleep(500);
                                    webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_2_1']")).Click();
                                }
                                else
                                {
                                    elemExist = false;
                                    webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_2_1']")).Click();
                                }
                            }
                        }
                        else
                            i++;                      
                    }
                }
                catch
                {
                    //Thread.Sleep(500);
                    //querCity.Click();
                    Thread.Sleep(500);
                    webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_2_1']")).Click();
                }
            }
        }

        private void InputCountry(IWebDriver webDriver)
        {
            Thread.Sleep(500);
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

        private int? YearBirth(IWebDriver webDriver)
        {
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

            
/*            Thread.Sleep(500);
            if (IsElementExist(By.XPath("//*[@id='profile_short']/div[1]/div[2]/a[2]"), webDriver))
            {
                IWebElement querBirth = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[1]/div[2]/a[2]"));
                string birth = querBirth.Text.Substring(0, 4);
                return Convert.ToInt32(birth);
            }
            else
                return null; */
        }

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

        private List<string> Educations(IWebDriver webDriver)
        {
            List<string> education = new List<string>();
            try
            {
                FindEducation(webDriver, education);
                return education;
            }
            catch
            {
                return education;
            }
        }

        private void FindEducation(IWebDriver webDriver, List<string> education)
        {
            for (int i = 0; i < 10; i++)
            {
                if (IsElementExist(By.XPath("//*[@id='profile_short']/div[" + i + "]"), webDriver))
                    ExistEducation(webDriver, education, i);
            }
        }

        private void ExistEducation(IWebDriver webDriver, List<string> education, int i)
        {
            IWebElement querEducation = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[" + i + "]"));
            if (querEducation.Text.Contains("Место учёбы:") || querEducation.Text.Contains("Образование:"))
            {
                string ed1 = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[" + i + "]/div[2]")).Text;
                education.Add(ed1);
            }
            if (querEducation.Text.Contains("Показать подробную информацию"))
            {
                FindAllInformation(webDriver, education, querEducation);
            }
        }

        private void FindAllInformation(IWebDriver webDriver, List<string> education, IWebElement querEducation)
        {
            querEducation.Click();
            for (int j = 0; j < 10; j++)
            {
                if (IsElementExist(By.XPath("//*[@id='profile_full']/div[" + j + "]"), webDriver))
                {
                    FindDivEducation(webDriver, education, j);
                }
            }
        }

        private void FindDivEducation(IWebDriver webDriver, List<string> education, int j)
        {
            if (webDriver.FindElement(By.XPath("//*[@id='profile_full']/div[" + j + "]")).Text.StartsWith("Образование"))
            {
                for (int k = 0; k < 10; k++)
                {
                    FindEducation(webDriver, education, j, k);
                }
            }
        }

        private void FindEducation(IWebDriver webDriver, List<string> education, int j, int k)
        {
            if (IsElementExist(By.XPath("//*[@id='profile_full']/div[" + j + "]/div[2]/div[" + k + "]"), webDriver))
            {
                IWebElement querAllEducation = webDriver.FindElement(By.XPath("//*[@id='profile_full']/div[" + j + "]/div[2]/div[" + k + "]"));
                if (querAllEducation.Text.Contains("Вуз:") || querAllEducation.Text.Contains("Школа:"))
                {
                    string ed1 = webDriver.FindElement(By.XPath("//*[@id='profile_full']/div[" + j + "]/div[2]/div[" + k + "]/div[2]/a[1]")).Text;
                    education.Add(ed1);
                }
            }
        }

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
        }

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

