using System;
using System.Threading.Tasks;
using Branslekollen.Core.Domain.Business;
using Branslekollen.Core.Services;
using Serilog;

namespace Branslekollen.Core.ViewModels
{
    public class StatisticsViewModel : ViewModelBase
    {
        private readonly IConsumptionCalculator _consumptionCalculator;

        public StatisticsViewModel(
            IApplicationState applicationState,
            IVehicleService vehicleService,
            ISavedState savedState, 
            IConsumptionCalculator consumptionCalculator)
            : base(applicationState, vehicleService, savedState)
        {
            _consumptionCalculator = consumptionCalculator;
        }

        public new async Task Initialize()
        {
            await base.Initialize();
        }

        public async Task<double?> GetAverageConsumptionAsLiterPerKm()
        {
            var vehicle = await VehicleService.GetById(ActiveVehicleId);
            var averageConsumption = _consumptionCalculator.CalculateAverageConsumptionAsLiterPerKm(vehicle, DateTime.Parse("1900-01-01"), DateTime.Parse("2100-01-01"));
            Log.Verbose("StatisticsViewModel.GetAverageConsumptionAsLiterPerKm: Average consumption = {AverageConsumption}", averageConsumption);
            return averageConsumption;
        }
    }
}
