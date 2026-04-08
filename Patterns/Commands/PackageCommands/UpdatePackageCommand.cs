using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns.Memento;
using TouristAgencyApp.Patterns.Memento.PackageMemento;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns.Commands.PackageCommands
{
    public class UpdatePackageCommand : ICommand
    {
        private readonly IDatabaseService _db;
        private readonly TravelPackageMemento _beforeMemento;
        private readonly TravelPackageMemento _afterMemento;
        private bool _executed;
        private bool _undone;
        private bool _redone;
        public UpdatePackageCommand(IDatabaseService db, TravelPackage package)
        {
            _db = db;
            _afterMemento = package.CreateMemento();
            List<TravelPackage> packages = _db.GetAllPackages();
            foreach (TravelPackage pkg in packages)
            {
                if (pkg.Id == package.Id)
                {
                    _beforeMemento = pkg.CreateMemento();
                    break;
                }
            }

        }
        public void Execute()
        {
            if (_executed) return;
            TravelPackage pk = (TravelPackage)Activator.CreateInstance(_afterMemento.OriginatorType);
            pk.Restore(_afterMemento);
            _db.UpdatePackage(pk);
            _executed = true;
            _undone = false;
            _redone = false;
        }

        public void Undo()
        {
            if (!_executed || _undone)
                return;
            TravelPackage pk = (TravelPackage)Activator.CreateInstance(_beforeMemento.OriginatorType);
            pk.Restore(_beforeMemento);
            _db.UpdatePackage(pk);
            _undone = true;
            _redone = false;
        }
        public void Redo()
        {
            if (!_executed || !_undone || _redone)
                return;
            TravelPackage pk = (TravelPackage)Activator.CreateInstance(_afterMemento.OriginatorType);
            pk.Restore(_afterMemento);
            _db.UpdatePackage(pk);
            _undone = false;
            _redone = true;
        }
    }
}
