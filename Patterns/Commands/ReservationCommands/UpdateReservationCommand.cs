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
    public class UpdateReservationCommand : ICommand
    {
        private readonly IDatabaseService _db;
        private readonly Reservation _reservation;
        private readonly Reservation _updated;
        private readonly ReservationMemento _beforeMemento;
        private readonly ReservationMemento _afterMemento;
        private bool _executed;
        private bool _undone;
        private bool _redone;
        public UpdateReservationCommand(IDatabaseService db, int id, int numPerson, string extra)
        {
            _db = db;
            var current = _db.GetReservationById(id);
            _beforeMemento = current.CreateMemento();
            var updated = new Reservation
            {
                Id = id,
                ClientId = current.ClientId,
                PackageId = current.PackageId,
                ReservationDate = current.ReservationDate,
                PackageName = current.PackageName,
                NumPersons = numPerson,
                ExtraServices = extra
            };
            _afterMemento = updated.CreateMemento();
        }

        public void Execute()
        {
            if (_executed) return;
            var updated = new Reservation();
            updated.Restore(_afterMemento);
            _db.UpdateReservation(updated.Id, updated.NumPersons, updated.ExtraServices);
            _executed = true;
            _undone = false;
            _redone = false;
        }

        public void Undo()
        {
            if (!_executed || _undone)
                return;
            var before = new Reservation();
            before.Restore(_beforeMemento);

            _db.UpdateReservation(before.Id, before.NumPersons, before.ExtraServices);
            _undone = true;
            _redone = false;
        }
        public void Redo()
        {
            if (!_executed || !_undone || _redone)
                return;
            var after = new Reservation();
            after.Restore(_afterMemento); 
            _db.UpdateReservation(after.Id, after.NumPersons, after.ExtraServices);
            _undone = false;
            _redone = true;
        }
    }
}
