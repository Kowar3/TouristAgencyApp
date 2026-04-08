using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Patterns.Memento;

namespace TouristAgencyApp.Models
{
    public class Client
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PassportNumber { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string FullName => $"{FirstName} {LastName}";
        public ClientMemento CreateMemento()
        {
            return new ClientMemento(Id, FirstName, LastName, PassportNumber, BirthDate, Email, Phone);
        }
        public void Restore(ClientMemento memento)
        {
            Id = memento.Id;
            FirstName = memento.FirstName;
            LastName = memento.LastName;
            PassportNumber = memento.PassportNumber;
            BirthDate = memento.BirthDate;
            Email = memento.Email;
            Phone = memento.Phone;
        }
    }
}
