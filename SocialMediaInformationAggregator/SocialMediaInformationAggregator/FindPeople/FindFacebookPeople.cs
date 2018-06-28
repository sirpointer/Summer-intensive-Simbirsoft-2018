using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaInformationAggregator.FindPeople
{
    public partial class FindPeople
    {
        public List<PersonInformation> _peopleFromFacebook;

        public List<PersonInformation> PeopleFromFacebook { get => _peopleFromFacebook; private set => _peopleFromFacebook = value; }

        public void FindPeopleOnFacebook(IWebDriver webDriver, SearchOptions searchOptions)
        {
            throw new NotImplementedException();
        }
    }
}
