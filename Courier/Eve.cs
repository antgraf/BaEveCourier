using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using WindowEntity;
using System.Diagnostics;
using Logger;
using Courier.Exceptions;
using System.Drawing;
using System.IO;
using BACommon;

namespace Courier
{
	public enum CharacterPosition
	{
		Main = 0,
		Left = 1,
		Right = 2
	}

	public partial class Eve
	{
		private const int pCommonTimeout = 15 * 1000;	// milliseconds
		private const int pMinWindowWidth = 1000;
		private const string pEveProcessName = "ExeFile";
		private const int pStandardWaitTime = (int)(1.5 * 1000);	// milliseconds
		private const int pWindowWaitTime = 15;	// seconds
		private const int pWindowWaitInterval = 5;	// seconds
		private const int pWindowWaitAttempts = 5;
		private const int pRandomWaitTime = (int)(0.75 * 1000);
		private const int pRandomWaitDelta = 35;
		private const string pDebugFolder = "Debug";
		private const string pImageExtension = ".png";
		private const double pStandardColorDeviation = 0.1;//6 / Window.NormalizationCoefficientForColorDeviation;	// sqrt(3^2 + 3^2 + 3^2) < 5.2
		private const string pEveSettingsFolder = @"CCP\EVE\{0}\settings";
		private const string pTranquilityServerSuffix = "_tranquility";

		private const string pImageStatusOk = @"Images\status_ok.png";
		private const string pImageSkull = @"Images\skull.png";
		private const string pImageCourierMission = @"Images\courier_mission.png";
		private const string pImageNoMissions = @"Images\no_missions.png";
		private const string pImageRemoteMission = @"Images\remote_mission.png";
		private const string pImageLowSecMission = @"Images\low_sec.png";
		private const string pImageUnDock = @"Images\undock.png";
		private const string pImageSetDestination = @"Images\set_destination.png";
		private const string pImageDock = @"Images\dock.png";
		private const string pImageAgentWarning = @"Images\agent_warning.png";

		private const string pSettings1 = @"Data\core_char__.dat";
		private const string pSettings2 = @"Data\core_public__.dat";
		private const string pSettings3 = @"Data\core_user__.dat";
		private const string pSettings4 = @"Data\prefs.ini";

		private string pLocalPath = null;
		private Window pEveWindow = null;
		private Timer pTimeOut = new Timer();
		private bool pTimedOut = false;
		private Process pEveProcess = null;
		private IMessageHandler pMessager = null;
		private string pPathToEve = null;
		private string pLogin = null;
		private string pPassword = null;
		private CharacterPosition pPosition = CharacterPosition.Main;
		private List<string> pAgents = null;
		private string pCurrentAgent = null;
		private bool pCircleAgents = false;

		public Eve(IMessageHandler messager)
		{
			pMessager = messager;
			pTimeOut.AutoReset = false;
			pTimeOut.Interval = pCommonTimeout;
			pTimeOut.Elapsed += new ElapsedEventHandler(pTimeOut_Elapsed);
		}

		private string Relative2AbsolutePath(string path)
		{
			if(StringUtils.IsEmpty(pLocalPath))
			{
				return FileUtils.Relative2AbsolutePath(path);
			}
			else
			{
				return FileUtils.CombineWinPath(pLocalPath, path);
			}
		}

		public void Log(string stage, string msg)
		{
			if(pMessager != null)
			{
				pMessager.SendMessage(stage, msg);
			}
		}

		private bool FindImage(Coordinate topLeft, Coordinate bottomRight, string imageName)
		{
			Bitmap screen = pEveWindow.Screenshot(topLeft, bottomRight);
			SaveDebugImage(screen);
			Bitmap fragment = new Bitmap(Relative2AbsolutePath(imageName));
			pEveWindow.AllowedColorDeviation = pStandardColorDeviation;
			return pEveWindow.FindImageWithColorDeviation(screen, fragment) != null;
		}

