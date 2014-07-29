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
    class json
    {
        const string LIST_SHARES_FILENAME = "knownshares.json";
        const string LIST_USERS_FILENAME = "users.json";

        private static List<Fileshare> knownShares = enumKnownShares();
        private static List<User> knownUsers = enumUsers();

        public class Fileshare
        {
            [JsonProperty("id")]
            public int id { get; set; }
            public string name { get; set; }
            public string server { get; set; }
            public string folder { get; set; }
            public string domain { get; set; }

            public NetworkDrive convert()
            {
                NetworkDrive netDrive = new NetworkDrive();
                netDrive.ShareName = "\\\\" + server + "\\" + folder;

                return netDrive;
            }

            public NetworkDrive convert(string driveLetter)
            {
                NetworkDrive netDrive = this.convert();
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

            public List<NetworkDrive> convertToList()
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
                                netDrives.Add(share.convert(driveLetter));
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
                ProgramUtils.writeLog("Error: unable to read knownshares.json file");
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
                ProgramUtils.writeLog("Error: unable to read users.json file");
            }

            return users;
        }

        private static string getJsonStringFromUsers()
        {
            string fullPath = Program.readFromAppConfig("usersJson_Path") + "\\" + LIST_USERS_FILENAME;
            return getJsonString(fullPath);
        }

        private static string getJsonStringFromKnownFileshares()
        {
            string fullPath = Program.readFromAppConfig("knownsharesJson_Path") + "\\" + LIST_SHARES_FILENAME;
            return getJsonString(fullPath);
        }

        private static string getJsonString(string fullPath)
        {
            //Read json from file on network
            StreamReader file = new StreamReader(fullPath);
            return file.ReadToEnd();
        }



        #endregion

        #region Data Processing
        public static List<NetworkDrive> getUserDrivesFromJson()
        {
            List<NetworkDrive> userDrives = null;

            //Get the currently logged in users' object
            List<User> users = json.enumUsers();
            foreach (User userObj in users)
            {
                if (userObj.username.Equals(Environment.UserName, StringComparison.OrdinalIgnoreCase))
                {
                    userDrives = userObj.convertToList();
                    break; //no need to continue the loop once a match is found
                }
            }

            if (userDrives == null)
            {
                ProgramUtils.writeLog("Could not find a user or any network drives");
            }

            return userDrives;
        }
        public static List<NetworkDrive> getKnownSharesFromJson()
        {
            List<NetworkDrive> knownShares = new List<NetworkDrive>();
            List<Fileshare> enumerated = enumKnownShares();

            foreach (Fileshare share in enumerated)
            {
                knownShares.Add(new NetworkDrive("\\\\" + share.server + "\\" + share.folder));
            }
            return knownShares;
        }
        #endregion

        #region modify methods

        public void addFileshare(NetworkDrive netDrive)
        {
            if (netDrive.LocalDrive == "" || netDrive.LocalDrive == null)
            {
                //no drive letter found on this network drive object
                //Set Drive letter to the one found in knownshares.json
                NetworkDrive matched = ThisComputer.jsonKnownShares.Find(share => share.ShareName == netDrive.ShareName);
                netDrive.LocalDrive = matched.LocalDrive;
            }

            JArray usersFile = JArray.Parse(getJsonStringFromUsers());

            //TO BE FINISHED
        }

        #endregion

    }
}
