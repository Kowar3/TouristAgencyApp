using TouristAgencyApp.Forms;
using TouristAgencyApp.Services;

namespace TouristAgencyApp
{
    static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();

            Application.Run(new StartupForm());
        }
    }
}