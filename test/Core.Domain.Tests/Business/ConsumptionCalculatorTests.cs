using System;
using Branslekollen.Core.Domain.Business;
using Branslekollen.Core.Domain.Models;
using Xunit;
using FluentAssertions;

namespace API.Tests.Business
{
    // http://stackoverflow.com/questions/41350323/unit-testing-a-net-standard-1-6-library
    public class ConsumptionCalculatorTests
    {
        private const double EPSILON = 0.00001;

        private readonly Refueling _refueling1;
        private readonly Refueling _refueling2;
        private readonly Refueling _refueling3;
        private readonly Vehicle _vehicle;
        private readonly ConsumptionCalculator _sut;

        public ConsumptionCalculatorTests()
        {
            _refueling1 = new Refueling
            {
                MissedRefuelings = false,
                RefuelingDate = DateTime.Parse("2016-11-10"),
                NumberOfLiters = 45.7,
                OdometerInKm = 45789,
                FullTank = true
            };
            _refueling2 = new Refueling
            {
                MissedRefuelings = false,
                RefuelingDate = DateTime.Parse("2016-11-20"),
                NumberOfLiters = 39,
                OdometerInKm = 46507,
                FullTank = true
            };
            _refueling3 = new Refueling
            {
                MissedRefuelings = false,
                RefuelingDate = DateTime.Parse("2016-11-30"),
                NumberOfLiters = 50,
                OdometerInKm = 47490,
                FullTank = true
            };
            _vehicle = new Vehicle();
            _sut = new ConsumptionCalculator();
        }

        [Fact]
        public void CalculateFuelConsumption_ShouldReturnUndefinedConsumption_IfNoRefuelings()
        {
            // ARRANGE

            // ACT
            var result = _sut.CalculateAverageConsumptionAsLiterPerKm(_vehicle, DateTime.Parse("2016-11-01"), DateTime.Parse("2016-11-30"));

            // ASSERT
            Assert.False(result.HasValue);
        }

        [Fact]
        public void CalculateFuelConsumption_ShouldReturnUndefinedConsumption_IfOnlyOneRefueling_AndItIsWithinTimeInterval()
        {
            // ARRANGE
            _vehicle.Refuelings.Add(_refueling1);

            // ACT
            var result = _sut.CalculateAverageConsumptionAsLiterPerKm(_vehicle, DateTime.Parse("2016-11-01"), DateTime.Parse("2016-12-10"));

            // ASSERT
            Assert.False(result.HasValue);
        }

        [Fact]
        public void CalculateFuelConsumption_ShouldReturnConsumption_BasedOnLastRefueling_IfTwoRefuelings_BothWithinTimeframe()
        {
            // ARRANGE
            _vehicle.Refuelings.Add(_refueling1);
            _vehicle.Refuelings.Add(_refueling2);

            // ACT
            var result = _sut.CalculateAverageConsumptionAsLiterPerKm(_vehicle, DateTime.Parse("2016-11-01"), DateTime.Parse("2016-11-30"));

            // ASSERT
            result.Should().BeApproximately(0.0543175487465181, EPSILON);
        }

        [Fact]
        public void CalculateFuelConsumption_ShouldReturnConsumption_BasedOnLastTwoRefuelings_IfThreeRefuelings_AllWithinTimeframe()
        {
            // ARRANGE
            _vehicle.Refuelings.Add(_refueling1);
            _vehicle.Refuelings.Add(_refueling2);
            _vehicle.Refuelings.Add(_refueling3);

            // ACT
            var result = _sut.CalculateAverageConsumptionAsLiterPerKm(_vehicle, DateTime.Parse("2016-11-01"), DateTime.Parse("2016-12-10"));

            // ASSERT
            result.Should().BeApproximately(0.052591, EPSILON);
        }

        [Fact]
        public void CalculateFuelConsumption_ShouldReturnConsumption_BasedOnLastTwoRefuelings_IfThreeRefuelings_AndTheFirstIsBeforeTimeInterval()
        {
            // ARRANGE
            _vehicle.Refuelings.Add(_refueling1);
            _vehicle.Refuelings.Add(_refueling2);
            _vehicle.Refuelings.Add(_refueling3);

            // ACT
            var result = _sut.CalculateAverageConsumptionAsLiterPerKm(_vehicle, DateTime.Parse("2016-11-15"), DateTime.Parse("2016-12-10"));

            // ASSERT
            result.Should().BeApproximately(0.052591, EPSILON);
        }

