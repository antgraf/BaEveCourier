using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using WindowEntity;
using System.Diagnostics;
using Logger;
using System.Drawing;
using System.IO;
using BACommon;
using EveOperations.Exceptions;

namespace EveOperations
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
		private const int pLoadWaitTime = (int)(3.5 * 1000);	// milliseconds
		private const int pGateWaitTime = (int)(0.5 * 1000);	// milliseconds
		private const int pWarpWaitTime = (int)(2.5 * 1000);	// milliseconds
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
		private const string pImageNoMissions2 = @"Images\no_missions2.png";
		private const string pImageRemoteMission = @"Images\remote_mission.png";
		private const string pImageLowSecMission = @"Images\low_sec.png";
		private const string pImageUnDock = @"Images\undock.png";
		private const string pImageSetDestination = @"Images\set_destination.png";
		private const string pImageDock = @"Images\dock.png";
		private const string pImageAgentWarning = @"Images\agent_warning.png";
		private const string pImageWrongLocationWarning = @"Images\wrong_location.png";
		private const string pImageShutdownWarning = @"Images\shutdown.png";
		private const string pImageSelectedGate = @"Images\selected_gate.png";
		private const string pImageUnSelectedGate = @"Images\unselected_gate.png";
		private const string pImageActiveWarpButton = @"Images\warp_button_active.png";
		private const string pImageActiveActivateButton = @"Images\activate_button_active.png";
		private const string pImageWarehouseTab = @"Images\warehouse.png";
		private const string pImageWarpGrey = @"Images\warp_grey.png";
		private const string pImageWarpBlue = @"Images\warp_blue.png";

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
			return FindImageCoordinate(topLeft, bottomRight, imageName) != null;
		}

		private Coordinate FindImageCoordinate(Coordinate topLeft, Coordinate bottomRight, string imageName)
		{
			Bitmap screen = pEveWindow.Screenshot(topLeft, bottomRight);
			SaveDebugImage(screen);
			Bitmap fragment = new Bitmap(Relative2AbsolutePath(imageName));
			pEveWindow.AllowedColorDeviation = pStandardColorDeviation;
			Coordinate found = pEveWindow.FindImageWithColorDeviation(screen, fragment);
			if(found == null)
			{
				return null;
			}
			WindowEntity.Point rel1 = topLeft.ToRelative(pEveWindow);
			WindowEntity.Point rel2 = found.ToRelative(pEveWindow);
			return new Coordinate(CoordinateType.Relative, new WindowEntity.Point() { X = rel1.X + rel2.X, Y = rel1.Y + rel2.Y });
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
				Log("SetEveSettings", "CheckAndCloseWrongLocationWarning");
				CheckAndCloseWrongLocationWarning();
				Log("SetEveSettings", "MinimizeLeftPanel");
				MinimizeLeftPanel();
				Log("SetEveSettings", "UnPinOverview");
				UnPinOverview();
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

		public bool SelectCharacter(CharacterPosition position)
		{
			bool ok = false;
			try
			{
				Log("SelectCharacter", "ClickCharacter");
				ClickCharacter(position);
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

				Log("SetDestinationToAgent", "CallAgentLocationMenu");
				CallAgentLocationMenu();
				WaitRandom();
				Log("SetDestinationToAgent", "CheckAgentLocationDestinationMenuItem");
				if(CheckAndGoAgentLocationDestinationMenuItem())
				{
					Log("SetDestinationToAgent", "SetAgentDestination");
					SetAgentDestination();
					ok = true;
				}

				Log("SetDestinationToAgent", "CheckAgentLocationDockMenuItem");
				if(CheckAndGoAgentLocationDockMenuItem())
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

		public bool CheckIfAtAgentsStation(string agent)
		{
			bool ok = false;
			try
			{
				Log("CheckIfAtAgentsStation", "CheckInDock");
				bool in_dock = CheckInDock();
				if(in_dock)
				{
					Log("CheckIfAtAgentsStation", "OpenAgentWindow");
					OpenAgentWindow(agent);
					Log("CheckIfAtAgentsStation", "CallAgentLocationMenu");
					CallAgentLocationMenu();
					Log("CheckIfAtAgentsStation", "CheckAgentLocationDestinationMenuItem");
					ok = !CheckAndGoAgentLocationDestinationMenuItem();
				}
			}
			catch(Exception ex)
			{
				Log("CheckIfAtAgentsStation", ex.ToString());
				CleanUp();
			}
			Log("CheckIfAtAgentsStation", "Complete");
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
				ToggleMap();
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

		public bool GetCourierMission()
		{
			bool ok = false;
			try
			{
				Log("GetCourierMission", "SetCourierMissionDestination");
				SetCourierMissionDestination();
				Log("GetCourierMission", "AcceptMission");
				AcceptMission();
				Log("GetCourierMission", "OpenCargo");
				OpenCargo();
				Log("GetCourierMission", "ActivateWarehouseWindow");
				if(!ActivateWarehouseWindow())
				{
					throw new CannotFindWarehouseException();
				}
				Log("GetCourierMission", "SelectAllInWarehouse");
				SelectAllInWarehouse();
				Log("GetCourierMission", "MoveFromWarehouseToCargo");
				MoveFromWarehouseToCargo();
				ok = true;
			}
			catch(Exception ex)
			{
				Log("GetCourierMission", ex.ToString());
				CleanUp();
			}
			Log("GetCourierMission", "Complete");
			return ok;
		}

		public void WaitWhileWarping()
		{
			Log("WaitWhileWarping", "IsWarping");
			while(IsWarping())
			{
				pEveWindow.Wait(pWarpWaitTime);
			}
			Log("WaitWhileWarping", "CheckActivateButtonActive & CheckInDock");
			if(CheckActivateButtonActive() || CheckInDock())
			{
				Log("WaitWhileWarping", "Warp stop evidence found");
			}
			else
			{
				pEveWindow.Wait(pLoadWaitTime);
				Log("WaitWhileWarping", "IsWarping");
				while(IsWarping())
				{
					pEveWindow.Wait(pWarpWaitTime);
				}
			}
			Log("WaitWhileWarping", "Complete");
		}

		public bool WarpThroughActiveGate()
		{
			bool found = false;
			Log("WarpToActiveGate", "CheckAndSelectDestinationGate");
			if(CheckAndSelectDestinationGate())
			{
				found = true;
				Log("WarpToActiveGate", "CheckWarpButtonActive");
				while(!CheckWarpButtonActive())
				{
					pEveWindow.Wait(pGateWaitTime);
				}
				Log("WarpToActiveGate", "CheckActivateButtonActive");
				while(!CheckActivateButtonActive())
				{
					pEveWindow.Wait(pGateWaitTime);
				}
				Log("WarpToActiveGate", "Activate");
				Activate();
				WaitRandom();
				Activate();	// clicky-click to go thru gate asap
				pEveWindow.Wait(pLoadWaitTime);
			}
			Log("WarpToActiveGate", "Complete");
			return found;
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

		public bool Debug
		{
			get
			{
				return Directory.Exists(Relative2AbsolutePath(pDebugFolder));
			}
		}
	}
}
