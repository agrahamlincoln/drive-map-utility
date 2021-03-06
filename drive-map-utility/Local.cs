﻿using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Text.RegularExpressions;

namespace drive_map_utility
{
    /// <summary>This class is used to store and gather information about the current machine that the application is running on.
    /// </summary>
    /// <remarks>Includes: Lists of currently mapped shares, .NET versions, Available Drive Letters
    /// Excludes: Information from files, utility methods (methods that do not gather/report computer information)
    /// </remarks>
    class Local
    {
        public static List<NetworkDrive> currentlyMappedDrives = getCurrentlyMappedDrives();
        public static List<NetworkDrive> userDrives = json.getUserDrivesFromJson();

        /// <summary>Adds a Network Drive object to the current user's json object
        /// </summary>
        /// <param name="netDrive"></param>
        public void addFileshare(NetworkDrive netDrive)
        {
            if (netDrive.LocalDrive == "" || netDrive.LocalDrive == null)
            {
                //no drive letter found on this network drive object
                //Set Drive letter to the one found in knownshares.json
                NetworkDrive matched = Program.findDriveInList(netDrive.ShareName, json.jsonKnownShares);
                netDrive.LocalDrive = matched.LocalDrive;
            }

            Local.userDrives.Add(netDrive);
        }


        #region Computer Information Queries

        /// <summary>Checks the current machine for .NET Installations
        /// </summary>
        /// <returns>Boolean value representing whether .NET 3.5 or greater is installed or not.</returns>
        public static bool HasNet35()
        {
            bool returnValue = false;
            List<string> versions = GetVersionFromRegistry();
            Regex netVersion = new Regex("(v3\\.5|v4.0|v4)+");

            foreach (string ver in versions)
            {
                if (netVersion.IsMatch(ver))
                {
                    Utilities.writeLog(".NET version " + ver + " is installed.");
                    returnValue = true;
                    break;
                }
            }
            if (returnValue == false)
                Utilities.writeLog(".NET 3.5 must be installed to run this.");
            return returnValue;
        }

