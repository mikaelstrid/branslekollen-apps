using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Branslekollen.Core.ApiModels;
using Branslekollen.Core.Models;
using Branslekollen.Core.Persistence;
using Flurl;
using Flurl.Http;
using Serilog;

namespace Branslekollen.Core.Services
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetAll();
        Task<VehicleApiModel> Create(string name, string fuelType);
        Task<Vehicle> GetById(string vehicleId);
        void AddRefueling(string vehicleid, DateTime date, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank);
    }

    public class DummyVehicleService : IVehicleService
    {
        private readonly List<Vehicle> _vehicles;

        public DummyVehicleService()
        {
            _vehicles = new List<Vehicle>
            {
                new Vehicle("Volvo V90", "petrol") { Refuelings = new List<Refueling>
                {
                    new Refueling
                    {
                        CreationTime = DateTime.Parse("2017-01-15 21:39"),
                        Date = DateTime.Parse("2017-01-14"),
                        DistanceTravelledInKm = 400,
                        FullTank = true,
                        Id = Guid.NewGuid().ToString(),
                        MissedRefuelings = false,
                        NumberOfLiters = 20,
                        OdometerInKm = 1756,
                        PricePerLiter = 13.37
                    }
                }}
            };
        }

        public Task<List<Vehicle>> GetAll()
        {
            Log.Verbose("DummyVehicleService.GetAll...");
            Task.Delay(1000);
            return Task.FromResult(_vehicles);
        }

        public Task<VehicleApiModel> Create(string name, string fuelType)
        {
            Log.Verbose("DummyVehicleService.Create...");
            Task.Delay(1000);
            return Task.FromResult(new VehicleApiModel
            {
                Id = Guid.NewGuid().ToString(),
                Fuel = "diesel",
                Name = "Volvo V90",
                Refuelings = new List<RefuelingApiModel>()
            });
        }

        public async Task<Vehicle> GetById(string vehicleId)
        {
            return (await GetAll()).First();
        }

        public void AddRefueling(string vehicleid, DateTime date, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank)
        {
            _vehicles.First().Refuelings.Add(new Refueling
            {
                Id = Guid.NewGuid().ToString(),
                Date = date,
                PricePerLiter = pricePerLiter,
                NumberOfLiters = numberOfLiters,
                OdometerInKm = odometerInKm,
                FullTank = fullTank
            });
        }
    }

    public class VehicleService : IVehicleService
    {
        private const string VEHICLES_ENDPOINT = "/vehicles";

        private readonly IConfiguration _configuration;
        private readonly ILocalStorage _localStorage;

        private List<Vehicle> _cachedVehicles;

        public VehicleService(IConfiguration configuration, ILocalStorage localStorage)
        {
            _configuration = configuration;
            _localStorage = localStorage;
            _cachedVehicles = new List<Vehicle>();
        }

        public async Task<List<Vehicle>> GetAll()
        {
            try
            {
                if (_cachedVehicles.Any())
                {
                    Log.Verbose("VehicleService.GetAll: Found {VehiclesCount} local vehicles in memory",
                        _cachedVehicles.Count);
                    return _cachedVehicles;
                }
                else
                {
                    var localVehicleIds = await _localStorage.GetVehicleIds();
                    if (localVehicleIds.Any())
                    {
                        var url = $"{_configuration.ApiBaseUrl}{VEHICLES_ENDPOINT}/ids";
                        Log.Verbose("VehicleService.GetAll: Making GET request to {Url} with ids {LocalVehicleIds}...",
                            url, localVehicleIds);

                        var receivedVehicles = await url
                            .SetQueryParam("ids", localVehicleIds)
                            .WithTimeout(5)
                            .GetJsonAsync<List<VehicleApiModel>>();

                        Log.Verbose("VehicleService.GetAll: ...received {@Vehicles}", receivedVehicles);

                        _localStorage.SaveVehicleIds(receivedVehicles.Select(v => v.Id).ToList());
                        _cachedVehicles = receivedVehicles.Select(v => v.ToDomainModel()).ToList();
                        return _cachedVehicles;
                    }
                    else
                    {
                        Log.Verbose("VehicleService.GetAll: No local vehicle ids, return empty list");
                        return new List<Vehicle>();
                    }
                }
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

        public async Task<VehicleApiModel> Create(string name, string fuelType)
        {
            try
            {
                var url = $"{_configuration.ApiBaseUrl}{VEHICLES_ENDPOINT}";

                Log.Verbose("VehicleService.Create: Making POST request to {Url} with {@Parameters}", url, new { Name = name, FuelType = fuelType });

                var apiModel = new VehicleApiModel { Name = name, Fuel = fuelType };

                var createdVehicle = await url
                    .WithTimeout(5)
                    .PostJsonAsync(apiModel)
                    .ReceiveJson<VehicleApiModel>();

                Log.Verbose("VehicleService.Create: Request sent successfully, received {@CreateVehicle}", createdVehicle);

                var localVehicleIds = await _localStorage.GetVehicleIds();
                localVehicleIds.Add(createdVehicle.Id);
                _localStorage.SaveVehicleIds(localVehicleIds);
                Log.Verbose("VehicleService.Create: Vehicle ids {@VehicleIds} saved to local storage", localVehicleIds);

                return createdVehicle;
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

        public async Task<Vehicle> GetById(string vehicleId)
        {
            return (await GetAll()).FirstOrDefault(v => v.Id == vehicleId);
        }

        public void AddRefueling(string vehicleid, DateTime date, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank)
        {
            throw new NotImplementedException();
        }
    }
}
