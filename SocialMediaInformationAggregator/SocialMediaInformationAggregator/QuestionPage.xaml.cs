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
    /// Логика взаимодействия для QuestionPage.xaml
    /// </summary>
    public partial class QuestionPage : Page
    {
        public string connectionString;
        public QuestionPage()
        {
            string dataDirectory = Directory.GetCurrentDirectory();
            AppDomain.CurrentDomain.SetData("DataDirectory", dataDirectory); //Переопределяем |DataDirectory|, директория, откуда загружается база данных
            connectionString = "Data Source=LAPTOP-8FE5V0OM\\SQLEXPRESS;Initial Catalog=SMIA;Integrated Security=True";
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection(connectionString);
            try
            {
                conn.Open();

                string query = string.Format("UPDATE Users SET [FirstQuestion] = '{0}', [SecondQuestion]='{1}', [ThirdQuestion]='{2}' " +
                                             "WHERE (Login='{3}')",
                                             textBoxSer.Text, textBoxPet.Text, textBoxHobby.Text, App.LoginGlobalVeryForMethod);
                SqlCommand comm = new SqlCommand(query, conn);
                comm.ExecuteNonQuery();
                conn.Close();
                this.NavigationService.Navigate(new Uri("InputPage.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
