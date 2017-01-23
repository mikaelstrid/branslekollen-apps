//using System;
//using Branslekollen.Core.Domain.Models;

//namespace Branslekollen.Core.ApiModels
//{
//    public class RefuelingApiModel
//    {
//        public string Id { get; set; }
//        public DateTime CreationTimeUtc { get; set; }
//        public DateTime RefuelingDate { get; set; }
//        public bool MissedRefuelings { get; set; }
//        public double NumberOfLiters { get; set; }
//        public double PricePerLiter { get; set; }
//        public int OdometerInKm { get; set; }
//        //public int? DistanceTravelledInKm { get; set; }
//        public bool FullTank { get; set; }

//        public Refueling ToDomainModel()
//        {
//            return new Refueling
//            {
//                Id = Id,
//                CreationTimeUtc = CreationTimeUtc,
//                RefuelingDate = RefuelingDate,
//                MissedRefuelings = MissedRefuelings,
//                NumberOfLiters = NumberOfLiters,
//                PricePerLiter = PricePerLiter,
//                OdometerInKm = OdometerInKm,
//                //DistanceTravelledInKm = DistanceTravelledInKm,
//                FullTank = FullTank
//            };
//        }
//    }
//}