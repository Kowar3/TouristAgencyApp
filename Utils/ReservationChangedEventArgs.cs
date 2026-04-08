using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Utils
{
    public class ReservationChangedEventArgs: EventArgs
    {
        
        public Reservation Reservation { get; }
        public string Action { get; }
        public int Id { get; set; }
        public ReservationChangedEventArgs(Reservation reservation, string action)
        {
            Reservation = reservation;
            Action = action;
        }
        
    }
}
