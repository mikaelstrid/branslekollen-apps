//using System.Collections.Generic;
//using System.Linq;
//using Branslekollen.Core.Domain.Models;

//namespace Branslekollen.Core.ApiModels
//{
//    public class VehicleApiModel
//    {
//        public string Id { get; set; }
//        public string Name { get; set; }
//        public string Fuel { get; set; }
//        public List<RefuelingApiModel> Refuelings { get; set; } = new List<RefuelingApiModel>();

//        public Vehicle ToDomainModel()
//        {
//            return new Vehicle
//            {
//                Id = Id,
//                Name = Name,
//                FuelType = Fuel,
//                Refuelings = Refuelings.Select(r => r.ToDomainModel()).ToList()
//            };
//        }
//    }
//}