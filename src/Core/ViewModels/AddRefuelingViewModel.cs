using System;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class AddRefuelingViewModel : RefuelingViewModelBase
    {
        public AddRefuelingViewModel(IVehicleService vehicleService, string vehicleId) : base(vehicleService, vehicleId, null) { }

        public override void HandleSave(DateTime refuelDate, double pricePerLiter, double volumeInLiters, int odometerInKm, bool fullTank)
        {
            VehicleService.AddRefueling(VehicleId, refuelDate, pricePerLiter, volumeInLiters, odometerInKm, fullTank);
        }

        public override void HandleDelete() { }
    }
}
