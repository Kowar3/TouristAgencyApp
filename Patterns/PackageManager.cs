using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns.Commands;
using TouristAgencyApp.Patterns.Commands.PackageCommands;
using TouristAgencyApp.Services;
using TouristAgencyApp.Utils;

namespace TouristAgencyApp.Patterns
{
    public class PackageManager
    {
        private readonly CommandInvoker _invoker = new();
        private readonly IDatabaseService _dbService;
        public event EventHandler<PackageChangedEventArgs>? PackageChanged;
        public PackageManager(IDatabaseService dbService)
        {
            _dbService = dbService;
        }
        protected virtual void OnPackageChanged(TravelPackage package, string action, int id)
        {
            var eventArgs = new PackageChangedEventArgs(package, action);
            eventArgs.Id = id;
            PackageChanged?.Invoke(this, eventArgs);
        }
        public int AddPackage(TravelPackage package)
        {
            var addCommand = new AddPackageCommand(_dbService, package);
            _invoker.ExecuteCommand(addCommand);
            OnPackageChanged(package, "Added", addCommand.PackageId);
            return addCommand.PackageId;
        }
        public void UpdatePackage(TravelPackage package)
        {
            var updateCommand = new UpdatePackageCommand(_dbService, package);
            _invoker.ExecuteCommand(updateCommand);
            OnPackageChanged(package, "Updated",0);
        }

        public void UndoLastAction()
        {
            _invoker.UndoLastCommand();
        }
        public void RedoLastAction()
        {
            _invoker.RedoLastAction();
        }
    }
}
