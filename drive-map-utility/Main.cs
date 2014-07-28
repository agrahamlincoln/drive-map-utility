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
        public Main()
        {
            InitializeComponent();
            setOutlineText();
            populateListBoxes();
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
            AddNewShare addNewForm = new AddNewShare();
            addNewForm.Show();
        }

        private void Main_closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            updateDrives();
        }

        #endregion

        #region Processes

        private void updateDrives()
        {
            if (mappedList.Items.Count > 0)
            {
                if (usernameTxtBox.Text != "" && passwordTxtBox.Text != "")
                {

                }

                updateStatus("Figuring out what needs to be mapped");
                List<NetworkDrive> fullList = enumerateSharesfromListBox(mappedList);
                foreach (NetworkDrive share in fullList)
                {
                    //if the share is not currently mapped
                    if (!ThisComputer.isMapped(share))
                    {
                        /*try
                        {*/
                        //share.PromptForCredentials = true;
                        share.MapDrive(usernameTxtBox.Text, passwordTxtBox.Text);
                        /* }
                         catch
                         {
                             ProgramUtils.writeLog("Failed to map network drive: " + share.ShareName);
                         }*/
                    }
                }
            }
        }

        /** Get list of shares from listbox item
         */
        private List<NetworkDrive> enumerateSharesfromListBox(ListBox shareList)
        {
            List<NetworkDrive> driveList = new List<NetworkDrive>();
            NetworkDrive matched = null;

            foreach (string shareName in shareList.Items)
            {
                try
                {
                    string fullpath = shareName.Split(' ')[1];
                    matched = ThisComputer.jsonUsersFile.Find(share => share.ShareName == fullpath);
                    driveList.Add(matched);
                }
                catch
                {
                    ProgramUtils.writeLog("Error while attempting to map drive: \"" + shareName + "\"");
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
                to.Items.Add(from.SelectedItem);
                from.Items.Remove(from.SelectedItem);
            }
            catch { }
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
            this.formOutline.Text = Environment.UserName;
            this.Refresh();
        }

        /* Get list of all network drives that are currently un-mapped */
        private List<NetworkDrive> getUnmappedDrives()
        {
            List<NetworkDrive> unmappedShares = new List<NetworkDrive>();
            foreach (NetworkDrive share in ThisComputer.jsonUsersFile)
            {
                if (!ThisComputer.isMapped(share))
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

            NetworkDrive matched = null;
            if (ThisComputer.jsonUsersFile != null)
            {
                foreach (NetworkDrive share in ThisComputer.jsonUsersFile)
                {
                    //check if share is mapped, otherwise put in other list
                    if (ThisComputer.isMapped(share))
                    {
                        matched = ThisComputer.currentlyMappedShares.Find(item => item.ShareName == share.ShareName);
                        mappedShares.Add(matched.LocalDrive + " " + share.ShareName);
                    }
                    else
                        unmappedShares.Add(share.LocalDrive + " " + share.ShareName);
                }
            }
            else
            {
                ProgramUtils.writeLog("Error: No user drives found.");
                mappedShares.Add("No drives found.");
                unmappedShares.Add("No drives found.");
            }

            AddToListBox(mappedList, mappedShares);
            AddToListBox(knownList, unmappedShares);
            this.Refresh();
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
