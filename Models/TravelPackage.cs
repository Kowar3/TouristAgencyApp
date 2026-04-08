using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Patterns.Memento;
using TouristAgencyApp.Patterns.Memento.PackageMemento;

namespace TouristAgencyApp.Models
{
    public abstract class TravelPackage
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public decimal Price { get; set; }
        public string Type { get; set; } = "";
        public string Destination { get; set; } = "";
        public string Details { get; set; } = "";
        public abstract void Restore(TravelPackageMemento memento);
        public abstract TravelPackageMemento CreateMemento();
    }

    public class SeaPackage : TravelPackage
    {
        public string Accommodation { get; set; } = "";
        public string Transport { get; set; } = "";
        public override TravelPackageMemento CreateMemento()
        {
            return new SeaPackageMemento(Id, Name, Price, Type, Destination, Accommodation, Transport, Details);
        }

        public override void Restore(TravelPackageMemento memento)
        {
            if(memento is SeaPackageMemento mementoS)
            {
                Id = mementoS.Id;
                Name = mementoS.Name;
                Price = mementoS.Price;
                Type = mementoS.Type;
                Destination = mementoS.Destination;
                Accommodation = mementoS.Accommodation;
                Transport = mementoS.Transport;
                Details = mementoS.Details;
            }
            
        }
        public override string ToString()
        {
            return $"Smeštaj: {Accommodation}, Prevoz: {Transport}";
        }
    }

    public class MountainPackage : TravelPackage
    {
        public string Accommodation { get; set; } = "";
        public string Transport { get; set; } = "";
        public string Activities { get; set; } = "";
        public override TravelPackageMemento CreateMemento()
        {
            return new MountainPackageMemento(Id, Name, Price, Type, Destination, Accommodation, Transport, Activities,Details);
        }

        public override void Restore(TravelPackageMemento memento)
        {
            if(memento is MountainPackageMemento mementoM)
            {
                Id = memento.Id;
                Name = memento.Name;
                Price = memento.Price;
                Type = memento.Type;
                Destination = mementoM.Destination;
                Accommodation = mementoM.Accommodation;
                Transport = mementoM.Transport;
                Details = mementoM.Details;
                Activities = mementoM.Activities;
            }
            
        }
        public override string ToString()
        {
            return $"Smeštaj: {Accommodation}, Prevoz: {Transport}, Aktivnosti: {Activities}";
        }
    }

    public class ExcursionPackage : TravelPackage
    {
        public string Transport { get; set; } = "";
        public string Guide { get; set; } = "";
        public int Duration { get; set; }
        public override TravelPackageMemento CreateMemento()
        {
            return new ExcursionPackageMemento(Id, Name, Price, Type, Destination, Details, Transport, Guide, Duration);
        }

        public override void Restore(TravelPackageMemento memento)
        {
            if(memento is ExcursionPackageMemento mementoE)
            {
                Id = memento.Id;
                Name = memento.Name;
                Price = memento.Price;
                Type = memento.Type;
                Destination = mementoE.Destination;
                Guide = mementoE.Guide;
                Transport = mementoE.Transport;
                Details = mementoE.Details;
                Duration = mementoE.Duration;
                Destination = mementoE.Destination;
            }
            
        }
        public override string ToString()
        {
            return $"Prevoz: {Transport}, Vodič: {Guide}, Trajanje: {Duration} dana";
        }
    }

    public class CruisePackage : TravelPackage
    {
        public string Ship { get; set; } = "";
        public string Route { get; set; } = "";
        public DateTime DepartureDate { get; set; }
        public string CabinType { get; set; } = "";
        public override string ToString()
        {
            return $"Brod: {Ship}, Ruta: {Route}, Polazak: {DepartureDate:dd.MM.yyyy}, Kabina: {CabinType}";
        }
        public override TravelPackageMemento CreateMemento()
        {
            return new CruisePackageMemento(Id, Name, Price, Type, Details, Ship, Route, DepartureDate, CabinType, Destination);
        }
        public override void Restore(TravelPackageMemento memento)
        {
            if(memento is CruisePackageMemento mementoC)
            {
                Id = memento.Id;
                Name = memento.Name;
                Price = memento.Price;
                Type = memento.Type;
                Ship = mementoC.Ship;
                Route = mementoC.Route;
                DepartureDate = mementoC.DepartureDate;
                CabinType = mementoC.CabinType;
                Destination = mementoC.Destination;
            }
            
        }
    }
}
