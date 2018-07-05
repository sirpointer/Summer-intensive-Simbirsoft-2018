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

#if DEBUG
            App.CurrentUserLogin = "admin";
#else
            App.CurrentUserLogin = null;
#endif
            

            try
            {
                DatabaseInteraction.PeopleFromDb.GetFoundFirstNames(App.CurrentUserLogin);
            }
            catch { }

        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (PagesFrame.CanGoBack)
            {
                bool isAuthPage = PagesFrame.Source.ToString().Contains("RegistrationPage.xaml") || PagesFrame.Source.ToString().Contains("QuestionPage.xaml")
                    || PagesFrame.Source.ToString().Contains("MainWindow.xaml") || PagesFrame.Source.ToString().Contains("ForgottenPasswordPage.xaml")
                    || PagesFrame.Source.ToString().Contains("ChangePasswordPage.xaml") || PagesFrame.Source.ToString().Contains("AuthorizationPage.xaml");

                if (!isAuthPage)
                    PagesFrame.GoBack();
                else if (PagesFrame.Source.ToString().Contains("MainWindow.xaml"))
                    return;
                else
                    PagesFrame.Navigate(new Uri("InputPage.xaml", UriKind.Relative));
            }
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            PagesFrame.Navigate(new Uri("AuthorizationPage.xaml", UriKind.Relative));
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUserLogin != null)
            {
                try
                {
                    App.PersonInformation = DatabaseInteraction.PeopleFromDb.GetFoundPeople(App.CurrentUserLogin).Distinct().ToList();
                }
                catch
                {
                    App.PersonInformation = new List<FindPeople.PersonInformation>();
                    MessageBox.Show("Не удалось загрузить историю.");
                    return;
                }

                for (int i = 0; i < App.PersonInformation.Count; i++)
                {
                    App.PersonInformation[i].RemoveNulls();
                }

                if (App.PersonInformation.Count < 1)
                {
                    MessageBox.Show("История пуста.");
                    return;
                }
                else
                    PagesFrame.Navigate(new Uri("ListOfPeoplePage.xaml", UriKind.Relative));
            }
            else
                MessageBox.Show("Для просмотра истории поиска нужно авторизироваться.");

        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            App.CurrentUserLogin = null;
        }
    }
}
