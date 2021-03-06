using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
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
        }
        
        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();

            if (string.IsNullOrWhiteSpace(RefuelingId))
            {
                Date = DateTime.Now.ToString("d");
            }
            else
            {
                var refueling = (await VehicleService.GetByIdAsync(ActiveVehicleId))
                    .Refuelings
                    .Single(r => r.Id == RefuelingId);

                Date = refueling.RefuelingDate.ToString("d");
                Price = refueling.PricePerLiter.ToString();
                Volume = refueling.NumberOfLiters.ToString();
                Odometer = refueling.OdometerInKm.ToString();
                FullTank = refueling.FullTank;
            }
        }

        public override void OnSaveInstanceState(ISavedState savedState)
        {
            base.OnSaveInstanceState(savedState);
            savedState.PutString(Constants.RefuelingIdName, RefuelingId);
            Log.Verbose("RefuelingViewModel.OnSaveInstanceState: Saved refueling id '{RefuelingId}' to saved state", RefuelingId);
        }

        public async Task HandleSaveAsync(DateTime refuelDate, double pricePerLiter, double volumeInLiters, int odometerInKm, bool fullTank)
        {
            CheckIfInitialized();

            if (string.IsNullOrWhiteSpace(RefuelingId))
                await VehicleService.AddRefuelingAsync(ActiveVehicleId, refuelDate, pricePerLiter, volumeInLiters, odometerInKm, fullTank);
            else
                await VehicleService.UpdateRefuelingAsync(ActiveVehicleId, RefuelingId, refuelDate, pricePerLiter, volumeInLiters, odometerInKm, fullTank);
        }

        public async Task HandleDeleteAsync()
        {
            CheckIfInitialized();

            if (string.IsNullOrWhiteSpace(RefuelingId))
                return;
            else
                await VehicleService.DeleteRefuelingAsync(ActiveVehicleId, RefuelingId);
        }
    }
}