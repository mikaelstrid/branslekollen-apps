using System;
using System.Linq;
using Branslekollen.Core.Domain.Models;

namespace Branslekollen.Core.Domain.Business
{
    public class RandomConsumptionCalculator : IConsumptionCalculator
    {
        private readonly Random _random;
        
        public RandomConsumptionCalculator()
        {
            _random = new Random();
        }

        public double? CalculateAverageConsumption(Vehicle vehicle, DateTime startDate, DateTime endDate)
        {
            if (vehicle?.Refuelings == null || !vehicle.Refuelings.Any()) return null;
            return _random.Next(30, 80) / 100;
        }
    }
}