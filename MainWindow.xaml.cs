using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Threading;
using OpenQA.Selenium.Support.UI;
using OpenQA.Selenium.Interactions;

namespace WPF1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            IWebDriver driver = new FirefoxDriver(@"C:\Users\User\OneDrive\летний интенсив\", new FirefoxOptions() { BrowserExecutableLocation = @"C:\Program Files\Mozilla Firefox\firefox.exe" });

            driver.Navigate().GoToUrl("https://vk.com/search?c%5Bper_page%5D=40&c%5Bphoto%5D=1&c%5Bsection%5D=people");

            IWebElement querName = driver.FindElement(By.Id("search_query"));
            querName.SendKeys(tbName.Text + " " + tbLName.Text + " \n");

            WebDriverWait element = new WebDriverWait(driver, TimeSpan.FromSeconds(10));

            IWebElement web = driver.FindElement(By.XPath("//*[@id='container3']"));         //"//*[@id='container5']/table/tbody/tr/td[1]"));
            //web.SendKeys(OpenQA.Selenium.Keys.Down);
            web.Click();
            element.Timeout = TimeSpan.FromSeconds(10);

            web.SendKeys("Россия");
            //web.FindElement(By.XPath("//*[@id='option_list_options_container_3_2']"));
            MessageBox.Show(web.Text+web.Location.X+web.Location.Y);

           // SelectElement select = new SelectElement(web);
           // Thread.Sleep(5000);
            //select.SelectByText("По дате регистрации");
            
            
            
            
            
            
            
            
            
            
            //Actions action = new Actions(driver);
            //action.MoveByOffset(1000, 300).Click();
            //action.Release(web);
            //web.Click();
            //web.SendKeys("Россия");
            
            //web.Click();

            //SelectElement select = new SelectElement(web);
            //select.SelectByText("Россия");

            //IWebElement elem = driver.FindElement(By.XPath("//*[@id='option_list_options_container_4_1']/b/em"));
            //elem.Click();
            //action.MoveToElement(elem, 1, 1);
            
            //IWebElement querCountry = 
            //querCountry.Clear();
            //querCountry.SendKeys("Россия \n");


            //IWebElement querCity = driver.FindElement(By.XPath("//*[@id='container4']/table/tbody/tr/td[1]/input[1]"));
            //querCity.SendKeys(querCity.Text+"\n");

            //element.Timeout = TimeSpan.FromSeconds(10);
            
            
            
            //Thread.Sleep(500);
            //IWebElement element = new WebDriverWait(driver, Timeout.InfiniteTimeSpan).Until(ExpectedConditions.PresenceOfAllElementsLocatedBy(By.ClassName("name")));
            /*IWebElement name = driver.FindElement(By.ClassName("name"));

            Thread.Sleep(500);

            driver.FindElement(By.LinkText(name.Text)).Click();

            Thread.Sleep(500);

            IWebElement city = driver.FindElement(By.XPath("//*[@id='profile_short']/div[2]/div[2]/a"));

            MessageBox.Show(city.Text);

            for (int i=0; i<5; i++)
            {
                IWebElement city = driver.FindElement(By.Name("labeled"));
                str += city.Text + "\n";
            } */

            //driver.Quit();
        }
    }
}
