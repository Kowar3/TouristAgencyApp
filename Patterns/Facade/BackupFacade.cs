using System;
using System.Windows.Forms;

namespace TouristAgencyApp.Patterns.Facade
{
    public class BackupFacade
    {
        private readonly IBackupService _backupService;

        public BackupFacade(string dbPath)
        {
            _backupService = new LoggingBackupService(new BackupService(dbPath));
        }

        public void CreateBackup(bool showMessage = true)
        {
            try
            {
                _backupService.CreateBackup();

                if (showMessage)
                    MessageBox.Show("Backup uspešno kreiran!", "Backup", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Greška pri kreiranju backup-a: {ex.Message}", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
