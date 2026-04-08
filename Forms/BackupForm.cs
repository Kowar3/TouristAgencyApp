using System;
using System.Windows.Forms;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Patterns.Facade;

namespace TouristAgencyApp.Forms
{
    public partial class BackupForm : Form
    {
        BackupFacade _backupFacade;
        public BackupForm()
        {
            InitializeComponent();
        }

        private void btnBackup_Click(object sender, EventArgs e)
        {
            string dbFile = "agencija.db";
            _backupFacade = new BackupFacade(dbFile);
            _backupFacade.CreateBackup();
            MessageBox.Show("Backup kreiran!");
        }
    }
}
