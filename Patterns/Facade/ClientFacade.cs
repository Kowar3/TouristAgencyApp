using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns.Observer.ClientObserver;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns
{
    public class ClientFacade
    {
        private readonly ClientManager _clientManager;
        private readonly ClientSubject _clientSubject;
        private readonly IDatabaseService _dbService;

        public ClientFacade(IDatabaseService dbService)
        {
            _dbService = dbService;
            _clientManager = new ClientManager(dbService);
            _clientSubject = new ClientSubject();
            _clientSubject.SubscribeToManager(_clientManager);
            
            _clientSubject.Attach(new ClientLogger());
            _clientSubject.Attach(new ClientNotifier());
        }

        public List<Client> GetAllClients() => _dbService.GetAllClients().ToList();

        public void AddClient(Client client)
        {
            int id = _clientManager.AddClient(client);
        }

        public void UpdateClient(Client client)
        {
            _clientManager.updateClient(client);
        }

        public void RemoveClient(int clientId)
        {
            _clientManager.RemoveClient(clientId);
        }

        public void UndoLastAction()
        {
            _clientManager.UndoLastAction();
        }

        public void RedoLastAction()
        {
            _clientManager.RedoLastAction();
        }

        public List<Reservation> GetClientReservations(int clientId) => 
            _dbService.GetReservationsByClient(clientId);

        public Client? GetClientById(int id) => _dbService.GetClientById(id);

        public string? GetPackageName(int packageId)
        {
            return _dbService.GetAllPackages()
                   .FirstOrDefault(p => p.Id == packageId)?
                   .Name;
        }
    }
}

