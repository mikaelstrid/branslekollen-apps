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
                            OdometerInKm = 1756,
                            PricePerLiter = 13.37
                        },
                        new Refueling
                        {
                            Id = Guid.NewGuid().ToString(),
                            CreationTimeUtc = DateTime.Parse("2017-02-23 06:54"),
                            RefuelingDate = DateTime.Parse("2017-02-22"),
                            FullTank = true,
                            MissedRefuelings = false,
                            NumberOfLiters = 19.6,
                            OdometerInKm = 2540,
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

        public Task<Vehicle> Create(string name, FuelType fuelType)
        {
            Log.Verbose("DummyVehicleService.Create...");
            Task.Delay(1000);
            var vehicle = new Vehicle("0604C0A6-5D8E-4E7C-A5E2-1F3565F3BF67", name, fuelType);
            _vehicles.Add(vehicle);
            return Task.FromResult(vehicle);
        }

        public Task<Vehicle> GetById(string vehicleId)
        {
            return Task.FromResult(_vehicles.FirstOrDefault(v => v.Id == vehicleId));
        }

        public void AddRefueling(string vehicleid, DateTime date, double pricePerLiter, double numberOfLiters, int odometerInKm, bool fullTank)
        {
            _vehicles.First(v => v.Id == vehicleid).Refuelings.Add(new Refueling
            {
                Id = Guid.NewGuid().ToString(),
                CreationTimeUtc = DateTime.UtcNow,
                RefuelingDate = date,
                PricePerLiter = pricePerLiter,
                NumberOfLiters = numberOfLiters,
                OdometerInKm = odometerInKm,
                FullTank = fullTank
            });
        }
    }
}