using System.Windows.Forms;

namespace TouristAgencyApp.Utils
{
    public class ReservationAddContext
    {
        public Form Form { get; set; }
        public Button BtnSave { get; set; }
        public ComboBox CbDestinations { get; set; }
        public ComboBox CbPackages { get; set; }
        public NumericUpDown NumPersons { get; set; }
    }
}
