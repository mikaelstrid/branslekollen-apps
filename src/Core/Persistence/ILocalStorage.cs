using System;
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
                new VehicleDescriptor {Id = Guid.NewGuid().ToString(), Name = "Volvo V90"}
            });
        }
    }
}

