using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;

namespace SocialMediaInformationAggregator.FindPeople
{
    public partial class FindPeople : IFindPeople
    {
        private List<PersonInformation> _peopleFromVK;

        public List<PersonInformation> PeopleFromVK { get => _peopleFromVK; private set => _peopleFromVK = value; }
        
        public void FindPeopleOnVK(IWebDriver webDriver, SearchOptions searchOptions)
        {
            throw new NotImplementedException();
        }
    }
}
