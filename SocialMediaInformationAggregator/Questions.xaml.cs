using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для Questions.xaml
    /// </summary>
    public partial class Questions : Window
    {
        public string login;
        public string connectionString;
        public Questions(string log, string connect)
        {
            login = log;
            connectionString = connect;
            InitializeComponent();
            MainWindow mainWindow = new MainWindow();
            mainWindow.Close();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {

            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();
                string query = string.Format("UPDATE Пользователи SET [Первый вопрос] = '{0}', [Второй вопрос]='{1}', [Третий вопрос]='{2}' " +
                                             "WHERE (Логин='{3}')",
                                             textBoxSer.Text, textBoxPet.Text, textBoxHobby.Text, login);
                SqlCommand comm = new SqlCommand(query, conn);

                comm.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
