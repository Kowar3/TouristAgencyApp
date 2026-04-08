using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouristAgencyApp.Patterns
{
    public interface IBackupService
    {
        void CreateBackup();
    }

    public class BackupService : IBackupService
    {
        private readonly string _dbPath;
        public BackupService(string dbPath) { _dbPath = dbPath; }
        public void CreateBackup()
        {
            string backupPath = _dbPath + ".backup_" + DateTime.Now.ToString("yyyyMMdd_HHmmss");
            File.Copy(_dbPath, backupPath);
        }
    }

    public class LoggingBackupService : IBackupService
    {
        private readonly IBackupService _inner;
        public LoggingBackupService(IBackupService inner) { _inner = inner; }
        public void CreateBackup()
        {
            _inner.CreateBackup();
            Console.WriteLine("Backup created at " + DateTime.Now);
        }
    }
}
