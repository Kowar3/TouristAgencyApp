using System;
using System.Drawing.Text;
using System.Windows.Forms;
using System.Xml.Linq;
using TouristAgencyApp.Models;
using TouristAgencyApp.Patterns;
using TouristAgencyApp.Patterns.Facade;
using TouristAgencyApp.Services;
using TouristAgencyApp.Utils;

namespace TouristAgencyApp.Forms

{
    public partial class PackagesForm : Form
    {
        private readonly PackageFacade _packageFacade;
        private string type;

        public PackagesForm(IDatabaseService dbService)
        {
            InitializeComponent();
            _packageFacade = new PackageFacade(dbService);
            type = "Svi paketi";
            btnEdit.Click += (s, e) => Izmeni();
            btnAdd.Click += (s, e) => Dodaj();
            btnUndo.Click += (s, e) => OpozoviAkciju();
            btnRedo.Click += (s, e) => NazoviAkciju();
            comboBoxMain.SelectedIndexChanged += (s, e) =>
            {
                type = comboBoxMain.SelectedItem.ToString();
                LoadPackages();
            };
            LoadPackages();
        }

        private void LoadPackages()
        {
            grid.DataSource = null;
            grid.Columns.Clear();
            var data = _packageFacade.GetPackagesByType(type);
            setupGrid(data); 
        }
        private void OpozoviAkciju()
        {
            _packageFacade.Undo();
            LoadPackages();
        }
        private void NazoviAkciju()
        {
            _packageFacade.Redo();
            LoadPackages();
        }

