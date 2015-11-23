using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FlightsDto = Flights.Dto;

namespace Flights.Domain.Command
{
    public interface ICarrierCommand
    {
        FlightsDto.Carrier Merge(string name);
    }
}
