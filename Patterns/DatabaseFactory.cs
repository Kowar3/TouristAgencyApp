using Microsoft.VisualBasic.Devices;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouristAgencyApp.Services;

namespace TouristAgencyApp.Patterns
{
    public static class DatabaseFactory
    {
        private static readonly ConcurrentDictionary<string, IDatabaseService> _instances = new();
        private static readonly object _lock = new();

        public static IDatabaseService GetDatabaseService(string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
                throw new ArgumentException("Prazan connection string.", nameof(connectionString));

            var key = connectionString.Trim().ToLowerInvariant();

            if (_instances.TryGetValue(key, out var existingInstance))
                return existingInstance;

            lock (_lock)
            {
                if (_instances.TryGetValue(key, out existingInstance))
                {
                    return existingInstance;
                }

                IDatabaseService dbService;

                if (key.Contains("data source"))
                {
                    dbService = new SQLiteDatabaseService(connectionString);
                }
                else if (key.Contains("server") || key.Contains("uid") || key.Contains("user id") || key.Contains("mysql"))
                {
                    dbService = new MySQLDatabaseService(connectionString);
                }
                else
                {
                    throw new InvalidOperationException("Nepoznata baza (connection string ne prepoznatljiv).");
                }

                _instances[key] = dbService;

                return dbService;
            }
        }

        public static void ResetAll()
        {
            _instances.Clear();
        }
    }

}
