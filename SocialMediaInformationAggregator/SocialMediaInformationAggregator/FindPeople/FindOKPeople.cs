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
using System.Net;

namespace SocialMediaInformationAggregator.FindPeople
{
    public partial class FindPeople : IFindPeople
    {
        private List<PersonInformation> _peopleFromOK = new List<PersonInformation>();

        public List<PersonInformation> PeopleFromOK { get => _peopleFromOK; private set => _peopleFromOK = value; }

        public void FindPeopleOnOK(IWebDriver webDriver, SearchOptions searchOptions)
        {
            {
                webDriver.Navigate().GoToUrl("https://ok.ru/");

                //ввод логина
                IWebElement login = webDriver.FindElement(By.Id("field_email"));
                login.SendKeys("89278163946");

                //ввод пароля
                IWebElement password = webDriver.FindElement(By.Id("field_password"));
                password.SendKeys("seleniumtest[]{}" + Keys.Enter);
                Thread.Sleep(1000);
                //переход к поиску
                webDriver.Navigate().GoToUrl("https://ok.ru/search?st.mode=Users");

                //Ввод имени и фамилии
                IWebElement Name = webDriver.FindElement(By.Id("query_usersearch"));
                Name.SendKeys(searchOptions.Name + " " + searchOptions.LastName);

                //сколько лет от
                IWebElement from = webDriver.FindElement(By.Id("field_fromage"));
                //var selectElement = new SelectElement(from);
                //Thread.Sleep(1000);
                //selectElement.SelectByText(f.ToString());
                from.Click();
                from.SendKeys(searchOptions.YearOfBirth.ToString());
                Thread.Sleep(500);
                //до
                IWebElement to = webDriver.FindElement(By.Name("st.tillAge"));
                //var selectElem2 = new SelectElement(to);
                //Thread.Sleep(1000);
                //selectElement.SelectByText(t.ToString());
                to.Click();
                to.SendKeys(searchOptions.ForThisYear.ToString());

                IWebElement Country = webDriver.FindElement(By.Id("customPlaceItemSpan"));
                Country.Click();
                //ввод страны
                IWebElement SearchCountry = webDriver.FindElement(By.Id("field_country_int"));
                Thread.Sleep(1000);
                var selel = new SelectElement(SearchCountry);
                selel.SelectByText("Россия");
                // SearchCountry.Click();
                // SearchCountry.SendKeys("Россия");
                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                //ввод города
                if (IsElementExist(By.Id("field_city"), webDriver) && (searchOptions.City != null))
                {
                    IWebElement City = webDriver.FindElement(By.Id("field_city"));
                    City.SendKeys(searchOptions.City);
                }
                IWebElement iop = webDriver.FindElement(By.Id("genderDoesntMatterSpan"));
                iop.Click();
                if (IsElementExist(By.Id("community1CustomSpan"), webDriver) && searchOptions.Schools != null)
                {
                    IWebElement School = webDriver.FindElement(By.Id("community1CustomSpan"));
                    Thread.Sleep(1000);
                    School.Click();
                }
                if (IsElementExist(By.XPath("//*[@id='community1CityInput']"), webDriver) && searchOptions.City != null && searchOptions.Schools != null)
                {
                    IWebElement SchoolCity = webDriver.FindElement(By.XPath("//*[@id='community1CityInput']"));
                    Thread.Sleep(1000);
                    SchoolCity.SendKeys(searchOptions.City);
                    Thread.Sleep(1000);
                    SchoolCity.SendKeys(Keys.ArrowDown + Keys.Enter);
                    Thread.Sleep(1000);
                    IWebElement ChooseSchool = webDriver.FindElement(By.XPath("//*[@id='community1Input']"));
                    ChooseSchool.SendKeys(searchOptions.Schools);
                    Thread.Sleep(500);
                    ChooseSchool.SendKeys(Keys.Enter);
                }
                // Копирование ссылок
                //1 человек в списке

                Thread.Sleep(1000);
                from.Click();

                List<string> education1 = new List<string>();
                for (int i = 1; i < 6; i++)
                {
                    if (IsElementExist(By.XPath("//*[@id='gs_result_list']/div[" + i + "]/div/div[2]/div[1]/div[1]/div[1]/a"), webDriver))
                    {
                        Thread.Sleep(500);
                        //Переход на конкретного человека
                        IWebElement people = webDriver.FindElement(By.XPath("//*[@id='gs_result_list']/div[" + i + "]/div/div[2]/div[1]/div[1]/div[1]/a"));
                        //webDriver.FindElement(By.XPath("//*[@id='gs_result_list']/div[" + i + "]/div/div[2]/div[1]/div[1]/div[1]/a")).Click();
                        people.Click();
                        Thread.Sleep(500);
                        string name = webDriver.FindElement(By.XPath("//*[@id='hook_Block_MiddleColumnTopCardFriend']/div/div/div[1]/div/span[1]/h1")).Text;
                        string YearB = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[1]/div[1]/div[2]")).Text;
                        string[] q = YearB.Split(' ');
                        foreach (var p in q)
                        {
                            if (p.Length == 4 && p.All(x => char.IsDigit(x)))
                            {
                                YearB = p;
                                break;
                            }
                        }

                        Thread.Sleep(500);

                        string city = "";

                        if (IsElementExists(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[1]/div[2]/div[2]"), webDriver))
                        {
                            city = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[1]/div[2]/div[2]")).Text;
                        }

                        List<string> education = new List<string>();

                        //Timer timer = new Timer()


                        if (IsElementExist(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[1]"), webDriver))
                        {
                            IWebElement more = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[1]"));
                            more.Click();
                            Thread.Sleep(500);
                            education = FindEducationOK(webDriver);

                        }


                        PersonInformation personInformation = new PersonInformation
                        {
                            Name = name.Split(' ')[0],
                            LastName = name.Split(' ')[1],
                            YearOfBirth = Convert.ToInt32(YearB),
                            Cities = new List<string>() { city },
                            Education = education,
                            SocialNetwork = SocialNetwork.OK,
                            // Photo = new Image() { }


                        };

                        PeopleFromOK.Add(personInformation);

                        webDriver.Navigate().Back();
                    }
                }




                //проверка

                bool IsElementExists(By iClassName, IWebDriver webDriver1)
                {
                    try
                    {
                        webDriver1.FindElement(iClassName);
                        return true;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                }

            }

        }


        private List<string> FindEducationOK(IWebDriver webDriver)
        {
            List<string> allEducations = new List<string>();
            for (int i = 1; i < 20; i++)
            {
                if (IsElementExist(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[" + i + "]"), webDriver))    //CssSelector("# hook_Block_AboutUserSummary > div > div > div.h-mod.user-profile_full-info.__active > div:nth-child(" + i + ")"), webDriver))
                {
                    IWebElement web = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[" + i + "]"));        //CssSelector("# hook_Block_AboutUserSummary > div > div > div.h-mod.user-profile_full-info.__active > div:nth-child(" + i + ")"));
                    if (web.Text.StartsWith("Учеба"))
                    {
                        for (int j = 1; j < 20; j++)
                        {
                            if (IsElementExist(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[" + (i + 1) + "]/div[" + j + "]"), webDriver))
                            {
                                IWebElement path = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[" + (i + 1) + "]/div[" + j + "]"));
                                if (path.Text.StartsWith("Учится") || path.Text.StartsWith("Окончил") || path.Text.StartsWith("Окончила"))
                                {
                                    allEducations.Add(webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[" + (i + 1) + "]/div[" + j + "]/div[2]/div[1]")).Text);
                                }//*[@id="hook_Block_AboutUserSummary"]/div/div/div[2]/div[5]/div
                            }
                            else
                                break;
                        }
                        break;
                        //*[@id="hook_Block_AboutUserSummary"]/div/div/div[2]/div[3]/div[1]
                        //*[@id="hook_Block_AboutUserSummary"]/div/div/div[2]/div[3]/div[2]
                        //*[@id="hook_Block_AboutUserSummary"]/div/div/div[2]/div[3]/div[3]
                        //*[@id="hook_Block_AboutUserSummary"]/div/div/div[2]/div[3]/div[1]/div[2]/div[1]
                        //*[@id="hook_Block_AboutUserSummary"]/div/div/div[2]/div[3]/div[2]/div[2]/div[1]
                    }
                    //Thread.Sleep(100);
                }
            }
            return allEducations;
        }
    }
}
