using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace TouristAgencyApp.Patterns.Memento
{
    public class ClientMemento : IMemento<Client>
    {
        public int Id { get; set; }
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string PassportNumber { get; set; } = "";
        public DateTime BirthDate { get; set; }
        public string Email { get; set; } = "";
        public string Phone { get; set; } = "";
        public string FullName => $"{FirstName} {LastName}";
        public ClientMemento(int id, string firstName, string lastName, string passportNumber, DateTime birthDate, string email, string phone)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            PassportNumber = passportNumber;
            BirthDate = birthDate;
            Email = email;
            Phone = phone;
        }
        public Client GetState()
        {
            return new Client
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                PassportNumber = PassportNumber,
                BirthDate = BirthDate,
                Email = Email,
                Phone = Phone
            };
        }
    }
}
