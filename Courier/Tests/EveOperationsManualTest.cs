using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WindowEntity;

namespace Courier
{
	public partial class Eve
	{
		[TestFixture]
		public class EveOperationsManualTest
		{
			#region Test settings

			private const string pTestPath = @"E:\projects\BA\Bin";
			private const string pEvePath = @"C:\Games\EveOnlineBot\eve.exe";
			private const string pEveLogin = "user";
			private const string pEvePassword = "******";
			private const string pEveAgent = "Hakar Ogok";
			//private const string pEveAgent = "Eindolf Fer";
			
			#endregion

			private Eve pEve = new Eve(null);

			private void Init()
			{
				pEve.LocalPath = pTestPath;
			}

			private void AttachToEve()
			{
				Init();
				WindowsMan.ResetWindows();
				pEve.EveWindow = WindowsMan.AttachTo(Eve.pEveProcessName);
				if(pEve.EveWindow == null || pEve.EveWindow.Width < pMinWindowWidth)
				{
					throw new NullReferenceException("Cannot attach to Eve.");
				}
			}

			[Test]
			public void ResetEveSettings()
			{
				Init();
				Assert.True(pEve.ResetEveSettings(pEvePath));
			}

			[Test]
			public void Launch()
			{
				Init();
				Assert.True(pEve.Launch(pEvePath));
			}

			[Test]
			public void CheckLoginScreen()
			{
				AttachToEve();
				Assert.True(pEve.CheckLoginScreen());
			}

			[Test]
			public void DoLogin()
			{
				AttachToEve();
				Assert.True(pEve.DoLogin(pEveLogin, pEvePassword));
			}

			[Test]
			public void CheckLoginError()
			{
				AttachToEve();
				Assert.True(pEve.CheckLoginError());
			}

			[Test]
			public void SelectCharacter()
			{
				AttachToEve();
				//pEve.Position = CharacterPosition.Left;
				Assert.True(pEve.SelectCharacter());
			}

			[Test]
			public void SetEveSettings()
			{
				AttachToEve();
				Assert.True(pEve.SetEveSettings());
			}

			[Test]
			public void CheckInDockTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckInDock());
			}

			[Test]
			public void CheckInDockFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckInDock());
			}

			[Test]
			public void OpenAgentWindow()
			{
				AttachToEve();
				pEve.OpenAgentWindow(pEveAgent);
			}

			[Test]
			public void CheckAgentHasNoMissionsTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckAgentHasNoMissions());
			}

			[Test]
			public void CheckAgentHasNoMissionsFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckAgentHasNoMissions());
			}

			[Test]
			public void CheckAgentLocationDestinationMenuItemTrue()
			{
				AttachToEve();
				pEve.CallAgentLocationMenu();
				Assert.True(pEve.CheckAndGoAgentLocationDestinationMenuItem());
				pEve.SetAgentDestination();
			}

			[Test]
			public void CheckAgentLocationDestinationMenuItemFalse()
			{
				AttachToEve();
				pEve.CallAgentLocationMenu();
				Assert.False(pEve.CheckAndGoAgentLocationDestinationMenuItem());
			}

			[Test]
			public void CheckAgentLocationDockMenuItemTrue()
			{
				AttachToEve();
				pEve.CallAgentLocationMenu();
				Assert.True(pEve.CheckAndGoAgentLocationDockMenuItem());
				pEve.DockToAgent();
			}

			[Test]
			public void CheckAgentLocationDockMenuItemFalse()
			{
				AttachToEve();
				pEve.CallAgentLocationMenu();
				Assert.False(pEve.CheckAndGoAgentLocationDockMenuItem());
			}

			[Test]
			public void UnDock()
			{
				AttachToEve();
				pEve.UnDock();
			}

			[Test]
			public void Close()
			{
				AttachToEve();
				pEve.Close();
				Assert.Null(WindowsMan.UpdateWindow(pEve.EveWindow));
			}

			[Test]
			public void MakeBlackScreen()
			{
				AttachToEve();
				Assert.True(pEve.MakeBlackScreen());
			}

			[Test]
			public void CheckAndCloseAgentWarningTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckAndCloseAgentWarning());
				Assert.False(pEve.CheckAndCloseAgentWarning());
			}

			[Test]
			public void CheckAndCloseAgentWarningFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckAndCloseAgentWarning());
			}

			[Test]
			public void CheckIfAtAgentsStationTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckIfAtAgentsStation(pEveAgent));
			}

			[Test]
			public void CheckIfAtAgentsStationFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckIfAtAgentsStation(pEveAgent));
			}

			[Test]
			public void CheckAgentProvideCourierMissionTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckAgentProvideCourierMission());
			}

			[Test]
			public void CheckAgentProvideCourierMissionFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckAgentProvideCourierMission());
			}

			[Test]
			public void GetCourierMission()
			{
				AttachToEve();
				Assert.True(pEve.GetCourierMission());
			}

			[Test]
			public void CheckRemoteAgentHasMissionTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckRemoteAgentHasMission());
			}

			[Test]
			public void CheckRemoteAgentHasMissionFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckRemoteAgentHasMission());
			}

			[Test]
			public void CheckAndCloseWrongLocationWarningTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckAndCloseWrongLocationWarning());
				Assert.False(pEve.CheckAndCloseWrongLocationWarning());
			}

			[Test]
			public void CheckAndCloseWrongLocationWarningFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckAndCloseWrongLocationWarning());
			}

			[Test]
			public void CheckAndCloseShutdownWarningTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckAndCloseShutdownWarning());
				Assert.False(pEve.CheckAndCloseShutdownWarning());
			}

			[Test]
			public void CheckAndCloseShutdownWarningFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckAndCloseShutdownWarning());
			}

			[Test]
			public void CheckWarpButtonActiveTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckWarpButtonActive());
			}

			[Test]
			public void CheckWarpButtonActiveFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckWarpButtonActive());
			}

			[Test]
			public void CheckActivateButtonActiveTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckActivateButtonActive());
			}

			[Test]
			public void CheckActivateButtonActiveFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckActivateButtonActive());
			}

			[Test]
			public void CheckAndSelectDestinationGateTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckAndSelectDestinationGate());
			}

			[Test]
			public void CheckAndSelectDestinationGateFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckAndSelectDestinationGate());
			}

			[Test]
			public void LoadAllToCargo()
			{
				AttachToEve();
				pEve.OpenCargo();
				Assert.True(pEve.ActivateWarehouseWindow());
				pEve.SelectAllInWarehouse();
				pEve.MoveFromWarehouseToCargo();
			}

			[Test]
			public void CloseAgentWindow()
			{
				AttachToEve();
				pEve.CloseAgentWindow();
			}

			[Test]
			public void Align()
			{
				AttachToEve();
				pEve.Align();
			}

			[Test]
			public void Warp()
			{
				AttachToEve();
				pEve.Warp();
			}

			[Test]
			public void Activate()
			{
				AttachToEve();
				pEve.Activate();
			}
		}
	}
}
