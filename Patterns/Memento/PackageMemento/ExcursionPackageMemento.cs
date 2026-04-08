using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Patterns.Memento.PackageMemento
{
    public class ExcursionPackageMemento : TravelPackageMemento
    {
        public string Destination { get; set; } = "";
        public string Transport { get; set; } = "";
        public string Guide { get; set; } = "";
        public int Duration { get; set; }
        public ExcursionPackageMemento(int id, string name, decimal price, string type, string destination, string details, string transport, string guide, int duration)
            : base(id, name, price, type, details, destination)
        {
            Destination = destination;
            Transport = transport;
            Guide = guide;
            Duration = duration;
            OriginatorType = typeof(ExcursionPackage);
        }
        public override TravelPackage GetState()
        {
            return GetStateExcursionPackage();
        }
        public ExcursionPackage GetStateExcursionPackage()
        {
            return new ExcursionPackage
            {
                Id = Id,
                Name = Name,
                Price = Price,
                Type = Type,
                Destination = Destination,
                Duration = Duration,
                Transport = Transport,
                Details = Details,
                Guide = Guide
            };
        }
    }
}
