using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Branslekollen.Core.Domain.Models;

namespace Branslekollen.Core.Domain.Business
{
    public class ConsumptionCalculator : IConsumptionCalculator
    {
        public double? CalculateAverageConsumptionAsLiterPerKm(Vehicle vehicle, DateTime startDate, DateTime endDate)
        {
            Debug.Assert(startDate.TimeOfDay == TimeSpan.Zero);
            Debug.Assert(endDate.TimeOfDay == TimeSpan.Zero);

            if (vehicle.Refuelings == null || vehicle.Refuelings.Count < 2) return null;

            var calculationModels = ConvertToCalculationModels(vehicle.Refuelings);

            var refuelings = calculationModels
                .Where(r => startDate <= r.RefuelingDate && r.RefuelingDate <= endDate)
                .Where(r => r.FuelConsumptionInLitersPerKm.HasValue)
                .ToArray();

            return refuelings.Any() 
                ? refuelings.Average(r => r.FuelConsumptionInLitersPerKm.Value) 
                : (double?)null;
        }

        private static IEnumerable<RefuelingCalculationModel> ConvertToCalculationModels(IEnumerable<Refueling> refuelings)
        {
            var result = new List<RefuelingCalculationModel>();
            Refueling previousRefueling = null;
            foreach (var refueling in refuelings)
            {
                var calculationModel = new RefuelingCalculationModel
                {
                    RefuelingDate = refueling.RefuelingDate,
                    MissedRefuelings = refueling.MissedRefuelings,
                    NumberOfLiters = refueling.NumberOfLiters,
                    FullTank = refueling.FullTank,
                    DistanceTravelledInKm = refueling.OdometerInKm - previousRefueling?.OdometerInKm
                };
                result.Add(calculationModel);
                previousRefueling = refueling;
            }
            return result;
        }

        private class RefuelingCalculationModel
        {
            public DateTime RefuelingDate { get; set; }
            public bool MissedRefuelings { get; set; }
            public double NumberOfLiters { get; set; }
            public int? DistanceTravelledInKm { get; set; }
            public bool FullTank { get; set; }

            public double? FuelConsumptionInLitersPerKm
            {
                get
                {
                    if (!FullTank || !DistanceTravelledInKm.HasValue) return null;
                    return NumberOfLiters / DistanceTravelledInKm.Value;
                }
            }
        }
    }
}