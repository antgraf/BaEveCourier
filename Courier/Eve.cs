using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using WindowEntity;
using System.Diagnostics;
using Logger;
using Courier.Exceptions;

namespace Courier
{
	public class Eve
	{
		private const int pCommonTimeout = 15 * 1000;	// milliseconds
		private const int pMinWindowWidth = 1000;
		private const string pEveProcessName = "ExeFile";
		private const int pWindowWaitTime = 15;	// seconds
		private const int pWindowWaitInterval = 5;	// seconds
		private const int pWindowWaitAttempts = 5;

		private Window pEveWindow = null;
		private Timer pTimeOut = new Timer();
		private bool pTimedOut = false;
		private Process pEveProcess = null;
		private FileLogger pLogger = null;
		private string pPathToEve = null;
		private string pLogin = null;
		private string pPassword = null;

		public Eve(FileLogger logger)
		{
			pLogger = logger;
			pTimeOut.AutoReset = false;
			pTimeOut.Interval = pCommonTimeout;
			pTimeOut.Elapsed += new ElapsedEventHandler(pTimeOut_Elapsed);
		}

		public void Log(string msg)
		{
			if(pLogger != null)
			{
				pLogger.Log(msg);
			}
		}

		private void StartTimer()
		{
			pTimedOut = false;
			pTimeOut.Start();
		}

		private void StopTimer()
		{
			pTimeOut.Stop();
		}

		private void pTimeOut_Elapsed(object sender, ElapsedEventArgs e)
		{
			pTimedOut = true;
		}

		public void CleanUp()
		{
			if(pEveWindow != null)
			{
				pEveWindow.Close();
				pEveWindow = null;
			}
			if(pEveProcess != null)
			{
				if(!pEveProcess.HasExited)
				{
					pEveProcess.Kill();
				}
				pEveProcess = null;
			}
		}

		private void LaunchApplication(string pathToEve)
		{
			pEveProcess = WindowsMan.RunProcess(pathToEve);
			if(pEveProcess == null)
			{
				throw new CannotLaunchProcessException(pathToEve);
			}
			pEveWindow = WindowsMan.WaitAndAttachTo(pEveProcessName, pWindowWaitTime, pWindowWaitInterval, pWindowWaitAttempts);
			if(pEveWindow == null)
			{
				throw new CannotLaunchProcessException(pEveProcessName);
			}
		}

		private void SkipSplashAndAttach()
		{
			int attempts = pWindowWaitAttempts;
			if(pEveWindow.Width < pMinWindowWidth)
			{
				System.Threading.ManualResetEvent wait = new System.Threading.ManualResetEvent(false);
				Timer timer = new Timer(pWindowWaitInterval * 1000);
				timer.AutoReset = true;
				timer.Elapsed += delegate(object sender, ElapsedEventArgs e)
				{
					if(attempts-- <= 0)
					{
						timer.Stop();
						wait.Set();
					}
					pEveWindow = WindowsMan.UpdateWindow(pEveWindow);
					if(pEveWindow != null && pEveWindow.Width > pMinWindowWidth)
					{
						timer.Stop();
						wait.Set();
					}
				};
				timer.Start();
				wait.WaitOne();
			}
			if(pEveWindow == null || pEveWindow.Width < pMinWindowWidth)
			{
				throw new CannotLaunchProcessException(string.Format("Eve window not found. Attempts remain: {0}", attempts));
			}
		}

		private bool CheckLoginScreen()
		{
			throw new NotImplementedException();
		}

		private void EraseLogin()
		{
			// POINT: login field
			Coordinate login_pt = new Coordinate(
				new StretchedPoint() { X = 0.451456310679612, Y = 0.896595208070618 });
			pEveWindow.LeftClick(login_pt);
			// TODO
			throw new NotImplementedException();
		}

		public bool Launch(string pathToEve)
		{
			try
			{
				LaunchApplication(pathToEve);
				SkipSplashAndAttach();
			}
			catch(Exception ex)
			{
				Log(ex.ToString());
				CleanUp();
				return false;
			}
			return true;
		}

		public bool DoLogin(string login, string password)
		{
			try
			{
				if(!CheckLoginScreen())
				{
					throw new CannotFindLoginScreenException();
				}
				EraseLogin();
				// TODO
				throw new NotImplementedException();
			}
			catch(Exception ex)
			{
				Log(ex.ToString());
				CleanUp();
				return false;
			}
			return true;
		}

		public void Close()
		{
			bool makeCompilerHappy = pTimedOut;
			// TODO
			throw new NotImplementedException();
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
	}
}
