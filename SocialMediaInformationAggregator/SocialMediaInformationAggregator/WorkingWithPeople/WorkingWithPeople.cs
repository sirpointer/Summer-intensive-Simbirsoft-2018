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
            
            bool similarEducation = false;
            
            foreach (var pers in anotherSocialNetworkPersonList)
            {
                similarEducation = EducationAreSimilar(person, pers);

                if (similarEducation)
                {
                    PersonInformation p = new PersonInformation();

                    foreach (string cit in pers.Cities)
                        p.Cities.Add(cit);

                    foreach (string ed in pers.Education)
                        p.Education.Add(ed);

                    p.LastName = pers.LastName;
                    p.Name = pers.Name;
                    p.Photo = new System.Windows.Controls.Image();
                    p.ProfileLink = pers.ProfileLink;
                    p.SocialNetwork = pers.SocialNetwork;
                    p.YearOfBirth = pers.YearOfBirth;
                }
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
            foreach (var firstEducation in man.Education)
            {
                foreach (var secondEducation in manFromList.Education)
                {
                    if (IsItSchool(firstEducation, secondEducation))
                    {
                        if (SchoolsAreEqual(firstEducation, secondEducation))
                            return true;
                        else
                            return false;
                    }
                    else
                    {
                        if (firstEducation.Equals(secondEducation, StringComparison.CurrentCultureIgnoreCase))
                            return true;
                        else
                            return false;
                    }
                }
            }

            return false;
        }

        private static bool IsItSchool(string firstEducation, string secondEducation)
        {
            bool school = (firstEducation.IndexOf("ШКОЛА", StringComparison.CurrentCultureIgnoreCase) > 0) 
                && (secondEducation.IndexOf("ШКОЛА", StringComparison.CurrentCultureIgnoreCase) > 0);

            bool lyceum = (firstEducation.IndexOf("ЛИЦЕЙ", StringComparison.CurrentCultureIgnoreCase) > 0)
                && (secondEducation.IndexOf("ЛИЦЕЙ", StringComparison.CurrentCultureIgnoreCase) > 0);

            bool gymnasium = (firstEducation.IndexOf("ГИМНАЗИЯ", StringComparison.CurrentCultureIgnoreCase) > 0) 
                && (secondEducation.IndexOf("ГИМНАЗИЯ", StringComparison.CurrentCultureIgnoreCase) > 0);


            return (school || lyceum || gymnasium);
        }

        private static bool SchoolsAreEqual(string firstSchool, string secondSchool)
        {
            StringBuilder firstSB = new StringBuilder(firstSchool);
            StringBuilder secondSB = new StringBuilder(secondSchool);
            StringBuilder firstNum = new StringBuilder(string.Empty);
            StringBuilder secondNum = new StringBuilder(string.Empty);

            for (int i = 0; i < firstSB.Length; i++)
            {
                if (char.IsDigit(firstSB[i]))
                {
                    firstNum.Append(firstSB[i]);
                }
            }

            for (int i = 0; i < secondSB.Length; i++)
            {
                if (char.IsDigit(secondSB[i]))
                {
                    secondNum.Append(secondSB[i]);
                }
            }

            if (firstNum.ToString().Equals(secondNum.ToString(), StringComparison.Ordinal))
                return true;
            else
                return false;
        }
    }
}