        /// <summary>Checks registry on this machine for keys added during .NET installs.
        /// </summary>
        /// <returns>string List of .NET versions installed.</returns>
        public static List<string> GetVersionFromRegistry()
        {
            List<string> versions = new List<string>();

            // Opens the registry key for the .NET Framework entry. 
            using (RegistryKey ndpKey =
                RegistryKey.OpenRemoteBaseKey(RegistryHive.LocalMachine, "").
                OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
            {
                // As an alternative, if you know the computers you will query are running .NET Framework 4.5  
                // or later, you can use: 
                // using (RegistryKey ndpKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine,  
                // RegistryView.Registry32).OpenSubKey(@"SOFTWARE\Microsoft\NET Framework Setup\NDP\"))
                foreach (string versionKeyName in ndpKey.GetSubKeyNames())
                {
                    if (versionKeyName.StartsWith("v"))
                    {

                        RegistryKey versionKey = ndpKey.OpenSubKey(versionKeyName);
                        string name = (string)versionKey.GetValue("Version", "");
                        string sp = versionKey.GetValue("SP", "").ToString();
                        string install = versionKey.GetValue("Install", "").ToString();
                        if (install == "")
                        { //no install info, must be later.
                            //writeLog(versionKeyName + "  " + name);
                            versions.Add(versionKeyName);
                        }
                        else
                        {
                            if (sp != "" && install == "1")
                            {
                                //writeLog(versionKeyName + "  " + name + "  SP" + sp);
                                versions.Add(versionKeyName);
                            }

                        }
                        if (name != "")
                        {
                            continue;
                        }
                        foreach (string subKeyName in versionKey.GetSubKeyNames())
                        {
                            RegistryKey subKey = versionKey.OpenSubKey(subKeyName);
                            name = (string)subKey.GetValue("Version", "");
                            if (name != "")
                                sp = subKey.GetValue("SP", "").ToString();
                            install = subKey.GetValue("Install", "").ToString();
                            if (install == "")
                            { //no install info, must be later.
                                //writeLog(versionKeyName + "  " + name);
                            }
                            else
                            {
                                if (sp != "" && install == "1")
                                {
                                    //writeLog("  " + subKeyName + "  " + name + "  SP" + sp);
                                }
                                else if (install == "1")
                                {
                                    //writeLog("  " + subKeyName + "  " + name);
                                }
                            }
                        }
                    }
                }
            }
            return versions;
        }

        /// <summary>Read all network connections using WMI and generate a list of Network Drives
        /// </summary>
        /// <returns>NetworkDrive List -- All Currently mapped drives on this machine.</returns>
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
                    driveLetter = String.Format("{0}", queryObj["LocalName"]).ToUpper();
                    fullPath = String.Format("{0}", queryObj["RemotePath"]);

                    //create new object storing information from WMI
                    mappedDrives.Add(new NetworkDrive(driveLetter, fullPath));
                }
            }
            catch (ManagementException e)
            {
                Utilities.writeLog("An error occurred while querying for WMI data: " + e.Message);
            }
            return mappedDrives;
        }

        /// <summary>Removes a specific object from the currently mapped drives list.
        /// </summary>
        /// <param name="driveLetter">Drive to remove from list.</param>
        public static void unmapDrive(string driveLetter)
        {
            int indexof = currentlyMappedDrives.FindIndex(drive => Utilities.matchString_IgnoreCase(drive.LocalDrive, driveLetter));
            currentlyMappedDrives.RemoveAt(indexof);
        }

        /// <summary>Checks whether this network drive is mapped or not.
        /// </summary>
        /// <param name="drive">Network Drive to check.</param>
        /// <returns>Boolean value of whether drive is mapped or not.</returns>
        public static bool hasMapping(NetworkDrive drive)
        {
            // Checks to see if the current drive is mapped already
            bool isMapped = false;

            NetworkDrive foundObject;
            foundObject = currentlyMappedDrives.Find(mappedDrive => Utilities.matchString_IgnoreCase(mappedDrive.ShareName, drive.ShareName));
            if (foundObject != null)
                isMapped = true;

            return isMapped;
        }

        /// <summary>Returns a list of Network Drives that are either mapped or unmapped, depending on parameter input
        /// </summary>
        /// <param name="driveList">Primary list of Network Drives.</param>
        /// <param name="isMapped">Whether to return drives that are mapped or unmapped.</param>
        /// <returns>List of network drives that are either mapped or unmapped. depending on parameter input.</returns>
        public static List<NetworkDrive> getMappedStatus(List<NetworkDrive> driveList, bool isMapped)
        {
            List<NetworkDrive> matchedDrives = new List<NetworkDrive>();
            foreach (NetworkDrive share in driveList)
            {
                //if the share matches the bool value currently mapped
                if (Local.hasMapping(share) == isMapped)
                {
                    matchedDrives.Add(share);
                }
            }

            return matchedDrives;
        }

        #endregion

        #region Drive Letters

        /// <summary>Verifies and returns a completely valid drive letter.
        /// </summary>
        /// <param name="driveLetter">Drive Letter to verify</param>
        /// <returns>A valid drive letter, either the next available, the current or CharAt(1) if there are no available drive letters.</returns>
        public static char driveLetterVerify(string driveLetter)
        {
            char validLetter = Convert.ToChar(1);
            char passedLetter;

            //1. Attempt to parse a drive letter from the string.
            try
            {
                passedLetter = Utilities.parseSingleLetter(driveLetter);
            }
            catch (ArgumentException)
            {
                //Failed to find a drive letter from this string
                //Return the first capital letter in the alphabet.
                passedLetter = 'A';
            }

            //Get the next available drive letter
            try
            {
                validLetter = getNextAvailableDriveLetter(passedLetter);
            }
            catch (Exception e)
            {
                throw new Exception("Could not find any available/valid drive letters.\nCall Stack: " + e.ToString());
            }
            return validLetter;
        }

        /// <summary>Lists all available Drive Letters on this machine</summary>
        /// <returns> char List -- All Available drive letters on this machine</returns>
        public static List<char> getAvailableDriveLetters()
        {
            List<char> driveLetters = Utilities.getAlphabetUppercase();

            foreach (string drive in Directory.GetLogicalDrives())
            {
                // removed used drive letters from possible drive letters
                driveLetters.Remove(drive[0]);
            }

            return driveLetters;
        }

        /// <summary>Retrieves the next available drive letter after a specified letter
        /// </summary>
        /// <param name="driveLetter">Drive letter to start at</param>
        /// <returns>Next Available Drive Letter, Will return itself if current drive letter is available.</returns>
        public static char getNextAvailableDriveLetter(string driveLetter)
        {
            char driveLetterChar = Utilities.parseSingleLetter(driveLetter);
            return ogetNextAvailableDriveLetter(driveLetterChar);
        }

        /// <summary>Retrieves the next available drive letter after a specified letter
        /// </summary>
        /// <param name="driveLetter">Drive letter to start at</param>
        /// <returns>Next Available Drive Letter, Will return itself if current drive letter is available.</returns>
        public static char getNextAvailableDriveLetter(char driveLetter)
        {
            return ogetNextAvailableDriveLetter(driveLetter);
        }

        /// <summary>Method that performs operations of overloaded methods to retrieve next available drive letter.
        /// </summary>
        /// <param name="driveLetter">Drive Letter to start at (in char format)</param>
        /// <returns>char -- Next Available Drive Letter</returns>
        private static char ogetNextAvailableDriveLetter(char driveLetter)
        {
            char availableLetter = Convert.ToChar(1);
            for (int i = driveLetter; i < 91; i++)
            {
                //starting at the driveletter char value
                if (isDriveLetterAvailable(Convert.ToChar(i)))
                {
                    availableLetter = Convert.ToChar(i);
                    break;
                }
            }
            if (availableLetter == Convert.ToChar(1))
            {
                throw new Exception("Could not find an available drive letter");
            }
            return availableLetter;
        }

        /// <summary>Verifies whether a drive letter is available or not.
        /// </summary>
        /// <param name="driveLetter">Drive letter to check</param>
        /// <returns>boolean value of whether drive letter is available or not.</returns>
        public static bool isDriveLetterAvailable(string driveLetter)
        {
            char driveLetterChar = Utilities.parseSingleLetter(driveLetter);
            return oisDriveLetterAvailable(driveLetterChar);
        }

        /// <summary>Verifies whether a drive letter is available or not.
        /// </summary>
        /// <param name="driveLetter">Drive letter to check</param>
        /// <returns>boolean value of whether drive letter is available or not.</returns>
        public static bool isDriveLetterAvailable(char driveLetter)
        {
            return oisDriveLetterAvailable(driveLetter);
        }

        /// <summary>Method that performs operations of overloaded methods to verify whether a drive letter is available or not.
        /// </summary>
        /// <param name="driveLetter">Drive letter to check</param>
        /// <returns>Boolean value of whether drive letter is available or not.</returns>
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
    }
}
