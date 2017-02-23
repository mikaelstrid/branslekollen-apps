using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Branslekollen.Core.Domain.Models;
using Branslekollen.Core.Persistence;
using Flurl;
using Flurl.Http;

namespace Branslekollen.Core.Services
{
    public interface IVehicleService
    {
        Task<List<Vehicle>> GetAll();
        Task<Vehicle> Create(string name = "", FuelType fuelType = FuelType.Unknown);
        Task<Vehicle> GetById(string vehicleId);
        Task DeleteAll();

        Task AddRefueling(string vehicleId, DateTime refuelDate, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank);
        Task UpdateRefueling(string vehicleId, string refuelingId, DateTime refuelDate, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank);
        Task DeleteRefueling(string vehicleId, string refuelingId);
        Task<Vehicle> GetLastUsed();
    }

    //public class VehicleService : IVehicleService
    //{
    //    private const string VEHICLES_ENDPOINT = "/vehicles";

    //    private readonly IConfiguration _configuration;
    //    private readonly ILocalStorage _localStorage;

    //    private List<Vehicle> _cachedVehicles;

    //    public VehicleService(IConfiguration configuration, ILocalStorage localStorage)
    //    {
    //        _configuration = configuration;
    //        _localStorage = localStorage;
    //        _cachedVehicles = new List<Vehicle>();
    //    }

    //    public async Task<List<Vehicle>> GetAll()
    //    {
    //        try
    //        {
    //            if (_cachedVehicles.Any())
    //            {
    //                Log.Verbose("VehicleService.GetAll: Found {VehiclesCount} local vehicles in memory",
    //                    _cachedVehicles.Count);
    //                return _cachedVehicles;
    //            }
    //            else
    //            {
    //                var localVehicleIds = await _localStorage.GetVehicleIds();
    //                if (localVehicleIds.Any())
    //                {
    //                    var url = $"{_configuration.ApiBaseUrl}{VEHICLES_ENDPOINT}/ids";
    //                    Log.Verbose("VehicleService.GetAll: Making GET request to {Url} with ids {LocalVehicleIds}...",
    //                        url, localVehicleIds);

    //                    var receivedVehicles = await url
    //                        .SetQueryParam("ids", localVehicleIds)
    //                        .WithTimeout(5)
    //                        .GetJsonAsync<List<VehicleApiModel>>();

    //                    Log.Verbose("VehicleService.GetAll: ...received {@Vehicles}", receivedVehicles);

    //                    _localStorage.SaveVehicleIds(receivedVehicles.Select(v => v.Id).ToList());
    //                    _cachedVehicles = receivedVehicles.Select(v => v.ToDomainModel()).ToList();
    //                    return _cachedVehicles;
    //                }
    //                else
    //                {
    //                    Log.Verbose("VehicleService.GetAll: No local vehicle ids, return empty list");
    //                    return new List<Vehicle>();
    //                }
    //            }
    //        }
    //        catch (FlurlHttpTimeoutException e1)
    //        {
    //            Log.Warning("VehicleService.Create: Timeout when calling API");
    //            throw new IOException("Timeout when calling API", e1);
    //        }
    //        catch (FlurlHttpException e2)
    //        {
    //            string errorMessage;
    //            if (e2.Call.Response != null)
    //                errorMessage = "Failed with response code " + e2.Call.Response.StatusCode;
    //            else
    //                errorMessage = "Totally failed before getting a response! " + e2.Message;

    //            Log.Warning("VehicleService.Create: " + errorMessage);
    //            throw new IOException("Error when calling API", e2);
    //        }
    //    }

    //    public async Task<Vehicle> Create(string name, string fuelType)
    //    {
    //        try
    //        {
    //            throw new NotImplementedException();
    //            //var url = $"{_configuration.ApiBaseUrl}{VEHICLES_ENDPOINT}";

    //            //Log.Verbose("VehicleService.Create: Making POST request to {Url} with {@Parameters}", url, new { Name = name, FuelType = fuelType });

    //            //var apiModel = new VehicleApiModel { Name = name, Fuel = fuelType };

    //            //var createdVehicle = await url
    //            //    .WithTimeout(5)
    //            //    .PostJsonAsync(apiModel)
    //            //    .ReceiveJson<VehicleApiModel>();

    //            //Log.Verbose("VehicleService.Create: Request sent successfully, received {@CreateVehicle}", createdVehicle);

    //            //var localVehicleIds = await _localStorage.GetVehicleIds();
    //            //localVehicleIds.Add(createdVehicle.Id);
    //            //_localStorage.SaveVehicleIds(localVehicleIds);
    //            //Log.Verbose("VehicleService.Create: Vehicle ids {@VehicleIds} saved to local storage", localVehicleIds);

    //            //return createdVehicle;
    //        }
    //        catch (FlurlHttpTimeoutException e1)
    //        {
    //            Log.Warning("VehicleService.Create: Timeout when calling API");
    //            throw new IOException("Timeout when calling API", e1);
    //        }
    //        catch (FlurlHttpException e2)
    //        {
    //            string errorMessage;
    //            if (e2.Call.Response != null)
    //                errorMessage = "Failed with response code " + e2.Call.Response.StatusCode;
    //            else
    //                errorMessage = "Totally failed before getting a response! " + e2.Message;

    //            Log.Warning("VehicleService.Create: " + errorMessage);
    //            throw new IOException("Error when calling API", e2);
    //        }
    //    }

    //    public async Task<Vehicle> GetById(string vehicleId)
    //    {
    //        return (await GetAll()).FirstOrDefault(v => v.Id == vehicleId);
    //    }

    //    public void AddRefueling(string vehicleId, DateTime date, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
