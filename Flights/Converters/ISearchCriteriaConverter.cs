using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightDomain = Flights.Domain.Dto;
using FlightDto = Flights.Dto;

namespace Flights.Converters
{
    public interface ISearchCriteriaConverter
    {
        FlightDto.SearchCriteria Convert(FlightDomain.SearchCriterias searchCriteria);

        IEnumerable<FlightDto.SearchCriteria> Convert(IEnumerable<FlightDomain.SearchCriterias> searchCriteria);
    }
}
