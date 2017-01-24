using System;
using System.Threading.Tasks;
using Branslekollen.Core.Domain.Models;
using Branslekollen.Core.Persistence;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class CreateVehicleViewModel
    {
        private readonly ILocalStorage _localStorage;
        private readonly IVehicleService _vehicleService;

        public CreateVehicleViewModel(
            ILocalStorage localStorage, 
            IVehicleService vehicleService)
        {
            _localStorage = localStorage;
            _vehicleService = vehicleService;
        }

        public async Task<Vehicle> CreateVehicle(string name, string fuelType)
        {
            var parsedFuelType = (FuelType) Enum.Parse(typeof(FuelType), fuelType, true);
            var vehicle = await _vehicleService.Create(name, parsedFuelType);

            await _localStorage.AddVehicleDescriptor(new VehicleDescriptor
            {
                Id = vehicle.Id,
                Name = vehicle.Name
            });

            return vehicle;
        }
    }
}
