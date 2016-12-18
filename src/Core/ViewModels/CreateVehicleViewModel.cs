using System.Threading.Tasks;
using Branslekollen.Core.ApiModels;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class CreateVehicleViewModel
    {
        private readonly IVehicleService _vehicleService;

        public CreateVehicleViewModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public async Task<VehicleApiModel> CreateVehicle(string name, string fuelType)
        {
            return await _vehicleService.Create(name, fuelType);
        }
    }
}
