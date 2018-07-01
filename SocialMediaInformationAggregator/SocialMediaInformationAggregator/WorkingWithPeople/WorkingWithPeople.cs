using SocialMediaInformationAggregator.FindPeople;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaInformationAggregator.WorkingWithPeople
{
    public static class WorkingWithPeople
    {
        /// <summary>
        /// Возвращает похожего на person человека из personList в другой соцсети.
        /// </summary>
        /// <param name="person"></param>
        /// <param name="personList"></param>
        /// <returns></returns>
        public static PersonInformation GetSimilarPerson(PersonInformation person, List<PersonInformation> personList)
        {
            if (person == null)
                throw new ArgumentNullException(nameof(person));

            List<PersonInformation> anotherSocialNetworkPersonList = new List<PersonInformation>();

            foreach (var pers in personList)
            {
                if (pers.SocialNetwork != person.SocialNetwork)
                    anotherSocialNetworkPersonList.Add(pers);
            }

            bool similarCity = false;
            bool similarEducation = false;
            
            foreach (var pers in anotherSocialNetworkPersonList)
            {
                similarCity = CityAreSimilar(person, pers);
                similarEducation = EducationAreSimilar(person, pers);

                if (similarCity && similarEducation)
                    return pers;
            }

            return null;
        }

        private static bool CityAreSimilar(PersonInformation man, PersonInformation manFromList)
        {
            foreach (var city in man.Cities)
            {
                foreach (var secondCity in manFromList.Cities)
                {
                    if (city.Equals(secondCity, StringComparison.CurrentCultureIgnoreCase))
                        return true;
                }
            }

            return false;
        }

        private static bool EducationAreSimilar(PersonInformation man, PersonInformation manFromList)
        {
            foreach (var education in man.Education)
            {
                foreach (var secondEducation in manFromList.Education)
                {
                    if (education.Equals(secondEducation, StringComparison.CurrentCultureIgnoreCase))
                        return true;
                }
            }

            return false;
        }
    }
}
