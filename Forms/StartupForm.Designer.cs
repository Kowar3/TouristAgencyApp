namespace TouristAgencyApp.Forms
{
    partial class StartupForm
    {
        private System.ComponentModel.IContainer components = null;
        private ComboBox cmbConfigs;
        private Button btnStart;
        private Label lblTitle;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.cmbConfigs = new ComboBox();
            this.btnStart = new Button();
            this.lblTitle = new Label();
            this.SuspendLayout();

            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(400, 200);
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Izaberite agenciju";

            this.lblTitle.Text = "Turistička agencija";
            this.lblTitle.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitle.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTitle.Dock = DockStyle.Top;
            this.lblTitle.Height = 50;

            this.cmbConfigs.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbConfigs.Font = new Font("Segoe UI", 10F);
            this.cmbConfigs.Location = new Point(50, 70);
            this.cmbConfigs.Width = 300;

            this.btnStart.Text = "Pokreni aplikaciju";
            this.btnStart.Font = new Font("Segoe UI", 10F, FontStyle.Bold);
            this.btnStart.BackColor = Color.FromArgb(52, 152, 219);
            this.btnStart.ForeColor = Color.White;
            this.btnStart.FlatStyle = FlatStyle.Flat;
            this.btnStart.FlatAppearance.BorderSize = 0;
            this.btnStart.Width = 300;
            this.btnStart.Height = 40;
            this.btnStart.Location = new Point(50, 120);
            this.btnStart.Click += new EventHandler(this.btnStart_Click);

            this.Controls.Add(this.lblTitle);
            this.Controls.Add(this.cmbConfigs);
            this.Controls.Add(this.btnStart);

            this.ResumeLayout(false);
        }
    }
}