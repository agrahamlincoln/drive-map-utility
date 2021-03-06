﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Configuration;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Linq;

namespace drive_map_utility
{
    /// <summary>This class stores all information regarding JSON reading, writing, and operations on the data from these files.
    /// </summary>
    public class json
    {
        const string LIST_SHARES_FILENAME = "knownshares.json";
        const string LIST_USERS_FILENAME = "users.json";

        static string USERS_JSON_FULL_FILEPATH = Program.readFromAppConfig("usersJson_Path") + "\\" + LIST_USERS_FILENAME;
        static string SHARES_JSON_FULL_FILEPATH = Program.readFromAppConfig("knownsharesJson_Path") + "\\" + LIST_SHARES_FILENAME;

        //Class Variables
        private static List<Fileshare> knownShares = enumKnownShares();
        private static List<User> knownUsers = enumUsers();

        /// <summary>jsonKnownShares -- List of all shares enumerated from the knownshares.json file.
        /// </summary>
        public static List<NetworkDrive> jsonKnownShares = ListConvert(knownShares);

        /// <summary>Utility Class Fileshare stores json serialized objects with Fileshare information.
        /// </summary>
        public class Fileshare
        {
            // Reads from the json files and set the values below
            [JsonProperty("id")]
            public int id { get; set; }
            public string name { get; set; }
            public string server { get; set; }
            public string folder { get; set; }
            public string domain { get; set; }

            public NetworkDrive convertToNetworkDrive()
            {
                // Setting the sharename in NetworkDrive.cs to \\\\-servername-\\-foldername- (accessed from above code snipit)
                NetworkDrive netDrive = new NetworkDrive();
                netDrive.ShareName = "\\\\" + server + "\\" + folder;

                return netDrive;
            }

            public NetworkDrive convertToNetworkDrive(string driveLetter)
            {
                // Sets the drive letter
                NetworkDrive netDrive = this.convertToNetworkDrive();
                netDrive.LocalDrive = driveLetter;
                return netDrive;
            }

        }

        /// <summary>Utility class User stores json serialized objects with User information.
        /// </summary>
        public class User
        {
            [JsonProperty("username")]
            //Class Variables
            public string username { get; set; }
            public List<string> fileshares { get; set; }

            public User(string uname)
            {
                // Sets the username
                this.username = uname;
                this.fileshares = new List<string>();
            }

            public List<NetworkDrive> convertToNetworkDriveList()
            {
                //Regex to parse the strings from file
                Regex numMatch = new Regex("[0-9]+"); //matches all numbers
                Regex driveLetterMatch = new Regex("[A-Z]$"); //matches a single capital letter

                //List to convert
                List<NetworkDrive> netDrives = new List<NetworkDrive>();
                //List from file to match
                List<Fileshare> sharesFromFile = enumKnownShares();

                //Utility variables for this loop
                string idFromUser;
                string driveLetter;
                string idFromFile;
                //Iterate through both lists, matching shares on ID
                foreach (Fileshare share in sharesFromFile)
                {
                    foreach (string shareString in fileshares)
                    {
                        //only process if the text file has properly formatted share strings.
                        if (driveLetterMatch.IsMatch(shareString) && numMatch.IsMatch(shareString))
                        {
                            //set strings
                            idFromUser = numMatch.Match(shareString).ToString();
                            driveLetter = driveLetterMatch.Match(shareString).ToString();
                            idFromFile = share.id.ToString();

                            //match up the two id's
                            if (idFromUser.Equals(idFromFile))
                                netDrives.Add(share.convertToNetworkDrive(driveLetter));
                        }
                    }
                }
                return netDrives;
            }
        }

        #region Read from JSON file

        /// <summary>Deserializes the Json String from known shares file
        /// </summary>
        /// <returns>Fileshare object List of all known shares</returns>
        private static List<Fileshare> enumKnownShares()
        {
            //List of fileshares to return
            List<Fileshare> knownShares = new List<Fileshare>();

            try
            {
                string jsonString = Utilities.readFile(SHARES_JSON_FULL_FILEPATH);
                knownShares = JsonConvert.DeserializeObject<List<Fileshare>>(jsonString);
            }
            catch
            {
                Utilities.writeLog("Error: unable to read knownshares.json file");
            }

            return knownShares;
        }

        /// <summary>Deserializes the Json String from the users file
        /// </summary>
        /// <returns>User object List of all users from the users json file.</returns>
        private static List<User> enumUsers()
        {
            List<User> users = new List<User>();
            try
            {
                string jsonString = Utilities.readFile(USERS_JSON_FULL_FILEPATH);
                if (jsonString != "")
                {
                    users = JsonConvert.DeserializeObject<List<User>>(jsonString);
                }
                else
                {
                    //instantiate the object
                    users = new List<User>();
                    //add the current user.
                    users.Add(new User(Environment.UserName));
                }
            }
            catch
            {
                Utilities.writeLog("Error: unable to read users.json file");
            }

            return users;
        }

