using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
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
using System.Windows.Shapes;

namespace SocialMediaInformationAggregator
{
    /// <summary>
    /// Логика взаимодействия для RemakePass.xaml
    /// </summary>
    public partial class RemakePass : Window
    {
        public RemakePass()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionDB"].ConnectionString;

            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                List<string> k = new List<string>();
                string query = string.Format("SELECT * FROM Пользователи WHERE Логин=@a");
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    comm.Parameters.AddWithValue("@a", textBoxLogin.Text);
                    SqlDataReader reader = comm.ExecuteReader();
                    string frstAnsw = "";
                    string secAnsw = "";
                    string thirdAnsw = "";
                    while (reader.Read())
                    {
                        frstAnsw = (string)reader[5];
                        secAnsw = (string)reader[6];
                        thirdAnsw = (string)reader[7];
                    }
                    if (frstAnsw==textBoxFrst.Text && secAnsw==textBoxSec.Text && thirdAnsw==textBoxThird.Text)
                    {
                        ChangePass changePass = new ChangePass(textBoxLogin.Text);
                        changePass.Show();
                    }
                    else
                    {
                        MessageBox.Show("Ваши ответы не совпадают с регистрационными данными.");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
