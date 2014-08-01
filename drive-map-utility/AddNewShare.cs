using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace drive_map_utility
{
    public partial class AddNewShare : Form
    {
        public AddNewShare()
        {
            InitializeComponent();
        }

        #region Form Processes

        private void fillFileShareSelect()
        {
            List<string> datasource = convertToStringList(ThisComputer.jsonKnownShares);

            foreach (string item in datasource)
            {
                fileShareSelect.Items.Add(item);
            }
            this.Refresh();
        }

        private void fillDriveLetterSelect()
        {
            List<char> availableLetters = ThisComputer.getAvailableDriveLetters();
            foreach (char letter in availableLetters)
            {
                driveLetterSelect.Items.Add(letter);
            }
            this.Refresh();
        }

        private void updateFullPathBox()
        {
            fullPathBox.Text = fileShareSelect.SelectedItem.ToString();
            this.Refresh();
        }

        #endregion

        private void AddNewShare_Load(object sender, EventArgs e)
        {
            fillFileShareSelect();
            fillDriveLetterSelect();
        }

        private void fileShareSelect_SelectedValueChanged(object sender, EventArgs e)
        {
            updateFullPathBox();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            NetworkDrive selectedDrive = ThisComputer.matchPathToKnownDrive(fullPathBox.Text);
            selectedDrive.LocalDrive = driveLetterSelect.Text;
            ThisComputer.jsonCurrentUserDrives.Add(selectedDrive);
            this.Close();
        }

        #region utilities

        /// <summary>Converts NetworkDrive list to list of strings.
        /// </summary>
        /// <param name="netDrives">list of Network Drive objects</param>
        /// <returns>List of network drive objects in string format.</returns>
        public static List<string> convertToStringList(List<NetworkDrive> netDrives)
        {
            List<string> stringList = new List<string>();
            foreach (NetworkDrive drive in netDrives)
            {
                stringList.Add(drive.ShareName);
            }
            return stringList;
        }

        /// <summary>Matches a full network path to a Network Drive object in the currently logged in user's list.
        /// </summary>
        /// <remarks>Returns null if none found.</remarks>
        /// <param name="fullPath">Full Network Path</param>
        /// <returns>Network Drive object that matches the passed path.</returns>
        public static NetworkDrive matchPathToUserDrive(string fullPath)
        {
            NetworkDrive match;
            match = ThisComputer.jsonCurrentUserDrives.Find(share => Utilities.matchString_IgnoreCase(share.ShareName, fullPath));
            return match;
        }

        /// <summary>Matches a full network path to a Network Drive object in the known drives list from file.
        /// </summary>
        /// <remarks>Returns null if none found.</remarks>
        /// <param name="fullPath">Full network path</param>
        /// <returns>Network Drive object that matches the passed path.</returns>
        public static NetworkDrive matchPathToKnownDrive(string fullPath)
        {
            NetworkDrive match;
            match = ThisComputer.jsonKnownShares.Find(share => Utilities.matchString_IgnoreCase(share.ShareName, fullPath));
            return match;
        }

        #endregion
    }
}
