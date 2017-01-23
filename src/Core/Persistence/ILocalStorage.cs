using System.Collections.Generic;
using System.Threading.Tasks;
using Branslekollen.Core.ViewModels;

namespace Branslekollen.Core.Persistence
{
    public interface ILocalStorage
    {
        Task<List<string>> GetVehicleIds();
        void SaveVehicleIds(List<string> vehicleIds);
        Task<List<VehicleDescriptor>> GetVehicleDescriptors();
    }
}

