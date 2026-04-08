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
    public class RemoveReservationCommand : ICommand
    {
        private readonly IDatabaseService _db;
        private readonly ReservationMemento _memento;
        private int _reservationId;
        private bool _executed;
        private bool _undone;
        private bool _redone;

        public RemoveReservationCommand(IDatabaseService db, int reservationId)
        {
            _db = db;
            _reservationId = reservationId;
            
            var res = _db.GetReservationById(reservationId);
            _memento = res.CreateMemento();
        }

        public void Execute()
        {
            if (_executed) return;
            _db.RemoveReservation(_reservationId);
            _executed = true;
            _undone = false;
            _redone = false;
        }

        public void Undo()
        {
            if (!_executed || _undone)
                return;
            var reservation = new Reservation();
            reservation.Restore(_memento);
            _reservationId = _db.AddReservation(reservation);
            _undone = true;
            _redone = false;
        }
        public void Redo()
        {
            if (!_executed || !_undone || _redone)
                return;
            _db.RemoveReservation(_reservationId);
            _undone = false;
            _redone = true;
        }
    }
}
