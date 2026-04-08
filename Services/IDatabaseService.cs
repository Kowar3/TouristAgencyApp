using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Services
{
    public interface IDatabaseService
    {
        List<Client> GetAllClients();
        List<TravelPackage> GetAllPackages();
        List<Reservation> GetReservationsByClient(int clientId);
        int AddClient(Client client);
        void RemoveClient(int clientId);
        int AddPackage(TravelPackage package);
        int AddReservation(Reservation reservation);
        void RemoveReservation(int reservationId);
        void UpdateClient(Client client);
        void UpdatePackage(TravelPackage package);
        void RemovePackage(int packageId);
        void UpdateReservation(int reservationId, int numPersons, string extraInfo);
        Reservation? GetReservationById(int id);
        Client? GetClientById(int id);
    }
}
