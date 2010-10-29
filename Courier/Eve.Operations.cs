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
using System.Reflection;
using System.IO;

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

		private void WaitRandom()
		{
			pEveWindow.WaitRandom(pRandomWaitTime, pRandomWaitDelta);
		}

		private string GetEveSettingsPath(string pathToEve)
		{
			string server_suffix = pTranquilityServerSuffix;
			string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			string profile = Path.GetDirectoryName(pathToEve)
				.Replace(":", "")
				.Replace(Path.DirectorySeparatorChar, '_')
				.ToLower()
				+ server_suffix;
			return Path.Combine(appdata, string.Format(pEveSettingsFolder, profile));
		}

		private void CopyDefaultSettings(string pathToEveSettings)
		{
			File.Copy(Relative2AbsolutePath(pSettings1),
				Path.Combine(pathToEveSettings, Path.GetFileName(pSettings1)));
			File.Copy(Relative2AbsolutePath(pSettings2),
				Path.Combine(pathToEveSettings, Path.GetFileName(pSettings2)));
			File.Copy(Relative2AbsolutePath(pSettings3),
				Path.Combine(pathToEveSettings, Path.GetFileName(pSettings3)));
			File.Copy(Relative2AbsolutePath(pSettings4),
				Path.Combine(pathToEveSettings, Path.GetFileName(pSettings4)));
		}

		private bool ClearEveSettings(string pathToEveSettings)
		{
			try
			{
				Directory.Delete(pathToEveSettings, true);
			}
			catch(Exception)
			{
				// ignore
			}
			return Directory.CreateDirectory(pathToEveSettings) != null;
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
			return FindImage(tl_pt, br_pt, pImageStatusOk);
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
				new StretchedPoint() { X = 0.313592233009709, Y = 0.64312736443884 });
			// POINT: bottom-right of "skull"
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.369902912621359, Y = 0.71500630517024 });
			return FindImage(tl_pt, br_pt, pImageSkull);
		}

		private void ClickCharacter(CharacterPosition position)
		{
			// POINT: left character
			Coordinate left = new Coordinate(
				new StretchedPoint() { X = 0.0970873786407767, Y = 0.650693568726356 });
			// POINT: right character
			Coordinate right = new Coordinate(
				new StretchedPoint() { X = 0.239805825242718, Y = 0.650693568726356 });
			switch(position)
			{
				case CharacterPosition.Left:
				{
					pEveWindow.LeftClick(left);
					pEveWindow.Wait(pStandardWaitTime);
					break;
				}
				case CharacterPosition.Right:
				{
					pEveWindow.LeftClick(right);
					pEveWindow.Wait(pStandardWaitTime);
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
				new StretchedPoint() { X = 0.149514563106796, Y = 0.655737704918033 });
			// POINT: bottom-right corner to search courier mission image
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.248543689320388, Y = 0.723833543505675 });
			return FindImage(tl_pt, br_pt, pImageCourierMission);
		}

		private bool CheckAgentHasNoMissions()
		{
			ActivateAgentWindow();
			// POINT: top-left corner to search text
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.109708737864078, Y = 0.413619167717528 });
			// POINT: bottom-right corner to search text
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.221359223300971, Y = 0.475409836065574 });
			return FindImage(tl_pt, br_pt, pImageNoMissions);
		}

		private bool CheckRemoteAgentHasMission()
		{
			ActivateAgentWindow();
			// POINT: top-left corner to search text
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.109708737864078, Y = 0.413619167717528 });
			// POINT: bottom-right corner to search text
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.221359223300971, Y = 0.475409836065574 });
			return FindImage(tl_pt, br_pt, pImageRemoteMission);
		}

		private bool CheckLowSecMission()	// todo "mission" -> "courier" + correct points
		{
			ActivateAgentWindow();
			// POINT: top-left corner to search text
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.32621359223301, Y = 0.450189155107188 });
			// POINT: bottom-right corner to search text
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.563106796116505, Y = 0.513240857503153 });
			return FindImage(tl_pt, br_pt, pImageLowSecMission);
		}

		private bool CheckInDock()
		{
			// POINT: top-left corner to search undock button
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.0058252427184466, Y = 0.921815889029004 });
			// POINT: bottom-right corner to search undock button
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.0359223300970874, Y = 0.952080706179067 });
			return FindImage(tl_pt, br_pt, pImageUnDock);
		}

		private void ActivateAgentWindow()
		{
			// POINT: agent window
			Coordinate window = new Coordinate(
				new StretchedPoint() { X = 0.115533980582524, Y = 0.190416141235813 });
			pEveWindow.LeftClick(window);
			WaitRandom();
		}

		private void AcceptMission()
		{
			ActivateAgentWindow();
			// POINT: accept button
			Coordinate button = new Coordinate(
				new StretchedPoint() { X = 0.655339805825243, Y = 0.849936948297604 });
			pEveWindow.LeftClick(button);
			WaitRandom();
		}

		private void RejectMission()
		{
			ActivateAgentWindow();
			// POINT: decline button
			Coordinate button = new Coordinate(
				new StretchedPoint() { X = 0.709708737864078, Y = 0.848675914249685 });
			pEveWindow.LeftClick(button);
			WaitRandom();
		}

		private void CallAgentLocationMenu()
		{
			ActivateAgentWindow();
			// POINT: agent location hyper-link
			Coordinate button = new Coordinate(
				new StretchedPoint() { X = 0.266990291262136, Y = 0.320302648171501 });
			pEveWindow.RightClick(button);
			WaitRandom();
		}

		private bool CheckAgentLocationDestinationMenuItem()
		{
			// POINT: top-left corner to search menu item
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.266990291262136, Y = 0.320302648171501 });
			// POINT: bottom-right corner to search menu item
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.448543689320388, Y = 0.539722572509458 });
			return FindImage(tl_pt, br_pt, pImageSetDestination);
		}

		private bool CheckAgentLocationDockMenuItem()
		{
			// POINT: top-left corner to search menu item
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.266990291262136, Y = 0.320302648171501 });
			// POINT: bottom-right corner to search menu item
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.448543689320388, Y = 0.539722572509458 });
			return FindImage(tl_pt, br_pt, pImageDock);
		}

		private void SetAgentDestination()
		{
			// POINT: set destination menu item
			Coordinate set_dest = new Coordinate(
				new StretchedPoint() { X = 0.297087378640777, Y = 0.353089533417402 });
			pEveWindow.LeftClick(set_dest);
			WaitRandom();
		}

		private void DockToAgent()
		{
			// POINT: dock menu item
			Coordinate dock = new Coordinate(
				new StretchedPoint() { X = 0.295145631067961, Y = 0.387137452711223 });
			pEveWindow.LeftClick(dock);
			WaitRandom();
		}

		private void UnDock()
		{
			// POINT: undock button
			Coordinate undock = new Coordinate(
				new StretchedPoint() { X = 0.0194174757281553, Y = 0.934426229508197 });
			pEveWindow.LeftClick(undock);
			WaitRandom();
		}

		private void OpenMap()
		{
			pEveWindow.KeySend("{F10}");	// open map
			WaitRandom();
		}

		private void MoveMapOut()
		{
			// POINT: drag from point
			Coordinate from = new Coordinate(
				new StretchedPoint() { X = 0.975728155339806, Y = 0.958385876418663 });
			// POINT: drag to point
			Coordinate to = new Coordinate(
				new StretchedPoint() { X = 0.724271844660194, Y = 0.950819672131147 });
			for(int i = 0; i < 4; i++)
			{
				pEveWindow.DragDropRight(from, to);	// drag map out
				WaitRandom();
			}
		}

		private void MinimizeMapControl()
		{
			// POINT: minimize map control button
			Coordinate undock = new Coordinate(
				new StretchedPoint() { X = 0.986407766990291, Y = 0.0416141235813367 });
			pEveWindow.LeftClick(undock);
			WaitRandom();
		}

		private void OpenOptionsWindow()
		{
			pEveWindow.KeySendAndWait("{ESC}", pStandardWaitTime);
		}

		private void SetOptions()
		{
			// POINT: video checkboxes
			Coordinate chkbox = null;
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.298865069356873 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.32156368221942 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.346784363177806 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.387378640776699, Y = 0.368221941992434 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.390920554854981 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.413619167717528 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.466582597730139 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.489281210592686 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.534678436317781 });
			pEveWindow.LeftClick(chkbox);
			// POINT: general settings tab
			Coordinate general = new Coordinate(
				new StretchedPoint() { X = 0.353398058252427, Y = 0.243379571248424 });
			pEveWindow.LeftClick(general);
			WaitRandom();
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.301387137452711 });
			pEveWindow.LeftClick(chkbox);
			chkbox = new Coordinate(
				new StretchedPoint() { X = 0.388349514563107, Y = 0.32156368221942 });
			pEveWindow.LeftClick(chkbox);
			// POINT: theme selector
			Coordinate theme = null;
			// POINT: black theme
			Coordinate black = null;
			theme = new Coordinate(
				new StretchedPoint() { X = 0.866990291262136, Y = 0.358133669609079 });
			black = new Coordinate(
				new StretchedPoint() { X = 0.779611650485437, Y = 0.413619167717528 });
			pEveWindow.LeftClick(theme);
			WaitRandom();
			pEveWindow.LeftClick(black);
			WaitRandom();
			// POINT: color sliders
			Coordinate color = null;
			// POINT: color sliders dragging target point
			Coordinate dragto = null;
			color = new Coordinate(
				new StretchedPoint() { X = 0.842718446601942, Y = 0.442622950819672 });
			dragto = new Coordinate(
				new StretchedPoint() { X = 0.885436893203884, Y = 0.443883984867591 });
			pEveWindow.DragDrop(color, dragto);
			color = new Coordinate(
				new StretchedPoint() { X = 0.809708737864078, Y = 0.532156368221942 });
			dragto = new Coordinate(
				new StretchedPoint() { X = 0.733009708737864, Y = 0.532156368221942 });
			pEveWindow.DragDrop(color, dragto);
			color = new Coordinate(
				new StretchedPoint() { X = 0.810679611650485, Y = 0.549810844892812 });
			dragto = new Coordinate(
				new StretchedPoint() { X = 0.736893203883495, Y = 0.551071878940731 });
			pEveWindow.DragDrop(color, dragto);
			color = new Coordinate(
				new StretchedPoint() { X = 0.809708737864078, Y = 0.563682219419924 });
			dragto = new Coordinate(
				new StretchedPoint() { X = 0.735922330097087, Y = 0.567465321563682 });
			pEveWindow.DragDrop(color, dragto);
			color = new Coordinate(
				new StretchedPoint() { X = 0.809708737864078, Y = 0.585119798234552 });
			dragto = new Coordinate(
				new StretchedPoint() { X = 0.885436893203884, Y = 0.585119798234552 });
			pEveWindow.DragDrop(color, dragto);
			theme = new Coordinate(
				new StretchedPoint() { X = 0.867961165048544, Y = 0.640605296343001 });
			black = new Coordinate(
				new StretchedPoint() { X = 0.772815533980583, Y = 0.692307692307692 });
			pEveWindow.LeftClick(theme);
			WaitRandom();
			pEveWindow.LeftClick(black);
			WaitRandom();
			theme = new Coordinate(
				new StretchedPoint() { X = 0.867961165048544, Y = 0.673392181588903 });
			black = new Coordinate(
				new StretchedPoint() { X = 0.772815533980583, Y = 0.721311475409836 });
			pEveWindow.LeftClick(theme);
			WaitRandom();
			pEveWindow.LeftClick(black);
			WaitRandom();
			// POINT: shortcuts settings tab
			Coordinate shortcuts = new Coordinate(
				new StretchedPoint() { X = 0.430097087378641, Y = 0.247162673392182 });
			pEveWindow.LeftClick(shortcuts);
			WaitRandom();
			// POINT: shortcuts list
			Coordinate list = new Coordinate(
				new StretchedPoint() { X = 0.650485436893204, Y = 0.300126103404792 });
			pEveWindow.LeftClick(list);
			WaitRandom();
			for(int i = 0; i < 32; i++)	// go to "open active ship's cargo" shortcut
			{
				pEveWindow.KeySend("{DOWN}");
			}
			WaitRandom();
			// POINT: add shortcut button
			Coordinate addbutton = new Coordinate(
				new StretchedPoint() { X = 0.609708737864078, Y = 0.412358133669609 });
			pEveWindow.LeftClick(addbutton);
			WaitRandom();
			// POINT: alt checkbox in shortcut window
			Coordinate altbutton = new Coordinate(
				new StretchedPoint() { X = 0.394174757281553, Y = 0.501891551071879 });
			pEveWindow.LeftClick(altbutton);
			WaitRandom();
			// POINT: textbox in shortcut window
			Coordinate textbox = new Coordinate(
				new StretchedPoint() { X = 0.446601941747573, Y = 0.549810844892812 });
			pEveWindow.LeftClick(textbox);
			WaitRandom();
			pEveWindow.KeySendAndWait("c", pStandardWaitTime);	// ctrl+alt+c for open cargo
			// POINT: ok button shortcut window
			Coordinate shortcutok = new Coordinate(
				new StretchedPoint() { X = 0.448543689320388, Y = 0.601513240857503 });
			pEveWindow.LeftClick(shortcutok);
			WaitRandom();
		}

		private void ClickQuitGameButton(bool confirmation)
		{
			// POINT: quit button
			Coordinate quit = new Coordinate(
				new StretchedPoint() { X = 0.836893203883495, Y = 0.791929382093317 });
			pEveWindow.LeftClick(quit);
			WaitRandom();
			if(confirmation)
			{
				// POINT: "do not..." checkbox
				Coordinate donot = new Coordinate(
					new StretchedPoint() { X = 0.349514563106796, Y = 0.597730138713745 });
				pEveWindow.LeftClick(donot);
				WaitRandom();
				// POINT: yes button
				Coordinate yes = new Coordinate(
					new StretchedPoint() { X = 0.476699029126214, Y = 0.62547288776797 });
				pEveWindow.LeftClick(yes);
				WaitRandom();
			}
		}

		private void MinimizeLeftPanel()
		{
			// POINT: minimize button
			Coordinate minimize = new Coordinate(
				new StretchedPoint() { X = 0.113592233009709, Y = 0.0479192938209332 });
			pEveWindow.LeftClick(minimize);
			WaitRandom();
		}

		private bool CheckAgentWarning()
		{
			// POINT: top-left corner to search warning text
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.346601941747573, Y = 0.469104665825977 });
			// POINT: bottom-right corner to search warning text
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.406796116504854, Y = 0.519546027742749 });
			return FindImage(tl_pt, br_pt, pImageAgentWarning);
		}

		private void CloseAgentWarning()
		{
			// POINT: "do not..." checkbox
			Coordinate donot = new Coordinate(
				new StretchedPoint() { X = 0.349514563106796, Y = 0.600252206809584 });
			pEveWindow.LeftClick(donot);
			WaitRandom();
			// POINT: ok button
			Coordinate ok = new Coordinate(
				new StretchedPoint() { X = 0.501941747572816, Y = 0.626733921815889 });
			pEveWindow.LeftClick(ok);
			WaitRandom();
		}
	}
}
