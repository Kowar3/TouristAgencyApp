using Google.Protobuf.WellKnownTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Services;
using TouristAgencyApp.Patterns.Commands;
using TouristAgencyApp.Patterns.Commands.ReservationCommands;
using TouristAgencyApp.Utils;

namespace TouristAgencyApp.Patterns
{
    public class ReservationManager
    {
        private readonly CommandInvoker _invoker = new();
        private readonly IDatabaseService _dbService;
        public event EventHandler<ReservationChangedEventArgs>? ReservationChanged; 
        public ReservationManager(IDatabaseService dbService)
        {
            _dbService = dbService;
        }
        protected virtual void OnPackageChanged(Reservation reservation, string action, int id)
        {
            var eventArgs = new ReservationChangedEventArgs(reservation, action);
            eventArgs.Id = id;
            ReservationChanged?.Invoke(this, eventArgs);
        }
        public int AddReservation(Reservation reservation)
        {
            var addCommand = new AddReservationCommand(_dbService, reservation);
            _invoker.ExecuteCommand(addCommand);
            OnPackageChanged(reservation, "Added", addCommand.ReservationId);
            return addCommand.ReservationId;
        }
        public void UpdateReservation(int reservationId, int numPersons, string txtExtra)
        {
            var updateCommand = new UpdateReservationCommand(_dbService, reservationId, numPersons, txtExtra);
            _invoker.ExecuteCommand(updateCommand);
            OnPackageChanged(new Reservation { Id=reservationId}, "Updated", reservationId);
        }
        public void RemoveReservation(int reservationId)
        {
            var removeCommand = new RemoveReservationCommand(_dbService, reservationId);
            _invoker.ExecuteCommand(removeCommand);
            OnPackageChanged(new Reservation { Id = reservationId }, "Removed", reservationId);
        }
        public void UndoLastAction()
        {
            _invoker.UndoLastCommand();
        }
        public void RedoLastAction()
        {
            _invoker.RedoLastAction();
        }
    }
}
