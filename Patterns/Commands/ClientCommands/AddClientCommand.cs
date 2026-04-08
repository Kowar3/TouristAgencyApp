using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns.Memento;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns.Commands.ClientCommands
{
    public class AddClientCommand : ICommand
    {
        private readonly IDatabaseService _db;
  
        private readonly ClientMemento _memento;
        private int _clientId;
        private bool _executed;
        private bool _undone;
        private bool _redone;
        public int ClientId => _clientId;

        public AddClientCommand(IDatabaseService db, Client client)
        {
            _db = db;
            _memento = client.CreateMemento();
        }

        public void Execute()
        {
            if (_executed) return;
            var client = new Client();
            client.Restore(_memento);
            int id = _db.AddClient(client);
            _executed = true;
            _undone = false;
            _redone = false;
            _clientId = id;
        }

        public void Undo()
        {
            if (!_executed || _undone) return;
            if (_clientId > 0) {
                _db.RemoveClient(_clientId);
                _undone = true;
                _redone = false;
            }
                

        }
        public void Redo()
        {
            if (!_executed || !_undone || _redone) return;
            var client = new Client();
            client.Restore(_memento);
            int id=_db.AddClient(client);
            _clientId = id;
            _undone = false;
            _redone = true;
        }
    }
}
