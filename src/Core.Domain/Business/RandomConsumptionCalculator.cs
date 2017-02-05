using System;
using System.Linq;
using Branslekollen.Core.Domain.Models;
using Serilog;

namespace Branslekollen.Core.Domain.Business
{
    public class RandomConsumptionCalculator : IConsumptionCalculator
    {
        private readonly Random _random;
        
        public RandomConsumptionCalculator()
        {
            _random = new Random();
        }

        public double? CalculateAverageConsumptionAsLiterPerKm(Vehicle vehicle, DateTime startDate, DateTime endDate)
        {
            if (vehicle?.Refuelings == null || !vehicle.Refuelings.Any()) return null;
            var next = _random.Next(30, 80);
            var average = next / 1000.0;
            Log.Verbose("RandomConsumptionCalculator.CalculateAverageConsumptionAsLiterPerKm: Next {Next} => Average {Average}", next, average);
            return average;
        }
    }
}