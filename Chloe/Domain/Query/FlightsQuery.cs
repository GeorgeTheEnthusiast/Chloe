using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Converters;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;
using Chloe.Dto.Enums;
using Chloe.Exceptions;

namespace Chloe.Domain.Query
{
    public class FlightsQuery : IFlightsQuery
    {
        private readonly IFlightsConverter _flightsConverter;

        public FlightsQuery(IFlightsConverter flightsConverter)
        {
            if (flightsConverter == null) throw new ArgumentNullException("flightsConverter");

            _flightsConverter = flightsConverter;
        }

        public IEnumerable<ChloeDto.Flight> GetAllChloe()
        {
            IEnumerable<ChloeDto.Flight> result;

            using (var flightDataModel = new ChloeDomain.ChloeEntities())
            {
                var Chloe = flightDataModel.Chloe;

                result = _flightsConverter.Convert(Chloe);
            }

            return result;
        }

        public IEnumerable<ChloeDto.Flight> GetChloeBySearchDate(DateTime date)
        {
            IEnumerable<ChloeDto.Flight> result;

            using (var flightDataModel = new ChloeDomain.ChloeEntities())
            {
                var Chloe = flightDataModel.Chloe
                    .Where(x => x.SearchDate.Year == date.Year
                                && x.SearchDate.Month == date.Month
                                && x.SearchDate.Day == date.Day);

                result = _flightsConverter.Convert(Chloe);
            }

            return result;
        }

        public IEnumerable<ChloeDto.Flight> GetChloeByReceiverGroup(ChloeDto.ReceiverGroup receiverGroup)
        {
            IEnumerable<ChloeDto.Flight> result;

            using (var flightDataModel = new ChloeDomain.ChloeEntities())
            {
                var Chloe = flightDataModel.Chloe
                    .Where(x => x.SearchCriterias.ReceiverGroups_Id == receiverGroup.Id);

                result = _flightsConverter.Convert(Chloe);
            }

            return result;
        }

        public IEnumerable<ChloeDto.Flight> GetChloeBySearchCriteria(ChloeDto.SearchCriteria searchCriteria)
        {
            IEnumerable<ChloeDto.Flight> result;

            using (var flightDataModel = new ChloeDomain.ChloeEntities())
            {
                var Chloe = flightDataModel.Chloe
                    .Where(x => x.SearchCriterias.Id == searchCriteria.Id);

                result = _flightsConverter.Convert(Chloe);
            }

            return result;
        }
    }
}
