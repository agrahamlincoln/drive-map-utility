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

        /* Create list of fileshares from knownshares file
         * Returns List of "Fileshare" objects that represent all known fileshares.
         */
        private static List<Fileshare> enumKnownShares()
        {
            //List of fileshares to return
            List<Fileshare> knownShares = new List<Fileshare>();

            try
            {
                //Get path of knownshares.json file
                AppSettingsReader appConfig = new AppSettingsReader();
                string knownsharesJson_path = (string)appConfig.GetValue("knownsharesJson_Path", typeof(string));

                //Read json from file on network
                StreamReader userFile = new StreamReader(knownsharesJson_path + "\\" + LIST_SHARES_FILENAME);
                string jsonFile = userFile.ReadToEnd();

                knownShares = JsonConvert.DeserializeObject<List<Fileshare>>(jsonFile);
            }
            catch
            {
                ProgramUtils.writeLog("Error: unable to read knownshares.json file");
            }

            return knownShares;
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

        /* Enumerates list of all users from file
         * Stores all the users as a "User" object
         * Returns: List<User> users from file.
         */
        private static List<User> enumUsers()
        {
            List<User> users = new List<User>();

            //Get path of users.json file
            AppSettingsReader appConfig = new AppSettingsReader();
            try
            {
                string usersJson_path = (string)appConfig.GetValue("usersJson_Path", typeof(string));

                //Read json from file on network
                StreamReader userFile = new StreamReader(usersJson_path + "\\" + LIST_USERS_FILENAME);
                string jsonFile = userFile.ReadToEnd();

                users = JsonConvert.DeserializeObject<List<User>>(jsonFile);
            }
            catch
            {
                ProgramUtils.writeLog("Error: unable to read users.json file");
            }

            return users;
        }

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
    }
}
