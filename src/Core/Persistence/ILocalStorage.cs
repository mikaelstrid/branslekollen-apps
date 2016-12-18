using System.Collections.Generic;
using System.Threading.Tasks;

namespace Branslekollen.Core.Persistence
{
    public interface ILocalStorage
    {
        Task<List<string>> GetVehicleIds();
        void SaveVehicleIds(List<string> vehicleIds);
    }
}
