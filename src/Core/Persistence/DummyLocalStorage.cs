using System.Collections.Generic;
using System.Threading.Tasks;
using Branslekollen.Core.ViewModels;

namespace Branslekollen.Core.Persistence
{
    public class DummyLocalStorage : ILocalStorage
    {
        private readonly List<VehicleDescriptor> _vehicleDescriptors = new List<VehicleDescriptor>();

        public DummyLocalStorage()
        {
            //return;
            _vehicleDescriptors.Add(
                new VehicleDescriptor
                {
                    Id = "01D16512-9B17-4956-A9AC-266C264234A9",
                    Name = "Volvo V90"
                }
            );
        }

        //public Task<List<string>> GetVehicleIds()
        //{
        //    throw new NotImplementedException();
        //}

        //public void SaveVehicleIds(List<string> vehicleIds)
        //{
        //    throw new NotImplementedException();
        //}

        public Task<List<VehicleDescriptor>> GetVehicleDescriptors()
        {
            return Task.FromResult(_vehicleDescriptors);
        }

        public Task AddVehicleDescriptor(VehicleDescriptor vehicleDescriptor)
        {
            _vehicleDescriptors.Add(vehicleDescriptor);
            return Task.FromResult(0);
        }
    }
}