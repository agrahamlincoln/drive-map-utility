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
        private static int currentUserIndex = getIndexofCurrentlyLoggedInUser();
        public static List<NetworkDrive> jsonKnownShares = ListConvert(knownShares); //to be moved into json class

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
            public List<NetworkDrive> fileshares_Obj { get; set; }

            public User(string uname)
            {
                // Sets the username
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

        /// <summary>Deserializes the Json String from known shares file
        /// </summary>
        /// <returns>Fileshare object List of all known shares</returns>
        private static List<Fileshare> enumKnownShares()
        {
            //List of fileshares to return
            List<Fileshare> knownShares = new List<Fileshare>();

            try
            {
                string jsonString = Utilities.readFile(USERS_JSON_FULL_FILEPATH);
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
                string jsonString = Utilities.readFile(SHARES_JSON_FULL_FILEPATH);
                users = JsonConvert.DeserializeObject<List<User>>(jsonString);
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
            int newID;
            int lastID = knownShares[knownShares.Count - 1].id;
            newID = lastID + 1;
            return newID;
        }

        #endregion

        #region Data Processing

        /// <summary>Matches a network drive to a fileshare (serialized) object.
        /// </summary>
        /// <remarks>Returns null if nothing found.</remarks>
        /// <param name="netDrive">Network drive to match</param>
        /// <returns>Fileshare object that the netdrive matches.</returns>
        public static Fileshare matchNetDrivetoKnownFileshare(NetworkDrive netDrive)
        {
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
            List<NetworkDrive> userDrives = null;

            userDrives = knownUsers[currentUserIndex].convertToNetworkDrive();

            if (userDrives == null)
            {
                Utilities.writeLog("Could not find a user or any network drives");
            }

            return userDrives;
        }

        /// <summary>Returns integer of index of the current user in the userList
        /// </summary>
        /// <returns>index of the current user in the userlist.</returns>
        public static int getIndexofCurrentlyLoggedInUser()
        {
            return knownUsers.FindIndex(user => Utilities.matchString_IgnoreCase(user.username, Local.localUser));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fileshares"></param>
        /// <returns></returns>
        public static List<NetworkDrive> ListConvert(List<Fileshare> fileshares)
        {
            List<NetworkDrive> netDrives = new List<NetworkDrive>();

            foreach (Fileshare share in fileshares)
            {
                netDrives.Add(new NetworkDrive("\\\\" + share.server + "\\" + share.folder));
            }
            return netDrives;
        }

        public void updateUsersJson()
        {
            //serialize json file from users
            List<User> allUsersFromFile = enumUsers();

            //find the current user object
            int userIndex = getIndexofCurrentlyLoggedInUser();
            if (currentUserIndex == -1)
            {
                //no user exists; create one
                allUsersFromFile.Add(new User(Environment.UserName));
            }

            //edit the User Object
            List<NetworkDrive> currentUserShares = Local.jsonCurrentUserDrives;
            foreach (NetworkDrive drive in currentUserShares)
            {
                string shareID = drive.convertToIdentifier();
                allUsersFromFile[currentUserIndex].fileshares.Add(shareID);
            }

            //write json back to file (overwrite)
            string jsonString = JsonConvert.SerializeObject(allUsersFromFile);
            StreamWriter sWriter = new StreamWriter(USERS_JSON_FULL_FILEPATH);
            sWriter.Write(jsonString, true);
        }

        #endregion

    }
}
