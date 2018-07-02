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

            PeopleListBox.SelectionChanged += PeopleListBox_SelectionChanged;

            PeopleListBox.Items.Clear();

            foreach (var person in App.PersonInformation)
                AddPersonToListBox(person);
        }

        private void PeopleListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            App.VkPerson = null;
            App.OkPerson = null;

            int index = (sender as ListBox).SelectedIndex;

            if (index != -1)
            {
                if (App.PersonInformation[index].SocialNetwork == FindPeople.SocialNetwork.VK)
                {
                    App.VkPerson = App.PersonInformation[index];
                    App.OkPerson = WorkingWithPeople.WorkingWithPeople.GetSimilarPerson(App.VkPerson, App.PersonInformation);
                }
                else
                {
                    App.OkPerson = App.PersonInformation[index];
                    App.VkPerson = WorkingWithPeople.WorkingWithPeople.GetSimilarPerson(App.OkPerson, App.PersonInformation);
                }
            }

            foreach (var ui in (Application.Current.MainWindow.Content as Grid).Children)
            {
                if (ui is Frame)
                    (ui as Frame).Navigate(new Uri("PersonPage.xaml", UriKind.Relative));
            }
        }



        // Добавление ListBoxItem.
        //----------------------------------------------------
        public void AddPersonToListBox(FindPeople.PersonInformation person)
        {
            Grid personGrid = new Grid() { HorizontalAlignment = HorizontalAlignment.Stretch, VerticalAlignment = VerticalAlignment.Stretch };
            SetPersonGrid(person, personGrid);

            ListBoxItem item = new ListBoxItem() { Height = 100, Content = personGrid };

            PeopleListBox.Items.Add(item);
        }

        private static void SetPersonGrid(FindPeople.PersonInformation person, Grid personGrid)
        {
            //personGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(100) });
            //personGrid.ColumnDefinitions.Add(new ColumnDefinition() { Width = new GridLength(1, GridUnitType.Star) });

            //ImageSource source = new ImageSourceConverter().ConvertFromString(@"pack://application:,,,/Assets/app.ico") as ImageSource;

            //Image photo = new Image() { Source = person.Photo.Source };

            StackPanel PersonInformationSP = new StackPanel() { Margin = new Thickness(0, 0, 10, 0) };
            SetPersonInfoSP(person, PersonInformationSP);

            //personGrid.Children.Add(photo);
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

            List<string> education = new List<string>();

            foreach (var ed in person.Education)
            {
                if (!string.IsNullOrWhiteSpace(ed))
                    education.Add(ed);
            }

            for (int i = 0; i < education.Count; i++)
            {
                if (i != education.Count - 1)
                    educationTB.Text += education[i] + ", ";
                else
                    educationTB.Text += education[i] + '.';
            }

            List<string> cities = new List<string>();

            foreach (var city in person.Cities)
            {
                if (!string.IsNullOrWhiteSpace(city))
                    cities.Add(city);
            }

            for (int i = 0; i < cities.Count; i++)
            {
                if (i != cities.Count - 1)
                    citiesTB.Text += cities[i] + ", ";
                else
                    citiesTB.Text += cities[i] + '.';
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

        //----------------------------------------------------
    }
}