		private void SaveDebugImage(Bitmap image)
		{
			if(Debug)
			{
				try
				{
					string filename = FileUtils.MakeValidFileName(DateTime.Now.ToString() + Guid.NewGuid()) + pImageExtension;
					image.Save(Relative2AbsolutePath(FileUtils.CombineWinPath(pDebugFolder, filename)));
				}
				catch(Exception ex)
				{
					Log("SaveDebugImage", ex.ToString());
				}
			}
		}

		public void CleanUp()
		{
			if(pEveWindow != null)
			{
				Log("CleanUp", "Close window");
				pEveWindow.Close();
				pEveWindow = null;
			}
			if(pEveProcess != null)
			{
				if(!pEveProcess.HasExited)
				{
					Log("CleanUp", "Kill process");
					pEveProcess.Kill();
				}
				pEveProcess = null;
			}
			Log("CleanUp", "Complete");
		}

		public bool ResetEveSettings(string pathToEve)
		{
			bool ok = false;
			try
			{
				if(StringUtils.IsEmpty(pathToEve))
				{
					throw new ArgumentException("Path is empty.", "pathToEve");
				}
				Log("ResetEveSettings", "GetEveSettingsPath");
				string settings = GetEveSettingsPath(pathToEve);
				Log("ResetEveSettings", "ClearEveSettings");
				if(ClearEveSettings(settings))
				{
					Log("ResetEveSettings", "CopyDefaultSettings");
					CopyDefaultSettings(settings);
					ok = true;
				}
				Log("ResetEveSettings", "Complete");
			}
			catch(Exception ex)
			{
				Log("ResetEveSettings", ex.ToString());
				CleanUp();
			}
			return ok;
		}

		public bool SetEveSettings()
		{
			bool ok = false;
			try
			{
				Log("SetEveSettings", "MinimizeLeftPanel");
				MinimizeLeftPanel();
				Log("SetEveSettings", "OpenOptionsWindow");
				OpenOptionsWindow();
				Log("SetEveSettings", "SetOptions");
				SetOptions();
				Log("SetEveSettings", "ClickQuitGameButton");
				ClickQuitGameButton(true);
				Log("SetEveSettings", "Complete");
				ok = true;
			}
			catch(Exception ex)
			{
				Log("SetEveSettings", ex.ToString());
			}
			CleanUp();
			return ok;
		}

		public bool Launch(string pathToEve)
		{
			try
			{
				Log("Launch", "LaunchApplication");
				LaunchApplication(pathToEve);
				Log("Launch", "SkipSplashAndAttach");
				SkipSplashAndAttach();
				Log("Launch", "Complete");
			}
			catch(Exception ex)
			{
				Log("Launch", ex.ToString());
				CleanUp();
				return false;
			}
			return true;
		}

		public bool DoLogin(string login, string password)
		{
			bool ok = false;
			try
			{
				// wait for login screen loads
				Log("DoLogin", "CheckLoginScreen");
				if(!CheckLoginScreen())
				{
					Log("DoLogin", "CheckLoginScreen again");
					pEveWindow.Wait(pWindowWaitInterval * 1000);
					if(!CheckLoginScreen())
					{
						Log("DoLogin", "Cannot find login screen");
						throw new CannotFindLoginScreenException();
					}
				}
				// enter login / pass
				Log("DoLogin", "EraseLogin");
				EraseLogin();
				Log("DoLogin", "EnterLoginPassword");
				EnterLoginPassword(login, password);
				// check login success
				pEveWindow.Wait(pWindowWaitInterval * 1000);
				Log("DoLogin", "CheckLoginError");
				ok = CheckLoginError();
				for(int i = 0; i < pWindowWaitAttempts && !ok; i++)
				{
					pEveWindow.Wait(pWindowWaitInterval * 1000);
					Log("DoLogin", "CheckLoginError " + i.ToString());
					ok = CheckLoginError();
				}
			}
			catch(Exception ex)
			{
				Log("DoLogin", ex.ToString());
				CleanUp();
			}
			Log("DoLogin", "Complete");
			return ok;
		}

