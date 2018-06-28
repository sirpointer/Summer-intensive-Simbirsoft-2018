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
    /// Логика взаимодействия для ListOfPeoplePage.xaml
    /// </summary>
    public partial class ListOfPeoplePage : Page
    {
        public ListOfPeoplePage()
        {
            InitializeComponent();

            AddPersonToListBox(new FindPeople.PersonInformation()
            {
                Name = "Nikita",
                LastName = "Novikov",
                Cities = new List<string>() { "Uluanovsk" },
                Education = new List<string>() { "ULSTU" },
                SocialNetwork = FindPeople.SocialNetwork.Facebook,
                YearOfBirth = 1998
            });
        }

        public void AddPersonToListBox(FindPeople.PersonInformation person)
        {
            Grid personGrid = new Grid() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };
            SetPersonGrid(person, personGrid);

            ListBoxItem item = new ListBoxItem() { Height = 100, Content = personGrid };

            PeopleListBox.Items.Add(item);
        }

        private static void SetPersonGrid(FindPeople.PersonInformation person, Grid personGrid)
        {
            personGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });
            personGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            ImageSource source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Assets/app.ico") as ImageSource;

            Image photo = new Image() { Source = source };

            StackPanel PersonInformationSP = new StackPanel() { Margin = new Thickness(0, 0, 10, 0) };
            SetPersonInfoSP(person, PersonInformationSP);

            personGrid.Children.Add(photo);
            personGrid.Children.Add(PersonInformationSP);

            Grid.SetColumn(PersonInformationSP, 1);
        }

        private static void SetPersonInfoSP(FindPeople.PersonInformation person, StackPanel PersonInformationSP)
        {
            StackPanel socialNetworkSP = new StackPanel() { Orientation = Orientation.Horizontal, Height = 20 };
            SetSocialNetworkSP(person, socialNetworkSP);


            TextBlock fullNameTB = new TextBlock() { Text = $"{person.Name} {person.LastName}" };
            TextBlock yearTB = new TextBlock() { Text = person.YearOfBirth.ToString() };
            TextBlock educationTB = new TextBlock();
            TextBlock citiesTB = new TextBlock();

            foreach (var ed in person.Education)
            {
                educationTB.Text += ed + ", ";
            }

            foreach (var city in person.Cities)
            {
                citiesTB.Text += city + ", ";
            }

            PersonInformationSP.Children.Add(fullNameTB);
            PersonInformationSP.Children.Add(yearTB);
            PersonInformationSP.Children.Add(educationTB);
            PersonInformationSP.Children.Add(citiesTB);
            PersonInformationSP.Children.Add(socialNetworkSP);
        }

        private static void SetSocialNetworkSP(FindPeople.PersonInformation person, StackPanel socialNetworkSP)
        {
            Image socialNetworkIcon = new Image();
            TextBlock socialNetworkNameTB = new TextBlock()
            {
                Margin = new Thickness(10, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center
            };

            switch (person.SocialNetwork)
            {
                case FindPeople.SocialNetwork.VK:
                    socialNetworkIcon.Source = new BitmapImage(new Uri(@"pack://application:,,,/Assets/iconVK.png"));
                    socialNetworkNameTB.Text = "ВКонтакте";
                    break;
                case FindPeople.SocialNetwork.OK:
                    socialNetworkIcon.Source = new BitmapImage(new Uri(@"pack://application:,,,/Assets/iconOK.png"));
                    socialNetworkNameTB.Text = "Одноклассники";
                    break;
                case FindPeople.SocialNetwork.Facebook:
                    socialNetworkIcon.Source = new BitmapImage(new Uri(@"pack://application:,,,/Assets/iconFacebook.png"));
                    socialNetworkNameTB.Text = "Facebook";
                    break;
            }

            socialNetworkSP.Children.Add(socialNetworkIcon);
            socialNetworkSP.Children.Add(socialNetworkNameTB);
        }
    }
}
