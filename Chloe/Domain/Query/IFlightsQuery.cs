using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;
using Chloe.Dto.Enums;

namespace Chloe.Domain.Query
{
    public interface IFlightsQuery
    {
        IEnumerable<ChloeDto.Flight> GetAllChloe();

        IEnumerable<ChloeDto.Flight> GetChloeBySearchDate(DateTime date);

        IEnumerable<ChloeDto.Flight> GetChloeByReceiverGroup(ChloeDto.ReceiverGroup receiverGroup);

        IEnumerable<ChloeDto.Flight> GetChloeBySearchCriteria(ChloeDto.SearchCriteria searchCriteria);
    }
}
