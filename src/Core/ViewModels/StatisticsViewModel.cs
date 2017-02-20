using System;
using System.Threading.Tasks;
using Branslekollen.Core.Domain.Business;
using Branslekollen.Core.Services;
using Serilog;

namespace Branslekollen.Core.ViewModels
{
    public class StatisticsViewModel
    {
        private readonly IVehicleService _vehicleService;
        private readonly IConsumptionCalculator _consumptionCalculator;
        private readonly string _vehicleId;

        public StatisticsViewModel(IVehicleService vehicleService, IConsumptionCalculator consumptionCalculator, string vehicleId)
        {
            _vehicleService = vehicleService;
            _consumptionCalculator = consumptionCalculator;
            if (string.IsNullOrWhiteSpace(vehicleId))
            {
                Log.Verbose("StatisticsViewModel.Ctor: No vehicle id specified, getting last used vehicle or creating a new.");
                _vehicleId = _vehicleService.GetLastUsedOrCreateNew().Result.Id;
            }
            else
            {
                _vehicleId = vehicleId;
            }
        }

        public async Task<double?> GetAverageConsumptionAsLiterPerKm()
        {
            var vehicle = await _vehicleService.GetById(_vehicleId);
            var averageConsumption = _consumptionCalculator.CalculateAverageConsumptionAsLiterPerKm(vehicle, DateTime.Parse("1900-01-01"), DateTime.Parse("2100-01-01"));
            Log.Verbose("StatisticsViewModel.GetAverageConsumptionAsLiterPerKm: Average consumption = {AverageConsumption}", averageConsumption);
            return averageConsumption;
        }
    }
}
