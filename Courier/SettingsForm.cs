using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using ExecutionActors;
using BACommon;

namespace Courier
{
	public enum SettingsFormResult
	{
		None,
		Close,
		Run,
		Setup
	}

	public partial class SettingsForm : Form
	{
		private PluginBase pPlugin = null;
		private SettingsFormResult pResult = SettingsFormResult.None;

		public SettingsFormResult Result
		{
			get { return pResult; }
		}

		public SettingsForm(PluginBase plugin)
		{
			pPlugin = plugin;
			InitializeComponent();
		}

		private void SaveSettings()
		{
			pPlugin.Settings[CourierSettings.Path] = txtPath.Text;
			pPlugin.Settings[CourierSettings.Login] = txtLogin.Text;
			pPlugin.Settings[CourierSettings.Password] = txtPassword.Text;
			pPlugin.Settings[CourierSettings.Position] = (CharacterPosition)cmbPosition.SelectedIndex;
			pPlugin.Settings[CourierSettings.Agents] = new List<string>(lstAgents.Items.OfType<string>());
			pPlugin.Settings[CourierSettings.CurrentAgent] = txtCurrentAgent.Text;
			pPlugin.Settings[CourierSettings.CircleAgents] = chkCircleAgents.Checked;
			pPlugin.SaveSettings();
		}

		private void LoadSettings()
		{
			pPlugin.LoadSettings();
			txtPath.Text = pPlugin.Settings.ContainsKey(CourierSettings.Path) ?
				(string)pPlugin.Settings[CourierSettings.Path] : CourierSettings.DefaultPath;
			txtLogin.Text = pPlugin.Settings.ContainsKey(CourierSettings.Login) ?
				(string)pPlugin.Settings[CourierSettings.Login] : CourierSettings.DefaultLogin;
			txtPassword.Text = pPlugin.Settings.ContainsKey(CourierSettings.Password) ?
				(string)pPlugin.Settings[CourierSettings.Password] : CourierSettings.DefaultPassword;
			cmbPosition.SelectedIndex = pPlugin.Settings.ContainsKey(CourierSettings.Position) ?
				(int)pPlugin.Settings[CourierSettings.Position] : (int)CourierSettings.DefaultPosition;
			lstAgents.Items.AddRange(pPlugin.Settings.ContainsKey(CourierSettings.Agents) ?
				((List<string>)pPlugin.Settings[CourierSettings.Agents]).ToArray() : CourierSettings.DefaultAgents);
			txtCurrentAgent.Text = pPlugin.Settings.ContainsKey(CourierSettings.CurrentAgent) ?
				(string)pPlugin.Settings[CourierSettings.CurrentAgent] : CourierSettings.DefaultCurrentAgent;
			chkCircleAgents.Checked = pPlugin.Settings.ContainsKey(CourierSettings.CircleAgents) ?
				(bool)pPlugin.Settings[CourierSettings.CircleAgents] : CourierSettings.DefaultCircleAgents;
		}

		private void Return(SettingsFormResult result)
		{
			pResult = result;
			Close();
		}

		private void btnBrowse_Click(object sender, EventArgs e)
		{
			dlgOpen.FileName = txtPath.Text;
			if(dlgOpen.ShowDialog(this) == DialogResult.OK)
			{
				txtPath.Text = dlgOpen.FileName;
			}
		}

		private void SettingsForm_Load(object sender, EventArgs e)
		{
			LoadSettings();
		}

		private void btnSetup_Click(object sender, EventArgs e)
		{
			Return(SettingsFormResult.Setup);
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void btnStart_Click(object sender, EventArgs e)
		{
			Return(SettingsFormResult.Run);
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Return(SettingsFormResult.Close);
		}

		private void setupToolStripMenuItem_Click(object sender, EventArgs e)
		{
			Return(SettingsFormResult.Setup);
		}

		private void saveToolStripMenuItem_Click(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			MessageBox.Show(this, "Eve Online Courier Missions Bot\r\n" + CourierSettings.Version,
				"About", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void btnAdd_Click(object sender, EventArgs e)
		{
			if(!StringUtils.IsEmpty(txtAgent.Text))
			{
				lstAgents.Items.Add(txtAgent.Text);
			}
		}

		private void btnRemove_Click(object sender, EventArgs e)
		{
			if(lstAgents.SelectedIndices.Count > 0)
			{
				foreach(int index in lstAgents.SelectedIndices)
				{
					lstAgents.Items.RemoveAt(index);
				}
			}
		}
	}
}
