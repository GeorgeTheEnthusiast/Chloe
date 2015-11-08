using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Flights.Converters;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Exceptions;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace Flights
{
    public static class Bootstrapper
    {
        private static IWindsorContainer _container;

        public static IWindsorContainer Container => _container;
        
        public static void Register()
        {
            if (_container != null)
                throw new DependenciesAlreadyRegisteredException();

            _container = new WindsorContainer();

            Container.Install(FromAssembly.This());

            Container.Register(
                Component.For<ICarriersConverter>().ImplementedBy(typeof(CarriersConverter)),
                Component.For<ICurrencyConverter>().ImplementedBy(typeof(CurrencyConverter)),
                Component.For<IFlightsConverter>().ImplementedBy(typeof(FlightsConverter)),
                Component.For<IRyanAirDateConverter>().ImplementedBy(typeof(RyanAirDateConverter)),
                Component.For<ISearchCriteriaConverter>().ImplementedBy(typeof(SearchCriteriaConverter)),
                Component.For<ICurrienciesCommand>().ImplementedBy(typeof(CurrenciesCommand)),
                Component.For<IFlightsCommand>().ImplementedBy(typeof(FlightsCommand)),
                Component.For<ICarrierQuery>().ImplementedBy(typeof(CarrierQuery)),
                Component.For<ICityQuery>().ImplementedBy(typeof(CityQuery)),
                Component.For<ICurrencyQuery>().ImplementedBy(typeof(CurrencyQuery)),
                Component.For<INotificationsReceiverQuery>().ImplementedBy(typeof(NotificationsReceiverQuery)),
                Component.For<ISearchCriteriaQuery>().ImplementedBy(typeof(SearchCriteriaQuery)),
                Component.For<IFlightService>().ImplementedBy(typeof(FlightService)),
                Component.For<IRyanAirWebSiteController>().ImplementedBy(typeof(RyanAirWebSiteController)),
                Component.For<IWizzAirWebSiteController>().ImplementedBy(typeof(WizzAirWebSiteController)),
                Component.For<IWebDriver>().ImplementedBy(typeof(ChromeDriver)),
                Component.For<IWizzAirCalendarConverter>().ImplementedBy(typeof(WizzAirCalendarConverter)),
                Component.For<IFlightsQuery>().ImplementedBy(typeof(FlightsQuery)),
                Component.For<INotificationReceiversConverter>().ImplementedBy(typeof(NotificationReceiversConverter))
                );
        }
    }
}
