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

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        public async Task DeleteAllVehiclesAsync()
        {
            await VehicleService.DeleteAllAsync();
            ApplicationState.ActiveVehicleId = "";
        }
    }
}
