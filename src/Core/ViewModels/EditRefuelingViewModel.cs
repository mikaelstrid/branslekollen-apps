using System;
using System.Globalization;
using System.Linq;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class EditRefuelingViewModel : RefuelingViewModelBase
    {
        public EditRefuelingViewModel(IVehicleService vehicleService, string vehicleId, string refuelingId) : base(vehicleService, vehicleId, refuelingId)
        {
            var refueling = VehicleService.GetById(VehicleId)
                .Result
                .Refuelings
                .Single(r => r.Id == refuelingId);

            Date = refueling.RefuelingDate.ToString("d");
            Price = refueling.PricePerLiter.ToString(CultureInfo.InvariantCulture);
            Volume = refueling.NumberOfLiters.ToString(CultureInfo.InvariantCulture);
            Odometer = refueling.OdometerInKm.ToString(CultureInfo.InvariantCulture);
            FullTank = refueling.FullTank;
        }

        public override void HandleSave(DateTime refuelDate, double pricePerLiter, double volumeInLiters, int odometerInKm, bool fullTank)
        {
            VehicleService.UpdateRefueling(VehicleId, RefuelingId, refuelDate, pricePerLiter, volumeInLiters, odometerInKm, fullTank);
        }

        public override void HandleDelete()
        {
            VehicleService.DeleteRefueling(VehicleId, RefuelingId);
        }
    }
}