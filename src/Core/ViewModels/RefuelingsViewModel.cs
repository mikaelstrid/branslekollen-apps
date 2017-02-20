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
        
        public async Task<List<Refueling>> GetRefuelings()
        {
            var vehicle = await VehicleService.GetById(ApplicationState.ActiveVehicleId);
            return vehicle != null ? vehicle.Refuelings : new List<Refueling>();
        }
    }
}
