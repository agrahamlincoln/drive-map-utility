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
        public static List<NetworkDrive> jsonCurrentUserDrives = json.getUserDrivesFromJson();
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

        #region Drive Letter Checking

        public static List<char> getAvailableDriveLetters()
        {
            List<char> driveLetters = getAlphabetUppercase();

            foreach (string drive in Directory.GetLogicalDrives())
            {
                // removed used drive letters from possible drive letters
                driveLetters.Remove(drive[0]); 
            }

            return driveLetters;
        }

        /** Returns charAt(1) if  there are no available drive letters.
        */
        public static char getNextAvailableDriveLetter(string driveLetter)
        {
            char driveLetterChar = parseLetter(driveLetter);
            return ogetNextAvailableDriveLetter(driveLetterChar);
        }

        /** Returns charAt(1) if  there are no available drive letters.
        */
        public static char getNextAvailableDriveLetter(char driveLetter)
        {
            return ogetNextAvailableDriveLetter(driveLetter);
        }

        private static char ogetNextAvailableDriveLetter(char driveLetter)
        {
            char availableLetter = Convert.ToChar(1);
            for (int i = driveLetter + 1; i < 91; i++)
            {
                //starting at the driveletter char value
                if (isDriveLetterAvailable(Convert.ToChar(i)))
                {
                    availableLetter = Convert.ToChar(i);
                    break;
                }
            }
            return availableLetter;
        }

        public static bool isDriveLetterAvailable(string driveLetter)
        {
            char driveLetterChar = parseLetter(driveLetter);
            return oisDriveLetterAvailable(driveLetterChar);
        }

        public static bool isDriveLetterAvailable(char driveLetter)
        {
            return oisDriveLetterAvailable(driveLetter);
        }

        private static bool oisDriveLetterAvailable(char driveLetter)
        {
            bool isAvailable = true;
            foreach (string drive in Directory.GetLogicalDrives())
            {
                if (driveLetter.Equals(drive[0]))
                    isAvailable = false;
            }

            return isAvailable;
        }

        #endregion

        public static NetworkDrive matchPathToUserDrive(string fullPath)
        {
            NetworkDrive match;
            match = ThisComputer.jsonCurrentUserDrives.Find(share => share.ShareName == fullPath);
            return match;
        }

        public static NetworkDrive matchPathtoKnownDrive(string fullPath)
        {
            NetworkDrive match;
            match = ThisComputer.jsonKnownShares.Find(share => share.ShareName == fullPath);
            return match;
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

        #region Utility Methods

        private static List<char> getAlphabetUppercase()
        {
            // Allocate space for alphabet
            List<char> alphabet = new List<char>(26);

            // increment from ASCII values for A-Z
            for (int i = 65; i < 91; i++)
            {
                // Add uppercase letters to possible drive letters
                alphabet.Add(Convert.ToChar(i));
            }
            return alphabet;
        }

        private static char parseLetter(string driveLetter)
        {
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

            return Convert.ToChar(driveLetter);
        }

        public static List<string> convertToList(List<NetworkDrive> netDrives)
        {
            List<string> stringList = new List<string>();
            foreach (NetworkDrive drive in netDrives)
            {
                stringList.Add(drive.ShareName);
            }
            return stringList;
        }

        #endregion
    }
}
