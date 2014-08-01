using System;
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;

namespace drive_map_utility
{
	/// <summary>
	/// Summary description for Form1.
	/// </summary>
	public class frmTest : System.Windows.Forms.Form{
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox txtDrive;
		private System.Windows.Forms.TextBox txtPassword;
		private System.Windows.Forms.TextBox txtUsername;
		private System.Windows.Forms.TextBox txtAddress;
		private System.Windows.Forms.Button button4;
		private System.Windows.Forms.Button button3;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.StatusBar conStatusBar;
		private System.Windows.Forms.StatusBarPanel conStatusBar_Panel_Action;
		private System.Windows.Forms.CheckBox conForce;
		private System.Windows.Forms.CheckBox conPersistant;
		private System.Windows.Forms.MainMenu conMenu;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem conMenu_RestoreAll;
		private System.Windows.Forms.CheckBox conPromptForCred;
		private System.Windows.Forms.CheckBox conSaveCred;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem conMenu_Dialog1;
		private System.Windows.Forms.MenuItem conMenu_Dialog2;
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.Container components = null;

		public frmTest()
		{
			//
			// Required for Windows Form Designer support
			//
			InitializeComponent();

			//
			// TODO: Add any constructor code after InitializeComponent call
			//
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if( disposing )
			{
				if (components != null) 
				{
					components.Dispose();
				}
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.conSaveCred = new System.Windows.Forms.CheckBox();
			this.conPromptForCred = new System.Windows.Forms.CheckBox();
			this.conForce = new System.Windows.Forms.CheckBox();
			this.conPersistant = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.txtDrive = new System.Windows.Forms.TextBox();
			this.txtPassword = new System.Windows.Forms.TextBox();
			this.txtUsername = new System.Windows.Forms.TextBox();
			this.txtAddress = new System.Windows.Forms.TextBox();
			this.button4 = new System.Windows.Forms.Button();
			this.button3 = new System.Windows.Forms.Button();
			this.conStatusBar = new System.Windows.Forms.StatusBar();
			this.conStatusBar_Panel_Action = new System.Windows.Forms.StatusBarPanel();
			this.conMenu = new System.Windows.Forms.MainMenu();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.conMenu_RestoreAll = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.conMenu_Dialog1 = new System.Windows.Forms.MenuItem();
			this.conMenu_Dialog2 = new System.Windows.Forms.MenuItem();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.conStatusBar_Panel_Action)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.conSaveCred);
			this.groupBox1.Controls.Add(this.conPromptForCred);
			this.groupBox1.Controls.Add(this.conForce);
			this.groupBox1.Controls.Add(this.conPersistant);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.txtDrive);
			this.groupBox1.Controls.Add(this.txtPassword);
			this.groupBox1.Controls.Add(this.txtUsername);
			this.groupBox1.Controls.Add(this.txtAddress);
			this.groupBox1.Controls.Add(this.button4);
			this.groupBox1.Controls.Add(this.button3);
			this.groupBox1.Location = new System.Drawing.Point(8, 0);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(424, 176);
			this.groupBox1.TabIndex = 12;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Map Drive Settings";			
			// 
			// conSaveCred
			// 
			this.conSaveCred.Location = new System.Drawing.Point(264, 144);
			this.conSaveCred.Name = "conSaveCred";
			this.conSaveCred.Size = new System.Drawing.Size(136, 24);
			this.conSaveCred.TabIndex = 29;
			this.conSaveCred.Text = "Save credentials";
			// 
			// conPromptForCred
			// 
			this.conPromptForCred.Location = new System.Drawing.Point(112, 144);
			this.conPromptForCred.Name = "conPromptForCred";
			this.conPromptForCred.Size = new System.Drawing.Size(136, 24);
			this.conPromptForCred.TabIndex = 28;
			this.conPromptForCred.Text = "Prompt for credentials";
			// 
			// conForce
			// 
			this.conForce.Location = new System.Drawing.Point(264, 120);
			this.conForce.Name = "conForce";
			this.conForce.Size = new System.Drawing.Size(136, 24);
			this.conForce.TabIndex = 25;
			this.conForce.Text = "Force dis/connetion";
			// 
			// conPersistant
			// 
			this.conPersistant.Checked = true;
			this.conPersistant.CheckState = System.Windows.Forms.CheckState.Checked;
			this.conPersistant.Location = new System.Drawing.Point(112, 120);
			this.conPersistant.Name = "conPersistant";
			this.conPersistant.Size = new System.Drawing.Size(136, 24);
			this.conPersistant.TabIndex = 24;
			this.conPersistant.Text = "Persistant connection";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(16, 120);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(88, 21);
			this.label5.TabIndex = 23;
			this.label5.Text = "Options:";
			this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(16, 96);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(88, 21);
			this.label4.TabIndex = 21;
			this.label4.Text = "Password:";
			this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(16, 72);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(88, 21);
			this.label3.TabIndex = 20;
			this.label3.Text = "Username:";
			this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(16, 48);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(88, 21);
			this.label2.TabIndex = 19;
			this.label2.Text = "Map to Drive:";
			this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(16, 24);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(88, 21);
			this.label1.TabIndex = 18;
			this.label1.Text = "Share Address:";
			this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			// 
			// txtDrive
			// 
			this.txtDrive.Location = new System.Drawing.Point(112, 48);
			this.txtDrive.Name = "txtDrive";
			this.txtDrive.Size = new System.Drawing.Size(176, 21);
			this.txtDrive.TabIndex = 17;
			this.txtDrive.Text = "Z:";
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(112, 96);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(176, 21);
			this.txtPassword.TabIndex = 16;
			this.txtPassword.Text = "";
			// 
			// txtUsername
			// 
			this.txtUsername.Location = new System.Drawing.Point(112, 72);
			this.txtUsername.Name = "txtUsername";
			this.txtUsername.Size = new System.Drawing.Size(176, 21);
			this.txtUsername.TabIndex = 15;
			this.txtUsername.Text = "";
			// 
			// txtAddress
			// 
			this.txtAddress.Location = new System.Drawing.Point(112, 24);
			this.txtAddress.Name = "txtAddress";
			this.txtAddress.Size = new System.Drawing.Size(176, 21);
			this.txtAddress.TabIndex = 14;
			this.txtAddress.Text = "\\\\ComputerName\\Share";
			// 
			// button4
			// 
			this.button4.Location = new System.Drawing.Point(296, 56);
			this.button4.Name = "button4";
			this.button4.Size = new System.Drawing.Size(120, 24);
			this.button4.TabIndex = 13;
			this.button4.Text = "UnMap Drive";
			this.button4.Click += new System.EventHandler(this.button4_Click);
			// 
			// button3
			// 
			this.button3.Location = new System.Drawing.Point(296, 24);
			this.button3.Name = "button3";
			this.button3.Size = new System.Drawing.Size(120, 24);
			this.button3.TabIndex = 12;
			this.button3.Text = "Map Drive";
			this.button3.Click += new System.EventHandler(this.button3_Click_1);
			// 
			// conStatusBar
			// 
			this.conStatusBar.Font = new System.Drawing.Font("Tahoma", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.conStatusBar.Location = new System.Drawing.Point(0, 179);
			this.conStatusBar.Name = "conStatusBar";
			this.conStatusBar.Panels.AddRange(new System.Windows.Forms.StatusBarPanel[] {
																							this.conStatusBar_Panel_Action});
			this.conStatusBar.ShowPanels = true;
			this.conStatusBar.Size = new System.Drawing.Size(440, 24);
			this.conStatusBar.SizingGrip = false;
			this.conStatusBar.TabIndex = 14;
			// 
			// conStatusBar_Panel_Action
			// 
			this.conStatusBar_Panel_Action.AutoSize = System.Windows.Forms.StatusBarPanelAutoSize.Spring;
			this.conStatusBar_Panel_Action.Text = "...";
			this.conStatusBar_Panel_Action.Width = 440;
			// 
			// conMenu
			// 
			this.conMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					this.menuItem1,
																					this.menuItem2});
			// 
			// menuItem1
			// 
			this.menuItem1.Index = 0;
			this.menuItem1.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.conMenu_RestoreAll});
			this.menuItem1.Text = "Functions";
			// 
			// conMenu_RestoreAll
			// 
			this.conMenu_RestoreAll.Index = 0;
			this.conMenu_RestoreAll.Text = "Restore All Drives";
			this.conMenu_RestoreAll.Click += new System.EventHandler(this.conMenu_RestoreAll_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.Index = 1;
			this.menuItem2.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {
																					  this.conMenu_Dialog1,
																					  this.conMenu_Dialog2});
			this.menuItem2.Text = "Dialogs";
			// 
			// conMenu_Dialog1
			// 
			this.conMenu_Dialog1.Index = 0;
			this.conMenu_Dialog1.Text = "Show \'Drive connection\'";
			this.conMenu_Dialog1.Click += new System.EventHandler(this.conMenu_Dialog1_Click);
			// 
			// conMenu_Dialog2
			// 
			this.conMenu_Dialog2.Index = 1;
			this.conMenu_Dialog2.Text = "Show \'Drive Disconnection\'";
			this.conMenu_Dialog2.Click += new System.EventHandler(this.conMenu_Dialog2_Click);
			// 
			// frmTest
			// 
			this.AutoScaleBaseSize = new System.Drawing.Size(5, 14);
			this.ClientSize = new System.Drawing.Size(440, 203);
			this.Controls.Add(this.conStatusBar);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((System.Byte)(0)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Menu = this.conMenu;
			this.Name = "frmTest";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "AejW.com - Network Drives Class - 0015";
			this.Load += new System.EventHandler(this.frmTest_Load);
			this.groupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.conStatusBar_Panel_Action)).EndInit();
			this.ResumeLayout(false);

		}
		#endregion

		[STAThread]
		static void Main(){
			Application.Run(new frmTest());
		}

		private void button3_Click_1(object sender, System.EventArgs e){
			NetworkDrive oNetDrive = new NetworkDrive();						
			zUpdateStatus("Mapping drive...");
			try{
				//set propertys
				oNetDrive.Force = this.conForce.Checked;
				oNetDrive.Persistent = this.conPersistant.Checked;
				oNetDrive.LocalDrive = txtDrive.Text;
				oNetDrive.PromptForCredentials = conPromptForCred.Checked;
				oNetDrive.ShareName = txtAddress.Text;
				oNetDrive.SaveCredentials = conSaveCred.Checked;
				//match call to options provided
				if(txtPassword.Text==""&&txtUsername.Text==""){					
					oNetDrive.MapDrive();
				}else if(txtUsername.Text==""){
					oNetDrive.MapDrive(txtPassword.Text);					
				}else{
					oNetDrive.MapDrive(txtUsername.Text,txtPassword.Text);
				}
				//update status
				zUpdateStatus("Drive map successful");
			}catch(Exception err){
				//report error
				zUpdateStatus("Cannot map drive! - "+err.Message);
				MessageBox.Show(this, "Cannot map drive!\nError: "+err.Message);
			}
			oNetDrive = null;
		}

		private void button4_Click(object sender, System.EventArgs e){
			NetworkDrive oNetDrive = new NetworkDrive();
			zUpdateStatus("Unmapping drive...");
			try{
				//unmap the drive
				oNetDrive.Force = conForce.Checked;
				oNetDrive.LocalDrive = txtDrive.Text;
				oNetDrive.UnMapDrive();
				//update status
				zUpdateStatus("Drive unmap successful");
			}catch(Exception err){
				//report error
				zUpdateStatus("Cannot unmap drive! - "+err.Message);
				MessageBox.Show(this, "Cannot unmap drive!\nError: "+err.Message);
			}
			oNetDrive = null;
		}

		private void frmTest_Load(object sender, System.EventArgs e){
			//set the address field to a share name for the current computer
			txtAddress.Text = "\\\\"+SystemInformation.ComputerName+"\\C$";
		}

		private void zUpdateStatus(string psStatus){
			//update the status bar and refresh
			this.conStatusBar.Panels[0].Text=psStatus;
			this.Refresh();
		}


		private void conMenu_RestoreAll_Click(object sender, System.EventArgs e){
			NetworkDrive oNetDrive = new NetworkDrive();
			try{
				zUpdateStatus("Restoring drive connections...");
				oNetDrive.RestoreDrives();
				//update status
				zUpdateStatus("Drive connections restore successful");
			}catch(Exception err){
				//report error
				zUpdateStatus("Error! - "+err.Message);
				MessageBox.Show(this, "Error!\nError: "+err.Message);
			}
			oNetDrive = null;
		}

		private void conMenu_Dialog1_Click(object sender, System.EventArgs e){
			//show the connection dialog
			NetworkDrive oNetDrive = new NetworkDrive();
			oNetDrive.ShowConnectDialog(this);
			oNetDrive = null;
		}

		private void conMenu_Dialog2_Click(object sender, System.EventArgs e){
			//show the disconnection dialog
			NetworkDrive oNetDrive = new NetworkDrive();
			oNetDrive.ShowDisconnectDialog(this);
			oNetDrive = null;
		}

	}
}
