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
            SqlConnection conn = new SqlConnection(App.connectionString);
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
                if (frstAnsw == textBoxFrst.Text && secAnsw == textBoxSec.Text && thirdAnsw == textBoxThird.Text)
                {
                    App.LoginGlobalVeryForMethod = textBoxLogin.Text;
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
            if (textBoxLogin.Text != "" && textBoxFrst.Text != "" && textBoxSec.Text != "" && textBoxThird.Text != "")
            {
                try
                {
                    SqlConnection conn = ConnectToDb();
                    conn.Open();
                    List<string> k = new List<string>();
                    string query = string.Format("SELECT * FROM Users WHERE Login=@a");
                    using (SqlCommand comm = new SqlCommand(query, conn))
                    {
                        comm.Parameters.AddWithValue("@a", textBoxLogin.Text);
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
