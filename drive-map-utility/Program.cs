using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace drive_map_utility
{
    /// <summary>Stores all Program-Specific Methods
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main());
        }

        #region utilities

        /// <summary>Converts NetworkDrive list to list of strings.
        /// </summary>
        /// <param name="netDrives">list of Network Drive objects</param>
        /// <returns>List of network drive objects in string format.</returns>
        public static List<string> convertToStringList(List<NetworkDrive> netDrives)
        {
            List<string> stringList = new List<string>();
            foreach (NetworkDrive drive in netDrives)
            {
                stringList.Add(drive.ShareName);
            }
            return stringList;
        }

        /// <summary>Matches a full network path to a Network Drive object in the currently logged in user's list.
        /// </summary>
        /// <remarks>Returns null if none found.</remarks>
        /// <param name="fullPath">Full Network Path</param>
        /// <returns>Network Drive object that matches the passed path.</returns>
        public static NetworkDrive findDriveInList(string fullPath, List<NetworkDrive> netDrives)
        {
            NetworkDrive match;
            match = netDrives.Find(share => Utilities.matchString_IgnoreCase(share.ShareName, fullPath));
            return match;
        }
        #endregion

        public static string readFromAppConfig(string customKey)
        {
            AppSettingsReader appConfig = new AppSettingsReader();
            return appConfig.GetValue(customKey, typeof(string)).ToString();
        }
    }
}