        [Fact]
        public void CalculateFuelConsumption_ShouldReturnConsumption_BasedOnLastRefueling_IfThreeRefuelings_AndTheFirstTwoAreBeforeTimeInterval()
        {
            // ARRANGE
            _vehicle.Refuelings.Add(_refueling1);
            _vehicle.Refuelings.Add(_refueling2);
            _vehicle.Refuelings.Add(_refueling3);

            // ACT
            var result = _sut.CalculateAverageConsumptionAsLiterPerKm(_vehicle, DateTime.Parse("2016-11-27"), DateTime.Parse("2016-12-10"));

            // ASSERT
            result.Should().BeApproximately(0.0508647, EPSILON);
        }

        [Fact]
        public void CalculateFuelConsumption_ShouldReturnUndefinedConsumption_IfThreeRefuelings_AndAllAreBeforeTimeInterval()
        {
            // ARRANGE
            _vehicle.Refuelings.Add(_refueling1);
            _vehicle.Refuelings.Add(_refueling2);
            _vehicle.Refuelings.Add(_refueling3);

            // ACT
            var result = _sut.CalculateAverageConsumptionAsLiterPerKm(_vehicle, DateTime.Parse("2016-12-05"), DateTime.Parse("2016-12-10"));

            // ASSERT
            result.Should().NotHaveValue();
        }

        [Fact]
        public void CalculateFuelConsumption_ShouldReturnUndefinedConsumption_IfThreeRefuelings_AndAllAreAfterTimeInterval()
        {
            // ARRANGE
            _vehicle.Refuelings.Add(_refueling1);
            _vehicle.Refuelings.Add(_refueling2);
            _vehicle.Refuelings.Add(_refueling3);

            // ACT
            var result = _sut.CalculateAverageConsumptionAsLiterPerKm(_vehicle, DateTime.Parse("2016-10-05"), DateTime.Parse("2016-10-31"));

            // ASSERT
            result.Should().NotHaveValue();
        }

        [Fact]
        public void CalculateFuelConsumption_ShouldReturnUndefinedConsumption_IfThreeRefuelings_AndOnlyFirstIsWithinTimeInterval()
        {
            // ARRANGE
            _vehicle.Refuelings.Add(_refueling1);
            _vehicle.Refuelings.Add(_refueling2);
            _vehicle.Refuelings.Add(_refueling3);

            // ACT
            var result = _sut.CalculateAverageConsumptionAsLiterPerKm(_vehicle, DateTime.Parse("2016-11-01"), DateTime.Parse("2016-11-15"));

            // ASSERT
            result.Should().NotHaveValue();
        }

        [Fact]
        public void CalculateFuelConsumption_ShouldReturnConsumption_BasedOnSecondRefueling_IfOnlySecondIsWithinTimeInterval()
        {
            // ARRANGE
            _vehicle.Refuelings.Add(_refueling1);
            _vehicle.Refuelings.Add(_refueling2);
            _vehicle.Refuelings.Add(_refueling3);

            // ACT
            var result = _sut.CalculateAverageConsumptionAsLiterPerKm(_vehicle, DateTime.Parse("2016-11-15"), DateTime.Parse("2016-11-25"));

            // ASSERT
            result.Should().BeApproximately(0.054317549, EPSILON);
        }

        [Fact]
        public void CalculateFuelConsumption_ShouldReturnConsumption_BasedOnSecondRefueling_IfTimeIntervalIsExactlyTheSameDateAsSecond()
        {
            // ARRANGE
            _vehicle.Refuelings.Add(_refueling1);
            _vehicle.Refuelings.Add(_refueling2);
            _vehicle.Refuelings.Add(_refueling3);

            // ACT
            var result = _sut.CalculateAverageConsumptionAsLiterPerKm(_vehicle, DateTime.Parse("2016-11-20"), DateTime.Parse("2016-11-20"));

            // ASSERT
            result.Should().BeApproximately(0.054317549, EPSILON);
        }
    }
}
