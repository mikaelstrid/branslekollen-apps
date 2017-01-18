using System.Collections.Generic;
using System.Threading.Tasks;
using Branslekollen.Core.Models;
using Branslekollen.Core.Persistence;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class RefuelingsViewModel
    {
        private readonly ILocalStorage _localStorage;
        private readonly IVehicleService _vehicleService;

        public string ActiveVehicleId { get; private set; }

        public RefuelingsViewModel(ILocalStorage localStorage, IVehicleService vehicleService)
        {
            _localStorage = localStorage;
            _vehicleService = vehicleService;
        }
        
        public async Task<List<VehicleDescriptor>> GetLocalVehicleDescriptors()
        {
            return await _localStorage.GetVehicleDescriptors();
        }

        public async Task<Vehicle> GetVehicle(string vehicleId)
        {
            return await _vehicleService.GetById(vehicleId);
        }

        public void SetActiveVehicle(string vehicleId)
        {
            ActiveVehicleId = vehicleId;
        }
    }
}
