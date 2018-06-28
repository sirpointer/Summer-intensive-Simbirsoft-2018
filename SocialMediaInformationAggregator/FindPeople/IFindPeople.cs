using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaInformationAggregator.FindPeople
{
    public interface IFindPeople
    {
        /// <summary>
        /// Список людей, найденных во ВКонтакте.
        /// </summary>
        List<PersonInformation> PeopleFromVK { get; }

        /// <summary>
        /// Ищет с помощью Selenium людей во ВКонтакте и заполняет PeopleFromVK.
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="searchOptions"></param>
        void FindPeopleOnVK(IWebDriver webDriver, SearchOptions searchOptions);

        /// <summary>
        /// Список людей, найденных в Одноклассниках.
        /// </summary>
        List<PersonInformation> PeopleFromOK { get; }

        /// <summary>
        /// Ищет с помощью Selenium людей в Одноклассниках и заполняет PeopleFromOK.
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="searchOptions"></param>
        void FindPeopleOnOK(IWebDriver webDriver, SearchOptions searchOptions);

        /// <summary>
        /// 
        /// </summary>
        List<PersonInformation> PeopleFromFacebook { get; }

        /// <summary>
        /// Ищет с помощью Selenium людей в Facebook и заполняет PeopleFromFacebook.
        /// </summary>
        /// <param name="webDriver"></param>
        /// <param name="searchOptions"></param>
        void FindPeopleOnFacebook(IWebDriver webDriver, SearchOptions searchOptions);
    }
}
