using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Patterns.Memento.PackageMemento
{
    public class MountainPackageMemento : TravelPackageMemento
    {
        public string Destination { get; set; } = "";
        public string Accommodation { get; set; } = "";
        public string Transport { get; set; } = "";
        public string Activities { get; set; } = "";
        public MountainPackageMemento(int id, string name, decimal price, string type, string destination, string accommodation, string transport, string activities,string details)
            : base(id, name, price, type, details,destination)
        {
            Destination = destination;
            Accommodation = accommodation;
            Transport = transport;
            Activities = activities;
            OriginatorType = typeof(MountainPackage);
        }
        public override TravelPackage GetState()
        {
            return GetStateMountainPackage();
        }

        public MountainPackage GetStateMountainPackage()
        {
            return new MountainPackage
            {
                Id = Id,
                Name = Name,
                Price = Price,
                Type = Type,
                Destination = Destination,
                Accommodation = Accommodation,
                Transport = Transport,
                Details = Details,
                Activities = Activities
            };
        }
    }
}
