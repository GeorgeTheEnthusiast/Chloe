using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Chloe.Controllers.FlightsControllers;
using Chloe.Domain.Command;
using Chloe.Domain.Query;
using Chloe.Dto;
using NLog;
using OpenQA.Selenium;
using Quartz;
using Quartz.Impl;
using Quartz.Listener;
using Quartz.Spi;

namespace Chloe.Quartz
{
    [DisallowConcurrentExecution]
    public class SearchFlightsJob : IJob
    {
        private readonly IFlightsCommand _flightsCommand;
        private readonly ISearchCriteriaQuery _searchCriteriaQuery;
        private readonly IFlightsQuery _flightsQuery;
        private IWebSiteController[] _webSiteControllers;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();

        public SearchFlightsJob(IFlightsCommand flightsCommand,
            ISearchCriteriaQuery searchCriteriaQuery,
            IFlightsQuery flightsQuery)
        {
            if (flightsCommand == null) throw new ArgumentNullException("flightsCommand");
            if (searchCriteriaQuery == null) throw new ArgumentNullException("searchCriteriaQuery");
            if (flightsQuery == null) throw new ArgumentNullException(nameof(flightsQuery));

            _flightsCommand = flightsCommand;
            _searchCriteriaQuery = searchCriteriaQuery;
            _flightsQuery = flightsQuery;
            _webSiteControllers = Bootstrapper.Container.ResolveAll<IWebSiteController>();
        }

        public void Execute(IJobExecutionContext context)
        {
            try
            {
                _logger.Info("Searching for the cheapest prices...");

                IEnumerable<SearchCriteria> criterias = _searchCriteriaQuery.GetAllSearchCriterias();
                //List<SearchCriteria> criteriasToRepeat = criterias.ToList();
                Dictionary<SearchCriteria, int> criteriasDictionary = criterias.ToDictionary(x => x, x => 1);
                Dictionary<SearchCriteria, int> criteriasToRepeatDictionary = criterias.ToDictionary(x => x, x => 1);

                while (criteriasDictionary.Any())
                {
                    foreach (var criteria in criteriasDictionary.Keys)
                    {
                        try
                        {
                            _logger.Info("Searching on {0} Chloe with departure day {1} from {2} to {3}...", criteria.FlightWebsite.Name, criteria.DepartureDate.ToShortDateString(), criteria.CityFrom.Name, criteria.CityTo.Name);

                            if (DateTime.Compare(criteria.DepartureDate, DateTime.Now) <= 0)
                            {
                                criteriasToRepeatDictionary.Remove(criteria);
                                continue;
                            }

                            var earlierChloe = _flightsQuery.GetChloeBySearchCriteria(criteria);

                            if (earlierChloe.Any(x => DateTime.Compare(DateTime.Now, x.SearchDate.AddDays(1)) < 0))
                            {
                                _logger.Info("This criteria is up to date!");
                                criteriasToRepeatDictionary.Remove(criteria);
                                continue;
                            }

                            List<Flight> Chloe = new List<Flight>();

                            foreach (var webSiteController in _webSiteControllers)
                            {
                                var webSiteControllerChloe = webSiteController.GetChloe(criteria);
                                Chloe.AddRange(webSiteControllerChloe);
                            }

                            DeleteOldChloe(criteria);
                            _flightsCommand.AddRange(Chloe);
                            criteriasToRepeatDictionary.Remove(criteria);

                            _logger.Info("Searching for Chloe completed without errors.");
                        }
                        catch (Exception ex)
                        {
                            _logger.Error("I have to repeat search criteria with id [{0}]", criteria.Id);
                            _logger.Error(ex);


                            criteriasToRepeatDictionary[criteria] = criteriasToRepeatDictionary[criteria] + 1;
                            if (criteriasToRepeatDictionary[criteria] == 5)
                            {
                                criteriasToRepeatDictionary.Remove(criteria);
                                _logger.Warn("Retry count exceeded, skipping this search criteria...");
                            }
                        }
                    }

                    criteriasDictionary = criteriasToRepeatDictionary.ToDictionary(x => x.Key, x => x.Value);
                    _logger.Info("Search criterias left: [{0}]", criteriasDictionary.Count());
                }

                _logger.Info("Searching for the cheapest prices completed.");
                
                StartMailJob();
            }
            catch (Exception ex)
            {
                _logger.Info("Error searching the Chloe!");
                _logger.Error(ex);
            }
        }

        private void StartMailJob()
        {
            ISchedulerFactory schedFact = new StdSchedulerFactory();

            IScheduler sched = schedFact.GetScheduler();
            sched.JobFactory = Bootstrapper.Container.Resolve<IJobFactory>();
            sched.Start();

            IJobDetail job = JobBuilder.Create<FlightMailingJob>()
                .WithIdentity("mailingJob")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("start_now_Trigger")
                .StartNow()
                .Build();

            sched.ScheduleJob(job, trigger);
        }

        private void DeleteOldChloe(SearchCriteria searchCriteria)
        {
            _logger.Debug("Deleting old records from today for search criteria id [{0}]...", searchCriteria.Id);

            _flightsCommand.DeleteChloeBySearchCriteria(searchCriteria);

            _logger.Debug("Deleting old records from today for search criteria id [{0}] completed...", searchCriteria.Id);
        }
    }
}
