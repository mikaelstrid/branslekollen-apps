using System;
using System.IO;
using System.Threading.Tasks;
using Branslekollen.Core.ApiModels;
using Branslekollen.Core.Persistence;
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
        private readonly ILocalStorage _localStorage;

        public VehicleService(IConfiguration configuration, ILocalStorage localStorage)
        {
            _configuration = configuration;
            _localStorage = localStorage;
        }

        public async Task<VehicleApiModel> Create(string name, string fuelType)
        {
            try
            {
                var url = $"{_configuration.ApiBaseUrl}{VEHICLES_ENDPOINT}";

                Log.Verbose("VehicleService.Create: Making POST request to {Url} with {@Parameters}", url, new { Name = name, FuelType = fuelType });

                var apiModel = new VehicleApiModel { Name = name, Fuel = fuelType };

                var cretedVehicle = await url
                    .PostJsonAsync(apiModel)
                    .ReceiveJson<VehicleApiModel>();

                Log.Verbose("VehicleService.Create: Request sent successfully, received {@CreateVehicle}", cretedVehicle);

                var vehicleIds = await _localStorage.GetVehicleIds();
                vehicleIds.Add(cretedVehicle.Id);
                _localStorage.SaveVehicleIds(vehicleIds);
                Log.Verbose("VehicleService.Create: Vehicle ids {@VehicleIds} saved to local storage", vehicleIds);

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
