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
using NLog;
using OpenQA.Selenium;

namespace Flights
{
    public class FlightSearchController : IFlightSearchController
    {
        private IFlightService _flightService;
        private readonly IFlightsCommand _flightsCommand;
        private readonly ISearchCriteriaQuery _searchCriteriaQuery;
        private readonly IWebDriver _driver;
        private List<int> _searchesToRepeat;
        private List<SearchCriteria> _searchCriterias;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public FlightSearchController(
            IFlightService flightService,
            IFlightsCommand flightsCommand,
            ISearchCriteriaQuery searchCriteriaQuery, 
            IWebDriver driver)
        {
            if (flightService == null) throw new ArgumentNullException("flightService");
            if (flightsCommand == null) throw new ArgumentNullException("flightsCommand");
            if (searchCriteriaQuery == null) throw new ArgumentNullException("searchCriteriaQuery");
            if (driver == null) throw new ArgumentNullException("driver");
            
            _flightService = flightService;
            _flightsCommand = flightsCommand;
            _searchCriteriaQuery = searchCriteriaQuery;
            _driver = driver;
        }

        public bool StartSearch()
        {
            PrepareSearch();

            foreach (var criterias in _searchCriterias)
            {
                if (!_searchesToRepeat.Contains(criterias.Id))
                    continue;

                try
                {
                    _logger.Info("Searching for flights from {0} with departures day {1} from {2} to {3}...", criterias.Carrier.Name, criterias.DepartureDate.ToShortDateString(), criterias.CityFrom.Name, criterias.CityTo.Name);
                    
                    _flightService = Bootstrapper.Container.Resolve<IFlightService>();

                    List<Flight> flights = _flightService.GetFlights(criterias);
                    
                    foreach(var flight in flights)
                        _flightsCommand.Add(flight);

                    _searchesToRepeat.Remove(criterias.Id);
                }
                catch (Exception e)
                {
                    _logger.Error(e);
                }
            }

            if (_searchesToRepeat.Count > 0)
            {
                _logger.Info("Left: " + _searchesToRepeat.Count);
                return false;
            }
            else
            {
                _driver.Quit();
                return true;
            }
        }

        public void DeleteOldFlights()
        {
            _logger.Debug("Deleting old records...");

            _flightsCommand.DeleteFlightsBySearchDate(DateTime.Now);

            _logger.Debug("Deleting old records completed...");
        }

        private void PrepareSearch()
        {
            if (_searchCriterias == null)
                _searchCriterias = _searchCriteriaQuery.GetAllSearchCriterias();

            if (_searchesToRepeat == null)
                _searchesToRepeat = _searchCriterias.Select(x => x.Id).ToList();
        }

        
    }
}
