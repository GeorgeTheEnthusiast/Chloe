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
    public class NotificationReceiversConverter : INotificationReceiversConverter
    {
        public NotificationReceiversConverter()
        {
            Mapper.CreateMap<ChloeDomain.NotificationReceivers, ChloeDto.NotificationReceiver>()
                .ForMember(x => x.Email, expression => expression.ResolveUsing(result => result.Email.Trim()));
        }

        public IEnumerable<ChloeDto.NotificationReceiver> Convert(IEnumerable<ChloeDomain.NotificationReceivers> input)
        {
            return Mapper.Map<IEnumerable<ChloeDto.NotificationReceiver>>(input);
        }
    }
}
