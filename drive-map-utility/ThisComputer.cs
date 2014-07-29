using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;
using System.IO;
using System.Text.RegularExpressions;

namespace drive_map_utility
{
    class ThisComputer
    {
        //Class variables
        public static List<NetworkDrive> currentlyMappedShares = getCurrentlyMappedDrives();
        public static List<NetworkDrive> jsonUsersFile = json.getUserDrivesFromJson();
        public static List<NetworkDrive> jsonKnownShares = json.getKnownSharesFromJson();

        private static List<NetworkDrive> getCurrentlyMappedDrives()
        {
            List<NetworkDrive> mappedDrives = new List<NetworkDrive>();
            try
            {
                ManagementObjectSearcher searcher =
                new ManagementObjectSearcher("root\\CIMV2",
                "SELECT * FROM Win32_NetworkConnection");

                string fullPath;
                string driveLetter;

                //Enumerate all network drives and store in ArrayList object.
                foreach (ManagementObject queryObj in searcher.Get())
                {
                    //get information using WMI
                    driveLetter = String.Format("{0}", queryObj["LocalName"]);
                    fullPath = String.Format("{0}", queryObj["RemotePath"]);

                    //create new object storing information from WMI
                    mappedDrives.Add(new NetworkDrive(driveLetter, fullPath));
                }
            }
            catch (ManagementException e)
            {
                ProgramUtils.writeLog("An error occurred while querying for WMI data: " + e.Message);
            }
            return mappedDrives;
        }

        public static string getNextAvailableDriveLetter()
        {
            // Allocate space for alphabet
            List<char> driveLetters = new List<char>(26); 

            // increment from ASCII values for A-Z
            for (int i = 65; i < 91; i++) 
            {
                // Add uppercase letters to possible drive letters
                driveLetters.Add(Convert.ToChar(i)); 
            }

            foreach (string drive in Directory.GetLogicalDrives())
            {
                // removed used drive letters from possible drive letters
                driveLetters.Remove(drive[0]); 
            }

            //return the first available drive letter
            return driveLetters[0].ToString(); 
        }

        public static bool isDriveLetterAvailable(string driveLetter)
        {
            bool isAvailable = true;
            Regex singleLetter = new Regex("^([A-z])");

            //validate driveLetter string parameter
            if (singleLetter.IsMatch(driveLetter))
            {
                //get first character only
                driveLetter = singleLetter.Match(driveLetter).ToString();
            }
            else
            {
                throw new ArgumentException("Error: Invalid driveletter passed to method");
            }

            foreach (string drive in Directory.GetLogicalDrives())
            {
                if (driveLetter.Equals(drive[0]))
                    isAvailable = false;
            }

            return isAvailable;
        }

        public static bool isMapped(NetworkDrive drive)
        {
            bool isMapped = false;
            List<NetworkDrive> currentDrives = getCurrentlyMappedDrives();

            foreach (NetworkDrive currentDrive in currentDrives)
                if (drive.ShareName.Equals(currentDrive.ShareName))
                    isMapped = true;

            return isMapped;
        }
    }
}
