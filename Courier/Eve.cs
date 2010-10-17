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

		public bool Launch(string pathToEve)
		{
			try
			{
				pEveProcess = WindowsMan.RunProcess(pathToEve);
				if(pEveProcess == null)
				{
					throw new CannotLaunchProcess(pathToEve);
				}
				pEveWindow = WindowsMan.WaitAndAttachTo(pEveProcessName, pWindowWaitTime, pWindowWaitInterval, pWindowWaitAttempts);
				if(pEveWindow == null)
				{
					throw new CannotLaunchProcess(pEveProcessName);
				}
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
					throw new CannotLaunchProcess(string.Format("Eve window not found. Attempts remain: {0}", attempts));
				}
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
	}
}
