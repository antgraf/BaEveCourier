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

namespace Courier
{
	public partial class Eve
	{
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
			if(pEveWindow.Width < pMinWindowWidth)	// splash screen
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
			if(pEveWindow == null || pEveWindow.Width < pMinWindowWidth)	// no window or splash screen
			{
				throw new CannotLaunchProcessException(string.Format("Eve window not found. Attempts remain: {0}", attempts));
			}
		}

		private bool CheckLoginScreen()
		{
			// POINT: top-left of "status ok"
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.562135922330097, Y = 0.891551071878941 });
			// POINT: bottom-right of "status ok"
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.614563106796116, Y = 0.904161412358134 });
			Bitmap status_check = pEveWindow.Screenshot(tl_pt, br_pt);
			Bitmap status_ok = new Bitmap(pImageStatusOk);
			Coordinate result = pEveWindow.FindImageExactly(status_check, status_ok);	// check image
			return result != null;
		}

		private void WaitRandom()
		{
			pEveWindow.WaitRandom(pRandomWaitTime, pRandomWaitDelta);
		}

		private void EraseLogin()
		{
			// POINT: login field
			Coordinate login_pt = new Coordinate(
				new StretchedPoint() { X = 0.451456310679612, Y = 0.896595208070618 });
			pEveWindow.LeftClick(login_pt);	// go to login field
			WaitRandom();
			pEveWindow.KeySendAndWait("^a");	// select all
			pEveWindow.KeySendAndWait("{DEL}");	// delete
		}

		private void EnterLoginPassword(string login, string password)
		{
			pEveWindow.KeySendAndWait(login);	// login
			pEveWindow.KeySendAndWait("{TAB}");	// switch text field
			pEveWindow.KeySendAndWait(password);	// password
			pEveWindow.KeySendAndWait("~");	// enter
		}

		private bool CheckLoginError()
		{
			// POINT: top-left of "skull"
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.329126213592233, Y = 0.66078184110971 });
			// POINT: bottom-right of "skull"
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.355339805825243, Y = 0.692307692307692 });
			Bitmap skull_check = pEveWindow.Screenshot(tl_pt, br_pt);
			Bitmap skull = new Bitmap(pImageSkull);
			Coordinate result = pEveWindow.FindImageExactly(skull_check, skull);	// check image
			return result != null;
		}

		private void ClickCharacter(CharacterPosition position)
		{
			// POINT: top-left of "skull"
			Coordinate left = new Coordinate(
				new StretchedPoint() { X = 0.0970873786407767, Y = 0.650693568726356 });
			// POINT: bottom-right of "skull"
			Coordinate right = new Coordinate(
				new StretchedPoint() { X = 0.239805825242718, Y = 0.650693568726356 });
			switch(position)
			{
				case CharacterPosition.Left:
				{
					pEveWindow.LeftClick(left);
					break;
				}
				case CharacterPosition.Right:
				{
					pEveWindow.LeftClick(right);
					break;
				}
			}
		}

		private void EnterGame()
		{
			// POINT: start game button
			Coordinate button = new Coordinate(
				new StretchedPoint() { X = 0.621359223300971, Y = 0.891551071878941 });
			pEveWindow.LeftClick(button);
		}
	}
}
