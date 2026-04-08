using System.Drawing.Drawing2D;
using TouristAgencyApp.Patterns;

namespace TouristAgencyApp.Forms
{
    partial class MainForm
    {
        private MenuStrip menuStrip;
        private ToolStripMenuItem agencijaMeni;
        private ToolStripMenuItem promeniAgencijuItem;
        private Panel headerPanel, mainPanel, statusBar;
        private Label lblAgency, lblSubtitle, lblStatus;
        private TableLayoutPanel buttonGrid;

        private void InitializeComponent()
        {
            this.SuspendLayout();

            this.Text = AppSettings.Instance.AgencyName;
            this.Width = 800;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(240, 244, 248);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.DoubleBuffered = true;

            menuStrip = new MenuStrip();
            agencijaMeni = new ToolStripMenuItem("Agencija");
            promeniAgencijuItem = new ToolStripMenuItem("Promeni agenciju");
            promeniAgencijuItem.Click += (s, e) =>
            {
                var startupForm = new StartupForm();
                this.Hide();
                startupForm.ShowDialog();
                this.Close();
            };
            agencijaMeni.DropDownItems.Add(promeniAgencijuItem);
            menuStrip.Items.Add(agencijaMeni);
            this.MainMenuStrip = menuStrip;
            this.Controls.Add(menuStrip);

            headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 120,
                BackColor = Color.Transparent
            };
            headerPanel.Paint += (s, e) => DrawGradientHeader(e.Graphics, headerPanel.Width, headerPanel.Height);
            this.Controls.Add(headerPanel);

            lblAgency = new Label
            {
                Text = AppSettings.Instance.AgencyName,
                Font = new Font("Segoe UI", 24, FontStyle.Bold),
                ForeColor = Color.White,
                Width = 800,
                Height = 60,
                Top = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblAgency);

            lblSubtitle = new Label
            {
                Text = "Sistem za upravljanje turističkim aranžmanima",
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.FromArgb(236, 240, 241),
                Width = 800,
                Height = 30,
                Top = 80,
                TextAlign = ContentAlignment.MiddleCenter,
                BackColor = Color.Transparent
            };
            headerPanel.Controls.Add(lblSubtitle);

            mainPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(40, 140, 40, 40)
            };
            this.Controls.Add(mainPanel);

            buttonGrid = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                ColumnStyles = { new ColumnStyle(SizeType.Percent, 50F), new ColumnStyle(SizeType.Percent, 50F) },
                RowStyles = { new RowStyle(SizeType.Percent, 50F), new RowStyle(SizeType.Percent, 50F) }
            };
            mainPanel.Controls.Add(buttonGrid);

            var btnClients = CreateModernButton("👥 Klijenti", "Upravljanje klijentima", Color.FromArgb(46, 204, 113));
            btnClients.Click += (s, e) => new ClientsForm(_dbService).ShowDialog();

            var btnPackages = CreateModernButton("✈️ Paketi", "Upravljanje turističkim paketima", Color.FromArgb(52, 152, 219));
            btnPackages.Click += (s, e) => new PackagesForm(_dbService).ShowDialog();

            var btnReservations = CreateModernButton("📋 Rezervacije", "Upravljanje rezervacijama", Color.FromArgb(155, 89, 182));
            btnReservations.Click += (s, e) => new ReservationsForm(_dbService).ShowDialog();

            var btnBackup = CreateModernButton("💾 Backup", "Ručno kreiranje backup-a", Color.FromArgb(231, 76, 60));
            btnBackup.Click += (s, e) => NapraviBackup(true);

            buttonGrid.Controls.Add(btnClients, 0, 0);
            buttonGrid.Controls.Add(btnPackages, 1, 0);
            buttonGrid.Controls.Add(btnReservations, 0, 1);
            buttonGrid.Controls.Add(btnBackup, 1, 1);

            statusBar = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                BackColor = Color.FromArgb(52, 73, 94)
            };
            this.Controls.Add(statusBar);

            lblStatus = new Label
            {
                Text = "Sistem spreman za rad",
                Font = new Font("Segoe UI", 9, FontStyle.Regular),
                ForeColor = Color.White,
                Width = 800,
                Height = 30,
                TextAlign = ContentAlignment.MiddleLeft,
                Padding = new Padding(10, 0, 0, 0)
            };
            statusBar.Controls.Add(lblStatus);

            this.ResumeLayout(false);
        }

        private Button CreateModernButton(string text, string tooltip, Color baseColor)
        {
            var button = new Button
            {
                Text = text,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = baseColor,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Dock = DockStyle.Fill,
                Margin = new Padding(10),
                Cursor = Cursors.Hand
            };

            var toolTip = new ToolTip();
            toolTip.SetToolTip(button, tooltip);

            button.MouseEnter += (s, e) =>
            {
                button.BackColor = ControlPaint.Light(baseColor);
                button.Font = new Font("Segoe UI", 15, FontStyle.Bold);
            };

            button.MouseLeave += (s, e) =>
            {
                button.BackColor = baseColor;
                button.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            };

            return button;
        }

        private void DrawGradientHeader(Graphics g, int width, int height)
        {
            var rect = new Rectangle(0, 0, width, height);
            using var brush = new LinearGradientBrush(rect, Color.FromArgb(52, 152, 219), Color.FromArgb(41, 128, 185), LinearGradientMode.Vertical);
            g.FillRectangle(brush, rect);
        }
    }
}