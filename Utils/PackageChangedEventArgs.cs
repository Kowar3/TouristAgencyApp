using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Utils
{
    public class PackageChangedEventArgs : EventArgs
    {
        public TravelPackage Package { get; }
        public string Action { get; }
        public int Id { get; set; }
        public PackageChangedEventArgs(TravelPackage package, string action)
        {
            Package = package;
            Action = action;
        }
    }
}
