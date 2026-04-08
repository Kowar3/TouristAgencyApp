using System.Collections.Generic;
using System.Linq;
using TouristAgencyApp.Models;
using TouristAgencyApp.Services;
using TouristAgencyApp.Patterns.Observer.PackageObserver;

namespace TouristAgencyApp.Patterns
{
    public class PackageFacade
    {
        private readonly IDatabaseService _dbService;
        private readonly PackageManager _manager;
        private readonly PackageSubject _subject;
        private List<TravelPackage> _cachedPackages;

        public PackageFacade(IDatabaseService dbService)
        {
            _dbService = dbService;
            _manager = new PackageManager(dbService);
            _subject = new PackageSubject();
            _subject.SubscribeToManager(_manager);
            _subject.Attach(new PackageLogger());
            _subject.Attach(new PackageNotifier());
            RefreshCache();
        }

        private void RefreshCache()
        {
            _cachedPackages = _dbService.GetAllPackages().ToList();
            foreach (var pkg in _cachedPackages)
                pkg.Details = pkg.ToString();
        }

        public List<TravelPackage> GetAllPackages() => _cachedPackages;

        public List<TravelPackage> GetPackagesByType(string type)
        {
            return type == "Svi paketi" 
                ? _cachedPackages.ToList() 
                : _cachedPackages.Where(p => p.Type == type).ToList();
        }

        public int AddPackage(TravelPackage package)
        {
            int id = _manager.AddPackage(package);
            RefreshCache();
            return id;
        }

        public void UpdatePackage(TravelPackage package)
        {
            _manager.UpdatePackage(package);
            RefreshCache();
        }

        public void Undo()
        {
            _manager.UndoLastAction();
            RefreshCache();
        }

        public void Redo()
        {
            _manager.RedoLastAction();
            RefreshCache();
        }
    }
}