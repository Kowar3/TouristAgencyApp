using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Patterns.Memento.PackageMemento
{
    public class SeaPackageMemento : TravelPackageMemento
    {
        public string Destination { get; }
        public string Accommodation { get; }
        public string Transport { get; }

        public SeaPackageMemento(int id, string name, decimal price, string type,string destination, string accommodation, string transport, string details)
            : base(id, name, price, type,details,destination)
        {
            Destination = destination;
            Accommodation = accommodation;
            Transport = transport;
            OriginatorType = typeof(SeaPackage);
        }

        public override TravelPackage GetState()
        {
            return GetStateSeaPackage();
        }

        public SeaPackage GetStateSeaPackage()
        {
            return new SeaPackage
            {
                Id = Id,
                Name = Name,
                Price = Price,
                Type = Type,
                Destination = Destination,
                Accommodation = Accommodation,
                Transport = Transport,
                Details = Details
            };
        }

    }
}
