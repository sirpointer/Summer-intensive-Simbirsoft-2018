using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using SocialMediaInformationAggregator.FindPeople;

namespace SocialMediaInformationAggregator
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static List<FindPeople.PersonInformation> personInformation;

        public static List<PersonInformation> PersonInformation { get => personInformation; set => personInformation = value; }
    }

}
