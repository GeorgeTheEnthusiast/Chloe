using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using Castle.Windsor.Installer;
using Flights.Controllers.FlightsControllers;
using Flights.Controllers.TimeTableControllers;
using Flights.Converters;
using Flights.Domain.Command;
using Flights.Domain.Query;
using Flights.Exceptions;
using Flights.NBPCurrency;
using Flights.Quartz;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using Quartz;
using Quartz.Spi;

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

            #region Converters

            Container.Register(
                Component.For<IFlightWebsiteConverter>().ImplementedBy(typeof (FlightWebsiteConverter)),
                Component.For<ICurrencyConverter>().ImplementedBy(typeof(CurrencyConverter)),
                Component.For<IFlightsConverter>().ImplementedBy(typeof(FlightsConverter)),
                Component.For<IRyanAirDateConverter>().ImplementedBy(typeof(RyanAirDateConverter)),
                Component.For<ISearchCriteriaConverter>().ImplementedBy(typeof(SearchCriteriaConverter)),
                Component.For<IWizzAirCalendarConverter>().ImplementedBy(typeof(WizzAirCalendarConverter)),
                Component.For<INotificationReceiversConverter>().ImplementedBy(typeof(NotificationReceiversConverter)),
                Component.For<INetConverter>().ImplementedBy(typeof(NetConverter)),
                Component.For<ICityConverter>().ImplementedBy(typeof(CityConverter)),
                Component.For<INorwegianDateConverter>().ImplementedBy(typeof(NorwegianDateConverter)),
                Component.For<ICommonConverters>().ImplementedBy(typeof(CommonConverters)),
                Component.For<INotificationReceiversGroupsConverter>().ImplementedBy(typeof(NotificationReceiversGroupsConverter)),
                Component.For<ICarrierConverter>().ImplementedBy(typeof(CarrierConverter)),
                Component.For<ITimeTableConverter>().ImplementedBy(typeof(TimeTableConverter)),
                Component.For<ITimeTableStatusConverter>().ImplementedBy(typeof(TimeTableStatusConverter)),
                Component.For<ITimeTablePeriodConverter>().ImplementedBy(typeof(TimeTablePeriodConverter))
                );

            #endregion

            #region Queries

            Container.Register(
                Component.For<IFlightWebsiteQuery>().ImplementedBy(typeof(FlightWebsiteQuery)),
                Component.For<ISearchCriteriaQuery>().ImplementedBy(typeof(SearchCriteriaQuery)),
                Component.For<IFlightsQuery>().ImplementedBy(typeof(FlightsQuery)),
                Component.For<ICityQuery>().ImplementedBy(typeof(CityQuery)),
                Component.For<ICarrierQuery>().ImplementedBy(typeof(CarrierQuery)),
                Component.For<ITimeTableStatusQuery>().ImplementedBy(typeof(TimeTableStatusQuery)),
                Component.For<INotificationsReceiverQuery>().ImplementedBy(typeof(NotificationsReceiverQuery)),
                Component.For<INotificationReceiversGroupsQuery>().ImplementedBy(typeof(NotificationReceiversGroupsQuery))
                );

            #endregion

            #region Commands

            Container.Register(
                Component.For<ICurrienciesCommand>().ImplementedBy(typeof(CurrenciesCommand)),
                Component.For<IFlightsCommand>().ImplementedBy(typeof(FlightsCommand)),
                Component.For<INetCommand>().ImplementedBy(typeof(NetCommand)),
                Component.For<ICitiesCommand>().ImplementedBy(typeof(CitiesCommand)),
                Component.For<ICarrierCommand>().ImplementedBy(typeof(CarrierCommand)),
                Component.For<ITimeTableCommand>().ImplementedBy(typeof(TimeTableCommand)),
                Component.For<ITimeTableStatusCommand>().ImplementedBy(typeof(TimeTableStatusCommand))
                );

            #endregion

            #region WebSiteControllers

            Container.Register(
                Component.For<IWebSiteController>().ImplementedBy(typeof(RyanAirWebSiteController)),
                Component.For<IWebSiteController>().ImplementedBy(typeof(WizzAirWebSiteController)),
                Component.For<IWebSiteController>().ImplementedBy(typeof(NorwegianWebSiteController)),
                Component.For<IWebSiteController>().ImplementedBy(typeof(GoogleFlightsWebSiteController))
                );

            #endregion

            #region FlightsNetControllers

            Container.Register(
                Component.For<IFlightsNetController>().ImplementedBy(typeof(RyanAirFlightsNetController)),
                Component.For<IFlightsNetController>().ImplementedBy(typeof(WizzAirFlightsNetController)),
                Component.For<IFlightsNetController>().ImplementedBy(typeof(NorwegianFlightsNetController))
                );

            #endregion

            #region TimeTableControllers

            Container.Register(
                //Component.For<ITimeTableController>().ImplementedBy(typeof(KrakowAirportTimeTableController)),
                //Component.For<ITimeTableController>().ImplementedBy(typeof(WizzAirTimeTableController)),
                //Component.For<ITimeTableController>().ImplementedBy(typeof(WroclawAirportTimeTableController)),
                Component.For<ITimeTableController>().ImplementedBy(typeof(KatowiceAirportTimeTableController))
                );

            #endregion

            #region Quartz

            Container.Register(
                Component.For<IJob>().ImplementedBy(typeof(SearchFlightsJob)),
                Component.For<IJob>().ImplementedBy(typeof(FlightMailingJob)),
                Component.For<IJob>().ImplementedBy(typeof(FlightsNetJob)),
                Component.For<IJob>().ImplementedBy(typeof(NBPCurrencyDownloaderJob)),
                Component.For<IJob>().ImplementedBy(typeof(CreateTimeTableJob)),
                Component.For<IJobFactory>().ImplementedBy(typeof(WindsorJobFactory))
                );

            #endregion

            #region Others

            Container.Register(
                Component.For<IWebDriver>().ImplementedBy(typeof(ChromeDriver)),
                Component.For<IXmlParser>().ImplementedBy(typeof(XmlParser)),
                Component.For<IXmlDownloader>().ImplementedBy(typeof(XmlDownloader)),
                Component.For<ICurrencySellRate>().ImplementedBy(typeof(CurrencySellRate))
                );

            #endregion
        }
    }
}
