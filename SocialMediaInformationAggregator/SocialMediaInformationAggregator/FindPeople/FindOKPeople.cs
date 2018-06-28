using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SocialMediaInformationAggregator.FindPeople
{
    public partial class FindPeople : IFindPeople
    {
        private List<PersonInformation> _peopleFromOK;

        public List<PersonInformation> PeopleFromOK { get => _peopleFromOK; private set => _peopleFromOK = value; }

        public void FindPeopleOnOK(IWebDriver webDriver, SearchOptions searchOptions)
        {
            throw new NotImplementedException();
        }
    }
}
