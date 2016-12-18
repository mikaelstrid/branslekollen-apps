namespace Branslekollen.Core.Models
{
    public class Vehicle
    {
        public string Name { get; set; }
        public string FuelType { get; set; }

        public Vehicle(string name, string fuelType)
        {
            Name = name;
            FuelType = fuelType;
        }
    }
}
