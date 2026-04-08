using System;
using System.Linq;
using System.Windows.Forms;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Services;
using TouristAgencyApp.Utils;

namespace TouristAgencyApp.Forms
{
    public partial class ReservationsForm : Form
    {
        private readonly ReservationFacade _reservationFacade;

        public ReservationsForm(IDatabaseService dbService)
        {
            InitializeComponent();
            _reservationFacade = new ReservationFacade(dbService);

            cbClients.SelectedIndexChanged += (s, e) => LoadReservations();
            btnAdd.Click += (s, e) => DodajRezervaciju();
            btnRemove.Click += (s, e) => OtkaziRezervaciju();
            btnEdit.Click += (s, e) => AzurirajRezervaciju();
            btnUndo.Click += (s, e) => OpozoviAkciju();
            btnRedo.Click += (s, e) => NazoviAkciju();

            LoadClients();
        }

        private void LoadClients()
        {
            var clientsList = _reservationFacade.GetAllClients();
            cbClients.DataSource = clientsList;
            cbClients.DisplayMember = "FullName";
            cbClients.ValueMember = "Id";
            if (cbClients.Items.Count > 0) cbClients.SelectedIndex = 0;
            LoadReservations();
        }

        private void LoadReservations()
        {
            if (cbClients.SelectedItem is Client c)
            {
                var reservations = _reservationFacade.GetReservationsByClient(c.Id);
                grid.DataSource = null;
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
                grid.DataSource = reservations;
                grid.ClearSelection();
                grid.AutoResizeColumns();
            }
        }

        private void OpozoviAkciju()
        {
            _reservationFacade.Undo();
            LoadReservations();
        }

        private void NazoviAkciju()
        {
            _reservationFacade.Redo();
            LoadReservations();
        }

        private void DodajRezervaciju()
        {
            _reservationFacade.RefreshCache();
            if (cbClients.SelectedItem is not Client c) return;
            var allPackages = _reservationFacade.GetAllPackages();
            if (allPackages.Count == 0)
            {
                MessageBox.Show("Nema dostupnih paketa.", "Upozorenje", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var destinations = _reservationFacade.GetUniqueDestinations();
            var ctx = KreirajDodavanjeRezervacijeDialog(destinations);

            void UpdatePackages()
            {
                if (ctx.CbDestinations.SelectedItem is not string d) return;
                var filtered = _reservationFacade.GetPackagesByDestination(d);
                ctx.CbPackages.DataSource = filtered;
                ctx.CbPackages.DisplayMember = "Name";
            }

            ctx.CbDestinations.SelectedIndexChanged += (s, e) => UpdatePackages();
            UpdatePackages();

            ctx.BtnSave.Click += (ss, ee) =>
            {
                if (ctx.CbDestinations.SelectedItem is not string dest || string.IsNullOrWhiteSpace(dest))
                {
                    MessageBox.Show("Molimo odaberite destinaciju.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (ctx.CbPackages.SelectedItem is not TravelPackage pkg)
                {
                    MessageBox.Show("Molimo odaberite paket.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                if (ctx.NumPersons.Value < 1 || ctx.NumPersons.Value > ctx.NumPersons.Maximum)
                {
                    MessageBox.Show($"Broj osoba mora biti između 1 i {ctx.NumPersons.Maximum}.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var reservation = new Reservation
                {
                    ClientId = c.Id,
                    ExtraServices = "",
                    NumPersons = (int)ctx.NumPersons.Value,
                    PackageId = pkg.Id,
                    ReservationDate = DateTime.Now,
                    PackageName = pkg.Name
                };

                _reservationFacade.AddReservation(reservation);
                btnUndo.Visible = true;
                ctx.Form.Close();
                LoadReservations();
            };

            ctx.Form.ShowDialog();
        }

        private void AzurirajRezervaciju()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Prvo izaberi rezervaciju za azuriranje!", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ctx = KreirajAzuriranjeRezervacijeDialog();
            if (ctx == null) return;

            ctx.BtnSave.Click += (ss, ee) =>
            {
                _reservationFacade.UpdateReservation(ctx.ReservationId, (int)ctx.NumPersons.Value, "");
                btnUndo.Visible = true;
                LoadReservations();
                ctx.Form.Close();
            };

            ctx.Form.ShowDialog();
        }

        private void OtkaziRezervaciju()
        {
            if (grid.SelectedRows.Count == 0)
            {
                MessageBox.Show("Prvo izaberi rezervaciju za otkazivanje!", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            var id = Convert.ToInt32(grid.SelectedRows[0].Cells[3].Value);
            var confirm = MessageBox.Show("Da li ste sigurni da želite da otkažete ovu rezervaciju?", "Potvrda", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (confirm == DialogResult.Yes)
            {
                _reservationFacade.RemoveReservation(id);
                btnUndo.Visible = true;
                LoadReservations();
            }
        }
    }
}
