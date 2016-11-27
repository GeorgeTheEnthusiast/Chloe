using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChloeDto = Chloe.Dto;

namespace Chloe.Domain.Command
{
    public interface ITimeTableStatusCommand
    {
        ChloeDto.TimeTableStatus Merge(ChloeDto.TimeTableStatus timeTableStatus);
    }
}
