using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Branslekollen.Core.Domain.Models;
using Serilog;

namespace Branslekollen.Core.Services
{
    public class DummyVehicleService : IVehicleService
    {
        private readonly List<Vehicle> _vehicles = new List<Vehicle>();

        public DummyVehicleService()
        {
            //return;
            _vehicles.Add(
                new Vehicle("01D16512-9B17-4956-A9AC-266C264234A9", "Volvo V90", FuelType.Petrol)
                {
                    Refuelings = new List<Refueling>
                    {
                        new Refueling
                        {
                            Id = Guid.NewGuid().ToString(),
                            CreationTimeUtc = DateTime.Parse("2017-01-15 21:39"),
                            RefuelingDate = DateTime.Parse("2017-01-14"),
                            FullTank = true,
                            MissedRefuelings = false,
                            NumberOfLiters = 20,
                            OdometerInKm = 1000,
                            PricePerLiter = 13.37
                        },
                        new Refueling
                        {
                            Id = Guid.NewGuid().ToString(),
                            CreationTimeUtc = DateTime.Parse("2017-02-23 06:54"),
                            RefuelingDate = DateTime.Parse("2017-02-22"),
                            FullTank = true,
                            MissedRefuelings = false,
                            NumberOfLiters = 25,
                            OdometerInKm = 1500,
                            PricePerLiter = 14.01
                        }
                    }
                });
        }

        public Task<List<Vehicle>> GetAll()
        {
            Log.Verbose("DummyVehicleService.GetAll...");
            Task.Delay(1000);
            return Task.FromResult(_vehicles);
        }

        public Task<Vehicle> Create(string name = "", FuelType fuelType = FuelType.Unknown)
        {
            Log.Verbose("DummyVehicleService.Create...");
            var vehicle = new Vehicle(Guid.NewGuid().ToString(), name, fuelType);
            _vehicles.Add(vehicle);
            return Task.FromResult(vehicle);
        }

        public Task<Vehicle> GetById(string vehicleId)
        {
            return Task.FromResult(_vehicles.FirstOrDefault(v => v.Id == vehicleId));
        }

        public void AddRefueling(string vehicleid, DateTime date, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank)
        {
            var vehicle = _vehicles.Single(v => v.Id == vehicleid);
            vehicle.Refuelings.Add(new Refueling
            {
                Id = Guid.NewGuid().ToString(),
                CreationTimeUtc = DateTime.UtcNow,
                RefuelingDate = date,
                PricePerLiter = pricePerLiter,
                NumberOfLiters = numberOfLiters,
                OdometerInKm = odometerInKm,
                FullTank = fullTank
            });
            vehicle.Refuelings.Sort((r1, r2) => r1.RefuelingDate.CompareTo(r2.RefuelingDate));
        }

        public void UpdateRefueling(string vehicleId, string refuelingId, DateTime refuelDate, double pricePerLiter, double volumeInLiters, int odometerInKm, bool fullTank)
        {
            var vehicle = _vehicles.Single(v => v.Id == vehicleId);
            var refueling = vehicle.Refuelings.Single(r => r.Id == refuelingId);
            refueling.RefuelingDate = refuelDate;
            refueling.PricePerLiter = pricePerLiter;
            refueling.NumberOfLiters = volumeInLiters;
            refueling.OdometerInKm = odometerInKm;
            refueling.FullTank = fullTank;
            vehicle.Refuelings.Sort((r1, r2) => r1.RefuelingDate.CompareTo(r2.RefuelingDate));
        }

        public void DeleteRefueling(string vehicleId, string refuelingId)
        {
            var vehicle = _vehicles.Single(v => v.Id == vehicleId);
            vehicle.Refuelings.RemoveAll(r => r.Id == refuelingId);
            vehicle.Refuelings.Sort((r1, r2) => r1.RefuelingDate.CompareTo(r2.RefuelingDate));
        }

        public Task<Vehicle> GetLastUsedOrCreateNew()
        {
            if (_vehicles.Any())
            {
                Log.Verbose("DummyVehicleService.GetLastUsedOrCreateNew: Using first vehicle with id {VehicleId}", _vehicles.First().Id);
                return Task.FromResult(_vehicles.First());
            }
            else
            {
                Log.Verbose("DummyVehicleService.GetLastUsedOrCreateNew: No vehicles found, creating new vehicle");
                return Create();
            }
        }
    }
}