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

        public override bool Equals(object obj)
        {
            var information = obj as PersonInformation;

            bool city = true;
            bool ed = true;

            for (int i = 0; i < information.Cities.Count; i++)
            {
                city = false;

                for (int j = 0; j < Cities.Count; j++)
                    if (Cities[j] == information.Cities[i])
                        city = true;

                if (city == false)
                    break;
            }

            for (int i = 0; i < information.Education.Count; i++)
            {
                ed = false;

                for (int j = 0; j < Education.Count; j++)
                    if (Education[j] == information.Education[i])
                        ed = true;

                if (ed == false)
                    break;
            }

            return information != null &&
                   Name == information.Name &&
                   LastName == information.LastName &&
                   EqualityComparer<int?>.Default.Equals(YearOfBirth, information.YearOfBirth) &&
                   city &&
                   ed &&
                   SocialNetwork == information.SocialNetwork &&
                   ProfileLink == information.ProfileLink;
        }

        public void RemoveNulls()
        {
            if (this.Name == null)
                this.Name = string.Empty;

            if (this.LastName == null)
                this.LastName = string.Empty;

            if (this.Photo == null)
                this.Photo = new Image();

            if (this.ProfileLink == null)
                this.ProfileLink = string.Empty;

            if (this.Cities == null)
                this.Cities = new List<string>();
            else
            {
                for (int i = 0; i < this.Cities.Count; i++)
                    if (this.Cities[i] == null)
                        this.Cities[i] = string.Empty;
            }

            if (this.Education == null)
                this.Education = new List<string>();
            else
            {
                for (int i = 0; i < this.Education.Count; i++)
                    if (this.Education[i] == null)
                        this.Education[i] = string.Empty;
            }
        }
    }

    public enum SocialNetwork { VK, OK, Facebook }
}
