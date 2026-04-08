using System;
using System.Windows.Forms;
using TouristAgencyApp.Patterns;

namespace TouristAgencyApp.Forms
{
    public partial class StartupForm : Form
    {
        private readonly StartupFacade _facade;

        public StartupForm()
        {
            InitializeComponent();
            _facade = new StartupFacade();
            LoadConfigs();
        }

        private void LoadConfigs()
        {
            cmbConfigs.Items.Clear();
            var configs = _facade.GetConfigFiles(AppDomain.CurrentDomain.BaseDirectory);
            cmbConfigs.Items.AddRange(configs);
            
            if (cmbConfigs.Items.Count > 0)
                cmbConfigs.SelectedIndex = 0;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (cmbConfigs.SelectedItem is StartupFacade.ConfigItem selected)
            {
                try
                {
                    var mainForm = _facade.InitializeApp(selected.FullPath);
                    this.Hide();
                    mainForm.ShowDialog();
                    this.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Greška pri startu: {ex.Message}", "Greška",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("Izaberite validan config fajl.", "Upozorenje",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}