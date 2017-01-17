using System;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class AddRefuelingViewModel
    {
        private readonly IVehicleService _vehicleService;

        public AddRefuelingViewModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public void AddRefueling(string vehicleid, DateTime date, double pricePerLiter, double volumeInLiters, int odometerInKm, bool fullTank)
        {
            _vehicleService.AddRefueling("vehicleId", date, pricePerLiter, volumeInLiters, odometerInKm, fullTank);
        }
    }
}
