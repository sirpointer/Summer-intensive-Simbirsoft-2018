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
using System.IO;

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
                Thread.Sleep(2000);
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
                Thread.Sleep(1000);
                if (IsElementExist(By.Id("community1CustomSpan"), webDriver) && searchOptions.Schools != null)
                {
                    IWebElement School = webDriver.FindElement(By.Id("community1CustomSpan"));
                    Thread.Sleep(3000);
                    from.Click();
                    School.Click();
                }
                if (IsElementExist(By.XPath("//*[@id='community1CityInput']"), webDriver) && searchOptions.City != null && searchOptions.Schools != null)
                {
                    IWebElement SchoolCity = webDriver.FindElement(By.XPath("//*[@id='community1CityInput']"));
                    Thread.Sleep(1000);
                    SchoolCity.SendKeys(searchOptions.City);
                    Thread.Sleep(1000);
                    SchoolCity.SendKeys(Keys.ArrowDown);
                    Thread.Sleep(1000);
                    SchoolCity.SendKeys(Keys.Enter);
                    Thread.Sleep(1000);
                    IWebElement ChooseSchool = webDriver.FindElement(By.XPath("//*[@id='community1Input']"));
                    ChooseSchool.SendKeys(searchOptions.Schools);
                    Thread.Sleep(500);
                    ChooseSchool.SendKeys(Keys.Enter);
                }
                if (IsElementExist(By.ClassName("gs_filter_t"), webDriver) && searchOptions.City != null && searchOptions.Education!=null)
                {
                    for (int i = 0; i < 6; i++)
                    {
                        var elements = webDriver.FindElements(By.ClassName("gs_filter_t"));
                        if (elements[i].Text.Contains("ВУЗ"))
                        {
                            Thread.Sleep(1000);
                            elements[i].Click();
                            IWebElement UniverSpan = webDriver.FindElement(By.Id("community3CustomSpan"));
                            UniverSpan.Click();
                            IWebElement UniverCity = webDriver.FindElement(By.Id("community3CityInput"));
                            UniverCity.SendKeys(searchOptions.City);
                            Thread.Sleep(1000);
                            UniverCity.SendKeys(Keys.ArrowDown);
                            Thread.Sleep(1000);
                            UniverCity.SendKeys(Keys.Enter);
                            Thread.Sleep(1000);
                            IWebElement Univer = webDriver.FindElement(By.Id("community3Input"));
                            Thread.Sleep(1000);
                            Univer.SendKeys(searchOptions.Education);
                            Thread.Sleep(1000);
                            Univer.SendKeys(Keys.ArrowDown);
                            Thread.Sleep(1000);
                            Univer.SendKeys(Keys.Enter);
                        }
                    }
                }
                // Копирование ссылок
                //1 человек в списке

                Thread.Sleep(1000);
                if (IsElementExist(By.ClassName("gs_result_i_t_name"), webDriver))
                {
                    List<string> education1 = new List<string>();
                    
                    for (int i = 0; i < 6; i++)
                    {
                        var iElement = webDriver.FindElements(By.ClassName("gs_result_i_t_name"));
                        if (i >= 0 && i < iElement.Count)
                        {
                            //Переход на конкретного человека
                            IWebElement people = iElement[i];
                            //webDriver.FindElement(By.XPath("//*[@id='gs_result_list']/div[" + i + "]/div/div[2]/div[1]/div[1]/div[1]/a")).Click();
                            ((IJavaScriptExecutor)webDriver).ExecuteScript("window.scrollTo(0, document.head.scrollHeight)");
                            Thread.Sleep(1000);
                            people.Click();
                            Thread.Sleep(500);
                            string name = webDriver.FindElement(By.XPath("//*[@id='hook_Block_MiddleColumnTopCardFriend']/div/div/div[1]/div/span[1]/h1")).Text;
                            string YearB = "";
                            if (IsElementExist(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[1]/div[1]/div[2]"), webDriver) && webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[1]/div[1]/div[2]")).Text.Contains("(") &&
                                 webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[1]/div[1]/div[2]")).Text.Contains(")"))
                            {
                                YearB = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[1]/div[1]/div[2]")).Text;
                                string[] q = YearB.Split(' ');
                                foreach (var p in q)
                                {
                                    if (p.Length == 4 && p.All(x => char.IsDigit(x)))
                                    {
                                        YearB = p;
                                        break;
                                    }
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


                            //PersonInformation personInformation = new PersonInformation
                            //{
                            //    Name = name.Split(' ')[0],
                            //    LastName = name.Split(' ')[1],
                            //    YearOfBirth = Convert.ToInt32(YearB),
                            //    Cities = new List<string>() { city },
                            //    Education = education,
                            //    SocialNetwork = SocialNetwork.OK,
                            //    // Photo = new Image() { }


                            //};
                            PersonInformation personInformation = new PersonInformation { };
                            personInformation.Name = name.Split(' ')[0];
                            personInformation.LastName = name.Split(' ')[1];
                            if (IsElementExist(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[1]/div[1]/div[2]"), webDriver) && webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[1]/div[1]/div[2]")).Text.Contains("(") &&
                                 webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[1]/div[1]/div[2]")).Text.Contains(")"))
                            {
                                personInformation.YearOfBirth = Convert.ToInt32(YearB);
                            }
                            personInformation.Cities = new List<string>() { city };
                            personInformation.Education = education;
                            personInformation.SocialNetwork = SocialNetwork.OK;
                          //  personInformation.Photo = PersonPhoto(webDriver, i);


                        PeopleFromOK.Add(personInformation);

                            webDriver.Navigate().Back();
                        }
                        else
                        {
                            break;
                        }
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
            void ScrollToBottom(IWebDriver driver)
            {
                long scrollHeight = 0;

                do
                {
                    IJavaScriptExecutor js = (IJavaScriptExecutor)driver;
                    var newScrollHeight = (long)js.ExecuteScript("window.scrollTo(0, document.body.scrollHeight); return document.body.scrollHeight;");

                    if (newScrollHeight == scrollHeight)
                    {
                        break;
                    }
                    else
                    {
                        scrollHeight = newScrollHeight;
                        Thread.Sleep(400);
                    }
                } while (true);
            }

        }
        private Image PersonPhoto(IWebDriver webDriver, int i)
        {
            IWebElement img = webDriver.FindElement(By.Id("viewImageLinkId"));
            string imageSrc = img.GetAttribute("src");
            Thread.Sleep(1000);
            Screenshot screenshot = ((ITakesScreenshot)webDriver.FindElement(By.XPath("//*[@id='viewImageLinkId']"))).GetScreenshot();
            string im = @"D:\Photo" + i.ToString() + ".png";
            screenshot.SaveAsFile(im);
            Image image = new Image();
            BitmapImage bitmapImage = new BitmapImage();
            bitmapImage.UriSource = new Uri(imageSrc);
            image.Source = bitmapImage;
            return image;
        }
        private List<string> FindEducationOK(IWebDriver webDriver)
        {
            List<string> allEducations = new List<string>();
            for (int i = 0; i < 20; i++)
            {
                var iElement = webDriver.FindElements(By.ClassName("user-profile_group_h"));
                if (i >= 0 && i < iElement.Count)
                {
                    IWebElement web = iElement[i];
                    if (web.Text == "Учеба")
                    {
                        for (int j = 0; j < 20; j++)
                        {
                            var iElement1 = webDriver.FindElements(By.ClassName("user-profile_i"));
                            if (j >= 0 && j < iElement1.Count)
                            {
                                IWebElement path = iElement1[j];
                                if (path.Text.Contains("Учится") || path.Text.Contains("Окончил") || path.Text.Contains("Окончила"))
                                {
                                    allEducations.Add(iElement1[j].Text);
                                }
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
