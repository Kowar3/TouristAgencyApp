using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using TouristAgencyApp.Services;
using TouristAgencyApp.Patterns.Facade;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Forms;

namespace TouristAgencyApp.Forms
{
    public partial class MainForm : Form
    {
        private readonly IDatabaseService _dbService;
        private readonly string _agencyName;
        private readonly BackupFacade _backupFacade;
        private System.Windows.Forms.Timer backupTimer;

        public MainForm(IDatabaseService dbService)
        {
            _dbService = dbService;
            _backupFacade = new BackupFacade("agencija.db");
            InitializeComponent();
            SetupBackupTimer();
        }

        private void SetupBackupTimer()
        {
            backupTimer = new System.Windows.Forms.Timer();
            backupTimer.Interval = 24 * 60 * 60 * 1000;
            backupTimer.Tick += (s, e) => NapraviBackup();
            backupTimer.Start();
        }

        private void NapraviBackup(bool showMsg = false)
        {
            _backupFacade.CreateBackup(showMsg);
        }
    }
}
