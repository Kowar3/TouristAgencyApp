using Org.BouncyCastle.Bcpg.OpenPgp;
using System.Drawing;
using System.Windows.Forms;
using System.Xml.Linq;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Utils;
namespace TouristAgencyApp.Forms
{
    partial class PackagesForm
    {
        private DataGridView grid;
        private Button btnUndo;
        private Button btnRedo;
        private Button btnAdd, btnEdit;
        private ComboBox comboBoxMain;

        private void InitializeComponent()
        {
            this.grid = new DataGridView();
            this.btnUndo = new Button();
            this.btnRedo = new Button();
            this.comboBoxMain = new ComboBox();

            this.Text = "Paketi";
            this.Width = 1100;
            this.Height = 560;
            this.StartPosition = FormStartPosition.CenterScreen;

            var headerPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 80,
                BackColor = Color.FromArgb(52, 152, 219)
            };
            var lblTitle = new Label
            {
                Text = "Upravljanje paketima",
                Font = new Font("Segoe UI", 18, FontStyle.Bold),
                ForeColor = Color.White,
                AutoSize = false,
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter
            };
            headerPanel.Controls.Add(lblTitle);

            var toolbarPanel = new Panel
            {
                Dock = DockStyle.Top,
                Height = 60,
                BackColor = Color.White,
                Padding = new Padding(20, 10, 20, 10)
            };

            btnAdd = CreateModernButton("➕ Dodaj paket", Color.FromArgb(46, 204, 113));
            btnAdd.Location = new Point(20, 10);

            btnEdit = CreateModernButton("✏️ Izmeni paket", Color.FromArgb(52, 152, 219));
            btnEdit.Location = new Point(200, 10);

            comboBoxMain.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxMain.Width = 180;
            comboBoxMain.Font = new Font("Segoe UI", 10);
            comboBoxMain.Left = 380;
            comboBoxMain.Top = 15;
            comboBoxMain.Items.AddRange(new string[] { "Svi paketi", "Sea", "Excursion", "Mountain", "Cruise" });
            comboBoxMain.SelectedIndex = 0;
            

            btnUndo = CreateModernButton("↩️ Undo", Color.FromArgb(255, 165, 0));
            btnUndo.Location = new Point(580, 10);
            btnUndo.TextAlign = ContentAlignment.MiddleCenter;
            btnUndo.Width = 100;

            btnRedo = CreateModernButton("Redo ↪️", Color.FromArgb(255, 165, 0));
            btnRedo.Location = new Point(700, 10);
            btnRedo.TextAlign = ContentAlignment.MiddleCenter;
            btnRedo.Width = 100;

            toolbarPanel.Controls.AddRange(new Control[] { btnAdd, btnEdit, comboBoxMain, btnUndo, btnRedo });

            grid.Dock = DockStyle.Fill;
            grid.ReadOnly = true;
            grid.AutoGenerateColumns = true;
            grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            grid.Font = new Font("Segoe UI", 11, FontStyle.Regular);
            grid.RowTemplate.Height = 35;
            grid.AllowUserToAddRows = false;
            grid.BackgroundColor = Color.White;
            grid.BorderStyle = BorderStyle.None;
            grid.GridColor = Color.LightGray;
            grid.EnableHeadersVisualStyles = false;
            grid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(52, 73, 94);
            grid.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            grid.ColumnHeadersHeight = 40;
            grid.AutoResizeColumns();

            this.Controls.Add(grid);          
            this.Controls.Add(toolbarPanel);  
            this.Controls.Add(headerPanel);
        }

