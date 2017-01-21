using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using FlightsDomain = Flights.Domain.Dto;
using FlightsDto = Flights.Dto;

namespace Flights.Converters
{
    public class NotificationReceiversGroupsConverter : INotificationReceiversGroupsConverter
    {
        public NotificationReceiversGroupsConverter()
        {
            Mapper.CreateMap<FlightsDomain.NotificationReceiversGroups, FlightsDto.NotificationReceiverGroup>()
                .ForMember(x => x.ReceiverGroup, expression => expression.MapFrom(src => src.ReceiverGroups))
                .ForMember(x => x.NotificationReceiver,
                    expression => expression.MapFrom(src => src.NotificationReceivers));

            Mapper.CreateMap<FlightsDomain.ReceiverGroups, FlightsDto.ReceiverGroup>();

            Mapper.CreateMap<FlightsDomain.NotificationReceivers, FlightsDto.NotificationReceiver>()
                .ForMember(x => x.Email, expression => expression.ResolveUsing(receivers => receivers.Email.Trim()));
        }
        
        public IEnumerable<FlightsDto.NotificationReceiverGroup> Convert(IEnumerable<FlightsDomain.NotificationReceiversGroups> input)
        {
            return Mapper.Map<IEnumerable<FlightsDto.NotificationReceiverGroup>>(input);
        }
    }
}
