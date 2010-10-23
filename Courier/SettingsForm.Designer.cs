namespace Courier
{
	partial class SettingsForm
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
			if(disposing && (components != null))
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
			this.menuMain = new System.Windows.Forms.MenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.settingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.setupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.statusBar = new System.Windows.Forms.StatusStrip();
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabSettings = new System.Windows.Forms.TabPage();
			this.grpCharacter = new System.Windows.Forms.GroupBox();
			this.cmbPosition = new System.Windows.Forms.ComboBox();
			this.label4 = new System.Windows.Forms.Label();
			this.grpLogin = new System.Windows.Forms.GroupBox();
			this.txtPassword = new System.Windows.Forms.MaskedTextBox();
			this.btnBrowse = new System.Windows.Forms.Button();
			this.txtLogin = new System.Windows.Forms.TextBox();
			this.txtPath = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnSetup = new System.Windows.Forms.Button();
			this.btnSave = new System.Windows.Forms.Button();
			this.btnStart = new System.Windows.Forms.Button();
			this.dlgOpen = new System.Windows.Forms.OpenFileDialog();
			this.tabMissions = new System.Windows.Forms.TabPage();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label5 = new System.Windows.Forms.Label();
			this.lstAgents = new System.Windows.Forms.ListBox();
			this.txtAgent = new System.Windows.Forms.TextBox();
			this.btnAdd = new System.Windows.Forms.Button();
			this.btnRemove = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.txtCurrentAgent = new System.Windows.Forms.TextBox();
			this.chkCircleAgents = new System.Windows.Forms.CheckBox();
			this.menuMain.SuspendLayout();
			this.tabControl.SuspendLayout();
			this.tabSettings.SuspendLayout();
			this.grpCharacter.SuspendLayout();
			this.grpLogin.SuspendLayout();
			this.tabMissions.SuspendLayout();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// menuMain
			// 
			this.menuMain.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.settingsToolStripMenuItem,
            this.helpToolStripMenuItem});
			this.menuMain.Location = new System.Drawing.Point(0, 0);
			this.menuMain.Name = "menuMain";
			this.menuMain.Size = new System.Drawing.Size(632, 24);
			this.menuMain.TabIndex = 0;
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exitToolStripMenuItem});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(35, 20);
			this.fileToolStripMenuItem.Text = "&File";
			// 
			// exitToolStripMenuItem
			// 
			this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
			this.exitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
			this.exitToolStripMenuItem.Text = "E&xit";
			this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
			// 
			// settingsToolStripMenuItem
			// 
			this.settingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.setupToolStripMenuItem,
            this.saveToolStripMenuItem});
			this.settingsToolStripMenuItem.Name = "settingsToolStripMenuItem";
			this.settingsToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
			this.settingsToolStripMenuItem.Text = "&Settings";
			// 
			// setupToolStripMenuItem
			// 
			this.setupToolStripMenuItem.Name = "setupToolStripMenuItem";
			this.setupToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
			this.setupToolStripMenuItem.Text = "Set&up";
			this.setupToolStripMenuItem.Click += new System.EventHandler(this.setupToolStripMenuItem_Click);
			// 
			// saveToolStripMenuItem
			// 
			this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
			this.saveToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
			this.saveToolStripMenuItem.Text = "Sa&ve";
			this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
			// 
			// helpToolStripMenuItem
			// 
			this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutToolStripMenuItem});
			this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
			this.helpToolStripMenuItem.Size = new System.Drawing.Size(40, 20);
			this.helpToolStripMenuItem.Text = "&Help";
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(103, 22);
			this.aboutToolStripMenuItem.Text = "&About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// statusBar
			// 
			this.statusBar.Location = new System.Drawing.Point(0, 431);
			this.statusBar.Name = "statusBar";
			this.statusBar.Size = new System.Drawing.Size(632, 22);
			this.statusBar.TabIndex = 1;
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.tabSettings);
			this.tabControl.Controls.Add(this.tabMissions);
			this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
			this.tabControl.Location = new System.Drawing.Point(0, 24);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(632, 367);
			this.tabControl.TabIndex = 2;
			// 
			// tabSettings
			// 
			this.tabSettings.Controls.Add(this.grpCharacter);
			this.tabSettings.Controls.Add(this.grpLogin);
			this.tabSettings.Location = new System.Drawing.Point(4, 22);
			this.tabSettings.Name = "tabSettings";
			this.tabSettings.Padding = new System.Windows.Forms.Padding(3);
			this.tabSettings.Size = new System.Drawing.Size(624, 341);
			this.tabSettings.TabIndex = 0;
			this.tabSettings.Text = "Settings";
			this.tabSettings.UseVisualStyleBackColor = true;
			// 
			// grpCharacter
			// 
			this.grpCharacter.Controls.Add(this.cmbPosition);
			this.grpCharacter.Controls.Add(this.label4);
			this.grpCharacter.Location = new System.Drawing.Point(8, 151);
			this.grpCharacter.Name = "grpCharacter";
			this.grpCharacter.Size = new System.Drawing.Size(279, 63);
			this.grpCharacter.TabIndex = 1;
			this.grpCharacter.TabStop = false;
			this.grpCharacter.Text = "Character";
			// 
			// cmbPosition
			// 
			this.cmbPosition.FormattingEnabled = true;
			this.cmbPosition.Items.AddRange(new object[] {
            "Main",
            "Left",
            "Right"});
			this.cmbPosition.Location = new System.Drawing.Point(9, 32);
			this.cmbPosition.Name = "cmbPosition";
			this.cmbPosition.Size = new System.Drawing.Size(264, 21);
			this.cmbPosition.TabIndex = 1;
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(6, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(47, 13);
			this.label4.TabIndex = 0;
			this.label4.Text = "Position:";
			// 
			// grpLogin
			// 
			this.grpLogin.Controls.Add(this.txtPassword);
			this.grpLogin.Controls.Add(this.btnBrowse);
			this.grpLogin.Controls.Add(this.txtLogin);
			this.grpLogin.Controls.Add(this.txtPath);
			this.grpLogin.Controls.Add(this.label3);
			this.grpLogin.Controls.Add(this.label2);
			this.grpLogin.Controls.Add(this.label1);
			this.grpLogin.Location = new System.Drawing.Point(8, 6);
			this.grpLogin.Name = "grpLogin";
			this.grpLogin.Size = new System.Drawing.Size(279, 139);
			this.grpLogin.TabIndex = 0;
			this.grpLogin.TabStop = false;
			this.grpLogin.Text = "Login information";
			// 
			// txtPassword
			// 
			this.txtPassword.Location = new System.Drawing.Point(9, 110);
			this.txtPassword.Name = "txtPassword";
			this.txtPassword.PasswordChar = '*';
			this.txtPassword.Size = new System.Drawing.Size(264, 20);
			this.txtPassword.TabIndex = 6;
			// 
			// btnBrowse
			// 
			this.btnBrowse.Location = new System.Drawing.Point(246, 32);
			this.btnBrowse.Name = "btnBrowse";
			this.btnBrowse.Size = new System.Drawing.Size(27, 20);
			this.btnBrowse.TabIndex = 4;
			this.btnBrowse.Text = "...";
			this.btnBrowse.UseVisualStyleBackColor = true;
			this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
			// 
			// txtLogin
			// 
			this.txtLogin.Location = new System.Drawing.Point(9, 71);
			this.txtLogin.Name = "txtLogin";
			this.txtLogin.Size = new System.Drawing.Size(264, 20);
			this.txtLogin.TabIndex = 5;
			// 
			// txtPath
			// 
			this.txtPath.Location = new System.Drawing.Point(9, 32);
			this.txtPath.Name = "txtPath";
			this.txtPath.Size = new System.Drawing.Size(236, 20);
			this.txtPath.TabIndex = 3;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(6, 94);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(56, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Password:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(6, 55);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(36, 13);
			this.label2.TabIndex = 1;
			this.label2.Text = "Login:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(85, 13);
			this.label1.TabIndex = 0;
			this.label1.Text = "Path to eve.exe:";
			// 
			// btnSetup
			// 
			this.btnSetup.Location = new System.Drawing.Point(12, 397);
			this.btnSetup.Name = "btnSetup";
			this.btnSetup.Size = new System.Drawing.Size(75, 23);
			this.btnSetup.TabIndex = 3;
			this.btnSetup.Text = "Setup";
			this.btnSetup.UseVisualStyleBackColor = true;
			this.btnSetup.Click += new System.EventHandler(this.btnSetup_Click);
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(93, 397);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(75, 23);
			this.btnSave.TabIndex = 4;
			this.btnSave.Text = "Save";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
			// 
			// btnStart
			// 
			this.btnStart.Location = new System.Drawing.Point(545, 397);
			this.btnStart.Name = "btnStart";
			this.btnStart.Size = new System.Drawing.Size(75, 23);
			this.btnStart.TabIndex = 6;
			this.btnStart.Text = "Start";
			this.btnStart.UseVisualStyleBackColor = true;
			this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
			// 
			// dlgOpen
			// 
			this.dlgOpen.FileName = "eve.exe";
			this.dlgOpen.Filter = "Eve|eve.exe";
			// 
			// tabMissions
			// 
			this.tabMissions.Controls.Add(this.groupBox1);
			this.tabMissions.Location = new System.Drawing.Point(4, 22);
			this.tabMissions.Name = "tabMissions";
			this.tabMissions.Size = new System.Drawing.Size(624, 341);
			this.tabMissions.TabIndex = 1;
			this.tabMissions.Text = "Missions";
			this.tabMissions.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.chkCircleAgents);
			this.groupBox1.Controls.Add(this.txtCurrentAgent);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.btnRemove);
			this.groupBox1.Controls.Add(this.btnAdd);
			this.groupBox1.Controls.Add(this.txtAgent);
			this.groupBox1.Controls.Add(this.lstAgents);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Location = new System.Drawing.Point(8, 3);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(292, 222);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Agents";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(6, 16);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(58, 13);
			this.label5.TabIndex = 0;
			this.label5.Text = "Agents list:";
			// 
			// lstAgents
			// 
			this.lstAgents.FormattingEnabled = true;
			this.lstAgents.Location = new System.Drawing.Point(9, 32);
			this.lstAgents.Name = "lstAgents";
			this.lstAgents.Size = new System.Drawing.Size(277, 95);
			this.lstAgents.TabIndex = 1;
			// 
			// txtAgent
			// 
			this.txtAgent.Location = new System.Drawing.Point(9, 133);
			this.txtAgent.Name = "txtAgent";
			this.txtAgent.Size = new System.Drawing.Size(183, 20);
			this.txtAgent.TabIndex = 2;
			// 
			// btnAdd
			// 
			this.btnAdd.Location = new System.Drawing.Point(198, 133);
			this.btnAdd.Name = "btnAdd";
			this.btnAdd.Size = new System.Drawing.Size(41, 20);
			this.btnAdd.TabIndex = 3;
			this.btnAdd.Text = "+";
			this.btnAdd.UseVisualStyleBackColor = true;
			this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
			// 
			// btnRemove
			// 
			this.btnRemove.Location = new System.Drawing.Point(245, 133);
			this.btnRemove.Name = "btnRemove";
			this.btnRemove.Size = new System.Drawing.Size(41, 20);
			this.btnRemove.TabIndex = 4;
			this.btnRemove.Text = "-";
			this.btnRemove.UseVisualStyleBackColor = true;
			this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(6, 156);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(166, 13);
			this.label6.TabIndex = 5;
			this.label6.Text = "Currently accepted mission agent:";
			// 
			// txtCurrentAgent
			// 
			this.txtCurrentAgent.Location = new System.Drawing.Point(9, 172);
			this.txtCurrentAgent.Name = "txtCurrentAgent";
			this.txtCurrentAgent.Size = new System.Drawing.Size(183, 20);
			this.txtCurrentAgent.TabIndex = 6;
			// 
			// chkCircleAgents
			// 
			this.chkCircleAgents.AutoSize = true;
			this.chkCircleAgents.Location = new System.Drawing.Point(9, 198);
			this.chkCircleAgents.Name = "chkCircleAgents";
			this.chkCircleAgents.Size = new System.Drawing.Size(117, 17);
			this.chkCircleAgents.TabIndex = 7;
			this.chkCircleAgents.Text = "Circle event agents";
			this.chkCircleAgents.UseVisualStyleBackColor = true;
			// 
			// SettingsForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(632, 453);
			this.Controls.Add(this.btnStart);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.btnSetup);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.statusBar);
			this.Controls.Add(this.menuMain);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.MainMenuStrip = this.menuMain;
			this.Name = "SettingsForm";
			this.Text = "Eve Online Courier Missions Bot [BA]";
			this.Load += new System.EventHandler(this.SettingsForm_Load);
			this.menuMain.ResumeLayout(false);
			this.menuMain.PerformLayout();
			this.tabControl.ResumeLayout(false);
			this.tabSettings.ResumeLayout(false);
			this.grpCharacter.ResumeLayout(false);
			this.grpCharacter.PerformLayout();
			this.grpLogin.ResumeLayout(false);
			this.grpLogin.PerformLayout();
			this.tabMissions.ResumeLayout(false);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip menuMain;
		private System.Windows.Forms.StatusStrip statusBar;
		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabSettings;
		private System.Windows.Forms.Button btnSetup;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.Button btnStart;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem settingsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem setupToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
		private System.Windows.Forms.GroupBox grpLogin;
		private System.Windows.Forms.Button btnBrowse;
		private System.Windows.Forms.TextBox txtLogin;
		private System.Windows.Forms.TextBox txtPath;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.MaskedTextBox txtPassword;
		private System.Windows.Forms.OpenFileDialog dlgOpen;
		private System.Windows.Forms.GroupBox grpCharacter;
		private System.Windows.Forms.ComboBox cmbPosition;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.TabPage tabMissions;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnRemove;
		private System.Windows.Forms.Button btnAdd;
		private System.Windows.Forms.TextBox txtAgent;
		private System.Windows.Forms.ListBox lstAgents;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.TextBox txtCurrentAgent;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.CheckBox chkCircleAgents;
	}
}