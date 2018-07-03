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
        public QuestionPage()
        {
            App.MakeConnectionString();
            InitializeComponent();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            SqlConnection conn = new SqlConnection(App.connectionString);
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
