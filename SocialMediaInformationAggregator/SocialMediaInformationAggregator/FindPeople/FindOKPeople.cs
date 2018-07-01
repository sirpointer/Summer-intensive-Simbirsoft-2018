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
                var f = (DateTime.Today.Year - searchOptions.ForThisYear);
                //var selectElement = new SelectElement(from);
                //Thread.Sleep(1000);
                //selectElement.SelectByText(f.ToString());
                from.Click();
                from.SendKeys(f.ToString());
                Thread.Sleep(500);
                //до
                IWebElement to = webDriver.FindElement(By.Name("st.tillAge"));
                var t = (DateTime.Today.Year - searchOptions.YearOfBirth);
                //var selectElem2 = new SelectElement(to);
                //Thread.Sleep(1000);
                //selectElement.SelectByText(t.ToString());
                to.Click();
                to.SendKeys(t.ToString());

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
                IWebElement City = webDriver.FindElement(By.Id("field_city"));
                City.SendKeys(searchOptions.City);
                // Копирование ссылок
                //1 человек в списке

                Thread.Sleep(1000);
                from.Click();
                //if (IsElementExists(By.CssSelector("#gs_result_list > div:nth-child(1) > div > div.caption.gs_result_i > div.gs_result_i_t > div.shortcut-wrap > div.ellip > a")))
                //{
                //    IWebElement p1 = webDriver.FindElement(By.CssSelector("#gs_result_list > div:nth-child(1) > div > div.caption.gs_result_i > div.gs_result_i_t > div.shortcut-wrap > div.ellip > a"));

                //    p1.GetAttribute("href");


                //}
                ////2 человек
                //if (IsElementExists(By.CssSelector("#gs_result_list > div:nth-child(2) > div > div.caption.gs_result_i > div.gs_result_i_t > div.shortcut-wrap > div.ellip > a")))
                //{
                //    IWebElement p2 = webDriver.FindElement(By.CssSelector("#gs_result_list > div:nth-child(2) > div > div.caption.gs_result_i > div.gs_result_i_t > div.shortcut-wrap > div.ellip > a"));
                //    p2.GetAttribute("href");

                //}
                ////3 человек
                //if (IsElementExists(By.CssSelector("#gs_result_list > div:nth-child(3) > div > div.caption.gs_result_i > div.gs_result_i_t > div.shortcut-wrap > div.ellip > a")))
                //{
                //    IWebElement p3 = webDriver.FindElement(By.CssSelector("#gs_result_list > div:nth-child(3) > div > div.caption.gs_result_i > div.gs_result_i_t > div.shortcut-wrap > div.ellip > a"));
                //    p3.GetAttribute("href");
                //}
                List<string> education1 = new List<string>();
                for (int i = 1; i < 6; i++)
                {
                    if (IsElementExist(By.XPath("//*[@id='gs_result_list']/div[" + i + "]/div/div[2]/div[1]/div[1]/div[1]/a"), webDriver))
                    {
                        Thread.Sleep(500);
                        //Переход на конкретного человека
                        IWebElement people = webDriver.FindElement(By.XPath("//*[@id='gs_result_list']/div[" + i + "]/div/div[2]/div[1]/div[1]/div[1]/a"));
                        webDriver.FindElement(By.XPath("//*[@id='gs_result_list']/div[" + i + "]/div/div[2]/div[1]/div[1]/div[1]/a")).Click();
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
                        if (IsElementExist(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[1]"), webDriver))
                        {
                            IWebElement more = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[1]"));
                            more.Click();
                        }

                        string city = "";
                        if (IsElementExists(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[1]/div[2]/div[2]"), webDriver))
                        {
                            city = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[1]/div[2]/div[2]")).Text;
                        }
                        string education = "";
                        IWebElement ed = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[2]"));
                        if (IsElementExists(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[2]"), webDriver) || ed.Text.Contains("Учеба") )
                        {
                            for (int a = 0; a < 5; i++)
                            {
                                if (IsElementExist(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[3]/div[" + a + "]/div[2]/div[1]/div/a/span"),webDriver))
                                {
                                    education = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[3]/div[" + a + "]/div[2]/div[1]/div/a/span")).Text;
                                    education1.Add(education);
                                }
                            }
                        }
                        //IWebElement photo = webDriver.FindElement(By.XPath("//*[@id='hook_Block_LeftColumnTopCardFriend']/div[1]/a"));
                        //string photoSRC = photo.GetAttribute("srcset");
                        ////Загружаем изображение на диск
                        //WebClient wc = new WebClient();
                        //wc.DownloadFileAsync(new Uri(photoSRC), @"D:\Photo\"+i + System.IO.Path.GetFileName(@"D:\Photo\"+i));//System.IO.Path.GetFileName(path) - получает имя файла
                        PersonInformation personInformation = new PersonInformation
                        {
                            Name = name.Split(' ')[0],
                            LastName = name.Split(' ')[1],
                            YearOfBirth = Convert.ToInt32(YearB),
                            Cities = new List<string>() { city },
                            Education = education1,
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
       
        private void FindEducationOK(IWebDriver webDriver, List<string> education)
        {
            for (int i = 0; i < 10; i++)
            {
                if (IsElementExist(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[3]/div[" + i + "]/div[2]/div[1]/div/a/span"), webDriver)) ;
                ExistEducationOK(webDriver, education, i);
            }
        }

        private void ExistEducationOK(IWebDriver webDriver, List<string> education, int i)
        {
            IWebElement querEducation = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[3]/div[" + i + "]/div[2]/div[1]/div/a/span"));
            if (querEducation.Text.Contains("Место учёбы:") || querEducation.Text.Contains("Образование:"))
            {
                string ed1 = webDriver.FindElement(By.XPath("//*[@id='profile_short']/div[" + i + "]/div[2]")).Text;
                education.Add(ed1);
            }
            if (querEducation.Text.Contains("Показать подробную информацию"))
            {
                FindAllInformationOK(webDriver, education, querEducation);
            }
        }

        private void FindAllInformationOK(IWebDriver webDriver, List<string> education, IWebElement querEducation)
        {
            querEducation.Click();
            for (int j = 0; j < 10; j++)
            {
                if (IsElementExist(By.XPath("//*[@id='profile_full']/div[" + j + "]"), webDriver))
                {
                    FindDivEducationOK(webDriver, education, j);
                }
            }
        }

        private void FindDivEducationOK(IWebDriver webDriver, List<string> education, int j)
        {
            if (webDriver.FindElement(By.XPath("//*[@id='profile_full']/div[" + j + "]")).Text.StartsWith("Образование"))
            {
                for (int k = 0; k < 10; k++)
                {
                    FindEducationOK(webDriver, education, j, k);
                }
            }
        }

        private void FindEducationOK(IWebDriver webDriver, List<string> education, int j, int k)
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
    }
}
