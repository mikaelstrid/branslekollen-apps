using Serilog;

namespace Branslekollen.Core.Services
{
    public interface IApplicationState
    {
        string ActiveVehicleId { get; set; }
        void Restore(ISavedState savedState);
        void Save(ISavedState savedState);
    }

    public class ApplicationState : IApplicationState
    {
        public string ActiveVehicleId { get; set; } = "";

        public void Restore(ISavedState savedState)
        {
            ActiveVehicleId = savedState.GetString(Constants.VehicleIdName);
            Log.Verbose("ApplicationState.Restore: Restored active vehicle id '{ActiveVehicleId}' from saved state", ActiveVehicleId);
        }

        public void Save(ISavedState savedState)
        {
            savedState.PutString(Constants.VehicleIdName, ActiveVehicleId);
            Log.Verbose("ApplicationState.Save: Saved active vehicle id '{ActiveVehicleId}' to saved state", ActiveVehicleId);
        }
    }
}
