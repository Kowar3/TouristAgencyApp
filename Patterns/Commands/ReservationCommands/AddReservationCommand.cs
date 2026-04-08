using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns.Memento;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns.Commands.ReservationCommands
{
    public class AddReservationCommand : ICommand
    {
        private readonly IDatabaseService _db;
        private readonly ReservationMemento _memento;
        private int _reservationId;
        private bool _executed;
        private bool _undone;
        private bool _redone;
        public int ReservationId => _reservationId;

        public AddReservationCommand(IDatabaseService db, Reservation reservation)
        {
            _db = db;
            _memento = reservation.CreateMemento();
        }

        public void Execute()
        {
            if (_executed) return;
            var reservation = new Reservation();
            reservation.Restore(_memento);
            int id = _db.AddReservation(reservation);
            _executed = true;
            _undone = false;
            _redone = false;
            _reservationId = id;
        }

        public void Undo()
        {
            if (!_executed || _undone)
                return;
            if (_reservationId > 0)
            {
                _db.RemoveReservation(_reservationId);
                _undone = true;
                _redone = false;
            }
        }
        public void Redo()
        {
            if (!_executed || !_undone || _redone)
                return;
            var reservation = new Reservation();
            reservation.Restore(_memento);
            _reservationId = _db.AddReservation(reservation);
            _undone = false;
            _redone = true;
        }
    }
}
