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
    /// Логика взаимодействия для ChangePass.xaml
    /// </summary>
    public partial class ChangePass : Window
    {
        public string Login;
        public ChangePass(string login)
        {
            Login = login;
            InitializeComponent();
        }

        public void UpdateDb()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["ConnectionDB"].ConnectionString;
            try
            {
                SqlConnection conn = new SqlConnection(connectionString);
                conn.Open();
                List<string> k = new List<string>();
                string query = string.Format("UPDATE Пользователи  SET Пароль = @pass WHERE Логин=@login");
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
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
            if (passNew.Password==repeatPass.Password)
            {
                UpdateDb();
            }
            else
            {
                MessageBox.Show("Пароли не идентичны!");
            }
        }
    }
}
