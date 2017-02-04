using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Branslekollen.Core.Domain.Business;
using Branslekollen.Core.Persistence;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class StatisticsViewModel
    {
        private readonly ILocalStorage _localStorage;
        private readonly IVehicleService _vehicleService;
        private readonly IConsumptionCalculator _consumptionCalculator;

        public string ActiveVehicleId { get; set; }

        public StatisticsViewModel(ILocalStorage localStorage, IVehicleService vehicleService, IConsumptionCalculator consumptionCalculator)
        {
            _localStorage = localStorage;
            _vehicleService = vehicleService;
            _consumptionCalculator = consumptionCalculator;
        }
        
        public async Task<List<VehicleDescriptor>> GetLocalVehicleDescriptors()
        {
            return await _localStorage.GetVehicleDescriptors();
        }

        public async Task<decimal?> GetAverageConsumption()
        {
            var vehicle = await _vehicleService.GetById(ActiveVehicleId);
            if (vehicle == null) return null;

            return _consumptionCalculator.CalculateAverageConsumption(vehicle, DateTime.MinValue, DateTime.MaxValue);
        }
    }
}
