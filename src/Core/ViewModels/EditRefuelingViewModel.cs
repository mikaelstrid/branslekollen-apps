using System;
using System.Globalization;
using System.Linq;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class EditRefuelingViewModel : RefuelingViewModelBase
    {
        private readonly string _refuelingId;

        public EditRefuelingViewModel(IVehicleService vehicleService, string vehicleId, string refuelingId) : base(vehicleService, vehicleId)
        {
            _refuelingId = refuelingId;

            var refueling = VehicleService.GetById(VehicleId)
                .Result
                .Refuelings
                .Single(r => r.Id == refuelingId);

            Date = refueling.RefuelingDate.ToString("d");
            Price = refueling.PricePerLiter.ToString(CultureInfo.InvariantCulture);
            Volume = refueling.NumberOfLiters.ToString(CultureInfo.InvariantCulture);
            Odometer = refueling.OdometerInKm.ToString(CultureInfo.InvariantCulture);
        }

        public override void HandleSave(DateTime refuelDate, double pricePerLiter, double volumeInLiters, int odometerInKm, bool fullTank)
        {
        }
    }
}