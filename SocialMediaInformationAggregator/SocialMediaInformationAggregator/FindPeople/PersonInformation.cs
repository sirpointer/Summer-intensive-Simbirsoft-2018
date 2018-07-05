using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace SocialMediaInformationAggregator.FindPeople
{
    /// <summary>
    /// Класс, предназначенный для хранения информации о найденном человеке.
    /// </summary>
    public class PersonInformation
    {
        public PersonInformation()
        {
            this.Name = "";
            this.LastName = "";
            this.Cities = new List<string>();
            this.Education = new List<string>();
            this.Photo = new Image();
            this.SocialNetwork = SocialNetwork.VK;
        }
        
        public string Name { get; set; }

        public string LastName { get; set; }

        public int? YearOfBirth { get; set; }

        /// <summary>
        /// Все города, в которых он жил или живёт.
        /// </summary>
        public List<string> Cities { get; set; }

        /// <summary>
        /// Все местра образования, в которых он учился или учится.
        /// </summary>
        public List<string> Education { get; set; }

        public Image Photo { get; set; }

        public SocialNetwork SocialNetwork { get; set; }

        public string ProfileLink { get; set; }

        public PersonInformation Copy()
        {
            PersonInformation copy = new PersonInformation()
            {
                Name = this.Name,
                LastName = this.LastName,
                Photo = new Image(),
                ProfileLink = this.ProfileLink,
                SocialNetwork = this.SocialNetwork,
                YearOfBirth = this.YearOfBirth
            };

            foreach (string city in this.Cities)
                copy.Cities.Add(city);

            foreach (string ed in this.Education)
                copy.Education.Add(ed);

            return copy;
        }
    }

    public enum SocialNetwork { VK, OK, Facebook }
}
