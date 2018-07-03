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
                if (IsElementExist(By.Id("field_city"), webDriver))
                {
                    IWebElement City = webDriver.FindElement(By.Id("field_city"));
                    City.SendKeys(searchOptions.City);
                }
                Thread.Sleep(1000);
                from.Click();
                List<string> education1 = new List<string>();
                for (int i = 1; i < 6; i++)
                {
                    if (IsElementExist(By.XPath("//*[@id='gs_result_list']/div[" + i + "]/div/div[2]/div[1]/div[1]/div[1]/a"), webDriver))
                    {
                        Thread.Sleep(1000);
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
                        Thread.Sleep(2000);
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
                        Thread.Sleep(1000);
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
                            Education = EducationsOK(webDriver),
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
        private List<string> EducationsOK(IWebDriver webDriver)
        {
            List<string> education = new List<string>();
            for (int i = 2; i < 10; i+=2)
            {
                IWebElement EduC = webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div["+i +"]"));
                if (EduC.Text.StartsWith("Учеба"))
                {
                    List<IWebElement> blocksEducation = BlocksEducationOK(webDriver, i);
                    for (int j = 0; j < blocksEducation.Count; j++)
                    {
                        System.Windows.MessageBox.Show("QW");
                            string educat = blocksEducation[j].FindElement(By.XPath(".//a[1]")).Text;
                            education.Add(educat);
                        System.Windows.MessageBox.Show("ДОбавил");
                    }
                }
            }
            return education;
        }

        private List<IWebElement> BlocksEducationOK(IWebDriver webDriver, int j)
        {
            Thread.Sleep(500);
            List<IWebElement> blocksEducation = new List<IWebElement>();
            //for (int i = 0; i < 20; i++)
            //{
                //if (IsElementExist(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[" + i + "]/div[" + i + "]/div[2]/div[1]/div/a/span"), webDriver))
                //{
                //    System.Windows.MessageBox.Show("Нашёл школу");
                //    blocksEducation.Add(webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[" + i + "]/div[" + i + "]/div[2]/div[1]/div/a/span")));
                //}
               if (IsElementExist(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[5]/div/div[2]/div[1]/div/a/span"), webDriver))
                {
                    System.Windows.MessageBox.Show("Нашёл школу");
                    blocksEducation.Add(webDriver.FindElement(By.XPath("//*[@id='hook_Block_AboutUserSummary']/div/div/div[2]/div[5]/div/div[2]/div[1]/div/a/span")));
                }
           // }
            return blocksEducation;
        }

    }
}
