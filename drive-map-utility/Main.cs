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
            if (mappedList.Items.Count > 0)
            {
                updateStatus("Figuring out what needs to be mapped");
                List<NetworkDrive> fullList = enumerateSharesList(mappedList);
                foreach (NetworkDrive share in fullList)
                {
                    //if the share is not currently mapped
                    if (!ThisComputer.isMapped(share))
                    {
                        /*try
                        {*/
                            share.PromptForCredentials = true;
                            share.MapDrive();
                       /* }
                        catch
                        {
                            ProgramUtils.writeLog("Failed to map network drive: " + share.ShareName);
                        }*/
                    }
                }
            }
        }
        #endregion
        #region Processes

        private List<NetworkDrive> enumerateSharesList(ListBox shareList)
        {
            List<NetworkDrive> driveList = new List<NetworkDrive>();
            List<NetworkDrive> userDrives = json.getCurrentUserDrivesFromJson();
            NetworkDrive matched = null;

            foreach (string shareName in shareList.Items)
            {
                string fullpath = shareName.Split(' ')[1];
                matched = userDrives.Find(share => share.ShareName == fullpath);
                driveList.Add(matched);
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

        private List<NetworkDrive> getUnmappedDrives()
        {
            List<NetworkDrive> unmappedShares = new List<NetworkDrive>();
            List<NetworkDrive> userJsonDrives = json.getCurrentUserDrivesFromJson();
            foreach (NetworkDrive share in userJsonDrives)
            {
                if (!ThisComputer.isMapped(share))
                {
                    unmappedShares.Add(share);
                }
            }

            return unmappedShares;
        }

        private void populateListBoxes()
        {
            List<NetworkDrive> currentUserDrives = json.getCurrentUserDrivesFromJson();
            List<string> mappedShares = new List<string>();
            List<string> unmappedShares = new List<string>();
            List<NetworkDrive> currentlyMappedShares = ThisComputer.getCurrentlyMappedDrives();

            NetworkDrive matched = null;
            foreach (NetworkDrive share in currentUserDrives)
            {
                //check if share is mapped, otherwise put in other list
                if (ThisComputer.isMapped(share))
                {
                    matched = currentlyMappedShares.Find(item => item.ShareName == share.ShareName);
                    mappedShares.Add(matched.LocalDrive + " " + share.ShareName);
                }
                else
                    unmappedShares.Add(share.LocalDrive + " " + share.ShareName);
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
    }
}
