using System.Collections.Generic;
using System.Threading.Tasks;
using Branslekollen.Core.Persistence;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class AddRefuelingViewModel
    {
        private readonly ILocalStorage _localStorage;
        private readonly IVehicleService _vehicleService;

        public AddRefuelingViewModel(ILocalStorage localStorage, IVehicleService vehicleService)
        {
            _localStorage = localStorage;
            _vehicleService = vehicleService;
        }
        
        //public async Task<VehicleApiModel> AddRefueling(string vehicleId, DateTime date, decimal )
        //{
        //    return await _vehicleService.Create(name, fuelType);
        //}

        public async Task<List<VehicleDescriptor>> GetLocalVehicleDescriptors()
        {
            return await _localStorage.GetVehicleDescriptors();
        }
    }
}