        /// <summary>Gets the next fileshare ID in the list
        /// </summary>
        /// <returns>ID to assign to a fileshare</returns>
        public static int getNewID()
        {
            // Sets up new ID as the next element in the knowShares
            int newID;
            int lastID = knownShares[knownShares.Count - 1].id;
            newID = lastID + 1;
            return newID;
        }

        #endregion

        /// <summary>Matches a network drive to a fileshare (serialized) object.
        /// </summary>
        /// <remarks>Returns null if nothing found.</remarks>
        /// <param name="netDrive">Network drive to match</param>
        /// <returns>Fileshare object that the netdrive matches.</returns>
        public static Fileshare matchNetDrivetoKnownFileshare(NetworkDrive netDrive)
        {
            // Checks to see if the known share is the same as the netDrive
            Fileshare returnShare = knownShares.Find(share => share.convertToNetworkDrive() == netDrive);

            if (returnShare == null)
            {
                Utilities.writeLog("Could not match NetDrive to share, returning null");

            }

            return returnShare;
        }

        #region Users Json

        /// <summary>Retrieves the list of network drives for the currently logged in user.
        /// </summary>
        /// <returns>Network Drive List of all network drives related to the current user.</returns>
        public static List<NetworkDrive> getUserDrivesFromJson()
        {
            List<NetworkDrive> userDrives = new List<NetworkDrive>();
            int currentUserIndex = getIndexofCurrentlyLoggedInUser();
            if (currentUserIndex == -1)
            {
                //there is no user in the userfile.
                //check that the knownUsers is instanciated
                if (knownUsers == null)
                    knownUsers = new List<User>();

                //create a new user to begin writing to.
                knownUsers.Add(new User(Environment.UserName));
                //then try again.
                currentUserIndex = getIndexofCurrentlyLoggedInUser();
            }

            userDrives = knownUsers[currentUserIndex].convertToNetworkDriveList();

            if (userDrives == null)
            {
                Utilities.writeLog("Could not find a user or any network drives");
            }

            return userDrives;
        }

        /// <summary>Returns integer of index of the current user in the userList
        /// </summary>
        /// <returns>index of the current user in the userlist. Will return -1 if none found.</returns>
        public static int getIndexofCurrentlyLoggedInUser()
        {
            int userIndex;
            try 
            {
                userIndex = knownUsers.FindIndex(user => Utilities.matchString_IgnoreCase(user.username, Environment.UserName));
            }
            catch
            {
                //failed to find a user in the userfile
                userIndex = -1;
                Utilities.writeLog("Failed to find a user in the userfile.");
            }
            return userIndex;
        }

        /// <summary>Converts a list of Fileshares to a List of Network Drives
        /// </summary>
        /// <param name="fileshares">List of Fileshares to Convert</param>
        /// <returns>List of Network Drives</returns>
        public static List<NetworkDrive> ListConvert(List<Fileshare> fileshares)
        {
            List<NetworkDrive> netDrives = new List<NetworkDrive>();

            foreach (Fileshare share in fileshares)
            {
                netDrives.Add(new NetworkDrive("\\\\" + share.server + "\\" + share.folder));
            }
            return netDrives;
        }

        /// <summary>Updates the Users.json file, matches the user object to the current user drives and writes to file.
        /// </summary>
        public static void updateUsersJson()
        {
            //serialize json file from users
            List<User> allUsersFromFile = enumUsers();

            //find the current user object
            int currentUserIndex = getIndexofCurrentlyLoggedInUser();
            if (currentUserIndex == -1)
            {
                //no user exists; create one
                allUsersFromFile.Add(new User(Environment.UserName));
                currentUserIndex = getIndexofCurrentlyLoggedInUser();
            }

            //edit the User Object
            List<NetworkDrive> currentUserShares = Local.userDrives;
            foreach (NetworkDrive drive in currentUserShares)
            {
                string shareID = drive.convertToIdentifier();
                allUsersFromFile[currentUserIndex].fileshares.Add(shareID);
            }

            //write json back to file (overwrite)
            string jsonString = JsonConvert.SerializeObject(allUsersFromFile);
            StreamWriter sWriter = new StreamWriter(USERS_JSON_FULL_FILEPATH);
            sWriter.Write(jsonString);
            sWriter.Close();
        }

        public static void addNewFileshare(Fileshare fsObject)
        {
        }

        #endregion

    }
}
