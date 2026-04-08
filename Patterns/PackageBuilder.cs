using System;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Patterns
{
    public abstract class PackageBuilder
    {
        protected TravelPackage _package;

        public PackageBuilder()
        {
            _package = CreatePackage();
        }

        protected abstract TravelPackage CreatePackage();

        public PackageBuilder SetName(string name)
        {
            _package.Name = name;
            return this;
        }

        public PackageBuilder SetPrice(decimal price)
        {
            _package.Price = price;
            return this;
        }

        public abstract TravelPackage Build();
    }
    public class SeaPackageBuilder : PackageBuilder
    {
        protected override TravelPackage CreatePackage()
        {
            return new SeaPackage { Type = "Sea" };
        }

        public new SeaPackageBuilder SetName(string name)
        {
            base.SetName(name);
            return this;
        }

        public new SeaPackageBuilder SetPrice(decimal price)
        {
            base.SetPrice(price);
            return this;
        }

        public SeaPackageBuilder SetDestination(string destination)
        {
            if (_package is SeaPackage seaPackage)
            {
                seaPackage.Destination = destination;
            }
            return this;
        }

        public SeaPackageBuilder SetAccommodation(string accommodation)
        {
            if (_package is SeaPackage seaPackage)
            {
                seaPackage.Accommodation = accommodation;
            }
            return this;
        }

        public SeaPackageBuilder SetTransport(string transport)
        {
            if (_package is SeaPackage seaPackage)
            {
                seaPackage.Transport = transport;
            }
            return this;
        }

        public override TravelPackage Build()
        {
            return _package;
        }
    }
    public class MountainPackageBuilder : PackageBuilder
    {
        protected override TravelPackage CreatePackage()
        {
            return new MountainPackage { Type = "Mountain" };
        }

        public new MountainPackageBuilder SetName(string name)
        {
            base.SetName(name);
            return this;
        }

        public new MountainPackageBuilder SetPrice(decimal price)
        {
            base.SetPrice(price);
            return this;
        }

        public MountainPackageBuilder SetDestination(string destination)
        {
            if (_package is MountainPackage mountainPackage)
            {
                mountainPackage.Destination = destination;
            }
            return this;
        }

        public MountainPackageBuilder SetAccommodation(string accommodation)
        {
            if (_package is MountainPackage mountainPackage)
            {
                mountainPackage.Accommodation = accommodation;
            }
            return this;
        }

        public MountainPackageBuilder SetTransport(string transport)
        {
            if (_package is MountainPackage mountainPackage)
            {
                mountainPackage.Transport = transport;
            }
            return this;
        }

        public MountainPackageBuilder SetActivities(string activities)
        {
            if (_package is MountainPackage mountainPackage)
            {
                mountainPackage.Activities = activities;
            }
            return this;
        }

        public override TravelPackage Build()
        {
            return _package;
        }
    }
    public class ExcursionPackageBuilder : PackageBuilder
    {
        protected override TravelPackage CreatePackage()
        {
            return new ExcursionPackage { Type = "Excursion" };
        }

        public new ExcursionPackageBuilder SetName(string name)
        {
            base.SetName(name);
            return this;
        }

        public new ExcursionPackageBuilder SetPrice(decimal price)
        {
            base.SetPrice(price);
            return this;
        }

        public ExcursionPackageBuilder SetDestination(string destination)
        {
            if (_package is ExcursionPackage excursionPackage)
            {
                excursionPackage.Destination = destination;
            }
            return this;
        }

        public ExcursionPackageBuilder SetTransport(string transport)
        {
            if (_package is ExcursionPackage excursionPackage)
            {
                excursionPackage.Transport = transport;
            }
            return this;
        }

        public ExcursionPackageBuilder SetGuide(string guide)
        {
            if (_package is ExcursionPackage excursionPackage)
            {
                excursionPackage.Guide = guide;
            }
            return this;
        }

        public ExcursionPackageBuilder SetDuration(int duration)
        {
            if (_package is ExcursionPackage excursionPackage)
            {
                excursionPackage.Duration = duration;
            }
            return this;
        }

        public override TravelPackage Build()
        {
            return _package;
        }
    }
    public class CruisePackageBuilder : PackageBuilder
    {
        protected override TravelPackage CreatePackage()
        {
            return new CruisePackage { Type = "Cruise" };
        }

        public new CruisePackageBuilder SetName(string name)
        {
            base.SetName(name);
            return this;
        }

        public new CruisePackageBuilder SetPrice(decimal price)
        {
            base.SetPrice(price);
            return this;
        }

        public CruisePackageBuilder SetShip(string ship)
        {
            if (_package is CruisePackage cruisePackage)
            {
                cruisePackage.Ship = ship;
            }
            return this;
        }

        public CruisePackageBuilder SetRoute(string route)
        {
            if (_package is CruisePackage cruisePackage)
            {
                cruisePackage.Route = route;
            }
            return this;
        }

        public CruisePackageBuilder SetDepartureDate(DateTime departureDate)
        {
            if (_package is CruisePackage cruisePackage)
            {
                cruisePackage.DepartureDate = departureDate;
            }
            return this;
        }

        public CruisePackageBuilder SetCabinType(string cabinType)
        {
            if (_package is CruisePackage cruisePackage)
            {
                cruisePackage.CabinType = cabinType;
            }
            return this;
        }
        public CruisePackageBuilder SetDestination(string destination)
        {
            if (_package is CruisePackage cruisePackage)
            {
                cruisePackage.Destination = destination;
            }
            return this;
        }

        public override TravelPackage Build()
        {
            return _package;
        }
    }
    public static class PackageDirector
    {
        public static TravelPackage CreateSeaPackage(string name, decimal price, string destination, string accommodation, string transport)
        {
            return new SeaPackageBuilder()
                .SetName(name)
                .SetPrice(price)
                .SetDestination(destination)
                .SetAccommodation(accommodation)
                .SetTransport(transport)
                .Build();
        }

        public static TravelPackage CreateMountainPackage(string name, decimal price, string destination, string accommodation, string transport, string activities)
        {
            return new MountainPackageBuilder()
                .SetName(name)
                .SetPrice(price)
                .SetDestination(destination)
                .SetAccommodation(accommodation)
                .SetTransport(transport)
                .SetActivities(activities)
                .Build();
        }

        public static TravelPackage CreateExcursionPackage(string name, decimal price, string destination, string transport, string guide, int duration)
        {
            return new ExcursionPackageBuilder()
                .SetName(name)
                .SetPrice(price)
                .SetDestination(destination)
                .SetTransport(transport)
                .SetGuide(guide)
                .SetDuration(duration)
                .Build();
        }

        public static TravelPackage CreateCruisePackage(string name, decimal price, string ship, string route, DateTime departureDate, string cabinType, string destination)
        {
            destination = route.Split(',').Last().Trim();
            return new CruisePackageBuilder()
                .SetName(name)
                .SetPrice(price)
                .SetShip(ship)
                .SetRoute(route)
                .SetDepartureDate(departureDate)
                .SetCabinType(cabinType)
                .SetDestination(destination)
                .Build();
        }
        public static TravelPackage CreateSeaPackageForUpdate(int id, string name, decimal price, string destination, string accommodation, string transport)
        {
            var package = CreateSeaPackage(name, price, destination, accommodation, transport);
            package.Id = id;
            return package;
        }

        public static TravelPackage CreateMountainPackageForUpdate(int id, string name, decimal price, string destination, string accommodation, string transport, string activities)
        {
            var package = CreateMountainPackage(name, price, destination, accommodation, transport, activities);
            package.Id = id;
            return package;
        }

        public static TravelPackage CreateExcursionPackageForUpdate(int id, string name, decimal price, string destination, string transport, string guide, int duration)
        {
            var package = CreateExcursionPackage(name, price, destination, transport, guide, duration);
            package.Id = id;
            return package;
        }

        public static TravelPackage CreateCruisePackageForUpdate(int id, string name, decimal price, string ship, string route, DateTime departureDate, string cabinType, string destination)
        {
            destination = route.Split(',').Last().Trim();
            var package = CreateCruisePackage(name, price, ship, route, departureDate, cabinType, destination);
            package.Id = id;
            return package;
        }
    }
} 