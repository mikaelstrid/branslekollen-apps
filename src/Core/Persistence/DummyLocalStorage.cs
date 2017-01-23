using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Branslekollen.Core.ViewModels;

namespace Branslekollen.Core.Persistence
{
    public class DummyLocalStorage : ILocalStorage
    {
        public Task<List<string>> GetVehicleIds()
        {
            throw new NotImplementedException();
        }

        public void SaveVehicleIds(List<string> vehicleIds)
        {
            throw new NotImplementedException();
        }

        public Task<List<VehicleDescriptor>> GetVehicleDescriptors()
        {
            return Task.FromResult(new List<VehicleDescriptor>
            {
                new VehicleDescriptor { Id = "01D16512-9B17-4956-A9AC-266C264234A9", Name = "Volvo V90" }
            });
        }
    }
}