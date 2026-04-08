namespace TouristAgencyApp.Forms
{
    partial class BackupForm
    {
        private System.Windows.Forms.Button btnBackup;

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = "Backup";
            this.ClientSize = new System.Drawing.Size(300, 100);
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;

            this.btnBackup = new System.Windows.Forms.Button();
            this.btnBackup.Text = "Backup baze";
            this.btnBackup.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnBackup.Click += new System.EventHandler(this.btnBackup_Click);

            this.Controls.Add(this.btnBackup);

            this.ResumeLayout(false);
        }
    }
}
