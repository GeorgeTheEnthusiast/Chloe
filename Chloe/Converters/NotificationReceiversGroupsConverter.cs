using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using ChloeDomain = Chloe.Domain.Dto;
using ChloeDto = Chloe.Dto;

namespace Chloe.Converters
{
    public class NotificationReceiversGroupsConverter : INotificationReceiversGroupsConverter
    {
        public NotificationReceiversGroupsConverter()
        {
            Mapper.CreateMap<ChloeDomain.NotificationReceiversGroups, ChloeDto.NotificationReceiverGroup>()
                .ForMember(x => x.ReceiverGroup, expression => expression.MapFrom(src => src.ReceiverGroups))
                .ForMember(x => x.NotificationReceiver,
                    expression => expression.MapFrom(src => src.NotificationReceivers));

            Mapper.CreateMap<ChloeDomain.ReceiverGroups, ChloeDto.ReceiverGroup>();

            Mapper.CreateMap<ChloeDomain.NotificationReceivers, ChloeDto.NotificationReceiver>()
                .ForMember(x => x.Email, expression => expression.ResolveUsing(receivers => receivers.Email.Trim()));
        }
        
        public IEnumerable<ChloeDto.NotificationReceiverGroup> Convert(IEnumerable<ChloeDomain.NotificationReceiversGroups> input)
        {
            return Mapper.Map<IEnumerable<ChloeDto.NotificationReceiverGroup>>(input);
        }
    }
}
