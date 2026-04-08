using System.Collections.Generic;
using System.Linq;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns.Observer.ReservationObserver;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns
{
    public class ReservationFacade
    {
        private readonly ReservationManager _manager;
        private readonly IDatabaseService _db;
        private readonly ReservationSubject _subject;
        private List<TravelPackage> _cachedPackages;

        public ReservationFacade(IDatabaseService dbService)
        {
            _db = dbService;
            _manager = new ReservationManager(dbService);
            _subject = new ReservationSubject();
            _subject.Attach(new ReservationLogger());
            _subject.Attach(new ReservationNotifier());
            
            _subject.SubscribeToManager(_manager);
            _cachedPackages = _db.GetAllPackages(); 
        }

        public List<Reservation> GetReservationsByClient(int clientId)
        {
            var list = _db.GetReservationsByClient(clientId);
            
            foreach (var r in list)
            {
                var pkg = _cachedPackages.FirstOrDefault(p => p.Id == r.PackageId);
                r.PackageName = pkg?.Name ?? "(nepoznato)";
                r.Destination = pkg?.Destination ?? "(nepoznato)";
            }

            return list;
        }

        public List<TravelPackage> GetAllPackages() => _cachedPackages;

        public List<string> GetUniqueDestinations()
        {
            return _cachedPackages
                .Select(p => p.Destination)
                .Where(d => !string.IsNullOrWhiteSpace(d))
                .Distinct()
                .ToList();
        }

        public List<TravelPackage> GetPackagesByDestination(string destination)
        {
            return _cachedPackages
                .Where(p => p.Destination == destination)
                .ToList();
        }

        public int AddReservation(Reservation reservation)
        {
            int id = _manager.AddReservation(reservation);

            return id;
        }

        public void UpdateReservation(int reservationId, int numPersons, string extra)
        {
            _manager.UpdateReservation(reservationId, numPersons, extra);

        }

        public void RemoveReservation(int reservationId)
        {
            _manager.RemoveReservation(reservationId);

        }

        public void Undo() => _manager.UndoLastAction();

        public void Redo() => _manager.RedoLastAction();

        public List<Client> GetAllClients() => _db.GetAllClients();

        public void RefreshCache()
        {
            _cachedPackages = _db.GetAllPackages();
        }
    }
}