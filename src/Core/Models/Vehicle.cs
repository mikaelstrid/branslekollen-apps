using System.Collections.Generic;

namespace Branslekollen.Core.Models
{
    public class Vehicle
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string FuelType { get; set; }
        public List<Refueling> Refuelings { get; set; } = new List<Refueling>();

        public Vehicle() { }

        public Vehicle(string name, string fuelType)
        {
            Name = name;
            FuelType = fuelType;
        }
    }
}
