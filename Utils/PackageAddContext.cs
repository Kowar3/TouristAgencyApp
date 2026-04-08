using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TouristAgencyApp.Utils
{
    class PackageAddContext
    {
        public Form Form { get; set; }
        public Button BtnSave { get; set; }
        public ComboBox CbType { get; set; }
        public TextBox TxtName { get; set; }
        public NumericUpDown NumPrice { get; set; }
        public TextBox TxtDestination { get; set; }
        public TextBox TxtAcc { get; set; }
        public TextBox TxtTransport { get; set; }
        public TextBox TxtActivities { get; set; }
        public TextBox TxtGuide { get; set; }
        public NumericUpDown NumDuration { get; set; }
        public TextBox TxtShip { get; set; }
        public TextBox TxtRoute { get; set; }
        public DateTimePicker DtDeparture { get; set; }
        public TextBox TxtCabin { get; set; }
    }
}
