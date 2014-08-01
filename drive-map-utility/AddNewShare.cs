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
            List<string> datasource = Program.convertToStringList(json.jsonKnownShares);

            foreach (string item in datasource)
            {
                fileShareSelect.Items.Add(item);
            }
            this.Refresh();
        }

        private void fillDriveLetterSelect()
        {
            List<char> availableLetters = Local.getAvailableDriveLetters();
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
            NetworkDrive selectedDrive = Program.findDriveInList(fullPathBox.Text, json.jsonKnownShares);
            selectedDrive.LocalDrive = driveLetterSelect.Text;
            Local.jsonCurrentUserDrives.Add(selectedDrive);
            this.Close();
        }


    }
}
