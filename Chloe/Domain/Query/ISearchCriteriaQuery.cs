using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Dto;

namespace Flights.Domain.Query
{
    public interface ISearchCriteriaQuery
    {
        IEnumerable<SearchCriteria> GetAllSearchCriterias();

        IEnumerable<SearchCriteria> GetSearchCriteriasByReceiverGroupId(int receiverGroupId);
    }
}
