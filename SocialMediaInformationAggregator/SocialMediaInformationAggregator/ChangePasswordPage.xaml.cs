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
        public ChangePasswordPage()
        {
            App.MakeConnectionString();
            Login = App.LoginGlobalVeryForMethod.ToLower().Trim(' ');
            InitializeComponent();
        }

        public bool PasswordIsCorrect()
        /// проверяем, соответствует ли пароль требованиям надежности
        {
            int buf = 0;
            int buf_1 = 0;
            int buf_2 = 0;
            int buf_3 = 0;
            int buf_4 = 0;
            int buf_5 = 0;
            if (passNew.Password.ToCharArray().Count() >= 6)
            {
                foreach (var k in passNew.Password.ToCharArray())
                {
                    if (Char.IsDigit(k)) buf = 1;
                    if (Char.IsLetter(k)) buf_1 = 1;
                    if (Char.IsLower(k)) buf_2 = 1;
                    if (Char.IsUpper(k)) buf_3 = 1;
                    if (Char.IsSymbol(k)) buf_4 = 1;
                    if (Char.IsWhiteSpace(k)) buf_5 = 1;
                }
                if (buf + buf_1 + buf_2 + buf_3 + buf_4 + buf_5 >= 2)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show("Пароль должен соответствовать хотя бы 3-м из следующих требований: \n" +
                                    "- содержать цифры \n" +
                                    "- содержать буквы \n" +
                                    "- содержать символы в верхнем регистре \n" +
                                    "- содержить символы в нижнем регистре \n" +
                                    "- содержать специальные символы \n" +
                                    "- содержать пробелы");
                    return false;
                }

            }
            else
            {
                MessageBox.Show("Пароль должен состоять минимум из 6 символов!");
                passNew.Password = "";
                repeatPass.Password = "";
                return false;
            }
        }

        public void UpdateDb()
        {
            try
            {
                SqlConnection conn = new SqlConnection(App.ConnectionString);
                conn.Open();
                List<string> k = new List<string>();
                string query = string.Format("UPDATE Users  SET Password = @pass WHERE Login=@login");
                using (SqlCommand comm = new SqlCommand(query, conn))
                {
                    Login = App.LoginGlobalVeryForMethod.ToLower().Trim(' ');
                    comm.Parameters.AddWithValue("@login", Login);
                    comm.Parameters.AddWithValue("@pass", passNew.Password.GetHashCode());
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
            if (PasswordIsCorrect() && passNew.Password.GetHashCode() == repeatPass.Password.GetHashCode())
            {
                UpdateDb();
                this.NavigationService.Navigate(new Uri("InputPage.xaml", UriKind.Relative));
            }
            else
            {
                passNew.Password = "";
                repeatPass.Password="";
                MessageBox.Show("Пароли не идентичны!");
            }
        }
    }
}
