using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Support.UI;

namespace SocialMediaInformationAggregator.FindPeople
{
    public partial class FindPeople //: IFindPeople
    {
        private List<PersonInformation> _peopleFromVK = new List<PersonInformation>();

        public List<PersonInformation> PeopleFromVK { get => _peopleFromVK; private set => _peopleFromVK = value; }
        
        public void FindPeopleOnVK(IWebDriver webDriver)//, SearchOptions searchOptions)
        {

            webDriver.Navigate().GoToUrl("https://vk.com/search?c%5Bper_page%5D=40&c%5Bphoto%5D=1&c%5Bsection%5D=people");

            IWebElement querName = webDriver.FindElement(By.Id("search_query"));
            querName.SendKeys("Никита" + " " + "Новиков" + " \n");

            WebDriverWait element = new WebDriverWait(webDriver, TimeSpan.FromSeconds(10));

            IWebElement country = webDriver.FindElement(By.XPath("//*[@id='container3']"));         
            country.Click();
            element.Timeout = TimeSpan.FromSeconds(10);
            IWebElement countryFind = webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_3_2']"));
            countryFind.Click();

/*
            IWebElement city = webDriver.FindElement(By.XPath("//*[@id='container2']"));
            city.Click();
            element.Timeout = TimeSpan.FromSeconds(10);
            IWebElement cityFind = webDriver.FindElement(By.XPath("//*[@id='option_list_options_container_2_3']"));
            cityFind.Click();

*/

            Thread.Sleep(1000);
            IWebElement people = webDriver.FindElement(By.ClassName("name"));
            webDriver.FindElement(By.LinkText(people.Text)).Click();

            PersonInformation personInformation = new PersonInformation();
            personInformation.Name = "Никита";
            personInformation.LastName = "Новиков";
            personInformation.YearOfBirth = YearBirth(webDriver);
            personInformation.Cities = new List<string>() { LiveCity(webDriver) };
            personInformation.Education = new List<string>() { Educations(webDriver) };

            PeopleFromVK.Add(personInformation);
        }

        private int YearBirth(IWebDriver webDriver)
        {
            Thread.Sleep(500);
            IWebElement querBirth = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[1]/div[2]/a[2]"));
            string birth = querBirth.Text.Substring(0, 4);
            return Convert.ToInt32(birth);
        }

        private string LiveCity(IWebDriver webDriver)
        {
            Thread.Sleep(500);
            IWebElement querCity = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[2]/div[2]/a"));
            string city = querCity.Text;
            return city;
        }

        private string Educations(IWebDriver webDriver)
        {
            Thread.Sleep(500);
            IWebElement querEducation = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[3]/div[2]"));
            string education = querEducation.Text;
            return education;
        }
    }
}