		public bool SelectCharacter()
		{
			bool ok = false;
			try
			{
				Log("SelectCharacter", "ClickCharacter");
				ClickCharacter(pPosition);
				Log("SelectCharacter", "EnterGame");
				EnterGame();
				ok = true;
			}
			catch(Exception ex)
			{
				Log("SelectCharacter", ex.ToString());
				CleanUp();
			}
			Log("SelectCharacter", "Complete");
			return ok;
		}

		public bool SetDestinationToAgent(string agent, bool openAgentWindow)
		{
			bool ok = false;
			try
			{
				Log("SetDestinationToAgent", "CheckInDock");
				bool in_dock = CheckInDock();
				if(openAgentWindow)
				{
					Log("SetDestinationToAgent", "OpenAgentWindow");
					OpenAgentWindow(agent);
				}

				Log("SetDestinationToAgent", "CheckAgentHasNoMissions");
				if(CheckAgentHasNoMissions())
				{
					throw new AgentHasNoMissionsAvailableException();
				}

				CallAgentLocationMenu();
				WaitRandom();
				Log("SetDestinationToAgent", "CheckAgentLocationDestinationMenuItem");
				if(CheckAgentLocationDestinationMenuItem())
				{
					Log("SetDestinationToAgent", "SetAgentDestination");
					SetAgentDestination();
					ok = true;
				}

				Log("SetDestinationToAgent", "CheckAgentLocationDockMenuItem");
				if(CheckAgentLocationDockMenuItem())
				{
					Log("SetDestinationToAgent", "DockToAgent");
					DockToAgent();
					ok = true;
				}
			}
			catch(Exception ex)
			{
				Log("SetDestinationToAgent", ex.ToString());
				CleanUp();
			}
			Log("SetDestinationToAgent", "Complete");
			return ok;
		}

		public string GetAgent()
		{
			throw new NotImplementedException();
		}

		public bool MakeBlackScreen()
		{
			bool ok = false;
			try
			{
				Log("MakeBlackScreen", "OpenMap");
				OpenMap();
				Log("MakeBlackScreen", "MinimizeMapControl");
				MinimizeMapControl();
				Log("MakeBlackScreen", "MoveMapOut");
				MoveMapOut();
				ok = true;
			}
			catch(Exception ex)
			{
				Log("MakeBlackScreen", ex.ToString());
				CleanUp();
			}
			Log("MakeBlackScreen", "Complete");
			return ok;
		}

		public void Close()
		{
			Log("Close", "Click \"cross\" button");
			// POINT: close button
			Coordinate close_pt = new Coordinate(
				new StretchedPoint() { X = 0.988349514563107, Y = 0.0138713745271122 });
			pEveWindow.LeftClick(close_pt);	// click "cross" button
			// TODO: any confirmations?
			pEveWindow.Wait(pStandardWaitTime);
			Log("Close", "Complete");
		}

		public string LocalPath
		{
			get { return pLocalPath; }
			set { pLocalPath = value; }
		}

		public Window EveWindow
		{
			get { return pEveWindow; }
			set { pEveWindow = value; }
		}

		public string PathToEve
		{
			get { return pPathToEve; }
			set { pPathToEve = value; }
		}

		public string Login
		{
			get { return pLogin; }
			set { pLogin = value; }
		}

		public string Password
		{
			get { return pPassword; }
			set { pPassword = value; }
		}

		public CharacterPosition Position
		{
			get { return pPosition; }
			set { pPosition = value; }
		}

		public List<string> Agents
		{
			get { return pAgents; }
			set { pAgents = value; }
		}

		public string CurrentAgent
		{
			get { return pCurrentAgent; }
			set { pCurrentAgent = value; }
		}

		public bool CircleAgents
		{
			get { return pCircleAgents; }
			set { pCircleAgents = value; }
		}

		public bool Debug
		{
			get
			{
				return Directory.Exists(Relative2AbsolutePath(pDebugFolder));
			}
		}
	}
}
