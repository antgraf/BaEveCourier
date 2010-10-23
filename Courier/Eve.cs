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
		private const int pWindowWaitTime = 15;	// seconds
		private const int pWindowWaitInterval = 5;	// seconds
		private const int pWindowWaitAttempts = 5;
		private const int pRandomWaitTime = (int)(0.75 * 1000);
		private const int pRandomWaitDelta = 35;
		private const string pDebugFolder = "Debug";
		private const string pImageExtension = ".png";

		private const string pImageStatusOk = "Images/status_ok.png";
		private const string pImageSkull = "Images/skull.png";
		private const string pImageCourierMission = "Images/courier_mission.png";
		private const string pImageNoMissions = "Images/no_missions.png";
		private const string pImageRemoteMission = "Images/remote_mission.png";
		private const string pImageLowSecMission = "Images/low_sec.png";
		private const string pImageUnDock = "Images/undock.png";

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

		public void Log(string stage, string msg)
		{
			if(pMessager != null)
			{
				pMessager.SendMessage(stage, msg);
			}
		}

		private void SaveDebugImage(Bitmap image)
		{
			if(Debug)
			{
				try
				{
					string filename = FileUtils.MakeValidFileName(DateTime.Now.ToString() + Guid.NewGuid()) + pImageExtension;
					image.Save(FileUtils.CombineWinPath(pDebugFolder, filename));
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
			}
			catch(Exception ex)
			{
				Log("SelectCharacter", ex.ToString());
				CleanUp();
			}
			Log("SelectCharacter", "Complete");
			return ok;
		}

		public bool SetDestinationToAgent(string agent)
		{
			bool ok = false;
			try
			{
				Log("SetDestinationToAgent", "OpenAgentWindow");
				OpenAgentWindow(agent);
			}
			catch(Exception ex)
			{
				Log("SetDestinationToAgent", ex.ToString());
				CleanUp();
			}
			Log("SetDestinationToAgent", "Complete");
			return ok;
		}

		public void Close()
		{
			Log("Close", "Click \"cross\" button");
			// POINT: close button
			Coordinate close_pt = new Coordinate(
				new StretchedPoint() { X = 0.329126213592233, Y = 0.66078184110971 });
			pEveWindow.LeftClick(close_pt);	// click "cross" button
			// TODO: any confirmations?
			Log("Close", "Complete");
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
				return Directory.Exists(pDebugFolder);
			}
		}
	}
}
