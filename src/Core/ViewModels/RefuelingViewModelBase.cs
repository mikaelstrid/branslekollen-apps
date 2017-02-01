using System;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public abstract class RefuelingViewModelBase
    {
        protected readonly IVehicleService VehicleService;
        protected readonly string VehicleId;

        public string Date { get; set; } = "";
        public string Price { get; set; } = "";
        public string Volume { get; set; } = "";
        public string Odometer { get; set; } = "";

        protected RefuelingViewModelBase(IVehicleService vehicleService, string vehicleId)
        {
            VehicleService = vehicleService;
            VehicleId = vehicleId;
        }

        public abstract void HandleSave(DateTime refuelDate, double pricePerLiter, double volumeInLiters, int odometerInKm, bool fullTank);
    }
}