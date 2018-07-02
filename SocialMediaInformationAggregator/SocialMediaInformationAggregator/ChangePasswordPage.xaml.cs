using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
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
    /// Логика взаимодействия для ChangePasswordPage.xaml
    /// </summary>
    public partial class ChangePasswordPage : Page
    {
        public string Login;
        public string connectionString;
        public ChangePasswordPage()
        {
            string dataDirectory = Directory.GetCurrentDirectory();
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory); //Переопределяем |DataDirectory|, директория, откуда загружается база данных
            connectionString = @"Data Source = (LocalDB)\MSSQLLocalDB; AttachDbFilename = " + dataDirectory + @"\SMIA.mdf";
            ForgottenPasswordPage forgottenPasswordPage = new ForgottenPasswordPage();
            Login = App.LoginGlobalVeryForMethod;
            InitializeComponent();
        }

        public void UpdateDb()
        {
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                List<string> k = new List<string>();
                string query = string.Format("UPDATE Users  SET Password = @pass WHERE Login=@login");
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    Login = App.LoginGlobalVeryForMethod;
                    comm.Parameters.AddWithValue("@login", Login);
                    comm.Parameters.AddWithValue("@pass", passNew.Password);
                    comm.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (passNew.Password == repeatPass.Password)
            {
                UpdateDb();
                this.NavigationService.Navigate(new Uri("InputPage.xaml", UriKind.Relative));
            }
            else
            {
                MessageBox.Show("Пароли не идентичны!");
            }
        }
    }
}
