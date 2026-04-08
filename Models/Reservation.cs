using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Patterns.Memento;

namespace TouristAgencyApp.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int PackageId { get; set; }
        public int NumPersons { get; set; }
        public DateTime ReservationDate { get; set; }
        public string ExtraServices { get; set; } = "";
        public string PackageName { get; set; } = "";
        public string Destination { get; set; } = "";
        public ReservationMemento CreateMemento()
        {
            return new ReservationMemento(Id, ClientId, PackageId, NumPersons, ReservationDate, ExtraServices, PackageName);
        }
        public void Restore(ReservationMemento memento)
        {
            Id = memento.Id;
            ClientId = memento.ClientId;
            PackageId = memento.PackageId;
            NumPersons = memento.NumPersons;
            ReservationDate = memento.ReservationDate;
            ExtraServices = memento.ExtraServices;
            PackageName = memento.PackageName;
        }
    }
}
