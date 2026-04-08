using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;

namespace TouristAgencyApp.Utils
{
    public class ClientChangedEventArgs: EventArgs
    {
        public Client Client { get; }
        public string Action { get; }
        public int Id { get; set; }
        public ClientChangedEventArgs(Client client, string action)
        {
            Client= client;
            Action = action;
        }
    }
}
