using System.Threading.Tasks;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class ProfileViewModel : ViewModelBase
    {
        public ProfileViewModel(IApplicationState applicationState, IVehicleService vehicleService, ISavedState savedState)
            : base(applicationState, vehicleService, savedState)
        {
        }

        public new async Task Initialize()
        {
            await base.Initialize();
        }

        public async void DeleteAllVehicles()
        {
            await VehicleService.DeleteAll();
            ApplicationState.ActiveVehicleId = "";
        }
    }
}
