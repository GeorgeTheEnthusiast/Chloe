using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
            List<SearchCriteria> criteriasToRepeat = criterias.ToList();

            while (criterias.Any())
            {
                foreach (var criteria in criterias)
                {
                    try
                    {
                        _logger.Info("Searching for flights from {0} with departure day {1} from {2} to {3}...", criteria.Carrier.Name, criteria.DepartureDate.ToShortDateString(), criteria.CityFrom.Name, criteria.CityTo.Name);

                        if (DateTime.Compare(criteria.DepartureDate, DateTime.Now) <= 0)
                        {
                            criteriasToRepeat.Remove(criteria);
                            continue;
                        }
                            
                        foreach (var webSiteController in _webSiteControllers)
                        {
                            List<Flight> flights = webSiteController.GetFlights(criteria);

                            _flightsCommand.AddRange(flights);
                        }

                        criteriasToRepeat.Remove(criteria);
                    }
                    catch (Exception ex)
                    {
                        criteriasToRepeat.Add(criteria);
                    }
                }

                criterias = criteriasToRepeat.ToList();
            }
            
            _logger.Info("Searching for the cheapest prices completed.");
        }

        public void DeleteOldFlights()
        {
            _logger.Debug("Deleting old records from today...");

            _flightsCommand.DeleteFlightsBySearchDate(DateTime.Now);

            _logger.Debug("Deleting old records from today completed...");
        }
    }
}
