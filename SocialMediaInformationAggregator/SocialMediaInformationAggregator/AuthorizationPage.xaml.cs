using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
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
    /// Логика взаимодействия для AuthorizationPage.xaml
    /// </summary>
    public partial class AuthorizationPage : Page
    {
        public AuthorizationPage()
        {
            App.MakeConnectionString();
            InitializeComponent();
        }

        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SqlConnection conn = new SqlConnection(App.connectionString);
                conn.Open();
                List<string> k = new List<string>();
                string query = string.Format("SELECT Password FROM Users WHERE Login=@a");
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@a", textBoxLogin.Text);
                    SqlDataReader reader = comm.ExecuteReader();
                    string pass = "";
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            pass = (string)reader[0];
                        }

                        if (pass == passwordBox.Password.ToString())
                        {
                            App.CurrentUserLogin = textBoxLogin.Text;
                            this.NavigationService.Navigate(new Uri("InputPage.xaml", UriKind.Relative));
                        }
                        else
                        {
                            MessageBox.Show("Неверный пароль");
                        }
                    }
                    else
                    {
                        MessageBox.Show("Некорректный логин");
                    }
                }

                conn.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void ForgottenPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("ForgottenPasswordPage.xaml", UriKind.Relative));
        }

        private void RegistrationButton_Click_3(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new Uri("RegistrationPage.xaml", UriKind.Relative));
        }
    }
}