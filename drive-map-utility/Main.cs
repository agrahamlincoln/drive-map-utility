using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace drive_map_utility
{
    public partial class Main : Form
    {

        //Instantiate all program-level objects
        private FormMediator _formMediator;

        public Main()
        {
            InitializeComponent();
            setOutlineText();
            populateListBoxes();
        }

        public override void Refresh()
        {
            populateListBoxes();
            base.Refresh();
        }
        #region Form Controls

        private void mapSharesButton_Click(object sender, EventArgs e)
        {
            updateDrives();
        }

        private void addToMappedList_Click(object sender, EventArgs e)
        {
            MoveSelItem(knownList, mappedList);
        }

        private void removeFromMappedList_Click(object sender, EventArgs e)
        {
            MoveSelItem(mappedList, knownList);
        }

        private void addNewButton_Click(object sender, EventArgs e)
        {
            // Creates a form where you can manually enter the new fileshare
            AddNewShare addNewForm = new AddNewShare();
            _formMediator = new FormMediator(this, addNewForm);
            addNewForm.Show(this);
        }

        private void Main_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            updateDrives();
        }

        private void mapAll_btn_Click(object sender, EventArgs e)
        {
            MoveAllItems(knownList, mappedList);
        }

        private void unmapAll_btn_Click(object sender, EventArgs e)
        {
            MoveAllItems(mappedList, knownList);
        }

        #endregion

        #region Processes

        private void updateDrives()
        {
            if (mappedList.Items.Count > 0)
            {
                //shared in the mapped list that are not mapped
                List<NetworkDrive> needToBeMapped = convertListToNetworkDrive(mappedList, false);
                //map and unmap the drives
                if (usernameTxtBox.Text != "" && passwordTxtBox.Text != "")
                    mapList(needToBeMapped, usernameTxtBox.Text, passwordTxtBox.Text);
                else
                    mapList(needToBeMapped);
            }

            if (knownList.Items.Count > 0)
            {
                //shares in the unmapped list that are mapped
                List<NetworkDrive> needToBeUnmapped = convertListToNetworkDrive(knownList, true);
                unmapList(needToBeUnmapped);
            }
        }

        private List<NetworkDrive> convertListToNetworkDrive(ListBox shareList, bool isMapped)
        {
            List<NetworkDrive> matchedDrives = new List<NetworkDrive>();

            List<NetworkDrive> drivesFromListBox = enumerateSharesfromListBox(shareList);
            foreach (NetworkDrive share in drivesFromListBox)
            {
                //if the share matches the bool value currently mapped
                if (Local.hasMapping(share) == isMapped)
                {
                    matchedDrives.Add(share);
                }
            }

            return matchedDrives;
        }

        private void unmapList(List<NetworkDrive> listOfDrives)
        {
            foreach (NetworkDrive drive in listOfDrives)
                drive.UnMapDrive();
        }

        private void mapList(List<NetworkDrive> listOfDrives)
        {
            foreach (NetworkDrive drive in listOfDrives)
            {
                // Ask for credentials to have access to the drive
                drive.PromptForCredentials = true;
                if (Local.isDriveLetterAvailable(drive.LocalDrive))
                {
                    drive.MapDrive();
                }
                else
                {
                    drive.LocalDrive = Local.getNextAvailableDriveLetter(drive.LocalDrive).ToString();
                    drive.MapDrive();
                }
            }
        }
        private void mapList(List<NetworkDrive> listOfDrives, string username, string password)
        {
            foreach (NetworkDrive drive in listOfDrives)
            {
                // Takes in the username and password
                drive.MapDrive(username, password);
            }
        }

        /** Get list of shares from listbox item
         * Will retain the drive letter from the listbox if it is different than from json.
         */
        private List<NetworkDrive> enumerateSharesfromListBox(ListBox shareList)
        {
            List<NetworkDrive> driveList = new List<NetworkDrive>();
            NetworkDrive matched = null;

            string server;
            string folder;
            string driveLetter;
            string fullPath;

            foreach (string shareName in shareList.Items)
            {
                try
                {
                    // Separates the drive letter from the full path name
                    server = shareName.Split('\\')[2];
                    folder = shareName.Split('\\')[3];
                    fullPath = "\\\\" + server + "\\" + folder;
                    driveLetter = shareName.Split(' ')[0];
                    matched = Program.findDriveInList(fullPath, Local.jsonCurrentUserDrives);
                    if (matched == null)
                    {
                        //this drive is not known
                        Utilities.writeLog("Error: Drive is not Known");
                    }
                    if (!driveLetter.Equals(matched.LocalDrive))
                    {
                        matched.LocalDrive = driveLetter;   // Changes the drive letter to what is in the shareList
                    }
                    driveList.Add(matched);
                }
                catch
                {
                    Utilities.writeLog("Error while attempting to match drives from ListBox: \"" + shareName + "\"");
                }
            }

            return driveList;
        }

        #endregion

        #region Form Tools

        private void MoveSelItem(ListBox from, ListBox to)
        {
            try
            {
                // Removes item from one listbox and places into another
                to.Items.Add(from.SelectedItem);
                from.Items.Remove(from.SelectedItem);
            }
            catch { }
        }

        private void MoveAllItems(ListBox from, ListBox to)
        {
            for (int i = 0; i < from.Items.Count; i++)
            {
                // Moves all items from one listbox to the other
                to.Items.Add(from.Items[i].ToString());
            }
            from.Items.Clear(); 
        }

        private void AddToListBox(ListBox box, List<string> items)
        {
            foreach (string item in items)
            {
                box.Items.Add(item);
            }
        }

        private void setOutlineText()
        {
            // Sets the current username to the form
            this.formOutline.Text = Environment.UserName;
            this.Refresh();
        }

        /* Get list of all network drives that are currently un-mapped */
        private List<NetworkDrive> getUnmappedDrives()
        {
            List<NetworkDrive> unmappedShares = new List<NetworkDrive>();
            foreach (NetworkDrive share in Local.jsonCurrentUserDrives)
            {
                if (!Local.hasMapping(share))
                {
                    unmappedShares.Add(share);
                }
            }

            return unmappedShares;
        }

        /* Fills out listbox elements in form with unmapped and mapped drives */
        private void populateListBoxes()
        {
            List<string> mappedShares = new List<string>();
            List<string> unmappedShares = new List<string>();

            //Add Drives from JSON
            NetworkDrive matched = null;
            if (Local.jsonCurrentUserDrives != null)
            {
                foreach (NetworkDrive share in Local.jsonCurrentUserDrives)
                {
                    //check if share is mapped, otherwise put in other list
                    if (Local.hasMapping(share))
                    {
                        matched = Local.currentlyMappedDrives.Find(item => item.ShareName == share.ShareName);
                        mappedShares.Add(matched.LocalDrive + " " + share.ShareName);
                    }
                    else
                        unmappedShares.Add(share.LocalDrive + " " + share.ShareName);
                }
            }
            else
            {
                Utilities.writeLog("Error: No user drives found.");
                mappedShares.Add("No drives found from json.");
                unmappedShares.Add("No drives found from json.");
            }

            //Add Drives from computer
            foreach (NetworkDrive mapped in Local.currentlyMappedDrives)
            {
                mappedShares.Add(mapped.LocalDrive + " " + mapped.ShareName);
            }

            mappedList.Items.Clear();
            knownList.Items.Clear();
            AddToListBox(mappedList, mappedShares);
            AddToListBox(knownList, unmappedShares);
        }

        private void updateStatus(string status)
        {
            //update statusbar and refresh
            this.statusBar.Text = status;
            this.Refresh();
        }
        #endregion
    }
}
