using System;
using Branslekollen.Core.Domain.Models;

namespace Branslekollen.Core.Domain.Business
{
    public interface IConsumptionCalculator
    {
        decimal? CalculateAverageConsumption(Vehicle vehicle, DateTime startDate, DateTime endDate);
    }
}
