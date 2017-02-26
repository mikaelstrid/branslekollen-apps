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
        Task<List<Vehicle>> GetAllAsync();
        Task<Vehicle> CreateAsync(string name = "", FuelType fuelType = FuelType.Unknown);
        Task<Vehicle> GetByIdAsync(string vehicleId);
        Task DeleteAllAsync();

        Task AddRefuelingAsync(string vehicleId, DateTime refuelDate, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank);
        Task UpdateRefuelingAsync(string vehicleId, string refuelingId, DateTime refuelDate, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank);
        Task DeleteRefuelingAsync(string vehicleId, string refuelingId);
        Task<Vehicle> GetLastUsedAsync();
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

    //    public async Task<List<Vehicle>> GetAllAsync()
    //    {
    //        try
    //        {
    //            if (_cachedVehicles.Any())
    //            {
    //                Log.Verbose("VehicleService.GetAllAsync: Found {VehiclesCount} local vehicles in memory",
    //                    _cachedVehicles.Count);
    //                return _cachedVehicles;
    //            }
    //            else
    //            {
    //                var localVehicleIds = await _localStorage.GetVehicleIds();
    //                if (localVehicleIds.Any())
    //                {
    //                    var url = $"{_configuration.ApiBaseUrl}{VEHICLES_ENDPOINT}/ids";
    //                    Log.Verbose("VehicleService.GetAllAsync: Making GET request to {Url} with ids {LocalVehicleIds}...",
    //                        url, localVehicleIds);

    //                    var receivedVehicles = await url
    //                        .SetQueryParam("ids", localVehicleIds)
    //                        .WithTimeout(5)
    //                        .GetJsonAsync<List<VehicleApiModel>>();

    //                    Log.Verbose("VehicleService.GetAllAsync: ...received {@Vehicles}", receivedVehicles);

    //                    _localStorage.SaveVehicleIds(receivedVehicles.Select(v => v.Id).ToList());
    //                    _cachedVehicles = receivedVehicles.Select(v => v.ToDomainModel()).ToList();
    //                    return _cachedVehicles;
    //                }
    //                else
    //                {
    //                    Log.Verbose("VehicleService.GetAllAsync: No local vehicle ids, return empty list");
    //                    return new List<Vehicle>();
    //                }
    //            }
    //        }
    //        catch (FlurlHttpTimeoutException e1)
    //        {
    //            Log.Warning("VehicleService.CreateAsync: Timeout when calling API");
    //            throw new IOException("Timeout when calling API", e1);
    //        }
    //        catch (FlurlHttpException e2)
    //        {
    //            string errorMessage;
    //            if (e2.Call.Response != null)
    //                errorMessage = "Failed with response code " + e2.Call.Response.StatusCode;
    //            else
    //                errorMessage = "Totally failed before getting a response! " + e2.Message;

    //            Log.Warning("VehicleService.CreateAsync: " + errorMessage);
    //            throw new IOException("Error when calling API", e2);
    //        }
    //    }

    //    public async Task<Vehicle> CreateAsync(string name, string fuelType)
    //    {
    //        try
    //        {
    //            throw new NotImplementedException();
    //            //var url = $"{_configuration.ApiBaseUrl}{VEHICLES_ENDPOINT}";

    //            //Log.Verbose("VehicleService.CreateAsync: Making POST request to {Url} with {@Parameters}", url, new { Name = name, FuelType = fuelType });

    //            //var apiModel = new VehicleApiModel { Name = name, Fuel = fuelType };

    //            //var createdVehicle = await url
    //            //    .WithTimeout(5)
    //            //    .PostJsonAsync(apiModel)
    //            //    .ReceiveJson<VehicleApiModel>();

    //            //Log.Verbose("VehicleService.CreateAsync: Request sent successfully, received {@CreateVehicle}", createdVehicle);

    //            //var localVehicleIds = await _localStorage.GetVehicleIds();
    //            //localVehicleIds.Add(createdVehicle.Id);
    //            //_localStorage.SaveVehicleIds(localVehicleIds);
    //            //Log.Verbose("VehicleService.CreateAsync: Vehicle ids {@VehicleIds} saved to local storage", localVehicleIds);

    //            //return createdVehicle;
    //        }
    //        catch (FlurlHttpTimeoutException e1)
    //        {
    //            Log.Warning("VehicleService.CreateAsync: Timeout when calling API");
    //            throw new IOException("Timeout when calling API", e1);
    //        }
    //        catch (FlurlHttpException e2)
    //        {
    //            string errorMessage;
    //            if (e2.Call.Response != null)
    //                errorMessage = "Failed with response code " + e2.Call.Response.StatusCode;
    //            else
    //                errorMessage = "Totally failed before getting a response! " + e2.Message;

    //            Log.Warning("VehicleService.CreateAsync: " + errorMessage);
    //            throw new IOException("Error when calling API", e2);
    //        }
    //    }

    //    public async Task<Vehicle> GetByIdAsync(string vehicleId)
    //    {
    //        return (await GetAllAsync()).FirstOrDefault(v => v.Id == vehicleId);
    //    }

    //    public void AddRefuelingAsync(string vehicleId, DateTime date, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
}
