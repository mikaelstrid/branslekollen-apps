using System;
using System.Collections.Generic;

namespace Branslekollen.Core.Domain.Models
{
    public class Vehicle
    {
        public string Id { get; set; }
        public DateTime CreationTimeUtc { get; set; }
        public string Name { get; set; }
        public FuelType FuelType { get; set; }
        public List<Refueling> Refuelings { get; set; } = new List<Refueling>();

        public Vehicle() { }

        public Vehicle(string id, string name, FuelType fuelType)
        {
            Id = id;
            Name = name;
            FuelType = fuelType;
        }
    }
}
