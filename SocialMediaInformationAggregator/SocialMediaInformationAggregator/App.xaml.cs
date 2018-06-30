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
        public static List<PersonInformation> PersonInformation { get; set; }

        public static PersonInformation VkPerson { get; set; }

        public static PersonInformation OkPerson { get; set; }

        public static string CurrentUserLogin { get; set; }
    }

}
