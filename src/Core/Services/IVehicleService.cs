using System;
using System.IO;
using System.Threading.Tasks;
using Branslekollen.Core.ApiModels;
using Flurl.Http;
using Serilog;

namespace Branslekollen.Core.Services
{
    public interface IVehicleService
    {
        Task<VehicleApiModel> Create(string name, string fuelType);
    }

    public class VehicleService : IVehicleService
    {
        private const string VEHICLES_ENDPOINT = "/vehicles";

        private readonly IConfiguration _configuration;

        public VehicleService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<VehicleApiModel> Create(string name, string fuelType)
        {
            try
            {
                Log.Verbose("VehicleService.Create: Sending request to API with {Name} and {FuelType}", name, fuelType);

                var apiModel = new VehicleApiModel { Name = name, Fuel = fuelType };

                var cretedVehicle = await $"{_configuration.ApiBaseUrl}{VEHICLES_ENDPOINT}"
                    .PostJsonAsync(apiModel)
                    .ReceiveJson<VehicleApiModel>();

                Log.Verbose("VehicleService.Create: Request sent successfully, received {@CreateVehicle}", cretedVehicle);

                return cretedVehicle;

            }
            catch (FlurlHttpTimeoutException e1)
            {
                Log.Warning("VehicleService.Create: Timeout when calling API");
                throw new IOException("Timeout when calling API", e1);
            }
            catch (FlurlHttpException e2)
            {
                string errorMessage;
                if (e2.Call.Response != null)
                    errorMessage = "Failed with response code " + e2.Call.Response.StatusCode;
                else
                    errorMessage = "Totally failed before getting a response! " + e2.Message;

                Log.Warning("VehicleService.Create: " + errorMessage);
                throw new IOException("Error when calling API", e2);
            }
        }
    }
}
