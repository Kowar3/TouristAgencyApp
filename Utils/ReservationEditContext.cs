using System.Windows.Forms;

namespace TouristAgencyApp.Utils
{
    public class ReservationEditContext
    {
        public Form Form { get; set; }
        public Button BtnSave { get; set; }
        public NumericUpDown NumPersons { get; set; }
        public int ReservationId { get; set; }
        public string PackageName { get; set; }
    }
}
