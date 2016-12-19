using System.Collections.Generic;
using System.Threading.Tasks;
using Branslekollen.Core.Models;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class SplashViewModel
    {
        private readonly IVehicleService _vehicleService;

        public SplashViewModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async Task<List<Vehicle>> GetVehicles()
        {
            return await _vehicleService.GetAll();
        } 
    }
}
