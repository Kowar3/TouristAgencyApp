using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Patterns.Memento.PackageMemento
{
    public abstract class TravelPackageMemento : IMemento<TravelPackage>
    {
        public int Id { get; }
        public string Name { get; }
        public decimal Price { get; }
        public string Type { get; }
        public string Details { get; }
        public Type OriginatorType { get; set; }
        protected TravelPackageMemento(int id, string name, decimal price, string type, string details, string destination  )
        {
            Id = id;
            Name = name;
            Price = price;
            Type = type;
            Details = details;
            
        }

        public abstract TravelPackage GetState();
    }
}
