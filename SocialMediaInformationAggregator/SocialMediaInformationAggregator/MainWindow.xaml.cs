using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
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

namespace SocialMediaInformationAggregator
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IWebDriver driver = new FirefoxDriver(@"C:\Users\User\OneDrive\летний интенсив\", new FirefoxOptions() { BrowserExecutableLocation = @"C:\Program Files\Mozilla Firefox\firefox.exe" });
            FindPeople.FindPeople peopleVK = new FindPeople.FindPeople();
            peopleVK.FindPeopleOnVK(driver);
            //peopleVK.PeopleFromVK;
            MessageBox.Show(peopleVK.PeopleFromVK[0].Name+ peopleVK.PeopleFromVK[0].LastName+ peopleVK.PeopleFromVK[0].Cities[0]+ peopleVK.PeopleFromVK[0].Education[0]+ peopleVK.PeopleFromVK[0].YearOfBirth.ToString());


        }
    }
}
