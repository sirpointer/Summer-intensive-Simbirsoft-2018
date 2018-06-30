using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using SocialMediaInformationAggregator.FindPeople;

namespace SocialMediaInformationAggregator
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static List<PersonInformation> PersonInformation { get; set; }

        public static PersonInformation VkPerson { get; set; }

        public static PersonInformation OkPerson { get; set; }

        private static string _currentLogin;

        public static string CurrentUserLogin
        {
            get => _currentLogin;
            set
            {
                if (value == null)
                {
                    Logout();
                }
                else
                {
                    Login(value);
                }

                _currentLogin = value;
            }
        }

        private static void Login(string value)
        {
            foreach (var content in (Current.MainWindow.Content as Grid).Children)
            {
                if (content is StackPanel)
                {
                    StackPanel sp = (content as StackPanel).FindName("LoginAndHistorySP") as StackPanel;

                    (sp.FindName("LoginButton") as Button).Visibility = Visibility.Collapsed;
                    (sp.FindName("LogoutButton") as Button).Visibility = Visibility.Visible;
                    (sp.FindName("LoginTextBlock") as TextBlock).Text = value;
                }
            }
        }

        private static void Logout()
        {
            foreach (var content in (App.Current.MainWindow.Content as Grid).Children)
            {
                if (content is StackPanel)
                {
                    StackPanel sp = (content as StackPanel).FindName("LoginAndHistorySP") as StackPanel;

                    (sp.FindName("LoginButton") as Button).Visibility = Visibility.Visible;
                    (sp.FindName("LogoutButton") as Button).Visibility = Visibility.Collapsed;
                    (sp.FindName("LoginTextBlock") as TextBlock).Text = "Неавторизирован";
                }
            }
        }
    }

}
