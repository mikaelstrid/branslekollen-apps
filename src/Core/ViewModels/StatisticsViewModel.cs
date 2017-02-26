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

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        public async Task<double?> GetAverageConsumptionAsLiterPerKmAsync()
        {
            var vehicle = await VehicleService.GetByIdAsync(ActiveVehicleId);
            var averageConsumption = _consumptionCalculator.CalculateAverageConsumptionAsLiterPerKm(vehicle, DateTime.Parse("1900-01-01"), DateTime.Parse("2100-01-01"));
            Log.Verbose("StatisticsViewModel.GetAverageConsumptionAsLiterPerKmAsync: Average consumption = {AverageConsumption}", averageConsumption);
            return averageConsumption;
        }
    }
}
