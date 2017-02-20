//using System;
//using System.Threading.Tasks;
//using Branslekollen.Core.Domain.Models;
//using Branslekollen.Core.Services;

//namespace Branslekollen.Core.ViewModels
//{
//    public class CreateVehicleViewModel
//    {
//        private readonly IVehicleService _vehicleService;

//        public CreateVehicleViewModel(
//            IVehicleService vehicleService)
//        {
//            _vehicleService = vehicleService;
//        }

//        public async Task<Vehicle> CreateVehicle(string name, string fuelType)
//        {
//            var parsedFuelType = (FuelType) Enum.Parse(typeof(FuelType), fuelType, true);
//            var vehicle = await _vehicleService.Create(name, parsedFuelType);
//            return vehicle;
//        }
//    }
//}
