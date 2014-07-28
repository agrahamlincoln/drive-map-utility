﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Management;

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
