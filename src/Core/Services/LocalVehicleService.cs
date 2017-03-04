using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Branslekollen.Core.Domain.Models;
using Branslekollen.Core.Persistence;
using Newtonsoft.Json;
using Serilog;

namespace Branslekollen.Core.Services
{
    public class LocalVehicleService : IVehicleService
    {
        private const string STORAGE_KEY = "vehicles";

        private readonly ILocalStorage _localStorage;

        public LocalVehicleService(ILocalStorage localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<List<Vehicle>> GetAllAsync()
        {
            var json = await _localStorage.ReadAsync(STORAGE_KEY);
            return string.IsNullOrWhiteSpace(json) 
                ? new List<Vehicle>() 
                : JsonConvert.DeserializeObject<List<Vehicle>>(json);
        }

        public async Task<Vehicle> CreateAsync(string name = "", FuelType fuelType = FuelType.Unknown)
        {
            var vehicle = new Vehicle(Guid.NewGuid().ToString(), name, fuelType);
            var existingVehicles = await GetAllAsync();
            existingVehicles.Add(vehicle);
            await _localStorage.WriteAsync(STORAGE_KEY, JsonConvert.SerializeObject(existingVehicles));
            return vehicle;
        }

        public async Task<Vehicle> GetByIdAsync(string vehicleId)
        {
            return (await GetAllAsync()).SingleOrDefault(v => v.Id == vehicleId);
        }

        public async Task DeleteAllAsync()
        {
            await _localStorage.WriteAsync(STORAGE_KEY, string.Empty);
        }


        public async Task AddRefuelingAsync(string vehicleId, DateTime refuelDate, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank)
        {
            var existingVehicles = await GetAllAsync();
            var matchingVehicle = existingVehicles.SingleOrDefault(v => v.Id == vehicleId);
            if (matchingVehicle == null)
            {
                Log.Warning("LocalVehicleService.AddRefuelingAsync: Can't find any vehicle with id {VehicleId}", vehicleId);
                throw new ArgumentException($"No vehicle with id {vehicleId} found", nameof(vehicleId));
            }
            matchingVehicle.Refuelings.Add(new Refueling
            {
                Id = Guid.NewGuid().ToString(),
                CreationTimeUtc = DateTime.UtcNow,
                RefuelingDate = refuelDate,
                PricePerLiter = pricePerLiter,
                NumberOfLiters = numberOfLiters,
                OdometerInKm = odometerInKm,
                FullTank = fullTank,
                MissedRefuelings = false
            });
            matchingVehicle.Refuelings.Sort((first, second) => first.RefuelingDate.CompareTo(second.RefuelingDate));
            await _localStorage.WriteAsync(STORAGE_KEY, JsonConvert.SerializeObject(existingVehicles));
        }

        public async Task UpdateRefuelingAsync(string vehicleId, string refuelingId, DateTime refuelDate, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank)
        {
            var existingVehicles = await GetAllAsync();
            var matchingVehicle = existingVehicles.SingleOrDefault(v => v.Id == vehicleId);
            if (matchingVehicle == null)
            {
                Log.Warning("LocalVehicleService.UpdateRefuelingAsync: Can't find any vehicle with id {VehicleId}", vehicleId);
                throw new ArgumentException($"No vehicle with id {vehicleId} found", nameof(vehicleId));
            }
            var matchingRefueling = matchingVehicle.Refuelings.SingleOrDefault(r => r.Id == refuelingId);
            if (matchingRefueling == null)
            {
                Log.Warning("LocalVehicleService.UpdateRefuelingAsync: Can't find any refueling with id {RefuelingId}", refuelingId);
                throw new ArgumentException($"No refueling with id {refuelingId} found", nameof(refuelingId));
            }

            matchingRefueling.Id = Guid.NewGuid().ToString();
            matchingRefueling.CreationTimeUtc = DateTime.UtcNow;
            matchingRefueling.RefuelingDate = refuelDate;
            matchingRefueling.PricePerLiter = pricePerLiter;
            matchingRefueling.NumberOfLiters = numberOfLiters;
            matchingRefueling.OdometerInKm = odometerInKm;
            matchingRefueling.FullTank = fullTank;
            matchingRefueling.MissedRefuelings = false;

            matchingVehicle.Refuelings.Sort((first, second) => first.RefuelingDate.CompareTo(second.RefuelingDate));

            await _localStorage.WriteAsync(STORAGE_KEY, JsonConvert.SerializeObject(existingVehicles));
        }

        public async Task DeleteRefuelingAsync(string vehicleId, string refuelingId)
        {
            var existingVehicles = await GetAllAsync();
            var matchingVehicle = existingVehicles.SingleOrDefault(v => v.Id == vehicleId);
            if (matchingVehicle == null)
            {
                Log.Warning("LocalVehicleService.DeleteRefuelingAsync: Can't find any vehicle with id {VehicleId}", vehicleId);
                throw new ArgumentException($"No vehicle with id {vehicleId} found", nameof(vehicleId));
            }
            var matchingRefueling = matchingVehicle.Refuelings.SingleOrDefault(r => r.Id == refuelingId);
            if (matchingRefueling == null)
            {
                Log.Warning("LocalVehicleService.DeleteRefuelingAsync: Can't find any refueling with id {RefuelingId}", refuelingId);
                throw new ArgumentException($"No refueling with id {refuelingId} found", nameof(refuelingId));
            }

            matchingVehicle.Refuelings.Remove(matchingRefueling);
            await _localStorage.WriteAsync(STORAGE_KEY, JsonConvert.SerializeObject(existingVehicles));
        }

        public async Task<Vehicle> GetLastUsedAsync()
        {
            return (await GetAllAsync()).FirstOrDefault();
        }
    }
}