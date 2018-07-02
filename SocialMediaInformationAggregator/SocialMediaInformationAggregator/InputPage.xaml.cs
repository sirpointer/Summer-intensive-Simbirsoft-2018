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
using SocialMediaInformationAggregator.DatabaseInteraction;

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

            for (int i = DateTime.Now.Year - 15; i > 1950; i--)
            {
                FromYearCB.Items.Add(i.ToString());
                ToYearCB.Items.Add(i.ToString());
            }
        }

        /// <summary>
        /// Возвращает значение указывающее, обязатен ли год. 
        /// </summary>
        public bool YearFromChecked
        {
            get
            {
                if (YearFromOption.Text.Equals("+"))
                    return true;
                else
                    return false;
            }
        }

        public bool YearToChecked
        {
            get
            {
                if (!YearFromChecked || YearToOption.Text.Equals("-"))
                    return false;
                else
                    return true;
            }
        }

        public bool CityChecked
        {
            get
            {
                if (CityOption.Text.Equals("+"))
                    return true;
                else
                    return false;
            }
        }

        public bool EducationChecked
        {
            get
            {
                if (UniversityOption.Text.Equals("+"))
                    return true;
                else
                    return false;
            }
        }

        public bool SchoolChecked
        {
            get
            {
                if (SchoolOption.Text.Equals("+"))
                    return true;
                else
                    return false;
            }
        }

        private void OptionTB_PreviewMouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = sender as TextBlock;
            tb.Text = tb.Text == "+" ? tb.Text = "-" : tb.Text = "+";
        }

        private void FindButton_Click(object sender, RoutedEventArgs e)
        {
            if (!NameCheck())
                return;

            FindPeople.SearchOptions options = new FindPeople.SearchOptions()
            {
                Name = NameComboBox.Text,
                LastName = LastNameComboBox.Text
            };
            SetSerachOptions(options);
            AddFieldsToDb();


            //try
            //{
                WebDriverWorks(options);
            /*}
            catch
            {
                MessageBox.Show("Что-то пошло не так.");
                return;
            }*/

            foreach (var ui in (Application.Current.MainWindow.Content as Grid).Children)
            {
                if (ui is Frame)
                    (ui as Frame).Navigate(new Uri("ListOfPeoplePage.xaml", UriKind.Relative));
            }
        }

        private void AddFieldsToDb()
        {
            if (App.CurrentUserLogin != null)
            {
                DatabaseInteraction.PeopleFromDb.SetFoundFirstName(App.CurrentUserLogin, NameComboBox.Text);
                DatabaseInteraction.PeopleFromDb.SetFoundLastName(App.CurrentUserLogin, LastNameComboBox.Text);

                if (CityChecked && string.IsNullOrWhiteSpace(CityComboBox.Text))
                {
                    DatabaseInteraction.PeopleFromDb.SetFoundCity(App.CurrentUserLogin, CityComboBox.Text);
                }
                if (EducationChecked && string.IsNullOrWhiteSpace(UniversityComboBox.Text))
                {
                    DatabaseInteraction.PeopleFromDb.SetFoundUniversity(App.CurrentUserLogin, UniversityComboBox.Text);
                }
                if (SchoolChecked && string.IsNullOrWhiteSpace(SchoolComboBox.Text))
                {
                    DatabaseInteraction.PeopleFromDb.SetFoundSchool(App.CurrentUserLogin, SchoolComboBox.Text);
                }
            }
        }

        private bool NameCheck()
        {
            if (string.IsNullOrWhiteSpace(NameComboBox.Text) || string.IsNullOrWhiteSpace(LastNameComboBox.Text))
            {
                NameErrorTextBlock.Visibility = Visibility.Visible;
                return false;
            }
            else
            {
                NameErrorTextBlock.Visibility = Visibility.Collapsed;
                return true;
            }
        }

        private static void WebDriverWorks(FindPeople.SearchOptions options)
        {
            IWebDriver webDriver;

            try
            {
                webDriver = new ChromeDriver();
            }
            catch
            {
                try
                {
                    webDriver = new FirefoxDriver();
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

            bool vkIsOk = true;
            bool okIsOk = true;

            try
            {
                find.FindPeopleOnVK(webDriver, options);
            }
            catch
            {
                vkIsOk = false;
            }
            
            try
            {
                find.FindPeopleOnOK(webDriver, options);
            }
            catch
            {
                okIsOk = false;
            }

            App.PersonInformation = new List<FindPeople.PersonInformation>();

            if (vkIsOk)
            {
                foreach (var person in find.PeopleFromVK)
                    App.PersonInformation.Add(person);
            }
            
            if (okIsOk)
            {
                foreach (var person in find.PeopleFromOK)
                    App.PersonInformation.Add(person);
            }

            webDriver.Quit();

            //if (!vkIsOk && !okIsOk)
              //  throw new Exception("Поиск во Вконтакте и Одноклассниках закончился неудачей.");
        }


        private void SetSerachOptions(FindPeople.SearchOptions options)
        {
            YearsCheck(options);
            CityCheck(options);
            UniversityCheck(options);
            SchoolCheck(options);
        }

        private void SchoolCheck(FindPeople.SearchOptions options)
        {
            if (SchoolChecked)
            {
                if (!string.IsNullOrWhiteSpace(SchoolComboBox.Text))
                    options.Schools = SchoolComboBox.Text;
                else
                    options.Schools = null;
            }
            else
                options.Schools = null;
        }

        private void UniversityCheck(FindPeople.SearchOptions options)
        {
            if (EducationChecked)
            {
                if (!string.IsNullOrWhiteSpace(UniversityComboBox.Text))
                    options.Education = UniversityComboBox.Text;
                else
                    options.Education = null;
            }
            else
                options.Education = null;
        }

        private void CityCheck(FindPeople.SearchOptions options)
        {
            if (CityChecked)
            {
                if (!string.IsNullOrWhiteSpace(CityComboBox.Text))
                    options.City = CityComboBox.Text;
                else
                    options.City = null;
            }
            else
                options.City = null;
        }

        private void YearsCheck(FindPeople.SearchOptions options)
        {
            if (YearFromChecked)
            {
                if (!string.IsNullOrWhiteSpace(FromYearCB.Text))
                    options.YearOfBirth = Convert.ToInt32(FromYearCB.Text);
                else
                    options.YearOfBirth = null;

                YearToCheck(options);
            }
            else
            {
                options.YearOfBirth = null;
                options.ForThisYear = null;
            }
        }

        private void YearToCheck(FindPeople.SearchOptions options)
        {
            if (YearToChecked && !string.IsNullOrWhiteSpace(ToYearCB.Text))
            {
                var firstYear = Convert.ToInt32(ToYearCB.Text);
                var secondYear = Convert.ToInt32(FromYearCB.Text);

                if (firstYear > secondYear)
                {
                    options.YearOfBirth = secondYear;
                    options.ForThisYear = firstYear;
                }
                else
                {
                    options.YearOfBirth = firstYear;
                    options.ForThisYear = secondYear;
                }
            }
            else
                options = null;
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
                    case "UniversityComboBox":
                        SetEducationTooltips(cb);
                        break;
                    case "SchoolComboBox":
                        SetSchoolTooltips(cb);
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
            List<string> tips = DatabaseInteraction.PeopleFromDb.GetFoundLastNamea(App.CurrentUserLogin);

            box.Items.Clear();

            foreach (var tip in tips)
            {
                box.Items.Add(tip);
            }
        }

        private static void SetFirstNameTooltips(ComboBox box)
        {
            List<string> tips = DatabaseInteraction.PeopleFromDb.GetFoundFirstNames(App.CurrentUserLogin);

            box.Items.Clear();

            foreach (var tip in tips)
            {
                box.Items.Add(tip);
            }
        }

        private static void SetCityTooltips(ComboBox box)
        {
            List<string> tips = DatabaseInteraction.PeopleFromDb.GetFoundCities(App.CurrentUserLogin);

            box.Items.Clear();

            foreach (var tip in tips)
            {
                box.Items.Add(tip);
            }
        }

        private static void SetEducationTooltips(ComboBox box)
        {
            List<string> tips = DatabaseInteraction.PeopleFromDb.GetFoundUniversities(App.CurrentUserLogin);

            box.Items.Clear();

            foreach (var tip in tips)
            {
                box.Items.Add(tip);
            }
        }

        private static void SetSchoolTooltips(ComboBox box)
        {
            List<string> tips = DatabaseInteraction.PeopleFromDb.GetFoundSchools(App.CurrentUserLogin);

            box.Items.Clear();

            foreach (var tip in tips)
            {
                box.Items.Add(tip);
            }
        }
    }
}
