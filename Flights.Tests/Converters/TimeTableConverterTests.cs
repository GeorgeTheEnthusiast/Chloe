using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Flights.Converters;
using NUnit.Framework;
using FlightDto = Flights.Dto;
using FlightDomain = Flights.Domain.Dto;

namespace Flights.Tests.Converters
{
    [TestFixture]
    class TimeTableConverterTests
    {
        private ITimeTableConverter _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new TimeTableConverter();
        }

        [Test]
        public void ConvertDtoToDomainGeneratesValidData()
        {
            FlightDto.TimeTable input = new FlightDto.TimeTable()
            {
                Carrier = new FlightDto.Carrier()
                {
                    Id = 1,
                    Name = "ALA"
                },
                FlightWebsite = new FlightDto.FlightWebsite()
                {
                    Id = 2,
                    Name = "KOT",
                    Website = "www.wp.pl"
                },
                ArrivalDate = DateTime.Now,
                DepartureDate = DateTime.Now.AddDays(1),
                CityFrom = new FlightDto.City()
                {
                    Id = 3,
                    Name = "Mielec",
                    Alias = "MLC"
                },
                CityTo = new FlightDto.City()
                {
                    Id = 4,
                    Name = "Rzeszów",
                    Alias = "RZE"
                }
            };

            var output = _sut.Convert(input);

            Assert.AreEqual(input.ArrivalDate, output.ArrivalDate);
            Assert.AreEqual(input.Carrier.Id, output.Carrier_Id);
            Assert.AreEqual(input.CityFrom.Id, output.CityFrom_Id);
            Assert.AreEqual(input.CityTo.Id, output.CityTo_Id);
            Assert.AreEqual(input.DepartureDate, output.DepartureDate);
            Assert.AreEqual(input.FlightWebsite.Id, output.FlightWebsite_Id);
        }
    }
}
