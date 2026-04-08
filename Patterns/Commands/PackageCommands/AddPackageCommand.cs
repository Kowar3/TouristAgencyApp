using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns.Memento.PackageMemento;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns.Commands.PackageCommands
{
    public class AddPackageCommand : ICommand
    {
        private readonly IDatabaseService _db;
        private readonly TravelPackage _package;
        private readonly TravelPackageMemento _memento;
        private int _packageId;
        private bool _executed;
        private bool _undone;
        private bool _redone;
        public int PackageId => _packageId;

        public AddPackageCommand(IDatabaseService db, TravelPackage package)
        {
            _db = db;
            _memento = package.CreateMemento();

        }

        public void Execute()
        {
            if (_executed) return;
            TravelPackage pk = (TravelPackage)Activator.CreateInstance(_memento.OriginatorType);
            pk.Restore(_memento);
            int id = _db.AddPackage(pk);
            _executed = true;
            _undone = false;
            _redone = false;
            _packageId = id;
        }

        public void Undo()
        {
            if (!_executed || _undone)
                return;
            if (_packageId > 0)
            {
                _undone = true;
                _redone = false;
                _db.RemovePackage(_packageId);
            }
                
        }
        public void Redo()
        {
            if (!_executed || !_undone || _redone)
                return;
            TravelPackage pk = (TravelPackage)Activator.CreateInstance(_memento.OriginatorType);
            pk.Restore(_memento);
            _packageId = _db.AddPackage(pk);
            _undone = false;
            _redone = true;
        }
    }
}
