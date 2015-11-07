using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Converters;
using FlightDataModel = Flights.Domain.Dto;
using FlightDto = Flights.Dto;

namespace Flights.Domain.Query
{
    public class SearchCriteriaQuery : ISearchCriteriaQuery
    {
        private readonly ISearchCriteriaConverter _searchCriteriaConverter;
        
        public SearchCriteriaQuery(ISearchCriteriaConverter searchCriteriaConverter)
        {
            if (searchCriteriaConverter == null) throw new ArgumentNullException("searchCriteriaConverter");

            _searchCriteriaConverter = searchCriteriaConverter;
        }

        public List<FlightDto.SearchCriteria> GetAllSearchCriterias()
        {
            List<FlightDto.SearchCriteria> result;
            
            using (FlightDataModel.FlightsEntities flightDataModel = new FlightDataModel.FlightsEntities())
            {
                var domainModel = flightDataModel.SearchCriterias.ToList();
                result = _searchCriteriaConverter.Convert(domainModel);
            }

            return result;
        }
    }
}
