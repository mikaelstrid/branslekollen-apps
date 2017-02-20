using System;
using System.Globalization;
using System.Linq;
using Branslekollen.Core.Services;
using Serilog;

namespace Branslekollen.Core.ViewModels
{
    public class RefuelingViewModel : ViewModelBase
    {
        public string RefuelingId;

        public string Date { get; set; }
        public string Price { get; set; } = "";
        public string Volume { get; set; } = "";
        public string Odometer { get; set; } = "";
        public bool FullTank { get; set; } = true;

        public RefuelingViewModel(
            IApplicationState applicationState, 
            IVehicleService vehicleService, 
            ISavedState savedState,
            string refuelingId)
            : base(applicationState, vehicleService, savedState)
        {
            if (savedState.HasState)
            {
                RefuelingId = savedState.GetString(Constants.RefuelingIdName);
                Log.Verbose("RefuelingViewModel.Ctor: Restored refueling id '{RefuelingId}' from saved state", RefuelingId);
            }
            else
            {
                RefuelingId = refuelingId;
            }

            if (string.IsNullOrWhiteSpace(RefuelingId))
            {
                Date = DateTime.Now.ToString("d");
            }
            else
            {
                var refueling = VehicleService.GetById(ActiveVehicleId)
                    .Result
                    .Refuelings
                    .Single(r => r.Id == refuelingId);

                Date = refueling.RefuelingDate.ToString("d");
                Price = refueling.PricePerLiter.ToString(CultureInfo.InvariantCulture);
                Volume = refueling.NumberOfLiters.ToString(CultureInfo.InvariantCulture);
                Odometer = refueling.OdometerInKm.ToString(CultureInfo.InvariantCulture);
                FullTank = refueling.FullTank;
            }
        }

        public override void OnSaveInstanceState(ISavedState savedState)
        {
            base.OnSaveInstanceState(savedState);
            savedState.PutString(Constants.RefuelingIdName, RefuelingId);
            Log.Verbose("RefuelingViewModel.OnSaveInstanceState: Saved refueling id '{RefuelingId}' to saved state", RefuelingId);
        }

        public void HandleSave(DateTime refuelDate, double pricePerLiter, double volumeInLiters, int odometerInKm, bool fullTank)
        {
            if (string.IsNullOrWhiteSpace(RefuelingId))
                VehicleService.AddRefueling(ActiveVehicleId, refuelDate, pricePerLiter, volumeInLiters, odometerInKm, fullTank);
            else
                VehicleService.UpdateRefueling(ActiveVehicleId, RefuelingId, refuelDate, pricePerLiter, volumeInLiters, odometerInKm, fullTank);
        }

        public void HandleDelete()
        {
            if (string.IsNullOrWhiteSpace(RefuelingId))
                return;
            else
                VehicleService.DeleteRefueling(ActiveVehicleId, RefuelingId);
        }
    }
}