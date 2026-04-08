using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouristAgencyApp.Patterns
{
    public sealed class AppSettings
    {
        private static readonly Lazy<AppSettings> _inst =
            new(() => new AppSettings());

        public static AppSettings Instance => _inst.Value;

        public string AgencyName { get; private set; } = "Tourist Agency";
        public string ConnectionString { get; private set; } = "";

        private AppSettings() { }

        public void Load(string path = "config.txt")
        {
            if (!File.Exists(path))
                throw new FileNotFoundException("Nije pronađen config.txt", path);

            var lines = File.ReadAllLines(path);
            if (lines.Length < 2)
                throw new InvalidOperationException("config.txt mora imati bar 2 linije (naziv, konekcija).");

            AgencyName = lines[0].Trim();
            ConnectionString = lines[1].Trim();
        }
    }
}
