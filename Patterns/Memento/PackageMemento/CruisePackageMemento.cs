using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Patterns.Memento.PackageMemento
{
    public class CruisePackageMemento: TravelPackageMemento
    {
        public string Ship { get; set; } = "";
        public string Route { get; set; } = "";
        public DateTime DepartureDate { get; set; }
        public string CabinType { get; set; } = "";
        public string Destination { get; set; }
        public CruisePackageMemento(int id, string name, decimal price, string type, string details, string ship, string route, DateTime departureDate, string cabinType, string destination)
            :base(id,name,price,type,details, destination)
        {
            Ship = ship;
            Route = route;
            DepartureDate = departureDate;
            CabinType = cabinType;
            Destination = destination;
            OriginatorType = typeof(CruisePackage);
        }
        public override TravelPackage GetState()
        {
            return GetStateCruisePackage();
        }
        public CruisePackage GetStateCruisePackage()
        {
            return new CruisePackage
            {
                Id = Id,
                Name = Name,
                Price = Price,
                Type = Type,
                Ship = Ship,
                Route = Route,
                DepartureDate = DepartureDate,
                CabinType = CabinType,
                Destination =Destination
            };
        }
    }
}
