using System;
using Branslekollen.Core.Domain.Models;

namespace Branslekollen.Core.Domain.Business
{
    public interface IConsumptionCalculator
    {
        double? CalculateAverageConsumptionAsLiterPerKm(Vehicle vehicle, DateTime startDate, DateTime endDate);
    }
}
