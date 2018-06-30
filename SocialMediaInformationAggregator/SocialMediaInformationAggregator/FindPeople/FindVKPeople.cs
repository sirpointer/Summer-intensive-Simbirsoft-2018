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

            //Ищет человека в поисковике по имени, добавить TextBox, переделать по-человечески
            IWebElement querName = webDriver.FindElement(By.Id("search_query"));
            querName.SendKeys(searchOptions.Name + " " + searchOptions.LastName + "\n");

            WebDriverWait element = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));

            Country(webDriver);
            City(webDriver);
            Thread.Sleep(1000);
            //try
            //{
                for(int i=1;i<6;i++)
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
                        personInformation.Education = new List<string>() { Educations(webDriver) };
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

        private void City(IWebDriver webDriver)
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
                        if (IsElementExist(By.XPath("//*[@id='option_list_options_container_3_" + i + "']"), webDriver))
                        {
                            IWebElement city = webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_2_" + i + "']"));
                            i++;
                            if (city.Text == "Ульяновск")
                            {
                                elemExist = false;
                                city.Click();
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

        private void Country(IWebDriver webDriver)
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

            /*
            Thread.Sleep(500);
            if (IsElementExist(By.XPath("//*[@id='profile_short']/div[1]/div[2]/a[2]"), webDriver))
            {
                IWebElement querBirth = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[1]/div[2]/a[2]"));
                string birth = querBirth.Text.Substring(0, 4);
                return Convert.ToInt32(birth);
            }
            else
                return null;*/
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

        private string Educations(IWebDriver webDriver)
        {
            string education;
            try
            {
                for (int i = 0; i < 10; i++)
                {
                    if (IsElementExist(By.XPath("//*[@id='profile_short']/div[" + i + "]"), webDriver))
                    {
                        IWebElement querEducation = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[" + i + "]"));
                        if (querEducation.Text.Contains("Место учёбы:") || querEducation.Text.Contains("Образование:"))
                        {
                            education = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[" + i + "]/div[2]")).Text;
                            return education;
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