        private Button CreateModernButton(string text, Color color)
        {
            return new Button
            {
                Text = text,
                BackColor = color,
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Width = 160,
                Height = 35,
                FlatStyle = FlatStyle.Flat,
                FlatAppearance = { BorderSize = 0 },
                Cursor = Cursors.Hand
            };
        }
        private PackageAddContext DodajPaket()
        {
            PackageAddContext context = new PackageAddContext();
            var f = new Form { Text = "Novi paket", Width = 440, Height = 700, StartPosition = FormStartPosition.CenterParent };
            context.Form = f;
            var lblType = new Label { Text = "Tip paketa", Left = 20, Top = 20, Width = 200 };
            var cbType = new ComboBox { Left = 20, Top = 45, Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
            context.CbType = cbType;
            cbType.Items.AddRange(new[] { "Sea", "Mountain", "Excursion", "Cruise" });
            cbType.SelectedIndex = 0;

            var lblName = new Label { Text = "Naziv paketa", Left = 20, Top = 80, Width = 200 };
            var txtName = new TextBox { Top = 105, Left = 20, Width = 300 };
            context.TxtName = txtName;

            var lblPrice = new Label { Text = "Cena", Left = 20, Top = 140, Width = 200 };
            var numPrice = new NumericUpDown { Top = 165, Left = 20, Width = 300, DecimalPlaces = 2, Maximum = 1000000 };
            context.NumPrice = numPrice;

            var lblDestination = new Label { Text = "Destinacija", Left = 20, Top = 200, Width = 200 };
            var txtDestination = new TextBox { Top = 225, Left = 20, Width = 300 };
            context.TxtDestination = txtDestination;

            var lblAcc = new Label { Text = "Smeštaj", Left = 20, Top = 260, Width = 200 };
            var txtAcc = new TextBox { Top = 285, Left = 20, Width = 300 };
            context.TxtAcc = txtAcc;

            var lblTransport = new Label { Text = "Prevoz", Left = 20, Top = 320, Width = 200 };
            var txtTransport = new TextBox { Top = 345, Left = 20, Width = 300 };
            context.TxtTransport = txtTransport;

            var lblActivities = new Label { Text = "Dodatne aktivnosti", Left = 20, Top = 380, Width = 200 };
            var txtActivities = new TextBox { Top = 405, Left = 20, Width = 300 };
            context.TxtActivities = txtActivities;
            
            var lblGuide = new Label { Text = "Vodič", Left = 20, Top = 260, Width = 200 };
            var txtGuide = new TextBox { Top = 285, Left = 20, Width = 300 };
            context.TxtGuide = txtGuide;

            var lblDuration = new Label { Text = "Trajanje (dani)", Left = 20, Top = 380, Width = 200 };
            var numDuration = new NumericUpDown { Top = 405, Left = 20, Width = 300, Maximum = 60, Minimum = 0 };
            context.NumDuration = numDuration;

            var lblShip = new Label { Text = "Brod", Left = 20, Top = 200, Width = 200 };
            var txtShip = new TextBox { Top = 225, Left = 20, Width = 300 };
            context.TxtShip = txtShip;

            var lblRoute = new Label { Text = "Ruta", Left = 20, Top = 260, Width = 200 };
            var txtRoute = new TextBox { Top = 285, Left = 20, Width = 300 };
            context.TxtRoute = txtRoute;

            var lblDeparture = new Label { Text = "Datum polaska", Left = 20, Top = 320, Width = 200 };
            var dtDeparture = new DateTimePicker { Top = 345, Left = 20, Width = 300 };
            context.DtDeparture = dtDeparture;

            var lblCabin = new Label { Text = "Tip kabine", Left = 20, Top = 380, Width = 200 };
            var txtCabin = new TextBox { Top = 405, Left = 20, Width = 300 };
            context.TxtCabin = txtCabin;

            var btnSave = new Button { Text = "Sačuvaj", Top = 805, Left = 20, Width = 120, Height = 40, Font = new Font("Segoe UI", 11, FontStyle.Bold), TextAlign = ContentAlignment.MiddleCenter };
            context.BtnSave = btnSave;

            void UpdateFields()
            {
                f.Controls.Clear();
                f.Controls.AddRange(new Control[] {
            lblType, cbType,
            lblName, txtName,
            lblPrice, numPrice,
            btnSave
        });

                var type = cbType.SelectedItem.ToString();
                int baseHeight = 250;
                int fieldHeight = 60;
                int buttonPadding = 40;

                if (type == "Sea")
                {
                    f.Controls.AddRange(new Control[] {
            lblDestination, txtDestination,
            lblAcc, txtAcc,
            lblTransport, txtTransport
        });
                    btnSave.Top = baseHeight + (3 * fieldHeight) - 40;
                    f.Height = baseHeight + (3 * fieldHeight) + 40 + buttonPadding;
                }
                else if (type == "Mountain")
                {
                    f.Controls.AddRange(new Control[] {
            lblDestination, txtDestination,
            lblAcc, txtAcc,
            lblTransport, txtTransport,
            lblActivities, txtActivities
        });
                    btnSave.Top = baseHeight + (4 * fieldHeight) - 40;
                    f.Height = baseHeight + (4 * fieldHeight) + 40 + buttonPadding;
                }
                else if (type == "Excursion")
                {
                    f.Controls.AddRange(new Control[] {
            lblDestination, txtDestination,
            lblTransport, txtTransport,
            lblGuide, txtGuide,
            lblDuration, numDuration
        });
                    btnSave.Top = baseHeight + (4 * fieldHeight) - 40;
                    f.Height = baseHeight + (4 * fieldHeight) + 40 + buttonPadding;
                }
                else if (type == "Cruise")
                {
                    f.Controls.AddRange(new Control[] {
            lblShip, txtShip,
            lblRoute, txtRoute,
            lblDeparture, dtDeparture,
            lblCabin, txtCabin
        });
                    btnSave.Top = baseHeight + (4 * fieldHeight) - 40;
                    f.Height = baseHeight + (4 * fieldHeight) + 40 + buttonPadding;
                }
            }

            cbType.SelectedIndexChanged += (ss, ee) => UpdateFields();
            UpdateFields();


            return context;
        }
        
        private PackageEditContext IzmeniPaket()
        {
            
            PackageEditContext context = new PackageEditContext();
            if (grid.SelectedRows.Count == 0) return null;
            var pkg = grid.SelectedRows[0].DataBoundItem as TravelPackage;
            if (pkg == null) return null;

            var f = new Form { Text = "Izmeni paket", Width = 440, StartPosition = FormStartPosition.CenterParent };
            context.Form = f;
            var lblName = new Label { Text = "Naziv paketa", Left = 20, Top = 20, Width = 200 };
            var txtName = new TextBox { Text = pkg.Name, Top = 45, Left = 20, Width = 300 };
            context.TxtName = txtName;
            var lblPrice = new Label { Text = "Cena", Left = 20, Top = 80, Width = 200 };
            var numPrice = new NumericUpDown
            {
                Top = 105,
                Left = 20,
                Width = 300,
                DecimalPlaces = 2,
                Minimum = 0,
                Maximum = 1000000,
                Value = pkg.Price
            };
            context.NumPrice = numPrice;
            var btnSave = new Button
            {
                Text = "Sačuvaj",
                Left = 20,
                Width = 120,
                Height = 40,
                Font = new Font("Segoe UI", 11, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter
            };
            context.BtnSave = btnSave;
            if (pkg.Type == "Sea" && pkg is SeaPackage sea)
            {
                var lblDestination = new Label { Text = "Destinacija", Left = 20, Top = 140, Width = 200 };
                var txtDestination = new TextBox { Text = sea.Destination, Top = 165, Left = 20, Width = 300 };
                context.TxtDestination = txtDestination;
                var lblAcc = new Label { Text = "Smeštaj", Left = 20, Top = 200, Width = 200 };
                var txtAcc = new TextBox { Text = sea.Accommodation, Top = 225, Left = 20, Width = 300 };
                context.TxtAcc = txtAcc;
                var lblTransport = new Label { Text = "Prevoz", Left = 20, Top = 260, Width = 200 };
                var txtTransport = new TextBox { Text = sea.Transport, Top = 285, Left = 20, Width = 300 };
                context.TxtTransport = txtTransport;
                btnSave.Top = txtTransport.Bottom + 30;
                f.Height = btnSave.Bottom + 70;

                
                f.Controls.AddRange(new Control[] {
            lblName, txtName,
            lblPrice, numPrice,
            lblDestination, txtDestination,
            lblAcc, txtAcc,
            lblTransport, txtTransport,
            btnSave
        });
            }
            else if (pkg.Type == "Mountain" && pkg is MountainPackage mountain)
            {
                var lblDestination = new Label { Text = "Destinacija", Left = 20, Top = 140, Width = 200 };
                var txtDestination = new TextBox { Text = mountain.Destination, Top = 165, Left = 20, Width = 300 };
                context.TxtDestination = txtDestination;
                var lblAcc = new Label { Text = "Smeštaj", Left = 20, Top = 200, Width = 200 };
                var txtAcc = new TextBox { Text = mountain.Accommodation, Top = 225, Left = 20, Width = 300 };
                context.TxtAcc = txtAcc;
                var lblTransport = new Label { Text = "Prevoz", Left = 20, Top = 260, Width = 200 };
                var txtTransport = new TextBox { Text = mountain.Transport, Top = 285, Left = 20, Width = 300 };
                context.TxtTransport = txtTransport;
                var lblActivities = new Label { Text = "Dodatne aktivnosti", Left = 20, Top = 320, Width = 200 };
                var txtActivities = new TextBox { Text = mountain.Activities, Top = 345, Left = 20, Width = 300 };
                context.TxtActivities = txtActivities;
                btnSave.Top = txtActivities.Bottom + 30;
                f.Height = btnSave.Bottom + 70;

                
                f.Controls.AddRange(new Control[] {
            lblName, txtName,
            lblPrice, numPrice,
            lblDestination, txtDestination,
            lblAcc, txtAcc,
            lblTransport, txtTransport,
            lblActivities, txtActivities,
            btnSave
        });
            }
            else if (pkg.Type == "Excursion" && pkg is ExcursionPackage excursion)
            {
                var lblDestination = new Label { Text = "Destinacija", Left = 20, Top = 140, Width = 200 };
                var txtDestination = new TextBox { Text = excursion.Destination, Top = 165, Left = 20, Width = 300 };
                context.TxtDestination = txtDestination;
                var lblTransport = new Label { Text = "Prevoz", Left = 20, Top = 200, Width = 200 };
                var txtTransport = new TextBox { Text = excursion.Transport, Top = 225, Left = 20, Width = 300 };
                context.TxtTransport = txtTransport;
                var lblGuide = new Label { Text = "Vodič", Left = 20, Top = 260, Width = 200 };
                var txtGuide = new TextBox { Text = excursion.Guide, Top = 285, Left = 20, Width = 300 };
                context.TxtGuide = txtGuide;
                var lblDuration = new Label { Text = "Trajanje (dani)", Left = 20, Top = 320, Width = 200 };
                var numDuration = new NumericUpDown { Value = excursion.Duration, Top = 345, Left = 20, Width = 300, Maximum = 60, Minimum = 0 };
                context.NumDuration = numDuration;
                btnSave.Top = numDuration.Bottom + 30;
                f.Height = btnSave.Bottom + 70;

                
                f.Controls.AddRange(new Control[] {
            lblName, txtName,
            lblPrice, numPrice,
            lblDestination, txtDestination,
            lblTransport, txtTransport,
            lblGuide, txtGuide,
            lblDuration, numDuration,
            btnSave
        });
            }
            else if (pkg.Type == "Cruise" && pkg is CruisePackage cruise)
            {
                var lblShip = new Label { Text = "Brod", Left = 20, Top = 140, Width = 200 };
                var txtShip = new TextBox { Text = cruise.Ship, Top = 165, Left = 20, Width = 300 };
                context.TxtShip = txtShip;
                var lblRoute = new Label { Text = "Ruta", Left = 20, Top = 200, Width = 200 };
                var txtRoute = new TextBox { Text = cruise.Route, Top = 225, Left = 20, Width = 300 };
                context.TxtRoute = txtRoute;
                var lblDeparture = new Label { Text = "Datum polaska", Left = 20, Top = 260, Width = 200 };
                var dtDeparture = new DateTimePicker { Value = cruise.DepartureDate, Top = 285, Left = 20, Width = 300 };
                context.DtDeparture = dtDeparture;
                var lblCabin = new Label { Text = "Tip kabine", Left = 20, Top = 320, Width = 200 };
                var txtCabin = new TextBox { Text = cruise.CabinType, Top = 345, Left = 20, Width = 300 };
                context.TxtCabin = txtCabin;
                context.TxtDestination = new TextBox();
                context.TxtDestination.Text = cruise.Destination;
                btnSave.Top = txtCabin.Bottom + 30;
                f.Height = btnSave.Bottom + 70;

                
                f.Controls.AddRange(new Control[] {
            lblName, txtName,
            lblPrice, numPrice,
            lblShip, txtShip,
            lblRoute, txtRoute,
            lblDeparture, dtDeparture,
            lblCabin, txtCabin,
            btnSave
        });
            }
           
            
            context.Package = pkg;
            return context;
        }
        private void setupGrid(List<TravelPackage> lista)
        {
            grid.AutoGenerateColumns = true;
            grid.DataSource = lista;
            grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            grid.AutoResizeColumns();
            grid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            grid.ColumnHeadersHeight = 40;

            if (grid.Columns.Contains("Id"))
                grid.Columns["Id"].Visible = false;
            if (grid.Columns.Contains("Destination"))
                grid.Columns["Destination"].HeaderText = "Destinacija";
            if (grid.Columns.Contains("Name"))
                grid.Columns["Name"].HeaderText = "Naziv";
            if (grid.Columns.Contains("Price"))
                grid.Columns["Price"].HeaderText = "Cena";
            if (grid.Columns.Contains("Type"))
                grid.Columns["Type"].HeaderText = "Tip";
            if (grid.Columns.Contains("Details"))
                grid.Columns["Details"].HeaderText = "Detalji";
        }
        private Panel headerPanel;
        private Label lblTitle;
        private Panel toolbarPanel;
    }
}