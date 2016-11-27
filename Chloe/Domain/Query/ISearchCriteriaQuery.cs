using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Dto;

namespace Chloe.Domain.Query
{
    public interface ISearchCriteriaQuery
    {
        IEnumerable<SearchCriteria> GetAllSearchCriterias();

        IEnumerable<SearchCriteria> GetSearchCriteriasByReceiverGroupId(int receiverGroupId);
    }
}
