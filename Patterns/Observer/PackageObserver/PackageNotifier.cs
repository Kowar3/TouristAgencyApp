using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouristAgencyApp.Patterns.Observer.PackageObserver
{
    public class PackageNotifier : IObserver
    {
        public void Update(string message)
        {
            Console.WriteLine($"NOTIFIKACIJA: {message}");
        }
    }
}
