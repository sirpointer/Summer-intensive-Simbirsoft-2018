using OpenQA.Selenium;
using OpenQA.Selenium.Edge;
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
                Name = NameComboBox.Text,
                LastName = LastNameComboBox.Text
            };
            SetSerachOptions(options);
            WebDriverWorks(options);

            foreach (var ui in (Application.Current.MainWindow.Content as Grid).Children)
            {
                if (ui is Frame)
                    (ui as Frame).Navigate(new Uri("ListOfPeoplePage.xaml", UriKind.Relative));
            }
        }

        private static void WebDriverWorks(FindPeople.SearchOptions options)
        {
            IWebDriver webDriver;

            try
            {
                webDriver = new FirefoxDriver();
            }
            catch
            {
                try
                {
                    webDriver = new ChromeDriver();
                }
                catch
                {
                    try
                    {
                        webDriver = new EdgeDriver();
                    }
                    catch
                    {
                        webDriver = new InternetExplorerDriver();
                    }
                }
            }

            FindPeople.IFindPeople find = new FindPeople.FindPeople();

            find.FindPeopleOnVK(webDriver, options);
            find.FindPeopleOnOK(webDriver, options);

            App.PersonInformation = new List<FindPeople.PersonInformation>();

            foreach (var person in find.PeopleFromVK)
                App.PersonInformation.Add(person);

            foreach (var person in find.PeopleFromOK)
                App.PersonInformation.Add(person);

            webDriver.Quit();
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
                if (!string.IsNullOrWhiteSpace(CityComboBox.Text))
                    options.City = CityComboBox.Text;

            if (EducationOption.Text == "+")
                if (!string.IsNullOrWhiteSpace(EducationComboBox.Text))
                    options.City = EducationComboBox.Text;
        }


        private void InputComboBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUserLogin != null)
            {
                ComboBox cb = sender as ComboBox;

                switch (cb.Name)
                {
                    case "LastNameComboBox":
                        SetLastNameTooltips(cb);
                        break;
                    case "NameComboBox":
                        SetFirstNameTooltips(cb);
                        break;
                    case "CityComboBox":
                        SetCityTooltips(cb);
                        break;
                    case "EducationComboBox":
                        SetEducationTooltips(cb);
                        break;
                }

                if (cb.Items.Count != 0)
                    (sender as ComboBox).IsDropDownOpen = true;
            }
        }

        private void InputComboBox_LostFocus(object sender, RoutedEventArgs e)
        {
            (sender as ComboBox).IsDropDownOpen = false;
        }


        private void LastNameComboBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (App.CurrentUserLogin != null)
            {
                var box = sender as ComboBox;

                switch (box.Name)
                {
                    case "LastNameComboBox":
                        SetLastNameTooltips(box);
                        break;
                    case "NameComboBox":
                        SetFirstNameTooltips(box);
                        break;
                    case "CityComboBox":
                        SetCityTooltips(box);
                        break;
                    case "EducationComboBox":
                        SetEducationTooltips(box);
                        break;
                }
            }
        }

        private static void SetLastNameTooltips(ComboBox box)
        {
            List<string> tips = DatabaseInteraction.PeopleFromDb.LastNameFoundPeople(App.CurrentUserLogin, startWith: box.Text);

            box.Items.Clear();

            foreach (var tip in tips)
            {
                box.Items.Add(tip);
            }
        }

        private static void SetFirstNameTooltips(ComboBox box)
        {
            List<string> tips = DatabaseInteraction.PeopleFromDb.FirstNameFoundPeople(App.CurrentUserLogin, startWith: box.Text);

            box.Items.Clear();

            foreach (var tip in tips)
            {
                box.Items.Add(tip);
            }
        }

        private static void SetCityTooltips(ComboBox box)
        {
            List<string> tips = DatabaseInteraction.PeopleFromDb.CityFoundPeople(App.CurrentUserLogin, startWith: box.Text);

            box.Items.Clear();

            foreach (var tip in tips)
            {
                box.Items.Add(tip);
            }
        }

        private static void SetEducationTooltips(ComboBox box)
        {
            List<string> tips = DatabaseInteraction.PeopleFromDb.EducationFoundPeople(App.CurrentUserLogin, startWith: box.Text);

            box.Items.Clear();

            foreach (var tip in tips)
            {
                box.Items.Add(tip);
            }
        }
    }
}
