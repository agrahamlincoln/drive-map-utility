using Newtonsoft.Json;
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
    public class json
    {
        const string LIST_SHARES_FILENAME = "knownshares.json";
        const string LIST_USERS_FILENAME = "users.json";

        static string USERS_JSON_FULL_FILEPATH = Program.readFromAppConfig("usersJson_Path") + "\\" + LIST_USERS_FILENAME;
        static string SHARES_JSON_FULL_FILEPATH = Program.readFromAppConfig("knownsharesJson_Path") + "\\" + LIST_SHARES_FILENAME;

        //Class Variables
        private static List<Fileshare> knownShares = enumKnownShares();
        private static List<User> knownUsers = enumUsers();
        public static List<NetworkDrive> jsonKnownShares = ListConvert(knownShares); //to be moved into json class

        public class Fileshare
        {
            [JsonProperty("id")]
            public int id { get; set; }
            public string name { get; set; }
            public string server { get; set; }
            public string folder { get; set; }
            public string domain { get; set; }

            public NetworkDrive convertToNetworkDrive()
            {
                NetworkDrive netDrive = new NetworkDrive();
                netDrive.ShareName = "\\\\" + server + "\\" + folder;

                return netDrive;
            }

            public NetworkDrive convertToNetworkDrive(string driveLetter)
            {
                NetworkDrive netDrive = this.convertToNetworkDrive();
                netDrive.LocalDrive = driveLetter;
                return netDrive;
            }

        }

        public class User
        {
            [JsonProperty("username")]
            //Class Variables
            public string username { get; set; }
            public List<string> fileshares { get; set; }
            public List<NetworkDrive> fileshares_Obj { get; set; }

            public User(string uname)
            {
                this.username = uname;
            }

            public List<NetworkDrive> convertToNetworkDrive()
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
        /* Create list of fileshares from knownshares file
         * Returns List of "Fileshare" objects that represent all known fileshares.
         */
        private static List<Fileshare> enumKnownShares()
        {
            //List of fileshares to return
            List<Fileshare> knownShares = new List<Fileshare>();

            try
            {
                string jsonString = getJsonStringFromKnownFileshares();
                knownShares = JsonConvert.DeserializeObject<List<Fileshare>>(jsonString);
            }
            catch
            {
                Utilities.writeLog("Error: unable to read knownshares.json file");
            }

            return knownShares;
        }

        /* Enumerates list of all users from file
         * Stores all the users as a "User" object
         * Returns: List<User> users from file.
         */
        private static List<User> enumUsers()
        {
            List<User> users = new List<User>();
            try
            {
                string jsonString = getJsonStringFromUsers();
                users = JsonConvert.DeserializeObject<List<User>>(jsonString);
            }
            catch
            {
                Utilities.writeLog("Error: unable to read users.json file");
            }

            return users;
        }

        private static string getJsonStringFromUsers()
        {
            string fullPath = USERS_JSON_FULL_FILEPATH;
            return getJsonString(fullPath);
        }

        private static string getJsonStringFromKnownFileshares()
        {
            string fullPath = SHARES_JSON_FULL_FILEPATH;
            return getJsonString(fullPath);
        }

        private static string getJsonString(string fullPath)
        {
            //Read json from file on network
            StreamReader file = new StreamReader(fullPath);
            return file.ReadToEnd();
        }

        public static int getNewID()
        {
            int newID;
            int lastID = knownShares[knownShares.Count - 1].id;
            newID = lastID + 1;
            return newID;
        }

        #endregion

        #region Data Processing

        public static Fileshare matchNetDrivetoKnownFileshare(NetworkDrive netDrive)
        {
            Fileshare returnShare = knownShares.Find(share => share.convertToNetworkDrive() == netDrive);

            if (returnShare == null)
            {
                Utilities.writeLog("Could not match NetDrive to share, returning null");

            }

            return returnShare;
        }

        public static List<NetworkDrive> getUserDrivesFromJson()
        {
            List<NetworkDrive> userDrives = null;

            //Get the currently logged in users' object
            int userIndex = findIndexOfCurrentlyLoggedInUserObj();

            userDrives = knownUsers[userIndex].convertToNetworkDrive();

            if (userDrives == null)
            {
                Utilities.writeLog("Could not find a user or any network drives");
            }

            return userDrives;
        }

        /**
         * Returns -1 if it cannot find the index
        */
        private static int findIndexOfCurrentlyLoggedInUserObj()
        {
            int userIndex;

            List<User> users = json.enumUsers();
            userIndex = users.FindIndex(user => Utilities.matchString_IgnoreCase(user.username, Environment.UserName));

            return userIndex;
        }

        public static List<NetworkDrive> ListConvert(List<Fileshare> fileshares)
        {
            List<NetworkDrive> netDrives = new List<NetworkDrive>();

            foreach (Fileshare share in fileshares)
            {
                netDrives.Add(new NetworkDrive("\\\\" + share.server + "\\" + share.folder));
            }
            return netDrives;
        }
        #endregion

        #region modify methods



        public void updateUsersJson()
        {
            //serialize json file from users
            List<User> allUsersFromFile = enumUsers();

            //find the current user object
            int userIndex = findIndexOfCurrentlyLoggedInUserObj();
            if (userIndex == -1)
            {
                //no user exists; create one
                allUsersFromFile.Add(new User(Environment.UserName));
            }

            //edit the User Object
            List<NetworkDrive> currentUserShares = Local.jsonCurrentUserDrives;
            foreach (NetworkDrive drive in currentUserShares)
            {
                string shareID = drive.convertToIdentifier();
                allUsersFromFile[userIndex].fileshares.Add(shareID);
            }

            //write json back to file (overwrite)
            string jsonString = JsonConvert.SerializeObject(allUsersFromFile);
            StreamWriter sWriter = new StreamWriter(USERS_JSON_FULL_FILEPATH);
            sWriter.Write(jsonString, true);
        }

        #endregion

    }
}
