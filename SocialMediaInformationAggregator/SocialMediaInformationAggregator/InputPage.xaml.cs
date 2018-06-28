using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
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
    /// Логика взаимодействия для InputPage.xaml
    /// </summary>
    public partial class InputPage : Page
    {
        public InputPage()
        {
            InitializeComponent();

            for (int i = DateTime.Now.Year; i > 1900; i--)
            {
                FromYearCB.Items.Add(i.ToString());
                ToYearCB.Items.Add(i.ToString());
            }
        }

        private void OptionTB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            tb.Text = tb.Text == "+" ? tb.Text = "-" : tb.Text = "+";
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            FindPeople.SearchOptions options = new FindPeople.SearchOptions()
            {
                Name = NameTextBox.Text,
                LastName = LastNameTextBox.Text
            };
            SetSerachOptions(options);

            IWebDriver webDriver;

            try
            {
                webDriver = new EdgeDriver("WebDrivers/");
            }
            catch
            {
                webDriver = new FirefoxDriver(@"C:\Users\User\OneDrive\летний интенсив\", new FirefoxOptions() { BrowserExecutableLocation = @"C:\Program Files\Mozilla Firefox\firefox.exe" });
            }
            
            FindPeople.IFindPeople find = new FindPeople.FindPeople();

            find.FindPeopleOnVK(webDriver, options);
            //find.FindPeopleOnFacebook(webDriver, options);
           // find.FindPeopleOnOK(webDriver, options);
            
            App.PersonInformation = new List<FindPeople.PersonInformation>();
            
            foreach (var person in find.PeopleFromVK)
                App.PersonInformation.Add(person);

            /*foreach (var person in find.PeopleFromFacebook)
                App.PersonInformation.Add(person);

            foreach (var person in find.PeopleFromOK)
                App.PersonInformation.Add(person);*/

            webDriver.Quit();

            foreach (var ui in (Application.Current.MainWindow.Content as Grid).Children)
            {
                if (ui is Frame)
                    (ui as Frame).Navigate(new Uri("ListOfPeoplePage.xaml", UriKind.Relative));
            }
        }

        private void SetSerachOptions(FindPeople.SearchOptions options)
        {
            if (YearFromOption.Text == "+")
                if (FromYearCB.SelectedIndex != -1)
                    options.YearOfBirth = Convert.ToInt32(FromYearCB.SelectedValue);

            if (YearToOption.Text == "+")
                if (ToYearCB.SelectedIndex != -1)
                    options.ForThisYear = Convert.ToInt32(ToYearCB.SelectedValue);

            if (CityOption.Text == "+")
                if (!string.IsNullOrWhiteSpace(CityTB.Text))
                    options.City = CityTB.Text;

            if (EducationOption.Text == "+")
                if (!string.IsNullOrWhiteSpace(EducationTB.Text))
                    options.City = EducationTB.Text;
        }
    }
}
