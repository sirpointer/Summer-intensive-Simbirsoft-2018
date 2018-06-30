﻿using System;
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
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            /*if ("PersonPage.xaml".Contains(PagesFrame.Source.OriginalString))
            {
                PagesFrame.Navigate("ListOfPeoplePage.xaml");
            }
            else */if (PagesFrame.CanGoBack)
                PagesFrame.GoBack();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            PagesFrame.Navigate(new Uri("AuthorizationPage.xaml", UriKind.Relative));
        }

        private void HistoryButton_Click(object sender, RoutedEventArgs e)
        {
            if (App.CurrentUserLogin != null)
                App.PersonInformation = DatabaseInteraction.PeopleFromDb.GetFoundPeople(App.CurrentUserLogin);

            PagesFrame.Navigate(new Uri("ListOfPeoplePage.xaml", UriKind.Relative));
        }
    }
}
