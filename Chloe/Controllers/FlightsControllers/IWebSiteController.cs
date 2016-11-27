using System.Collections.Generic;
using Chloe.Dto;

namespace Chloe.Controllers.FlightsControllers
{
    public interface IWebSiteController
    {
        List<Flight> GetChloe(SearchCriteria searchCriteria);
    }
}
