using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Converters;
using FlightDataModel = Chloe.Domain.Dto;
using FlightDto = Chloe.Dto;

namespace Chloe.Domain.Query
{
    public class SearchCriteriaQuery : ISearchCriteriaQuery
    {
        private readonly ISearchCriteriaConverter _searchCriteriaConverter;
        
        public SearchCriteriaQuery(ISearchCriteriaConverter searchCriteriaConverter)
        {
            if (searchCriteriaConverter == null) throw new ArgumentNullException("searchCriteriaConverter");

            _searchCriteriaConverter = searchCriteriaConverter;
        }

        public IEnumerable<FlightDto.SearchCriteria> GetAllSearchCriterias()
        {
            IEnumerable<FlightDto.SearchCriteria> result;
            
            using (FlightDataModel.ChloeEntities flightDataModel = new FlightDataModel.ChloeEntities())
            {
                var domainModel = flightDataModel.SearchCriterias.ToList();
                result = _searchCriteriaConverter.Convert(domainModel);
            }

            return result;
        }

        public IEnumerable<FlightDto.SearchCriteria> GetSearchCriteriasByReceiverGroupId(int receiverGroupId)
        {
            IEnumerable<FlightDto.SearchCriteria> result;

            using (FlightDataModel.ChloeEntities flightDataModel = new FlightDataModel.ChloeEntities())
            {
                var domainModel = flightDataModel.SearchCriterias
                    .Where(x => x.ReceiverGroups_Id == receiverGroupId)
                    .ToList();
                result = _searchCriteriaConverter.Convert(domainModel);
            }

            return result;
        }
    }
}
