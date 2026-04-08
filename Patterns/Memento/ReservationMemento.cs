using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Patterns.Memento
{
    public class ReservationMemento : IMemento<Reservation>
    {
        public int Id { get; }
        public int ClientId { get; }
        public int PackageId { get; }
        public int NumPersons { get; }
        public DateTime ReservationDate { get; }
        public string ExtraServices { get; }
        public string PackageName { get; }
  
        public ReservationMemento(int id, int clientId, int packageId, int numPersons, DateTime reservationDate, string extraServices, string packageName)
        {
            Id = id;
            ClientId = clientId;
            PackageId = packageId;
            NumPersons = numPersons;
            ReservationDate = reservationDate;
            ExtraServices = extraServices;
            PackageName = packageName;
        }

        public Reservation GetState()
        {
            return new Reservation
            {
                Id = Id,
                ClientId = ClientId,
                PackageId = PackageId,
                NumPersons = NumPersons,
                ReservationDate = ReservationDate,
                ExtraServices = ExtraServices,
                PackageName = PackageName
            };
        }
    }
}
