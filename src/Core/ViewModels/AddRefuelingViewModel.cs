using System;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class AddRefuelingViewModel
    {
        private readonly IVehicleService _vehicleService;

        public string ActiveVehicleId { get; set; }

        public AddRefuelingViewModel(IVehicleService vehicleService)
        {
            _vehicleService = vehicleService;
        }

        public void AddRefueling(DateTime refuelDate, double pricePerLiter, double volumeInLiters, int odometerInKm, bool fullTank)
        {
            _vehicleService.AddRefueling(ActiveVehicleId, refuelDate, pricePerLiter, volumeInLiters, odometerInKm, fullTank);
        }
    }
}
