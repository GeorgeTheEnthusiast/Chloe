using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Quartz;
using Quartz.Spi;

namespace Flights.Quartz
{
    public class WindsorJobFactory : IJobFactory
    {
        public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
        {
            return Bootstrapper.Container.Resolve<IJob>(bundle.JobDetail.JobType.FullName);
        }

        public void ReturnJob(IJob job)
        {
        }
    }
}
