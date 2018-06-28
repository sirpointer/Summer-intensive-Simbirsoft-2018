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
            querName.SendKeys("Никита" + " " + "Новиков" + " \n");

            WebDriverWait element = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));

            //Выбирает страну - Россия
            IWebElement country = webDriver.FindElement(By.XPath("//*[@id='container3']"));
            country.Click();
            element.Timeout = TimeSpan.FromSeconds(10);
            IWebElement countryFind = webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_3_2']"));
            countryFind.Click();

            Thread.Sleep(1000);

            //Выбирает город, почему-то косячит и считает, что в этом городе людей нет, сделать по-человечески
            //Научиться динамически выбирать после выбора страны
            /* IWebElement city = webDriver.FindElement(By.XPath("//*[@id='container2']"));
             city.Click();
             element.Timeout = TimeSpan.FromSeconds(10);
             IWebElement cityFind = webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_2_3']"));
             cityFind.Click();
             */
            for (int i = 0; i < 5; i++)
            {
                Thread.Sleep(1000);
                //Переход на конкретного человека
                IWebElement people = webDriver.FindElements(By.ClassName("name"))[i];
                webDriver.FindElements(By.LinkText(people.Text))[i].Click();

                PersonInformation personInformation = new PersonInformation();
                personInformation.Name = "Никита";
                personInformation.LastName = "Новиков";
                personInformation.YearOfBirth = YearBirth(webDriver);
                personInformation.Cities = new List<string>() { LiveCity(webDriver) };
                personInformation.Education = new List<string>() { Educations(webDriver) };
                personInformation.SocialNetwork = SocialNetwork.VK;
                personInformation.Photo = Photo(webDriver, i);

                PeopleFromVK.Add(personInformation);

                webDriver.Navigate().Back();
            }

        }

        private int? YearBirth(IWebDriver webDriver)
        {
            Thread.Sleep(500);
            if (IsElementExist(By.XPath("//*[@id='profile_short']/div[1]/div[2]/a[2]"), webDriver))
            {
                IWebElement querBirth = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[1]/div[2]/a[2]"));
                string birth = querBirth.Text.Substring(0, 4);
                return Convert.ToInt32(birth);
            }
            else
                return null;
        }

        private string LiveCity(IWebDriver webDriver)
        {
            Thread.Sleep(500);
            if (IsElementExist(By.XPath("//*[@id='profile_short']/div[2]/div[2]/a"), webDriver))
            {
                IWebElement querCity = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[2]/div[2]/a"));
                string city = querCity.Text;
                return city;
            }
            else
                return null;
        }

        private string Educations(IWebDriver webDriver)
        {
            Thread.Sleep(500);
            if (IsElementExist(By.XPath("//*[@id='profile_short']/div[3]/div[2]"), webDriver))
            {
                IWebElement querEducation = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[3]/div[2]"));
                string education = querEducation.Text;
                return education;
            }
            else
                return null;
        }

        private Image Photo(IWebDriver webDriver, int i)
        {
            Thread.Sleep(1000);
            IWebElement img = webDriver.FindElement(By.XPath("//*[@id='profile_photo_link']/img"));
            Screenshot screenshot = ((ITakesScreenshot)webDriver.FindElement(By.XPath("//*[@id='profile_photo_link']/img"))).GetScreenshot();// .FindElement(By.XPath("")).
            string im = @"C:\Users\User\OneDrive\летний интенсив\1\" + i.ToString() + ".png";
            screenshot.SaveAsFile(im);
            Image image = new Image();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(i.ToString() + ".png", UriKind.Relative);
            image.Source = bitmapImage;

            //image. = 
            //image.Source = new BitmapImage(new Uri(im));
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
