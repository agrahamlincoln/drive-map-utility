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
            // This list all the known shares into the combo box "File Share Select"
            List<string> datasource = Program.convertToStringList(json.jsonKnownShares);

            foreach (string item in datasource)
            {
                fileShareSelect.Items.Add(item);
            }
            this.Refresh();
        }

        private void fillDriveLetterSelect()
        {
            // Checks for the available drive letters and fills the combo box "drive letter select" with them
            List<char> availableLetters = Local.getAvailableDriveLetters();
            foreach (char letter in availableLetters)
            {
                driveLetterSelect.Items.Add(letter);
            }
            this.Refresh();
        }

        private void updateFullPathBox()
        {
            // Fills the text box with the full file path that was selected
            fullPathBox.Text = fileShareSelect.SelectedItem.ToString();
            this.Refresh();
        }

        #endregion

        private void AddNewShare_Load(object sender, EventArgs e)
        {
            // Loads the available shares and drive letters
            fillFileShareSelect();
            fillDriveLetterSelect();
        }

        private void fileShareSelect_SelectedValueChanged(object sender, EventArgs e)
        {
            updateFullPathBox();
        }

        private void addButton_Click(object sender, EventArgs e)
        {
            // Adds the new share to current user drives and closes this form
            NetworkDrive selectedDrive = Program.findDriveInList(fullPathBox.Text, json.jsonKnownShares);
            selectedDrive.LocalDrive = driveLetterSelect.Text;
            Local.jsonCurrentUserDrives.Add(selectedDrive);
            this.Close();
        }


    }
}
