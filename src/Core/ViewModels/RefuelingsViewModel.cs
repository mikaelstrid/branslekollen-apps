using System.Collections.Generic;
using System.Threading.Tasks;
using Branslekollen.Core.Domain.Models;
using Branslekollen.Core.Services;

namespace Branslekollen.Core.ViewModels
{
    public class RefuelingsViewModel : ViewModelBase
    {
        public RefuelingsViewModel(IApplicationState applicationState, IVehicleService vehicleService, ISavedState savedState)
            : base(applicationState, vehicleService, savedState)
        {
        }

        public new async Task InitializeAsync()
        {
            await base.InitializeAsync();
        }

        public async Task<List<Refueling>> GetRefuelingsAsync()
        {
            var vehicle = await VehicleService.GetByIdAsync(ApplicationState.ActiveVehicleId);
            return vehicle != null ? vehicle.Refuelings : new List<Refueling>();
        }
    }
}
