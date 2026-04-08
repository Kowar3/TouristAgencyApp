using System;
using System.Drawing;
using System.Windows.Forms;
using TouristAgencyApp.Utils;

namespace TouristAgencyApp.Forms
{
    partial class ReservationsForm
    {
        private System.ComponentModel.IContainer components = null;
        private DataGridView grid;
        private ComboBox cbClients;
        private Button btnUndo;
        private Button btnRedo;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnEdit;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Text = "🛎️ Upravljanje rezervacijama";
            this.Width = 1260;
            this.Height = 600;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            var headerPanel = new Panel { Dock = DockStyle.Top, Height = 80, BackColor = Color.FromArgb(155, 89, 182) };
            var lblTitle = new Label { Text = "Upravljanje rezervacijama", Font = new Font("Segoe UI", 18, FontStyle.Bold), ForeColor = Color.White, AutoSize = false, Width = this.Width, Height = 50, Top = 15, Left = 0, TextAlign = ContentAlignment.MiddleCenter };
            headerPanel.Controls.Add(lblTitle);

            var toolbarPanel = new Panel { Dock = DockStyle.Top, Height = 70, BackColor = Color.White, Padding = new Padding(20, 10, 20, 10) };

            cbClients = new ComboBox { Width = 300, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 11, FontStyle.Regular), Location = new Point(20, 20) };

            btnAdd = CreateModernButton("➕ Nova rezervacija", Color.FromArgb(46, 204, 113));
            btnAdd.Location = new Point(340, 15);

            btnRemove = CreateModernButton("✕ Otkaži rezervaciju", Color.FromArgb(231, 76, 60));
            btnRemove.Location = new Point(520, 15);
            btnRemove.TextAlign = ContentAlignment.MiddleCenter;

            btnEdit = CreateModernButton(" Azuriraj rezervaciju", Color.FromArgb(0, 0, 255));
            btnEdit.Location = new Point(700, 15);
            btnEdit.TextAlign = ContentAlignment.MiddleCenter;

            btnUndo = CreateModernButton("↩️ Undo", Color.FromArgb(255, 165, 0));
            btnUndo.Location = new Point(880, 15);
            btnUndo.TextAlign = ContentAlignment.MiddleCenter;
            btnUndo.Width = 160;
            

            btnRedo = CreateModernButton("Redo ↪️", Color.FromArgb(255, 165, 0));
            btnRedo.Location = new Point(1060, 15);
            btnRedo.TextAlign = ContentAlignment.MiddleCenter;
            btnRedo.Width = 160;
            

            toolbarPanel.Controls.AddRange(new Control[] { cbClients, btnAdd, btnRemove, btnEdit, btnUndo, btnRedo });

            var contentPanel = new Panel { Dock = DockStyle.Fill, BackColor = Color.White };

