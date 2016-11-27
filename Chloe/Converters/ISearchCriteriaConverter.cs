using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightDomain = Chloe.Domain.Dto;
using FlightDto = Chloe.Dto;

namespace Chloe.Converters
{
    public interface ISearchCriteriaConverter
    {
        FlightDto.SearchCriteria Convert(FlightDomain.SearchCriterias input);

        IEnumerable<FlightDto.SearchCriteria> Convert(IEnumerable<FlightDomain.SearchCriterias> input);
    }
}
