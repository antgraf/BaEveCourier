using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using ExecutionActors;
using BACommon;
using EveOperations;

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
		private readonly PluginBase pPlugin = null;
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

		private void SetDefaultSettings()
		{
			if(!pPlugin.Settings.ContainsKey(CourierSettings.LastAgent))
			{
				pPlugin.Settings[CourierSettings.LastAgent] = CourierSettings.DefaultLastAgent;
			}
			if(!pPlugin.Settings.ContainsKey(CourierSettings.CurrentAgent))
			{
				pPlugin.Settings[CourierSettings.CurrentAgent] = CourierSettings.DefaultCurrentAgent;
			}
			if(!pPlugin.Settings.ContainsKey(CourierSettings.CurrentCargo))
			{
				pPlugin.Settings[CourierSettings.CurrentCargo] = CourierSettings.DefaultCurrentCargo;
			}
			if(!pPlugin.Settings.ContainsKey(CourierSettings.AgentTimers))
			{
				pPlugin.Settings[CourierSettings.AgentTimers] = CourierSettings.DefaultAgentTimers;
			}
		}

		private void SaveSettings()
		{
			pPlugin.Settings[CourierSettings.Path] = txtPath.Text;
			pPlugin.Settings[CourierSettings.Login] = txtLogin.Text;
			pPlugin.Settings[CourierSettings.Password] = txtPassword.Text;
			pPlugin.Settings[CourierSettings.Position] = (CharacterPosition)cmbPosition.SelectedIndex;
			pPlugin.Settings[CourierSettings.Agents] = new List<string>(lstAgents.Items.OfType<string>());
			pPlugin.Settings[CourierSettings.CircleAgents] = chkCircleAgents.Checked;
			pPlugin.SaveSettings();
		}

		private void LoadSettings()
		{
			pPlugin.LoadSettings();
			SetDefaultSettings();
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
			chkCircleAgents.Checked = pPlugin.Settings.ContainsKey(CourierSettings.CircleAgents) ?
				(bool)pPlugin.Settings[CourierSettings.CircleAgents] : CourierSettings.DefaultCircleAgents;
		}

		private void Return(SettingsFormResult result)
		{
			pResult = result;
			Close();
		}

		private void BtnBrowseClick(object sender, EventArgs e)
		{
			dlgOpen.FileName = txtPath.Text;
			if(dlgOpen.ShowDialog(this) == DialogResult.OK)
			{
				txtPath.Text = dlgOpen.FileName;
			}
		}

		private void SettingsFormLoad(object sender, EventArgs e)
		{
			LoadSettings();
		}

		private void BtnSetupClick(object sender, EventArgs e)
		{
			SaveSettings();
			Return(SettingsFormResult.Setup);
		}

		private void BtnSaveClick(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void BtnStartClick(object sender, EventArgs e)
		{
			SaveSettings();
			Return(SettingsFormResult.Run);
		}

		private void ExitToolStripMenuItemClick(object sender, EventArgs e)
		{
			Return(SettingsFormResult.Close);
		}

		private void SetupToolStripMenuItemClick(object sender, EventArgs e)
		{
			SaveSettings();
			Return(SettingsFormResult.Setup);
		}

		private void SaveToolStripMenuItemClick(object sender, EventArgs e)
		{
			SaveSettings();
		}

		private void AboutToolStripMenuItemClick(object sender, EventArgs e)
		{
			MessageBox.Show(this, "Eve Online Courier Missions Bot\r\n" + CourierSettings.Version,
				"About", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void BtnAddClick(object sender, EventArgs e)
		{
			if(!StringUtils.IsEmpty(txtAgent.Text))
			{
				lstAgents.Items.Add(txtAgent.Text.ToLower());
			}
		}

		private void BtnRemoveClick(object sender, EventArgs e)
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
