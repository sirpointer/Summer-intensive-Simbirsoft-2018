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
                //переход к поиску
                webDriver.Navigate().GoToUrl("https://ok.ru/search?st.mode=Users");

                //Ввод имени и фамилии
                IWebElement Name = webDriver.FindElement(By.Id("query_usersearch"));
                Name.SendKeys("Никита" + " " + "Новиков");
                // выбор пола



              
                //любой
                //if (AllRadioButton.Checked)
                //{
                //    all = webDriver.FindElement(By.Id("genderDoesntMatterSpan"));
                //    all.Click();
                //}//мужской
                //else if (MaleRadioButton.Checked)
                //{
                //IWebElement male = webDriver.FindElement(By.CssSelector("#gender > ul > li:nth-child(3) > div"));
                //    male.Click();
                //}//женский
                //else if (FemaleRadioButton.Checked)
                //{
                //    female = webDriver.FindElement(By.CssSelector("#gender > ul > li:nth-child(4) > div"));
                //    female.Click();
                //}

                //сколько лет от
                IWebElement from = webDriver.FindElement(By.Id("field_fromage"));
                from.Click();
                from.SendKeys("19");
                //до
                IWebElement to = webDriver.FindElement(By.Id("field_tillage"));
                to.Click();
                to.SendKeys("20");

                IWebElement Country = webDriver.FindElement(By.Id("customPlaceItemSpan"));
                Country.Click();
                //ввод страны
                IWebElement SearchCountry = webDriver.FindElement(By.Id("field_country_int"));
                Thread.Sleep(1000);
                SearchCountry.Click();
                SearchCountry.SendKeys("Россия");
                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                //ввод города
                IWebElement City = webDriver.FindElement(By.Id("field_city"));
                City.SendKeys("Ульяновск");
                // Копирование ссылок
                //1 человек в списке
                
                Thread.Sleep(1000);
                from.Click();
                //male.Click();
                //if (IsElementExists(By.CssSelector("#gs_result_list > div:nth-child(1) > div > div.caption.gs_result_i > div.gs_result_i_t > div.shortcut-wrap > div.ellip > a")))
                //{
                //    IWebElement p1 = webDriver.FindElement(By.CssSelector("#gs_result_list > div:nth-child(1) > div > div.caption.gs_result_i > div.gs_result_i_t > div.shortcut-wrap > div.ellip > a"));

                //   p1.GetAttribute("href");
                   

                //}
                ////2 человек
                //if (IsElementExists(By.CssSelector("#gs_result_list > div:nth-child(2) > div > div.caption.gs_result_i > div.gs_result_i_t > div.shortcut-wrap > div.ellip > a")))
                //{
                //    IWebElement p2 = webDriver.FindElement(By.CssSelector("#gs_result_list > div:nth-child(2) > div > div.caption.gs_result_i > div.gs_result_i_t > div.shortcut-wrap > div.ellip > a"));
                //  p2.GetAttribute("href");
                    
                //}
                ////3 человек
                //if (IsElementExists(By.CssSelector("#gs_result_list > div:nth-child(3) > div > div.caption.gs_result_i > div.gs_result_i_t > div.shortcut-wrap > div.ellip > a")))
                //{
                //    IWebElement p3 = webDriver.FindElement(By.CssSelector("#gs_result_list > div:nth-child(3) > div > div.caption.gs_result_i > div.gs_result_i_t > div.shortcut-wrap > div.ellip > a"));
                //   p3.GetAttribute("href");
                //}

                for (int i = 0; i < 2; i++)
                {
                    Thread.Sleep(1000);
                    //Переход на конкретного человека
                    IWebElement people = webDriver.FindElements(By.ClassName("gs_result_i_t_name"))[i];
                    webDriver.FindElements(By.LinkText(people.Text))[i].Click();
                    string name = webDriver.FindElement(By.XPath("//*[@id='hook_Block_MiddleColumnTopCardFriend']/div/div/div[1]/div/span[1]/h1")).Text;
                    string YearB = webDriver.FindElement(By.CssSelector("#hook_Block_AboutUserSummary > div > div > div.user-profile_list > div:nth-child(1) > div.user-profile_i_value")).Text;
                    string[] q = YearB.Split(' ');
                    foreach (var t in q)
                    {
                        if (t.Length == 4 && t.All(x => char.IsDigit(x)))
                        {
                            YearB = t;
                            break;
                        }
                    }
                    IWebElement more = webDriver.FindElement(By.CssSelector("#hook_Block_AboutUserSummary > div > div > div.h-mod.user-profile_full-info > div.user-profile_show-more.js-show-more-info"));
                    more.Click();
                    string city = "";
                    try
                    {
                        city = webDriver.FindElement(By.CssSelector("#hook_Block_AboutUserSummary > div > div > div.user-profile_list > div:nth-child(3) > div.user-profile_i_value")).Text;
                    }
                    catch
                    {

                    }
                    string education = "";
                    try
                    {
                        education = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[3]/div[3]/div[2]/div[1]/div/a/span")).Text;
                    }
                    catch
                    {

                    }
                    PersonInformation personInformation = new PersonInformation
                    {
                        Name = name.Split(' ')[0],
                        LastName = name.Split(' ')[1],
                        YearOfBirth = Convert.ToInt32(YearB),
                        Cities = new List<string>() { city },
                        Education = new List<string>() { education },
                        SocialNetwork = SocialNetwork.OK,
                        Photo = new Image()
                    };

                    PeopleFromOK.Add(personInformation);

                    webDriver.Navigate().Back();
                }
                //проверка
                bool IsElementExists(By iClassName)
                {
                    try
                    {
                        webDriver.FindElement(iClassName);
                        return true;
                    }
                    catch (NoSuchElementException)
                    {
                        return false;
                    }
                }

            }
        }
    }
}
