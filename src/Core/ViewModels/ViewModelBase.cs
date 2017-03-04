using System;
using System.Threading.Tasks;
using Branslekollen.Core.Services;
using Serilog;

namespace Branslekollen.Core.ViewModels
{
    public abstract class ViewModelBase
    {
        protected readonly IApplicationState ApplicationState;
        protected readonly IVehicleService VehicleService;

        private bool IsInitialized { get; set; }
        public bool FreshApplicationStart { get; private set; }

        public string ActiveVehicleId => ApplicationState.ActiveVehicleId;

        protected ViewModelBase(IApplicationState applicationState, IVehicleService vehicleService, ISavedState savedState)
        {
            ApplicationState = applicationState;
            VehicleService = vehicleService;

            if (!savedState.HasState) return;

            Log.Verbose("ViewModelBase.Ctor: State dictionary has state, restore the application state...");
            applicationState.Restore(savedState);
            Log.Verbose("ViewModelBase.Ctor: Application state restored");
        }

        protected async Task InitializeAsync()
        {
            IsInitialized = true;

            // The application state is already initialized
            if (!string.IsNullOrWhiteSpace(ApplicationState.ActiveVehicleId)) return;

            // Fresh start of the application, get the last used vehicle (or create a new one)
            Log.Verbose("ViewModelBase.InitializeAsync: No active vehicle id, getting last used vehicle or creating a new");
            var vehicle = await VehicleService.GetLastUsedAsync();
            if (vehicle == null)
            {
                Log.Verbose("ViewModelBase.InitializeAsync: No vehicles received from VehicleService, create a new one and set FreshApplicationStart = true");
                vehicle = await VehicleService.CreateAsync();
                FreshApplicationStart = true;
            }

            ApplicationState.ActiveVehicleId = vehicle.Id;
            Log.Verbose("ViewModelBase.InitializeAsync: Initialization complete, vehicle id set to {Vehicle}", vehicle.Id);
        }

        public virtual void OnSaveInstanceState(ISavedState savedState)
        {
            ApplicationState.Save(savedState);
        }

        protected void CheckIfInitialized()
        {
            if (!IsInitialized) throw new Exception("The viewmodel is not initialized");
        }
    }
}