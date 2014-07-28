namespace drive_map_utility
{
    partial class Main
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.knownList = new System.Windows.Forms.ListBox();
            this.mappedList = new System.Windows.Forms.ListBox();
            this.statusBar = new System.Windows.Forms.StatusStrip();
            this.formOutline = new System.Windows.Forms.GroupBox();
            this.addNewButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.mapSharesButton = new System.Windows.Forms.Button();
            this.removeFromMappedList = new System.Windows.Forms.Button();
            this.addToMappedList = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.Title = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.formOutline.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // knownList
            // 
            this.knownList.FormattingEnabled = true;
            this.knownList.Location = new System.Drawing.Point(254, 38);
            this.knownList.Name = "knownList";
            this.knownList.Size = new System.Drawing.Size(184, 173);
            this.knownList.TabIndex = 0;
            // 
            // mappedList
            // 
            this.mappedList.FormattingEnabled = true;
            this.mappedList.Location = new System.Drawing.Point(10, 38);
            this.mappedList.Name = "mappedList";
            this.mappedList.Size = new System.Drawing.Size(191, 173);
            this.mappedList.TabIndex = 1;
            // 
            // statusBar
            // 
            this.statusBar.Location = new System.Drawing.Point(0, 333);
            this.statusBar.Name = "statusBar";
            this.statusBar.Size = new System.Drawing.Size(490, 22);
            this.statusBar.TabIndex = 2;
            this.statusBar.Text = "statusStrip1";
            // 
            // formOutline
            // 
            this.formOutline.Controls.Add(this.addNewButton);
            this.formOutline.Controls.Add(this.label2);
            this.formOutline.Controls.Add(this.mapSharesButton);
            this.formOutline.Controls.Add(this.removeFromMappedList);
            this.formOutline.Controls.Add(this.addToMappedList);
            this.formOutline.Controls.Add(this.label1);
            this.formOutline.Controls.Add(this.mappedList);
            this.formOutline.Controls.Add(this.knownList);
            this.formOutline.ForeColor = System.Drawing.Color.Black;
            this.formOutline.Location = new System.Drawing.Point(19, 74);
            this.formOutline.Name = "formOutline";
            this.formOutline.Size = new System.Drawing.Size(448, 249);
            this.formOutline.TabIndex = 3;
            this.formOutline.TabStop = false;
            this.formOutline.Text = "Username";
            // 
            // addNewButton
            // 
            this.addNewButton.ForeColor = System.Drawing.Color.Black;
            this.addNewButton.Location = new System.Drawing.Point(254, 217);
            this.addNewButton.Name = "addNewButton";
            this.addNewButton.Size = new System.Drawing.Size(184, 23);
            this.addNewButton.TabIndex = 6;
            this.addNewButton.Text = "Add New";
            this.addNewButton.UseVisualStyleBackColor = true;
            this.addNewButton.Click += new System.EventHandler(this.addNewButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.Color.Maroon;
            this.label2.Location = new System.Drawing.Point(250, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 19);
            this.label2.TabIndex = 6;
            this.label2.Text = "Unmapped Fileshares";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // mapSharesButton
            // 
            this.mapSharesButton.ForeColor = System.Drawing.Color.Black;
            this.mapSharesButton.Location = new System.Drawing.Point(10, 217);
            this.mapSharesButton.Name = "mapSharesButton";
            this.mapSharesButton.Size = new System.Drawing.Size(191, 23);
            this.mapSharesButton.TabIndex = 5;
            this.mapSharesButton.Text = "Map Shares";
            this.mapSharesButton.UseVisualStyleBackColor = true;
            this.mapSharesButton.Click += new System.EventHandler(this.mapSharesButton_Click);
            // 
            // removeFromMappedList
            // 
            this.removeFromMappedList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.removeFromMappedList.Location = new System.Drawing.Point(207, 126);
            this.removeFromMappedList.Name = "removeFromMappedList";
            this.removeFromMappedList.Size = new System.Drawing.Size(41, 23);
            this.removeFromMappedList.TabIndex = 5;
            this.removeFromMappedList.Text = ">";
            this.removeFromMappedList.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.removeFromMappedList.UseVisualStyleBackColor = true;
            this.removeFromMappedList.Click += new System.EventHandler(this.removeFromMappedList_Click);
            // 
            // addToMappedList
            // 
            this.addToMappedList.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.addToMappedList.Location = new System.Drawing.Point(207, 97);
            this.addToMappedList.Name = "addToMappedList";
            this.addToMappedList.Size = new System.Drawing.Size(41, 23);
            this.addToMappedList.TabIndex = 3;
            this.addToMappedList.Text = "<";
            this.addToMappedList.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            this.addToMappedList.UseVisualStyleBackColor = true;
            this.addToMappedList.Click += new System.EventHandler(this.addToMappedList_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Calibri", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Maroon;
            this.label1.Location = new System.Drawing.Point(6, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(195, 19);
            this.label1.TabIndex = 2;
            this.label1.Text = "Currently Mapped Fileshares";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // Title
            // 
            this.Title.AutoSize = true;
            this.Title.Font = new System.Drawing.Font("Cambria", 24F, System.Drawing.FontStyle.Bold);
            this.Title.Location = new System.Drawing.Point(212, 34);
            this.Title.Name = "Title";
            this.Title.Size = new System.Drawing.Size(255, 37);
            this.Title.TabIndex = 4;
            this.Title.Text = "Fileshare Utility";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(490, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem,
            this.aboutToolStripMenuItem1});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(117, 22);
            this.aboutToolStripMenuItem.Text = "Readme";
            // 
            // aboutToolStripMenuItem1
            // 
            this.aboutToolStripMenuItem1.Name = "aboutToolStripMenuItem1";
            this.aboutToolStripMenuItem1.Size = new System.Drawing.Size(117, 22);
            this.aboutToolStripMenuItem1.Text = "About";
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(490, 355);
            this.Controls.Add(this.Title);
            this.Controls.Add(this.formOutline);
            this.Controls.Add(this.statusBar);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Main";
            this.Text = "Form1";
            this.formOutline.ResumeLayout(false);
            this.formOutline.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox knownList;
        private System.Windows.Forms.ListBox mappedList;
        private System.Windows.Forms.StatusStrip statusBar;
        private System.Windows.Forms.GroupBox formOutline;
        private System.Windows.Forms.Button addNewButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button mapSharesButton;
        private System.Windows.Forms.Button removeFromMappedList;
        private System.Windows.Forms.Button addToMappedList;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label Title;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem1;
    }
}

