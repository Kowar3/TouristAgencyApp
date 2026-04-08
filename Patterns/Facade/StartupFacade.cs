
using System;
using System.IO;
using TouristAgencyApp.Services;
using TouristAgencyApp.Forms;

namespace TouristAgencyApp.Patterns
{
    public class StartupFacade
    {
        public class ConfigItem
        {
            public string DisplayName { get; set; } = "";
            public string FullPath { get; set; } = "";
            public override string ToString() => DisplayName;
        }

       
        public ConfigItem[] GetConfigFiles(string baseDirectory)
        {
            var files = Directory.GetFiles(baseDirectory, "config*.txt");
            var items = new ConfigItem[files.Length];
            
            for (int i = 0; i < files.Length; i++)
            {
                items[i] = new ConfigItem 
                { 
                    DisplayName = Path.GetFileName(files[i]), 
                    FullPath = files[i] 
                };
            }
            
            return items;
        }

      
        public MainForm InitializeApp(string configPath)
        {
            AppSettings.Instance.Load(configPath);
            var db = DatabaseFactory.GetDatabaseService(AppSettings.Instance.ConnectionString);
            return new MainForm(db);
        }
    }
}