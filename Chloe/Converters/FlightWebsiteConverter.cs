using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChloeDto = Chloe.Dto;
using ChloeDomain = Chloe.Domain.Dto;

namespace Chloe.Converters
{
    public class FlightWebsiteConverter : IFlightWebsiteConverter
    {
        public FlightWebsiteConverter()
        {
            Mapper.CreateMap<ChloeDto.FlightWebsite, ChloeDomain.FlightWebsites>();

            Mapper.CreateMap<ChloeDomain.FlightWebsites, ChloeDto.FlightWebsite>()
                .ForMember(x => x.Name, expression => expression.ResolveUsing(flightWebsite => flightWebsite.Name.Trim()));
        }
        
        public ChloeDomain.FlightWebsites Convert(ChloeDto.FlightWebsite input)
        {
            return Mapper.Map<ChloeDomain.FlightWebsites>(input);
        }

        public ChloeDto.FlightWebsite Convert(ChloeDomain.FlightWebsites input)
        {
            return Mapper.Map<ChloeDto.FlightWebsite>(input);
        }

        public IEnumerable<ChloeDto.FlightWebsite> Convert(IEnumerable<ChloeDomain.FlightWebsites> input)
        {
            return Mapper.Map<IEnumerable<ChloeDto.FlightWebsite>>(input);
        }

        public IEnumerable<ChloeDto.FlightWebsite> Convert(DbSet<ChloeDomain.FlightWebsites> input)
        {
            return Mapper.Map<IEnumerable<ChloeDto.FlightWebsite>>(input);
        }
    }
}
