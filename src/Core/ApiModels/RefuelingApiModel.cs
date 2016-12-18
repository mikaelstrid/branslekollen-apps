using System;

namespace Branslekollen.Core.ApiModels
{
    public class RefuelingApiModel
    {
        public string Id { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime Date { get; set; }
        public bool MissedRefuelings { get; set; }
        public double NumberOfLiters { get; set; }
        public double PricePerLiter { get; set; }
        public int OdometerInKm { get; set; }
        public int? DistanceTravelledInKm { get; set; }
        public bool FullTank { get; set; }
    }
}