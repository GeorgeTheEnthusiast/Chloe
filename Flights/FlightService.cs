using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Converters;
using Flights.Dto;
using Flights.Dto.Enums;
using Flights.Exceptions;
using OpenQA.Selenium;

namespace Flights
{
    public class FlightService : IFlightService
    {
        private readonly IRyanAirWebSiteController _ryanAirWebSiteController;
        private readonly IWizzAirWebSiteController _wizzAirWebSiteController;

        public FlightService(IRyanAirWebSiteController ryanAirWebSiteController,
            IWizzAirWebSiteController wizzAirWebSiteController)
        {
            if (ryanAirWebSiteController == null) throw new ArgumentNullException("ryanAirWebSiteController");
            if (wizzAirWebSiteController == null) throw new ArgumentNullException("wizzAirWebSiteController");

            _ryanAirWebSiteController = ryanAirWebSiteController;
            _wizzAirWebSiteController = wizzAirWebSiteController;
        }

        public List<Flight> GetRyanAirFlights(SearchCriteria searchCriteria)
        {
            List<Flight> result = new List<Flight>();
            
            _ryanAirWebSiteController.NavigateToUrl();

            _ryanAirWebSiteController.MakeTicketOneWay();

            _ryanAirWebSiteController.FillCityFrom(searchCriteria);

            _ryanAirWebSiteController.FillCityTo(searchCriteria);

            _ryanAirWebSiteController.FindFlights();

            _ryanAirWebSiteController.FillDate(searchCriteria);

            _ryanAirWebSiteController.FindFlights();

            _ryanAirWebSiteController.GetInputValidationState();

            if (_ryanAirWebSiteController.IsNextPageLoadedSuccessfully() == false)
            {
                throw new FlightsPageIsNotLoadedCorrectlyException();
            }

            result = _ryanAirWebSiteController.GetFlights(searchCriteria);

            _ryanAirWebSiteController.TerminateSite();

            return result;
        }

        public List<Flight> GetWizzAirFlights(SearchCriteria searchCriteria)
        {
            List<Flight> result = new List<Flight>();

            _wizzAirWebSiteController.NavigateToUrl();
            
            _wizzAirWebSiteController.FillCityFrom(searchCriteria);

            _wizzAirWebSiteController.FillCityTo(searchCriteria);
            
            _wizzAirWebSiteController.FillDate(searchCriteria);

            _wizzAirWebSiteController.FindFlights();
            
            result = _wizzAirWebSiteController.GetFlights(searchCriteria);

            return result;
        }

        public List<Flight> GetFlights(SearchCriteria searchCriteria)
        {
            List<Flight> result = new List<Flight>();

            if (DateTime.Compare(searchCriteria.DepartureDate, DateTime.Now) <= 0)
                return new List<Flight>();

            if (searchCriteria.Carrier.Id == (int)CarrierType.RyanAir)
                result.AddRange(GetRyanAirFlights(searchCriteria));

            if (searchCriteria.Carrier.Id == (int)CarrierType.WizzAir)
                result.AddRange(GetWizzAirFlights(searchCriteria));

//            if (searchCriteria.Carrier.Id == (int)CarrierType.Norwegian)
//                result.AddRange(GetWizzszAirFlights(searchCriteria));

            return result;
        }
    }
}
