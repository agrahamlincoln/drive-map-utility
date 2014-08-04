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

        private void updateListsButton_Click(object sender, EventArgs e)
        {
            mapFromListBox(mappedList);
            unmapFromListBox(knownList);
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

        private void mapDrivesButton_Click(object sender, EventArgs e)
        {
            mapFromListBox(mappedList);
        }

        private void Main_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            mapFromListBox(mappedList);
            unmapFromListBox(knownList);
        }

        private void mapAll_btn_Click(object sender, EventArgs e)
        {
            MoveAllItems(knownList, mappedList);
        }

        private void unmapAll_btn_Click(object sender, EventArgs e)
        {
            MoveAllItems(mappedList, knownList);
        }

        private void unmapDrivesButton_Click(object sender, EventArgs e)
        {
            unmapFromListBox(knownList);
        }

        #endregion

        #region Processes

        /// <summary>Takes all network drives from a ListBox object and unmaps them.
        /// </summary>
        /// <param name="listBox">listbox object to be unmapped</param>
        private void unmapFromListBox(ListBox listBox)
        {
            //unmap all drives that are not in the list and are currently mapped
            if (listBox.Items.Count > 0)
            {
                //shares in the unmapped list that are mapped
                List<NetworkDrive> drivesFromList = enumerateSharesfromListBox(listBox);
                List<NetworkDrive> needToBeUnmapped = Local.getMappedStatus(drivesFromList, true);
                unmapList(needToBeUnmapped);
            }
        }

        /// <summary>Takes all network drives from a ListBox object and maps them.
        /// </summary>
        /// <param name="listBox">listbox to be mapped</param>
        private void mapFromListBox(ListBox listBox)
        {
            //map all drives that are in the list and arent curretly mapped.
            if (listBox.Items.Count > 0)
            {
                List<NetworkDrive> drivesFromList = enumerateSharesfromListBox(listBox);
                List<NetworkDrive> needToBeMapped = Local.getMappedStatus(drivesFromList, false);
                if (passwordTxtBox.Text == "" && usernameTxtBox.Text == "")
                {
                    Program.mapList(needToBeMapped);
                }
                else if (usernameTxtBox.Text == "")
                {
                    Program.mapList(needToBeMapped, passwordTxtBox.Text);
                }
                else
                {
                    Program.mapList(needToBeMapped, usernameTxtBox.Text, passwordTxtBox.Text);
                }
            }
        }

        /// <summary>Iterates through a list of NetworkDrives and unmaps them all.
        /// </summary>
        /// <param name="listOfDrives">List of Network DRives to unmap.</param>
        private void unmapList(List<NetworkDrive> listOfDrives)
        {
            foreach (NetworkDrive drive in listOfDrives)
            {
                drive.Force = true;
                try
                {
                    drive.UnMapDrive();
                }
                catch (Win32Exception e)
                {
                    Utilities.writeLog("Failed to unmap drive: " + drive.LocalDrive + " with the error " + e);
                }
            }
        }
        
        /// <summary>Get list of shares from listbox item
        /// <remarks>Will retain the drive letter from the listbox if it is different than from json.</remarks>
        /// </summary>
        /// <param name="shareList">ListBox item to get shares from</param>
        /// <returns>List of network Drives parsed from ListBox</returns>
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
                    matched = Program.findDriveInList(fullPath, Local.userDrives);
                    if (matched == null)
                    {
                        //this drive is not known
                        Utilities.writeLog("Drive is not Known, creating a new object.");

                        //creating new drive item
                        matched = new NetworkDrive(driveLetter, fullPath);

                        //add this to the current user drives list.
                        Local.userDrives.Add(matched);
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

        /// <summary>Lists all network drives known to this user that are not currently mapped.
        /// </summary>
        /// <returns>List of network drives known to this user that are not currently mapped.</returns>
        private List<NetworkDrive> getUnmappedDrives()
        {
            return Local.getMappedStatus(Local.userDrives, false);
        }

        /// <summary>Fills the two textboxes on the main form.
        /// Left will be all shares currently mapped/to be mapped
        /// Right will be all shares that are not mapped.
        /// </summary>
        private void populateListBoxes()
        {
            List<string> mappedShares = new List<string>();
            List<string> unmappedShares = new List<string>();

            //Add Drives from JSON
            NetworkDrive matched = null;
            if (Local.userDrives != null)
            {
                foreach (NetworkDrive share in Local.userDrives)
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
            this.toolStripStatusLabel1.Text = status;
            this.Refresh();
        }
        #endregion

        private void toolStripStatusLabel1_Click(object sender, EventArgs e)
        {

        }




    }
}
