using System.Collections.Generic;

namespace Branslekollen.Core.ApiModels
{
    public class VehicleApiModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Fuel { get; set; }
        public List<RefuelingApiModel> Refuelings { get; set; } = new List<RefuelingApiModel>();
    }
}