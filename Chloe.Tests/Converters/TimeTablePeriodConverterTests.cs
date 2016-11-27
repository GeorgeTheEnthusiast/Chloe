using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chloe.Converters;
using NUnit.Framework;

namespace Chloe.Tests.Converters
{
    [TestFixture]
    class TimeTablePeriodConverterTests
    {
        private ITimeTablePeriodConverter _sut;

        [SetUp]
        public void SetUp()
        {
            _sut = new TimeTablePeriodConverter();
        }

        [Test]
        [TestCaseSource("GenerateData")]
        public void GivingPeriodAndDaysCreatesValidOutput(TimeTablePeriodConverterModel model)
        {
            DateTime dateFrom = DateTime.Now;
            DateTime dateTo = DateTime.Now.AddMonths(5);

            var output = _sut.Convert(model.daysInWeek, dateFrom, dateTo);

            Assert.AreEqual(output.Any(x => x.DayOfWeek == DayOfWeek.Monday), model.Monday);
            Assert.AreEqual(output.Any(x => x.DayOfWeek == DayOfWeek.Tuesday), model.Tuesday);
            Assert.AreEqual(output.Any(x => x.DayOfWeek == DayOfWeek.Wednesday), model.Wednesday);
            Assert.AreEqual(output.Any(x => x.DayOfWeek == DayOfWeek.Thursday), model.Thursday);
            Assert.AreEqual(output.Any(x => x.DayOfWeek == DayOfWeek.Friday), model.Friday);
            Assert.AreEqual(output.Any(x => x.DayOfWeek == DayOfWeek.Saturday), model.Saturday);
            Assert.AreEqual(output.Any(x => x.DayOfWeek == DayOfWeek.Sunday), model.Sunday);
        }

        [Test]
        public void GivingEmptyDaysCreatesEmptyOutput()
        {
            DateTime dateFrom = DateTime.Now;
            DateTime dateTo = DateTime.Now.AddMonths(5);

            var output = _sut.Convert(new List<int>(), dateFrom, dateTo);

            Assert.AreEqual(output.Count(), 0);
        }

        [Test]
        public void GivingWrongDaysThrowsNotSupportedException()
        {
            DateTime dateFrom = DateTime.Now;
            DateTime dateTo = DateTime.Now.AddMonths(5);

            Assert.Throws<NotSupportedException>(() => _sut.Convert(new List<int>() { 1, 2, 3, 9 }, dateFrom, dateTo));
        }

        [Test]
        public void GivingWrongDatesThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => _sut.Convert(new List<int>() { 1, 2, 3 }, DateTime.Now, DateTime.Now));
            Assert.Throws<ArgumentException>(() => _sut.Convert(new List<int>() { 1, 2, 3 }, DateTime.Now.AddDays(1), DateTime.Now));
        }

        List<TimeTablePeriodConverterModel> GenerateData()
        {
            return new List<TimeTablePeriodConverterModel>()
            {
                new TimeTablePeriodConverterModel() { daysInWeek = new List<int>() { 1 }, Monday = true, Tuesday = false, Wednesday = false, Thursday = false, Friday = false, Saturday = false, Sunday = false },
                new TimeTablePeriodConverterModel() { daysInWeek = new List<int>() { 1, 2 }, Monday = true, Tuesday = true, Wednesday = false, Thursday = false, Friday = false, Saturday = false, Sunday = false },
                new TimeTablePeriodConverterModel() { daysInWeek = new List<int>() { 1, 2, 3 }, Monday = true, Tuesday = true, Wednesday = true, Thursday = false, Friday = false, Saturday = false, Sunday = false },
                new TimeTablePeriodConverterModel() { daysInWeek = new List<int>() { 1, 2, 3, 4 }, Monday = true, Tuesday = true, Wednesday = true, Thursday = true, Friday = false, Saturday = false, Sunday = false },
                new TimeTablePeriodConverterModel() { daysInWeek = new List<int>() { 1, 2, 3, 4, 5 }, Monday = true, Tuesday = true, Wednesday = true, Thursday = true, Friday = true, Saturday = false, Sunday = false },
                new TimeTablePeriodConverterModel() { daysInWeek = new List<int>() { 1, 2, 3, 4, 5, 6 }, Monday = true, Tuesday = true, Wednesday = true, Thursday = true, Friday = true, Saturday = true, Sunday = false },
                new TimeTablePeriodConverterModel() { daysInWeek = new List<int>() { 1, 2, 3, 4, 5, 6, 7 }, Monday = true, Tuesday = true, Wednesday = true, Thursday = true, Friday = true, Saturday = true, Sunday = true },
                new TimeTablePeriodConverterModel() { daysInWeek = new List<int>() { 1, 3, 6 }, Monday = true, Tuesday = false, Wednesday = true, Thursday = false, Friday = false, Saturday = true, Sunday = false },
                new TimeTablePeriodConverterModel() { daysInWeek = new List<int>() { 5 }, Monday = false, Tuesday = false, Wednesday = false, Thursday = false, Friday = true, Saturday = false, Sunday = false },
                new TimeTablePeriodConverterModel() { daysInWeek = new List<int>() { 2, 7 }, Monday = false, Tuesday = true, Wednesday = false, Thursday = false, Friday = false, Saturday = false, Sunday = true },
                new TimeTablePeriodConverterModel() { daysInWeek = new List<int>() { 3 }, Monday = false, Tuesday = false, Wednesday = true, Thursday = false, Friday = false, Saturday = false, Sunday = false },
            };
        }

        public class TimeTablePeriodConverterModel
        {
            public List<int> daysInWeek { get; set; }

            public bool Monday { get; set; }

            public bool Tuesday { get; set; }

            public bool Wednesday { get; set; }

            public bool Thursday { get; set; }

            public bool Friday { get; set; }

            public bool Saturday { get; set; }

            public bool Sunday { get; set; }
        }
    }
}
