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
using BACommon;

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
			SaveDebugImage(status_check);
			Bitmap status_ok = new Bitmap(FileUtils.Relative2AbsolutePath(pImageStatusOk));
			pEveWindow.AllowedColorDeviation = 6;	// sqrt(3^2 + 3^2 + 3^2) < 5.2
			Coordinate result = pEveWindow.FindImageWithColorDeviation(status_check, status_ok);	// check image
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
			pEveWindow.CtrlKeyDown();
			pEveWindow.KeySendAndWait("a");	// select all
			pEveWindow.CtrlKeyUp();
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
			SaveDebugImage(skull_check);
			Bitmap skull = new Bitmap(FileUtils.Relative2AbsolutePath(pImageSkull));
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

		private void OpenAgentWindow(string agent)
		{
			OpenPeopleAndPlaces();
			SelectSearchType(4);	// character (exact)
			EraseSearch();
			pEveWindow.KeySendAndWait(agent);	// search for agent
			pEveWindow.KeySendAndWait("~");
			// POINT: agent in the list
			Coordinate agent_pt = new Coordinate(
				new StretchedPoint() { X = 0.48252427184466, Y = 0.530895334174023 });
			pEveWindow.RightClick(agent_pt);	// open menu
			WaitRandom();
			// POINT: "start conversation" menu
			Coordinate menu_pt = new Coordinate(
				new StretchedPoint() { X = 0.524271844660194, Y = 0.559899117276166 });
			pEveWindow.LeftClick(menu_pt);
			WaitRandom();
		}

		private void OpenPeopleAndPlaces()
		{
			// POINT: people & places
			Coordinate pp = new Coordinate(
				new StretchedPoint() { X = 0.0203883495145631, Y = 0.240857503152585 });
			pEveWindow.LeftClick(pp);
			WaitRandom();
		}

		private void SelectSearchType(int index)
		{
			// POINT: search type selector
			Coordinate searchtype = new Coordinate(
				new StretchedPoint() { X = 0.406796116504854, Y = 0.32156368221942 });
			pEveWindow.LeftClick(searchtype);
			WaitRandom();
			for(int i = 0; i < 11; i++)	// go to the top-most search type
			{
				pEveWindow.KeySend("{UP}");
			}
			for(int i = 0; i < index; i++)	// go to search type
			{
				pEveWindow.KeySend("{DOWN}");
			}
			pEveWindow.KeySendAndWait("~");	// confirm search type
		}

		private void EraseSearch()
		{
			// POINT: search textbox
			Coordinate search = new Coordinate(
				new StretchedPoint() { X = 0.445631067961165, Y = 0.327868852459016 });
			pEveWindow.LeftClick(search);
			WaitRandom();
			pEveWindow.CtrlKeyDown();
			pEveWindow.KeySendAndWait("a");	// select all
			pEveWindow.CtrlKeyUp();
			pEveWindow.KeySendAndWait("{DEL}");	// delete
		}

		private bool CheckAgentProvideCourierMission()
		{
			ActivateAgentWindow();
			// POINT: top-left corner to search courier mission image
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.236893203883495, Y = 0.776796973518285 });
			// POINT: bottom-right corner to search courier mission image
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.348543689320388, Y = 0.85750315258512 });
			Bitmap screen = pEveWindow.Screenshot(tl_pt, br_pt);
			Bitmap courier = new Bitmap(FileUtils.Relative2AbsolutePath(pImageCourierMission));
			return pEveWindow.FindImageExactly(screen, courier) != null;
		}

		private bool CheckAgentHasNoMissions()
		{
			ActivateAgentWindow();
			// POINT: top-left corner to search text
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.196116504854369, Y = 0.5359394703657 });
			// POINT: bottom-right corner to search text
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.277669902912621, Y = 0.558638083228247 });
			Bitmap screen = pEveWindow.Screenshot(tl_pt, br_pt);
			Bitmap no = new Bitmap(FileUtils.Relative2AbsolutePath(pImageNoMissions));
			return pEveWindow.FindImageExactly(screen, no) != null;
		}

		private bool CheckRemoteAgentHasMission()
		{
			ActivateAgentWindow();
			// POINT: top-left corner to search text
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.196116504854369, Y = 0.5359394703657 });
			// POINT: bottom-right corner to search text
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.277669902912621, Y = 0.558638083228247 });
			Bitmap screen = pEveWindow.Screenshot(tl_pt, br_pt);
			Bitmap remote = new Bitmap(FileUtils.Relative2AbsolutePath(pImageRemoteMission));
			return pEveWindow.FindImageExactly(screen, remote) != null;
		}

		private bool CheckLowSecMission()
		{
			ActivateAgentWindow();
			// POINT: top-left corner to search text
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.32621359223301, Y = 0.450189155107188 });
			// POINT: bottom-right corner to search text
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.563106796116505, Y = 0.513240857503153 });
			Bitmap screen = pEveWindow.Screenshot(tl_pt, br_pt);
			Bitmap lowsec = new Bitmap(FileUtils.Relative2AbsolutePath(pImageLowSecMission));
			return pEveWindow.FindImageExactly(screen, lowsec) != null;
		}

		private bool CheckInDock()
		{
			// POINT: top-left corner to search undock button
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.0058252427184466, Y = 0.921815889029004 });
			// POINT: bottom-right corner to search undock button
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.0359223300970874, Y = 0.952080706179067 });
			Bitmap screen = pEveWindow.Screenshot(tl_pt, br_pt);
			Bitmap undock = new Bitmap(FileUtils.Relative2AbsolutePath(pImageUnDock));
			return pEveWindow.FindImageExactly(screen, undock) != null;
		}

		private void ActivateAgentWindow()
		{
			// POINT: agent window
			Coordinate window = new Coordinate(
				new StretchedPoint() { X = 0.200970873786408, Y = 0.32156368221942 });
			pEveWindow.LeftClick(window);
			WaitRandom();
		}

		private void AcceptMission()
		{
			ActivateAgentWindow();
			// POINT: accept button
			Coordinate button = new Coordinate(
				new StretchedPoint() { X = 0.741747572815534, Y = 0.979823455233291 });
			pEveWindow.LeftClick(button);
			WaitRandom();
		}

		private void RejectMission()
		{
			ActivateAgentWindow();
			// POINT: decline button
			Coordinate button = new Coordinate(
				new StretchedPoint() { X = 0.8, Y = 0.978562421185372 });
			pEveWindow.LeftClick(button);
			WaitRandom();
		}

		private void CallAgentLocationMenu()
		{
			ActivateAgentWindow();
			// POINT: agent location hyper-link
			Coordinate button = new Coordinate(
				new StretchedPoint() { X = 0.35631067961165, Y = 0.447667087011349 });
			pEveWindow.RightClick(button);
			WaitRandom();
		}

		private bool CheckAgentLocationDestinationMenuItem()
		{
			ActivateAgentWindow();
			// POINT: top-left corner to search menu item
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.35631067961165, Y = 0.447667087011349 });
			// POINT: bottom-right corner to search menu item
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.54368932038835, Y = 0.630517023959647 });
			Bitmap screen = pEveWindow.Screenshot(tl_pt, br_pt);
			Bitmap dest = new Bitmap(FileUtils.Relative2AbsolutePath(pImageSetDestination));
			return pEveWindow.FindImageExactly(screen, dest) != null;
		}

		private bool CheckAgentLocationDockMenuItem()
		{
			ActivateAgentWindow();
			// POINT: top-left corner to search menu item
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.35631067961165, Y = 0.447667087011349 });
			// POINT: bottom-right corner to search menu item
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.54368932038835, Y = 0.630517023959647 });
			Bitmap screen = pEveWindow.Screenshot(tl_pt, br_pt);
			Bitmap dock = new Bitmap(FileUtils.Relative2AbsolutePath(pImageDock));
			return pEveWindow.FindImageExactly(screen, dock) != null;
		}

		private void SetAgentDestination()
		{
			// POINT: set destination menu item
			Coordinate set_dest = new Coordinate(
				new StretchedPoint() { X = 0.397087378640777, Y = 0.477931904161412 });
			pEveWindow.LeftClick(set_dest);
			WaitRandom();
		}

		private void DockToAgent()
		{
			// POINT: dock menu item
			Coordinate dock = new Coordinate(
				new StretchedPoint() { X = 0.385436893203883, Y = 0.496847414880202 });
			pEveWindow.LeftClick(dock);
			WaitRandom();
		}
	}
}
