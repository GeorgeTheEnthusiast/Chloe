using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Converters;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;

namespace Chloe.Domain.Command
{
    public class FlightsCommand : IFlightsCommand
    {
        private readonly IFlightsConverter _flightsConverter;
        
        public FlightsCommand(IFlightsConverter flightsConverter)
        {
            if (flightsConverter == null) throw new ArgumentNullException("flightsConverter");

            _flightsConverter = flightsConverter;
        }

        public void Add(ChloeDto.Flight flight)
        {
            using (var ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                var domainChloe = _flightsConverter.Convert(flight);
                ChloeEntities.Chloe.Add(domainChloe);
                ChloeEntities.SaveChanges();
            }
        }

        public void DeleteChloeBySearchDate(DateTime date)
        {
            using (var ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                var ChloeToDelete = ChloeEntities.Chloe.Where(x => x.SearchDate.Year == date.Year
                                                                         && x.SearchDate.Month == date.Month
                                                                         && x.SearchDate.Day == date.Day);
                if (ChloeToDelete.Any())
                {
                    ChloeEntities.Chloe.RemoveRange(ChloeToDelete);
                    ChloeEntities.SaveChanges();
                }
            }
        }

        public void AddRange(IEnumerable<ChloeDto.Flight> Chloe)
        {
            using (var ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                var domainChloe = _flightsConverter.Convert(Chloe);
                ChloeEntities.Chloe.AddRange(domainChloe);
                ChloeEntities.SaveChanges();
            }
        }

        public void DeleteChloeBySearchCriteria(ChloeDto.SearchCriteria searchCriteria)
        {
            using (var ChloeEntities = new ChloeDomain.ChloeEntities())
            {
                var ChloeToDelete = ChloeEntities.Chloe.Where(x => x.SearchCriterias.Id == searchCriteria.Id);

                if (ChloeToDelete.Any())
                {
                    ChloeEntities.Chloe.RemoveRange(ChloeToDelete);
                    ChloeEntities.SaveChanges();
                }
            }
        }
    }
}
