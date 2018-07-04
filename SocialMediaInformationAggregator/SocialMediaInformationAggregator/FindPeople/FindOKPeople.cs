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
                Times12(webDriver, By.Id("field_fromage"));
                //сколько лет от
                IWebElement from = webDriver.FindElement(By.Id("field_fromage"));
                //var selectElement = new SelectElement(from);
                //Thread.Sleep(1000);
                //selectElement.SelectByText(f.ToString());
                from.Click();
                from.SendKeys(searchOptions.YearOfBirth.ToString());
                Thread.Sleep(2000);
                //до
                Times12(webDriver, By.Id("field_tillage"));
                IWebElement to = webDriver.FindElement(By.Id("field_tillage"));
                //var selectElem2 = new SelectElement(to);
                //Thread.Sleep(1000);
                //selectElement.SelectByText(t.ToString());
                to.Click();
                to.SendKeys(searchOptions.ForThisYear.ToString());
                Times12(webDriver, By.Id("customPlaceItemSpan"));
                IWebElement Country = webDriver.FindElement(By.Id("customPlaceItemSpan"));
                Country.Click();
                //ввод страны
                Times12(webDriver, By.Id("field_country_int"));
                IWebElement SearchCountry = webDriver.FindElement(By.Id("field_country_int"));
                var selel = new SelectElement(SearchCountry);
                selel.SelectByText("Россия");
                // SearchCountry.Click();
                // SearchCountry.SendKeys("Россия");
                webDriver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                //ввод города
                Times12(webDriver, By.Id("field_city"));
                if (IsElementExist(By.Id("field_city"), webDriver) && (searchOptions.City != null))
                {
                    IWebElement City = webDriver.FindElement(By.Id("field_city"));
                    City.SendKeys(searchOptions.City);
                }
                IWebElement iop = webDriver.FindElement(By.Id("genderDoesntMatterSpan"));
                iop.Click();
                if (IsElementExist(By.Id("community1CustomSpan"), webDriver) && searchOptions.Schools != null)
                {
                    Times12(webDriver, (By.Id("community1CustomSpan")));
                    IWebElement School = webDriver.FindElement(By.Id("community1CustomSpan"));
                    Thread.Sleep(3000);
                    School.Click();
                }
                Thread.Sleep(1000);

                if (IsElementExist(By.XPath("//*[@id='community1CityInput']"), webDriver) && (searchOptions.City != null) && searchOptions.Schools != null)
                {
                    IWebElement SchoolCity = webDriver.FindElement(By.XPath("//*[@id='community1CityInput']"));
                    Thread.Sleep(1000);
                    SchoolCity.SendKeys(searchOptions.City);
                    Thread.Sleep(1000);
                    SchoolCity.SendKeys(Keys.ArrowDown + Keys.Enter);
                    Thread.Sleep(1000);
                    Times12(webDriver, By.XPath("//*[@id='community1Input']"));
                    IWebElement ChooseSchool = webDriver.FindElement(By.XPath("//*[@id='community1Input']"));
                    ChooseSchool.SendKeys(searchOptions.Schools);
                    Thread.Sleep(500);
                    ChooseSchool.SendKeys(Keys.Enter);
                }
                from.Click();
                List<string> education1 = new List<string>();
                for (int i = 1; i < 6; i++)
                {
                    if (IsElementExist(By.XPath("//*[@id='gs_result_list']/div[" + i + "]/div/div[2]/div[1]/div[1]/div[1]/a"), webDriver))
                    {
                        Thread.Sleep(1000);
                        Times12(webDriver, By.XPath("//*[@id='gs_result_list']/div[" + i + "]/div/div[2]/div[1]/div[1]/div[1]/a"));
                        //Переход на конкретного человека
                        IWebElement people = webDriver.FindElement(By.XPath("//*[@id='gs_result_list']/div[" + i + "]/div/div[2]/div[1]/div[1]/div[1]/a"));
                        Thread.Sleep(1000);
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
                        Thread.Sleep(1000);
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
                        // IWebElement ed = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[2]"));                    
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
                           // Education = BlocksDivOK(webDriver),
                            SocialNetwork = SocialNetwork.OK,
                            // Photo = new Image() { }
                            ProfileLink = webDriver.Url

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
        private List<string> BlocksDivOK(IWebDriver webDriver)
        {
            List<string> education = new List<string>();
            List<IWebElement> blocksDiv = new List<IWebElement>();
            Thread.Sleep(500);
            for (int j = 2; j < 10; j+=2)
            {
                if (IsElementExist(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div["+ j +"]"), webDriver))
                {
                    IWebElement block = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div["+ j + "]"));
                    blocksDiv.Add(block);
                }
                for (int i = 0; i < blocksDiv.Count; i++)
                {
                    if (blocksDiv[i].Text.StartsWith("Учеба"))
                    {
                        List<IWebElement> blocksEducation = BlocksEducationOK(webDriver, j);
                        for (int p = 0; p < blocksEducation.Count; p++)
                        {
                            System.Windows.MessageBox.Show("wqeqw");
                            string educat = blocksEducation[j].FindElement(By.XPath(".//a[1]")).Text;
                            education.Add(educat);
                        }
                    }
                }
            }
            return education;
        }

        private List<IWebElement> BlocksEducationOK(IWebDriver webDriver, int j)
        {
            Thread.Sleep(500);
            List<IWebElement> blocksEducation = new List<IWebElement>();
            for (int i = 1; i < 10; i++)
            {
                if (IsElementExist(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[" + (j + 1) + "]/div[" + i + "]/div[2]/div[1]/div/a/span"), webDriver))
                {
                    System.Windows.MessageBox.Show("wq");
                    blocksEducation.Add(webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[" + (j + 1) + "]/div[" + i + "]/div[2]/div[1]/div/a/span")));
                }
                else if (IsElementExist(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[" + (j + 1) + "]/div/div[2]/div[1]/div/a/span"), webDriver))
                {
                    System.Windows.MessageBox.Show("wq");
                    blocksEducation.Add(webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[" + (j + 1) + "]/div/div[2]/div[1]/div/a/span")));
                }
            }
            return blocksEducation;
        }
        void Times12(IWebDriver webDriver, By by)
        {
            bool ex = true;
            while (ex)
            {
                if (IsElementExist(by, webDriver))
                    ex = false;
                else
                    Thread.Sleep(100);
            }
        }
    }
}
