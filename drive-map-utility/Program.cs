using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace drive_map_utility
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //local shared data
            List<NetworkDrive> currentlyMappedShares = ThisComputer.getCurrentlyMappedDrives();
            List<NetworkDrive> jsonUsersFile = json.getCurrentUserDrivesFromJson();
            List<NetworkDrive> jsonKnownShares = json.getKnownSharesFromJson();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }


    }
}
