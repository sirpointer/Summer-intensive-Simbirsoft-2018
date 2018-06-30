using System;
using System.Collections.Generic;
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
    /// Логика взаимодействия для PersonPage.xaml
    /// </summary>
    public partial class PersonPage : Page
    {
        public PersonPage()
        {
            InitializeComponent();

            if (App.VkPerson != null)
            {
                FindPeople.PersonInformation person = App.VkPerson;

                this.PersonImage = person.Photo;
                this.FullNameTextBlock.Text = person.Name + " " + person.LastName;
                this.YearTextBlock.Text = person.YearOfBirth.ToString();

                if (!string.IsNullOrWhiteSpace(person.ProfileLink))
                {
                    this.VkHyperLink.NavigateUri = new Uri(person.ProfileLink);
                }

                foreach (var ed in person.Education)
                    EducationVkStackPanel.Children.Add(GetVkTextBlock(ed));

                foreach (var city in person.Cities)
                    CitiesVkStackPanel.Children.Add(GetVkTextBlock(city));
            }
            else if (App.OkPerson != null)
            {
                FindPeople.PersonInformation person = App.OkPerson;

                this.PersonImage = person.Photo;
                this.FullNameTextBlock.Text = person.Name + " " + person.LastName;
                this.YearTextBlock.Text = person.YearOfBirth.ToString();

                if (!string.IsNullOrWhiteSpace(person.ProfileLink))
                {
                    this.OkHyperLink.NavigateUri = new Uri(person.ProfileLink);
                }


                foreach (var ed in person.Education)
                    EducationVkStackPanel.Children.Add(GetOkTextBlock(ed));

                foreach (var city in person.Cities)
                    CitiesVkStackPanel.Children.Add(GetOkTextBlock(city));
            }
        }

        public static TextBlock GetVkTextBlock(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new Exception("Поле для образования пустое.");
            else
            {
                return new TextBlock()
                {
                    Text = text,
                    Margin = new Thickness(5),
                    Foreground = Brushes.Blue,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
            }
        }

        public static TextBlock GetOkTextBlock(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new Exception("Поле для образования пустое.");
            else
            {
                return new TextBlock()
                {
                    Text = text,
                    Margin = new Thickness(5),
                    Foreground = Brushes.OrangeRed,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
            }
        }
    }
}
