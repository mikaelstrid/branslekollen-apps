using System;
using Branslekollen.Core.Domain.Models;

namespace Branslekollen.Core.Domain.Business
{
    public interface IConsumptionCalculator
    {
        double? CalculateAverageConsumption(Vehicle vehicle, DateTime startDate, DateTime endDate);
    }
}
