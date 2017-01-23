using System.Collections.Generic;
using System.Threading.Tasks;
using Branslekollen.Core.Domain.Models;
using Branslekollen.Core.Persistence;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class RefuelingsViewModel
    {
        private readonly ILocalStorage _localStorage;
        private readonly IVehicleService _vehicleService;

        public string ActiveVehicleId { get; set; }

        public RefuelingsViewModel(ILocalStorage localStorage, IVehicleService vehicleService)
        {
            _localStorage = localStorage;
            _vehicleService = vehicleService;
        }
        
        public async Task<List<VehicleDescriptor>> GetLocalVehicleDescriptors()
        {
            return await _localStorage.GetVehicleDescriptors();
        }

        public async Task<List<Refueling>> GetRefuelings()
        {
            var vehicle = await _vehicleService.GetById(ActiveVehicleId);
            return vehicle != null ? vehicle.Refuelings : new List<Refueling>();
        }
    }
}