            grid = new DataGridView
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                AutoGenerateColumns = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                RowTemplate = { Height = 35 },
                AllowUserToAddRows = false,
                BackgroundColor = Color.White,
                BorderStyle = BorderStyle.None,
                GridColor = Color.FromArgb(224, 224, 224),
                EnableHeadersVisualStyles = false
            };

            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "PackageName", HeaderText = "Paket", Width = 200 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "NumPersons", HeaderText = "Broj osoba", Width = 120 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "ReservationDate", HeaderText = "Datum rezervacije", Width = 160 });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Id", HeaderText = "Id", Width = 0, Visible = false });
            grid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = "Destination", HeaderText = "Destinacija", Width = 160 });

            var headerStyle = new DataGridViewCellStyle { Font = new Font("Segoe UI", 12, FontStyle.Bold), Alignment = DataGridViewContentAlignment.MiddleCenter, BackColor = Color.FromArgb(52, 73, 94), ForeColor = Color.White };
            grid.ColumnHeadersDefaultCellStyle = headerStyle;
            grid.ColumnHeadersHeight = 45;
            grid.DefaultCellStyle.BackColor = Color.White;
            grid.DefaultCellStyle.SelectionBackColor = Color.FromArgb(52, 152, 219);
            grid.DefaultCellStyle.SelectionForeColor = Color.White;
            grid.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);

            contentPanel.Controls.Add(grid);

            var statusBar = new Panel { Dock = DockStyle.Bottom, Height = 30, BackColor = Color.FromArgb(52, 73, 94) };
            var lblStatus = new Label { Text = "Spreman za rad", Font = new Font("Segoe UI", 9, FontStyle.Regular), ForeColor = Color.White, AutoSize = false, Width = this.Width, Height = 30, Top = 0, Left = 0, TextAlign = ContentAlignment.MiddleLeft, Padding = new Padding(10, 0, 0, 0) };
            statusBar.Controls.Add(lblStatus);

            this.Controls.Add(statusBar);
            this.Controls.Add(contentPanel);
            this.Controls.Add(toolbarPanel);
            this.Controls.Add(headerPanel);
        }

        private Button CreateModernButton(string text, Color baseColor)
        {
            var button = new Button { Text = text, Font = new Font("Segoe UI", 11, FontStyle.Bold), ForeColor = Color.White, BackColor = baseColor, FlatStyle = FlatStyle.Flat, Width = 160, Height = 40, Cursor = Cursors.Hand, TextAlign = ContentAlignment.MiddleCenter };
            button.FlatAppearance.BorderSize = 0;
            button.MouseEnter += (s, e) => { button.BackColor = ControlPaint.Light(baseColor); };
            button.MouseLeave += (s, e) => { button.BackColor = baseColor; };
            return button;
        }

        private ReservationAddContext KreirajDodavanjeRezervacijeDialog(System.Collections.Generic.List<string> destinacije)
        {
            var ctx = new ReservationAddContext();
            var f = new Form { Text = "Nova rezervacija", Width = 420, Height = 420, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, BackColor = Color.FromArgb(248, 249, 250) };
            ctx.Form = f;

            var lblDestinations = new Label { Text = "Destinacija:", Left = 20, Top = 20, Width = 180, Font = new Font("Segoe UI", 11) };
            var cbDestinations = new ComboBox { Left = 20, Top = 50, Width = 360, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 11) };
            cbDestinations.DataSource = destinacije;
            ctx.CbDestinations = cbDestinations;

            var lblPackages = new Label { Text = "Paket:", Left = 20, Top = 95, Width = 180, Font = new Font("Segoe UI", 11) };
            var cbPackages = new ComboBox { Left = 20, Top = 125, Width = 360, DropDownStyle = ComboBoxStyle.DropDownList, Font = new Font("Segoe UI", 11) };
            ctx.CbPackages = cbPackages;

            var lblPersons = new Label { Text = "Broj osoba:", Left = 20, Top = 170, Width = 180, Font = new Font("Segoe UI", 11) };
            var numPersons = new NumericUpDown { Left = 20, Top = 200, Width = 120, Minimum = 1, Maximum = 30, Value = 1, Font = new Font("Segoe UI", 11) };
            ctx.NumPersons = numPersons;

            var btnSave = new Button { Text = "Sačuvaj", Left = 20, Top = 315, Width = 150, Height = 40, Font = new Font("Segoe UI", 11, FontStyle.Bold), BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            ctx.BtnSave = btnSave;

            f.Shown += (s, e) =>
            {
                ctx.BtnSave.Top = f.ClientSize.Height - ctx.BtnSave.Height - 10;
                if (cbDestinations.Items.Count > 0) cbDestinations.SelectedIndex = 0;
            };

            f.Controls.AddRange(new Control[] { lblDestinations, cbDestinations, lblPackages, cbPackages, lblPersons, numPersons, btnSave });
            return ctx;
        }

        private ReservationEditContext KreirajAzuriranjeRezervacijeDialog()
        {
            if (grid.SelectedRows.Count == 0) return null;
            var ctx = new ReservationEditContext();
            var f = new Form { Text = "Azuriraj rezervaciju", Width = 420, Height = 350, StartPosition = FormStartPosition.CenterParent, FormBorderStyle = FormBorderStyle.FixedDialog, MaximizeBox = false, MinimizeBox = false, BackColor = Color.FromArgb(248, 249, 250) };
            ctx.Form = f;

            ctx.PackageName = grid.SelectedRows[0].Cells[0].Value?.ToString() ?? "";
            ctx.ReservationId = Convert.ToInt32(grid.SelectedRows[0].Cells[3].Value);

            var lblPackage = new Label { Text = "Paket: " + ctx.PackageName, Left = 20, Top = 20, Width = 360, Font = new Font("Segoe UI", 11) };
            var lblPersons = new Label { Text = "Broj osoba:", Left = 20, Top = 95, Width = 180, Font = new Font("Segoe UI", 11) };
            var numPersons = new NumericUpDown { Left = 20, Top = 125, Width = 120, Minimum = 1, Maximum = 30, Value = 1, Font = new Font("Segoe UI", 11) };
            numPersons.Value = Convert.ToDecimal(grid.SelectedRows[0].Cells[1].Value);
            ctx.NumPersons = numPersons;

            var btnSave = new Button { Text = "Sačuvaj", Left = 20, Top = 240, Width = 150, Height = 40, Font = new Font("Segoe UI", 11, FontStyle.Bold), BackColor = Color.FromArgb(46, 204, 113), ForeColor = Color.White, FlatStyle = FlatStyle.Flat, Cursor = Cursors.Hand };
            btnSave.FlatAppearance.BorderSize = 0;
            ctx.BtnSave = btnSave;

            f.Controls.AddRange(new Control[] { lblPackage, lblPersons, numPersons, btnSave });
            return ctx;
        }
    }
}
