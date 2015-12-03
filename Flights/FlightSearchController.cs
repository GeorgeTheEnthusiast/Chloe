using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Controllers.FlightsControllers;
using Flights.Converters;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Dto;
using Flights.Dto.Enums;
using NLog;
using OpenQA.Selenium;

namespace Flights
{
    public class FlightSearchController : IFlightSearchController
    {
        private readonly IFlightsCommand _flightsCommand;
        private readonly ISearchCriteriaQuery _searchCriteriaQuery;
        private readonly Logger _logger = LogManager.GetCurrentClassLogger();
        private IWebSiteController[] _webSiteControllers;

        public FlightSearchController(
            IFlightsCommand flightsCommand,
            ISearchCriteriaQuery searchCriteriaQuery)
        {
            if (flightsCommand == null) throw new ArgumentNullException("flightsCommand");
            if (searchCriteriaQuery == null) throw new ArgumentNullException("searchCriteriaQuery");

            _flightsCommand = flightsCommand;
            _searchCriteriaQuery = searchCriteriaQuery;
            _webSiteControllers = Bootstrapper.Container.ResolveAll<IWebSiteController>();
        }

        public void StartSearch()
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
                        _logger.Info("Searching on {0} flights with departure day {1} from {2} to {3}...", criteria.FlightWebsite.Name, criteria.DepartureDate.ToShortDateString(), criteria.CityFrom.Name, criteria.CityTo.Name);

                        if (DateTime.Compare(criteria.DepartureDate, DateTime.Now) <= 0)
                        {
                            criteriasToRepeatDictionary.Remove(criteria);
                            continue;
                        }

                        DeleteOldFlights(criteria);

                        foreach (var webSiteController in _webSiteControllers)
                        {
                            List<Flight> flights = webSiteController.GetFlights(criteria);

                            _flightsCommand.AddRange(flights);
                        }

                        criteriasToRepeatDictionary.Remove(criteria);

                        _logger.Info("Searching for flights completed without errors.");
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
        }

        private void DeleteOldFlights(SearchCriteria searchCriteria)
        {
            _logger.Debug("Deleting old records from today for search criteria id [{0}]...", searchCriteria.Id);

            _flightsCommand.DeleteFlightsBySearchCriteria(searchCriteria);

            _logger.Debug("Deleting old records from today for search criteria id [{0}] completed...", searchCriteria.Id);
        }
    }
}
