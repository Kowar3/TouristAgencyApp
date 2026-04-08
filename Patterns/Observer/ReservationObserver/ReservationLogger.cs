using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouristAgencyApp.Patterns.Observer.ReservationObserver
{
    public class ReservationLogger : IObserver
    {
        private readonly string _logFile;

        public ReservationLogger(string logFile = "reservations.log")
        {
            _logFile = logFile;
        }

        public void Update(string message)
        {
            var logEntry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss} - {message}";
            File.AppendAllText(_logFile, logEntry + Environment.NewLine);
        }
    }
}
