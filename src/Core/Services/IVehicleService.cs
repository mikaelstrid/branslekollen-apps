using System.Threading.Tasks;
using Branslekollen.Core.ApiModels;
using Flurl.Http;

namespace Branslekollen.Core.Services
{
    public interface IVehicleService
    {
        Task<VehicleApiModel> Create(string name, string fuelType);
    }

    public class VehicleService : IVehicleService
    {
        public async Task<VehicleApiModel> Create(string name, string fuelType)
        {
            var apiModel = new VehicleApiModel {Name = name, Fuel = fuelType};

            var cretedVehicle = await "http://169.254.80.80:51058/api/vehicles"
                .PostJsonAsync(apiModel)
                .ReceiveJson<VehicleApiModel>();

            return cretedVehicle;
        }
    }
}
