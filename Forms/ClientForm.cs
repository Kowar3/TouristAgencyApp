using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Patterns.Observer.ClientObserver;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Forms
{
    public partial class ClientsForm : Form
    {

        private readonly ClientFacade _clientFacade;

        public ClientsForm(IDatabaseService dbService)
        {
            _clientFacade = new ClientFacade(dbService);
            InitializeComponent();
            LoadClients();
        }
       
        private void OpozoviAkciju()
        {
            _clientFacade.UndoLastAction();
            LoadClients();
        }

        private void NapredAkcija()
        {
            _clientFacade.RedoLastAction();
            LoadClients();
        }
    }
}