        private void Dodaj()
        {
            PackageAddContext context = DodajPaket();

            context.BtnSave.Click += (ss, ee) =>
            {
                TravelPackage pkg = context.CbType.SelectedItem.ToString() switch
                {
                    "Sea" => PackageDirector.CreateSeaPackage(
                        context.TxtName.Text,
                        context.NumPrice.Value,
                        context.TxtDestination.Text,
                        context.TxtAcc.Text,
                        context.TxtTransport.Text),
                    "Mountain" => PackageDirector.CreateMountainPackage(
                        context.TxtName.Text,
                        context.NumPrice.Value,
                        context.TxtDestination.Text,
                        context.TxtAcc.Text,
                        context.TxtTransport.Text,
                        context.TxtActivities.Text),
                    "Excursion" => PackageDirector.CreateExcursionPackage(
                        context.TxtName.Text,
                        context.NumPrice.Value,
                        context.TxtDestination.Text,
                        context.TxtTransport.Text,
                        context.TxtGuide.Text,
                        (int)context.NumDuration.Value),
                    "Cruise" => PackageDirector.CreateCruisePackage(
                        context.TxtName.Text,
                        context.NumPrice.Value,
                        context.TxtShip.Text,
                        context.TxtRoute.Text,
                        context.DtDeparture.Value,
                        context.TxtCabin.Text,
                        context.TxtDestination.Text
                        ),
                    _ => throw new Exception("Unknown package type")
                };

                if (!Validate(context)) return;
                int id = _packageFacade.AddPackage(pkg);
                context.Form.Close();
                LoadPackages();
            };
            context.Form.ShowDialog();
        }
        private void Izmeni()
        {
            PackageEditContext context = IzmeniPaket();
            TravelPackage pkg = context.Package;
            if (pkg.Type == "Sea" && pkg is SeaPackage sea)
            {
                context.BtnSave.Click += (ss, ee) =>
                {
                    var updatedPackage = PackageDirector.CreateSeaPackageForUpdate(
                        sea.Id,
                        context.TxtName.Text,
                        context.NumPrice.Value,
                        context.TxtDestination.Text,
                        context.TxtAcc.Text,
                        context.TxtTransport.Text);
                    if (!Validate(context)) return;
                    _packageFacade.UpdatePackage(updatedPackage);
                    context.Form.Close();
                    LoadPackages();
                };
            }
            else if(pkg.Type == "Mountain" && pkg is MountainPackage mountain)
            {
                context.BtnSave.Click += (ss, ee) =>
                {
                    var updatedPackage = PackageDirector.CreateMountainPackageForUpdate(
                        mountain.Id,
                        context.TxtName.Text,
                        context.NumPrice.Value,
                        context.TxtDestination.Text,
                        context.TxtAcc.Text,
                        context.TxtTransport.Text,
                        context.TxtActivities.Text);
                    if (!Validate(context)) return;
                    _packageFacade.UpdatePackage(updatedPackage);
                    context.Form.Close();
                    LoadPackages();
                };
            }
            else if(pkg.Type == "Cruise" && pkg is CruisePackage cruise)
            {
                context.BtnSave.Click += (ss, ee) =>
                {
                    MessageBox.Show(context.TxtDestination.Text);
                    var updatedPackage = PackageDirector.CreateCruisePackageForUpdate(
                        cruise.Id,
                        context.TxtName.Text,
                        context.NumPrice.Value,
                        context.TxtShip.Text,
                        context.TxtRoute.Text,
                        context.DtDeparture.Value,
                        context.TxtCabin.Text,
                        context.TxtDestination.Text
                        );
                    if (!Validate(context)) return;
                    _packageFacade.UpdatePackage(updatedPackage);
                    context.Form.Close();
                    LoadPackages();
                };
            }
            else if (pkg.Type == "Excursion" && pkg is ExcursionPackage excursion)
            {
                context.BtnSave.Click += (ss, ee) =>
                {
                    var updatedPackage = PackageDirector.CreateExcursionPackageForUpdate(
                        excursion.Id,
                        context.TxtName.Text,
                        context.NumPrice.Value,
                        context.TxtDestination.Text,
                        context.TxtTransport.Text,
                        context.TxtGuide.Text,
                        (int)context.NumDuration.Value
                        );
                    if (!Validate(context)) return;
                    _packageFacade.UpdatePackage(updatedPackage);
                    context.Form.Close();
                    LoadPackages();
                };
            }
            context.Form.ShowDialog();
        }
        private bool Validate(PackageAddContext context)
        {
            var type = context.CbType.Text;

            if (string.IsNullOrWhiteSpace(context.TxtName.Text) || context.TxtName.Text.Length < 3)
            {
                MessageBox.Show("Naziv paketa mora imati najmanje 3 karaktera.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (context.NumPrice.Value <= 0 || context.NumPrice.Value > 1_000_000)
            {
                MessageBox.Show("Cena mora biti veća od 0 i manja od 1.000.000.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type!="Cruise" && (string.IsNullOrWhiteSpace(context.TxtDestination.Text) || context.TxtDestination.Text.Length < 2 || !System.Text.RegularExpressions.Regex.IsMatch(context.TxtDestination.Text, @"^[A-Za-zčćšđžČĆŠĐŽ\s]+$")))
            {
                MessageBox.Show("Destinacija mora sadržati samo slova i imati najmanje 2 karaktera.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if ((type =="Mountain" || type=="Sea") && (string.IsNullOrWhiteSpace(context.TxtAcc.Text) || context.TxtAcc.Text.Length < 3))
            {
                MessageBox.Show("Unesite smeštaj (min. 3 karaktera).", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if ((type != "Cruise") && string.IsNullOrWhiteSpace(context.TxtTransport.Text) && context.TxtTransport.Text.Length < 3)
            {
                MessageBox.Show("Prevoz mora imati najmanje 3 karaktera.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type == "Mountain" && string.IsNullOrWhiteSpace(context.TxtActivities.Text) && context.TxtActivities.Text.Length < 3)
            {
                MessageBox.Show("Aktivnosti moraju imati najmanje 3 karaktera.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type =="Excursion" && (string.IsNullOrWhiteSpace(context.TxtGuide.Text) && !System.Text.RegularExpressions.Regex.IsMatch(context.TxtGuide.Text, @"^[A-Za-zčćšđžČĆŠĐŽ\s]+$")))
            {
                MessageBox.Show("Vodič može sadržati samo slova i razmake.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type == "Excursion" && (context.NumDuration.Value < 1 || context.NumDuration.Value > 365))
            {
                MessageBox.Show("Trajanje mora biti između 1 i 365 dana.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type == "Cruise" && (string.IsNullOrWhiteSpace(context.TxtShip.Text) || context.TxtShip.Text.Length < 2))
            {
                MessageBox.Show("Naziv broda mora imati najmanje 2 karaktera.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type == "Cruise" && (string.IsNullOrWhiteSpace(context.TxtRoute.Text) || context.TxtRoute.Text.Length < 3))
            {
                MessageBox.Show("Ruta mora imati najmanje 3 karaktera.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type == "Cruise" && context.DtDeparture.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Datum polaska ne može biti u prošlosti.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type =="Cruise" && string.IsNullOrWhiteSpace(context.TxtCabin.Text) && context.TxtCabin.Text.Length < 2)
            {
                MessageBox.Show("Kabina mora imati najmanje 2 karaktera.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
        private bool Validate(PackageEditContext context)
        {
            var type = context.Package.Type;
            if (string.IsNullOrWhiteSpace(context.TxtName.Text) || context.TxtName.Text.Length < 3)
            {
                MessageBox.Show("Naziv paketa mora imati najmanje 3 karaktera.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (context.NumPrice.Value <= 0 || context.NumPrice.Value > 1_000_000)
            {
                MessageBox.Show("Cena mora biti veća od 0 i manja od 1.000.000.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type != "Cruise" && (string.IsNullOrWhiteSpace(context.TxtDestination.Text) || context.TxtDestination.Text.Length < 2 || !System.Text.RegularExpressions.Regex.IsMatch(context.TxtDestination.Text, @"^[A-Za-zčćšđžČĆŠĐŽ\s]+$")))
            {
                MessageBox.Show("Destinacija mora sadržati samo slova i imati najmanje 2 karaktera.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if ((type == "Mountain" || type == "Sea") && (string.IsNullOrWhiteSpace(context.TxtAcc.Text) || context.TxtAcc.Text.Length < 3))
            {
                MessageBox.Show("Unesite smeštaj (min. 3 karaktera).", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if ((type != "Cruise") && string.IsNullOrWhiteSpace(context.TxtTransport.Text) && context.TxtTransport.Text.Length < 3)
            {
                MessageBox.Show("Prevoz mora imati najmanje 3 karaktera.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type == "Mountain" && string.IsNullOrWhiteSpace(context.TxtActivities.Text) && context.TxtActivities.Text.Length < 3)
            {
                MessageBox.Show("Aktivnosti moraju imati najmanje 3 karaktera.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type == "Excursion" && (string.IsNullOrWhiteSpace(context.TxtGuide.Text) && !System.Text.RegularExpressions.Regex.IsMatch(context.TxtGuide.Text, @"^[A-Za-zčćšđžČĆŠĐŽ\s]+$")))
            {
                MessageBox.Show("Vodič može sadržati samo slova i razmake.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type == "Excursion" && (context.NumDuration.Value < 1 || context.NumDuration.Value > 365))
            {
                MessageBox.Show("Trajanje mora biti između 1 i 365 dana.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type == "Cruise" && (string.IsNullOrWhiteSpace(context.TxtShip.Text) || context.TxtShip.Text.Length < 2))
            {
                MessageBox.Show("Naziv broda mora imati najmanje 2 karaktera.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type == "Cruise" && (string.IsNullOrWhiteSpace(context.TxtRoute.Text) || context.TxtRoute.Text.Length < 3))
            {
                MessageBox.Show("Ruta mora imati najmanje 3 karaktera.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type == "Cruise" && context.DtDeparture.Value.Date < DateTime.Today)
            {
                MessageBox.Show("Datum polaska ne može biti u prošlosti.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (type == "Cruise" && string.IsNullOrWhiteSpace(context.TxtCabin.Text) && context.TxtCabin.Text.Length < 2)
            {
                MessageBox.Show("Kabina mora imati najmanje 2 karaktera.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }
    }
    
}