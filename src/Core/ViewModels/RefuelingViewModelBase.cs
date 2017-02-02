using System;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public abstract class RefuelingViewModelBase
    {
        protected readonly IVehicleService VehicleService;
        protected readonly string VehicleId;
        public string RefuelingId;

        public string Date { get; set; } = "";
        public string Price { get; set; } = "";
        public string Volume { get; set; } = "";
        public string Odometer { get; set; } = "";
        public bool FullTank { get; set; } = true;

        protected RefuelingViewModelBase(IVehicleService vehicleService, string vehicleId, string refuelingId)
        {
            VehicleService = vehicleService;
            VehicleId = vehicleId;
            RefuelingId = refuelingId;
        }

        public abstract void HandleSave(DateTime refuelDate, double pricePerLiter, double volumeInLiters, int odometerInKm, bool fullTank);
        public abstract void HandleDelete();
    }
}