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

        public async Task<List<Vehicle>> GetAll()
        {
            var json = await _localStorage.ReadJson(STORAGE_KEY);
            return string.IsNullOrWhiteSpace(json) 
                ? new List<Vehicle>() 
                : JsonConvert.DeserializeObject<List<Vehicle>>(json);
        }

        public async Task<Vehicle> Create(string name = "", FuelType fuelType = FuelType.Unknown)
        {
            var vehicle = new Vehicle(Guid.NewGuid().ToString(), name, fuelType);
            var existingVehicles = await GetAll();
            existingVehicles.Add(vehicle);
            _localStorage.WriteJson(STORAGE_KEY, JsonConvert.SerializeObject(existingVehicles));
            return vehicle;
        }

        public async Task<Vehicle> GetById(string vehicleId)
        {
            return (await GetAll()).SingleOrDefault(v => v.Id == vehicleId);
        }

        public async Task DeleteAll()
        {
            await _localStorage.WriteJson(STORAGE_KEY, string.Empty);
        }


        public async Task AddRefueling(string vehicleId, DateTime refuelDate, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank)
        {
            var existingVehicles = await GetAll();
            var matchingVehicle = existingVehicles.SingleOrDefault(v => v.Id == vehicleId);
            if (matchingVehicle == null)
            {
                Log.Warning("LocalVehicleService.AddRefueling: Can't find any vehicle with id {VehicleId}", vehicleId);
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
            await _localStorage.WriteJson(STORAGE_KEY, JsonConvert.SerializeObject(existingVehicles));
        }

        public async Task UpdateRefueling(string vehicleId, string refuelingId, DateTime refuelDate, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank)
        {
            var existingVehicles = await GetAll();
            var matchingVehicle = existingVehicles.SingleOrDefault(v => v.Id == vehicleId);
            if (matchingVehicle == null)
            {
                Log.Warning("LocalVehicleService.UpdateRefueling: Can't find any vehicle with id {VehicleId}", vehicleId);
                throw new ArgumentException($"No vehicle with id {vehicleId} found", nameof(vehicleId));
            }
            var matchingRefueling = matchingVehicle.Refuelings.SingleOrDefault(r => r.Id == refuelingId);
            if (matchingRefueling == null)
            {
                Log.Warning("LocalVehicleService.UpdateRefueling: Can't find any refueling with id {RefuelingId}", refuelingId);
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

            await _localStorage.WriteJson(STORAGE_KEY, JsonConvert.SerializeObject(existingVehicles));
        }

        public async Task DeleteRefueling(string vehicleId, string refuelingId)
        {
            var existingVehicles = await GetAll();
            var matchingVehicle = existingVehicles.SingleOrDefault(v => v.Id == vehicleId);
            if (matchingVehicle == null)
            {
                Log.Warning("LocalVehicleService.DeleteRefueling: Can't find any vehicle with id {VehicleId}", vehicleId);
                throw new ArgumentException($"No vehicle with id {vehicleId} found", nameof(vehicleId));
            }
            var matchingRefueling = matchingVehicle.Refuelings.SingleOrDefault(r => r.Id == refuelingId);
            if (matchingRefueling == null)
            {
                Log.Warning("LocalVehicleService.DeleteRefueling: Can't find any refueling with id {RefuelingId}", refuelingId);
                throw new ArgumentException($"No refueling with id {refuelingId} found", nameof(refuelingId));
            }

            matchingVehicle.Refuelings.Remove(matchingRefueling);
            await _localStorage.WriteJson(STORAGE_KEY, JsonConvert.SerializeObject(existingVehicles));
        }

        public async Task<Vehicle> GetLastUsed()
        {
            return (await GetAll()).FirstOrDefault();
        }
    }
}