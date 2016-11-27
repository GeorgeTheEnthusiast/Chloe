using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChloeDto = Chloe.Dto;

namespace Chloe.Domain.Command
{
    public interface IFlightsCommand
    {
        void Add(ChloeDto.Flight flight);

        void DeleteChloeBySearchDate(DateTime date);

        void AddRange(IEnumerable<ChloeDto.Flight> Chloe);

        void DeleteChloeBySearchCriteria(ChloeDto.SearchCriteria searchCriteria);
    }
}
