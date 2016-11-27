using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChloeDto = Chloe.Dto;

namespace Chloe.Domain.Command
{
    public interface ICurrienciesCommand
    {
        ChloeDto.Currency Add(ChloeDto.Currency currency);

        ChloeDto.Currency Merge(ChloeDto.Currency currency);
    }
}
