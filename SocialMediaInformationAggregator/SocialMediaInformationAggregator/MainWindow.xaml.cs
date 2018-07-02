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

            //App.CurrentUserLogin = "admin";
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            if (PagesFrame.CanGoBack)
                PagesFrame.GoBack();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            PagesFrame.Navigate(new Uri("AuthorizationPage.xaml", UriKind.Relative));
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUserLogin != null)
            {
                App.PersonInformation = DatabaseInteraction.PeopleFromDb.GetFoundPeople(App.CurrentUserLogin);

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
