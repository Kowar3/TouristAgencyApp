using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Utils;

namespace TouristAgencyApp.Patterns.Observer.ReservationObserver
{
    public class ReservationSubject : ISubject
    {
        private readonly List<IObserver> _observers = new();
        private readonly List<Reservation> _reservations = new();
        public void SubscribeToManager(ReservationManager manager)
        {
            manager.ReservationChanged += OnPackageChanged;
        }
        public void UnsubscribeToManager(ReservationManager manager)
        {
            manager.ReservationChanged -= OnPackageChanged;
        }
        private void OnPackageChanged(object? sender, ReservationChangedEventArgs e)
        {
            string message = e.Action switch
            {
                "Added" => $"Nova rezervacija dodata za paket: {e.Reservation.PackageName} | id: {e.Id}",
                "Updated" => $"Rezervacija sa id = {e.Reservation.Id} je azurirana",
                "Removed" => $"Rezervacija sa id ={e.Id} je obrisana",
                _ => "Nepoznata akcija"
            };

            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
        public void Attach(IObserver observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
        }

        public void Detach(IObserver observer)
        {
            _observers.Remove(observer);
        }

        public void Notify(string message)
        {
            foreach (var observer in _observers)
            {
                observer.Update(message);
            }
        }
    }
}
