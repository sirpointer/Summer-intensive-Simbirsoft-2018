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
    /// Логика взаимодействия для ForgottenPasswordPage.xaml
    /// </summary>
    public partial class ForgottenPasswordPage : Page
    {
        public ForgottenPasswordPage()
        {
            App.MakeConnectionString();
            InitializeComponent();
        }
        public SqlConnection ConnectToDb()
        {
            SqlConnection conn = new SqlConnection(App.ConnectionString);
            return conn;
        }
        public void CheckInputInform(SqlDataReader reader)
        {
            string frstAnsw = "";
            string secAnsw = "";
            string thirdAnsw = "";
            if (reader.HasRows)
            {
                while (reader.Read())
                {
                    frstAnsw = (string)reader[5];
                    secAnsw = (string)reader[6];
                    thirdAnsw = (string)reader[7];
                }
                if (frstAnsw.ToLower().Trim(' ') == textBoxFrst.Text.ToLower().Trim(' ') 
                    && secAnsw.ToLower().Trim(' ') == textBoxSec.Text.ToLower().Trim(' ') 
                    && thirdAnsw.ToLower().Trim(' ') == textBoxThird.Text.ToLower().Trim(' '))
                {
                    App.LoginGlobalVeryForMethod = textBoxLogin.Text.ToLower().Trim(' ');
                    this.NavigationService.Navigate(new Uri("ChangePasswordPage.xaml", UriKind.Relative));
                }
                else
                {
                    MessageBox.Show("Ваши ответы не совпадают с регистрационными данными.");
                }
            }
            else
            {
                MessageBox.Show("Некорректный логин.");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrWhiteSpace(textBoxFrst.Text)
                && !String.IsNullOrWhiteSpace(textBoxSec.Text)
                && !String.IsNullOrWhiteSpace(textBoxThird.Text)
                && !String.IsNullOrWhiteSpace(textBoxLogin.Text))
            {
                try
                {
                    SqlConnection conn = ConnectToDb();
                    conn.Open();
                    List<string> k = new List<string>();
                    string query = string.Format("SELECT * FROM Users WHERE Login=@a");
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@a", textBoxLogin.Text.ToLower().Trim(' '));
                        SqlDataReader reader = comm.ExecuteReader();
                        CheckInputInform(reader);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Некоторые поля не заполнены!");
            }
        }
    }
}
