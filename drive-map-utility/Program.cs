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

            //find all known shares that are not mapped
            List<NetworkDrive> sharesToBeMapped = Local.getMappedStatus(Local.userDrives, false);

            //attempt to map shares without passwords
            mapList(sharesToBeMapped);
            
            //prompt user for remaining share logins
            sharesToBeMapped = Local.getMappedStatus(Local.userDrives, false);

            //prompt for password and then map shares.
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


        /// <summary>Maps a list of network drives (No Credentials)
        /// </summary>
        /// <param name="listOfDrives">List of Network Drives to be mapped</param>
        public static void mapList(List<NetworkDrive> listOfDrives)
        {
            foreach (NetworkDrive drive in listOfDrives)
            {
                // Ask for credentials to have access to the drive
                //drive.PromptForCredentials = true;
                if (Local.isDriveLetterAvailable(drive.LocalDrive))
                {
                    drive.MapDrive();
                }
                else
                {
                    try
                    {
                        drive.LocalDrive = Local.getNextAvailableDriveLetter(drive.LocalDrive).ToString();
                        drive.MapDrive();
                    }
                    catch (Exception e)
                    {
                        Utilities.writeLog(e.ToString());
                    }
                    
                }
            }
        }

        /// <summary>Maps a list of network drives (Password Provided)
        /// </summary>
        /// <param name="listOfDrives">List of Network Drives to be mapped.</param>
        /// <param name="password">Password Provided</param>
        public static void mapList(List<NetworkDrive> listOfDrives, string password)
        {
            foreach (NetworkDrive drive in listOfDrives)
            {
                // Takes in the password
                drive.MapDrive(password);
            }
        }

        /// <summary>Maps a list of network drives (Credentials Provided)
        /// </summary>
        /// <param name="listOfDrives">List of Network Drives to be mapped.</param>
        /// <param name="username">username provided</param>
        /// <param name="password">password provided</param>
        public static void mapList(List<NetworkDrive> listOfDrives, string username, string password)
        {
            foreach (NetworkDrive drive in listOfDrives)
            {
                // Takes in the username and password
                drive.MapDrive(username, password);
            }
        }
    }
}
