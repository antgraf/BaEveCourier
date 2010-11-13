using System;
using System.Timers;
using WindowEntity;
using System.IO;
using EveOperations.Exceptions;

namespace EveOperations
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

		private void PTimeOutElapsed(object sender, ElapsedEventArgs e)
		{
			pTimedOut = true;
		}

		private void WaitRandom()
		{
			pEveWindow.WaitRandom(pRandomWaitTime, pRandomWaitDelta);
		}

		private static string GetEveSettingsPath(string pathToEve)
		{
			const string serverSuffix = pTranquilityServerSuffix;
			string appdata = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
			if(pathToEve == null)
			{
				return null;
			}
// ReSharper disable PossibleNullReferenceException
			string profile = Path.GetDirectoryName(pathToEve)
// ReSharper restore PossibleNullReferenceException
			                 	.Replace(":", "")
			                 	.Replace(Path.DirectorySeparatorChar, '_')
			                 	.ToLower()
			                 + serverSuffix;
			return Path.Combine(appdata, string.Format(pEveSettingsFolder, profile));
		}

		private void CopyDefaultSettings(string pathToEveSettings)
		{
// ReSharper disable AssignNullToNotNullAttribute
			File.Copy(Relative2AbsolutePath(pSettings1),
				Path.Combine(pathToEveSettings, Path.GetFileName(pSettings1)));
			File.Copy(Relative2AbsolutePath(pSettings2),
				Path.Combine(pathToEveSettings, Path.GetFileName(pSettings2)));
			File.Copy(Relative2AbsolutePath(pSettings3),
				Path.Combine(pathToEveSettings, Path.GetFileName(pSettings3)));
			File.Copy(Relative2AbsolutePath(pSettings4),
				Path.Combine(pathToEveSettings, Path.GetFileName(pSettings4)));
// ReSharper restore AssignNullToNotNullAttribute
		}

		private static bool ClearEveSettings(string pathToEveSettings)
		{
			try
			{
				Directory.Delete(pathToEveSettings, true);
			}
// ReSharper disable EmptyGeneralCatchClause
			catch
// ReSharper restore EmptyGeneralCatchClause
			{
				// ignore
			}
			return true;
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
				Timer timer = new Timer(pWindowWaitInterval * 1000) {AutoReset = true};
				timer.Elapsed += delegate
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
			Coordinate tlPt = new Coordinate(
				new StretchedPoint() { X = 0.562135922330097, Y = 0.891551071878941 });
			// POINT: bottom-right of "status ok"
			Coordinate brPt = new Coordinate(
				new StretchedPoint() { X = 0.614563106796116, Y = 0.904161412358134 });
			return FindImage(tlPt, brPt, pImageStatusOk);
		}

		private void EraseLogin()
		{
			// POINT: login field
			Coordinate loginPt = new Coordinate(
				new StretchedPoint() { X = 0.451456310679612, Y = 0.896595208070618 });
			pEveWindow.LeftClick(loginPt);	// go to login field
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
			Coordinate tlPt = new Coordinate(
				new StretchedPoint() { X = 0.313592233009709, Y = 0.64312736443884 });
			// POINT: bottom-right of "skull"
			Coordinate brPt = new Coordinate(
				new StretchedPoint() { X = 0.369902912621359, Y = 0.71500630517024 });
			return FindImage(tlPt, brPt, pImageSkull);
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

		private void ToggleMap()
		{
			pEveWindow.KeySend("{F10}");	// open map
			WaitRandom();
			pEveWindow.Wait(pLoadWaitTime);
		}

		private void MoveMapOut()
		{
			// POINT: drag from point
			Coordinate from = new Coordinate(
				new StretchedPoint() { X = 0.975728155339806, Y = 0.978385876418663 });
			// POINT: drag to point
			Coordinate to = new Coordinate(
				new StretchedPoint() { X = 0.754271844660194, Y = 0.980819672131147 });
			for(int i = 0; i < 4; i++)
			{
				pEveWindow.DragDropRight(from, to);	// drag map out
				WaitRandom();
			}
		}

		private void MinimizeMapControl()
		{
			// POINT: minimize map control button
			Coordinate minimize = new Coordinate(
				new StretchedPoint() { X = 0.986407766990291, Y = 0.0390920554854981 });
			pEveWindow.LeftClick(minimize);
			WaitRandom();
			pEveWindow.LeftClick(minimize);	// workaround of some eve problem with window control buttons
			WaitRandom();
		}

		private void OpenOptionsWindow()
		{
			pEveWindow.KeySendAndWait("{ESC}", pStandardWaitTime);
		}

		private bool CheckAndCloseWrongLocationWarning()
		{
			// POINT: top-left corner to search for waring
			Coordinate tlPt = new Coordinate(
				new StretchedPoint() { X = 0.395145631067961, Y = 0.398486759142497 });
			// POINT: bottom-right corner to search for waring
			Coordinate brPt = new Coordinate(
				new StretchedPoint() { X = 0.43495145631068, Y = 0.433795712484237 });
			bool found = FindImage(tlPt, brPt, pImageWrongLocationWarning);
			if(found)
			{
				// POINT: ok button
				Coordinate ok = new Coordinate(
					new StretchedPoint() { X = 0.5, Y = 0.626733921815889 });
				pEveWindow.LeftClick(ok);
				WaitRandom();
				pEveWindow.Wait(pLoadWaitTime);
			}
			return found;
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

		private bool CheckAndCloseShutdownWarning()
		{
			// POINT: top-left corner to search warning text
			Coordinate tlPt = new Coordinate(
				new StretchedPoint() { X = 0.394174757281553, Y = 0.395964691046658 });
			// POINT: bottom-right corner to search warning text
			Coordinate brPt = new Coordinate(
				new StretchedPoint() { X = 0.44368932038835, Y = 0.437578814627995 });
			bool found = FindImage(tlPt, brPt, pImageShutdownWarning);
			if(found)
			{
				// POINT: ok button
				Coordinate ok = new Coordinate(
					new StretchedPoint() { X = 0.503883495145631, Y = 0.622950819672131 });
				pEveWindow.LeftClick(ok);
				WaitRandom();
			}
			return found;
		}

		private void OpenCargo()
		{
			pEveWindow.CtrlKeyDown();
			pEveWindow.AltKeyDown();
			pEveWindow.KeySendAndWait("c");	// custom shortcut for open cargo
			pEveWindow.AltKeyUp();
			pEveWindow.CtrlKeyUp();
			pEveWindow.Wait(pLoadWaitTime);
		}

		private bool ActivateWarehouseWindow()
		{
			ActivateStationView();
			// POINT: top-left corner to search for warehouse tab
			Coordinate tlPt = new Coordinate(
				new StretchedPoint() { X = 0.725242718446602, Y = 0.489281210592686 });
			// POINT: bottom-right corner to search for warehouse tab
			Coordinate brPt = new Coordinate(
				new StretchedPoint() { X = 0.996116504854369, Y = 0.561160151324086 });
			Coordinate warehouse = FindImageCoordinate(tlPt, brPt, pImageWarehouseTab);
			if(warehouse != null)
			{
				pEveWindow.LeftClick(warehouse);
				WaitRandom();
				pEveWindow.Wait(pLoadWaitTime);
			}
			return warehouse != null;
		}

		private void SelectAllInWarehouse()
		{
			// POINT: warehouse window client area
			Coordinate warehouse = new Coordinate(
				new StretchedPoint() { X = 0.783495145631068, Y = 0.65573770491803 });
			pEveWindow.LeftClick(warehouse);
			WaitRandom();
			pEveWindow.CtrlKeyDown();
			pEveWindow.KeySendAndWait("a");	// select all
			pEveWindow.CtrlKeyUp();
			WaitRandom();
		}

		private void MoveFromWarehouseToCargo()
		{
			// POINT: warehouse window client area
			Coordinate warehouse = new Coordinate(
				new StretchedPoint() { X = 0.783495145631068, Y = 0.65573770491803 });
			// POINT: cargo window
			Coordinate cargo = new Coordinate(
				new StretchedPoint() { X = 0.509708737864078, Y = 0.515762925598991 });
			pEveWindow.DragDrop(warehouse, cargo);
			WaitRandom();
			pEveWindow.Wait(pLoadWaitTime);
		}

		private void ActivateOverview()
		{
			// POINT: overview
			Coordinate overview = new Coordinate(
				new StretchedPoint() { X = 0.939805825242719, Y = 0.168978562421185 });
			pEveWindow.LeftClick(overview);
			WaitRandom();
		}

		private void ClosePeopleAndPlaces()
		{
			OpenPeopleAndPlaces();
			// POINT: close button
			Coordinate button = new Coordinate(
				new StretchedPoint() { X = 0.731067961165049, Y = 0.269861286254729 });
			pEveWindow.LeftClick(button);
			WaitRandom();
			pEveWindow.LeftClick(button);	// workaround of some eve problem with window control buttons
			WaitRandom();
		}

		private void ActivateStationView()
		{
			// POINT: station view
			Coordinate station = new Coordinate(
				new StretchedPoint() { X = 0.753398058252427, Y = 0.0378310214375788 });
			pEveWindow.LeftClick(station);
			WaitRandom();
		}

		private void UnPinOverview()
		{
			// POINT: action panel pin button
			Coordinate button1 = new Coordinate(
				new StretchedPoint() { X = 0.954368932038835, Y = 0.0592686002522068 });
			pEveWindow.SetCursorPosition(button1);
			WaitRandom();
			pEveWindow.LeftClick(button1);
			// POINT: overview pin button
			Coordinate button2 = new Coordinate(
				new StretchedPoint() { X = 0.956310679611651, Y = 0.176544766708701 });
			pEveWindow.SetCursorPosition(button2);
			WaitRandom();
			pEveWindow.LeftClick(button2);
		}
	}
}
