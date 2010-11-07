using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WindowEntity;

namespace EveOperations
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
			[Category("Common")]
			public void ResetEveSettings()
			{
				Init();
				Assert.True(pEve.ResetEveSettings(pEvePath));
			}

			[Test]
			[Category("Common")]
			public void Launch()
			{
				Init();
				Assert.True(pEve.Launch(pEvePath));
			}

			[Test]
			[Category("Common")]
			public void CheckLoginScreen()
			{
				AttachToEve();
				Assert.True(pEve.CheckLoginScreen());
			}

			[Test]
			[Category("Common")]
			public void DoLogin()
			{
				AttachToEve();
				Assert.True(pEve.DoLogin(pEveLogin, pEvePassword));
			}

			[Test]
			[Category("Common")]
			public void CheckLoginError()
			{
				AttachToEve();
				Assert.True(pEve.CheckLoginError());
			}

			[Test]
			[Category("Common")]
			public void SelectCharacter()
			{
				AttachToEve();
				CharacterPosition position = CharacterPosition.Main;
				Assert.True(pEve.SelectCharacter(position));
			}

			[Test]
			[Category("Setup")]
			public void SetEveSettings()
			{
				AttachToEve();
				Assert.True(pEve.SetEveSettings());
			}

			[Test]
			[Category("Common")]
			public void CheckInDockTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckInDock());
			}

			[Test]
			[Category("Common")]
			public void CheckInDockFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckInDock());
			}

			[Test]
			[Category("Agent")]
			public void OpenAgentWindow()
			{
				AttachToEve();
				pEve.OpenAgentWindow(pEveAgent);
			}

			[Test]
			[Category("Agent")]
			public void CheckAgentHasNoMissionsTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckAgentHasNoMissions());
			}

			[Test]
			[Category("Agent")]
			public void CheckAgentHasNoMissionsFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckAgentHasNoMissions());
			}

			[Test]
			[Category("Agent")]
			public void CheckAgentLocationDestinationMenuItemTrue()
			{
				AttachToEve();
				pEve.CallAgentLocationMenu();
				Assert.True(pEve.CheckAndGoAgentLocationDestinationMenuItem());
				pEve.SetAgentDestination();
			}

			[Test]
			[Category("Agent")]
			public void CheckAgentLocationDestinationMenuItemFalse()
			{
				AttachToEve();
				pEve.CallAgentLocationMenu();
				Assert.False(pEve.CheckAndGoAgentLocationDestinationMenuItem());
			}

			[Test]
			[Category("Agent")]
			public void CheckAgentLocationDockMenuItemTrue()
			{
				AttachToEve();
				pEve.CallAgentLocationMenu();
				Assert.True(pEve.CheckAndGoAgentLocationDockMenuItem());
				pEve.DockToAgent();
			}

			[Test]
			[Category("Agent")]
			public void CheckAgentLocationDockMenuItemFalse()
			{
				AttachToEve();
				pEve.CallAgentLocationMenu();
				Assert.False(pEve.CheckAndGoAgentLocationDockMenuItem());
			}

			[Test]
			[Category("Common")]
			public void UnDock()
			{
				AttachToEve();
				pEve.UnDock();
			}

			[Test]
			[Category("Common")]
			public void Close()
			{
				AttachToEve();
				pEve.Close();
				Assert.Null(WindowsMan.UpdateWindow(pEve.EveWindow));
			}

			[Test]
			[Category("Common")]
			public void MakeBlackScreen()
			{
				AttachToEve();
				Assert.True(pEve.MakeBlackScreen());
			}

			[Test]
			[Category("Agent")]
			public void CheckAndCloseAgentWarningTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckAndCloseAgentWarning());
				Assert.False(pEve.CheckAndCloseAgentWarning());
			}

			[Test]
			[Category("Agent")]
			public void CheckAndCloseAgentWarningFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckAndCloseAgentWarning());
			}

			[Test]
			[Category("Agent")]
			public void CheckIfAtAgentsStationTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckIfAtAgentsStation(pEveAgent));
			}

			[Test]
			[Category("Agent")]
			public void CheckIfAtAgentsStationFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckIfAtAgentsStation(pEveAgent));
			}

			[Test]
			[Category("Agent")]
			public void CheckAgentProvideCourierMissionTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckAgentProvideCourierMission());
			}

			[Test]
			[Category("Agent")]
			public void CheckAgentProvideCourierMissionFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckAgentProvideCourierMission());
			}

			[Test]
			[Category("Agent")]
			public void GetCourierMission()
			{
				AttachToEve();
				Assert.True(pEve.GetCourierMission());
			}

			[Test]
			[Category("Agent")]
			public void CheckRemoteAgentHasMissionTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckRemoteAgentHasMission());
			}

			[Test]
			[Category("Agent")]
			public void CheckRemoteAgentHasMissionFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckRemoteAgentHasMission());
			}

			[Test]
			[Category("Common")]
			public void CheckAndCloseWrongLocationWarningTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckAndCloseWrongLocationWarning());
				Assert.False(pEve.CheckAndCloseWrongLocationWarning());
			}

			[Test]
			[Category("Common")]
			public void CheckAndCloseWrongLocationWarningFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckAndCloseWrongLocationWarning());
			}

			[Test]
			[Category("Common")]
			public void CheckAndCloseShutdownWarningTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckAndCloseShutdownWarning());
				Assert.False(pEve.CheckAndCloseShutdownWarning());
			}

			[Test]
			[Category("Common")]
			public void CheckAndCloseShutdownWarningFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckAndCloseShutdownWarning());
			}

			[Test]
			[Category("Navigation")]
			public void CheckWarpButtonActiveTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckWarpButtonActive());
			}

			[Test]
			[Category("Navigation")]
			public void CheckWarpButtonActiveFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckWarpButtonActive());
			}

			[Test]
			[Category("Navigation")]
			public void CheckActivateButtonActiveTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckActivateButtonActive());
			}

			[Test]
			[Category("Navigation")]
			public void CheckActivateButtonActiveFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckActivateButtonActive());
			}

			[Test]
			[Category("Navigation")]
			public void CheckAndSelectDestinationGateTrue()
			{
				AttachToEve();
				Assert.True(pEve.CheckAndSelectDestinationGate());
			}

			[Test]
			[Category("Navigation")]
			public void CheckAndSelectDestinationGateFalse()
			{
				AttachToEve();
				Assert.False(pEve.CheckAndSelectDestinationGate());
			}

			[Test]
			[Category("Common")]
			public void LoadAllToCargo()
			{
				AttachToEve();
				pEve.OpenCargo();
				Assert.True(pEve.ActivateWarehouseWindow());
				pEve.SelectAllInWarehouse();
				pEve.MoveFromWarehouseToCargo();
			}

			[Test]
			[Category("Agent")]
			public void CloseAgentWindow()
			{
				AttachToEve();
				pEve.CloseAgentWindow();
			}

			[Test]
			[Category("Common")]
			public void ClosePeopleAndPlaces()
			{
				AttachToEve();
				pEve.ClosePeopleAndPlaces();
			}

			[Test]
			[Category("Navigation")]
			public void Align()
			{
				AttachToEve();
				pEve.Align();
			}

			[Test]
			[Category("Navigation")]
			public void Warp()
			{
				AttachToEve();
				pEve.Warp();
			}

			[Test]
			[Category("Navigation")]
			public void Activate()
			{
				AttachToEve();
				pEve.Activate();
			}

			[Test]
			[Category("Common")]
			public void ActivateStationView()
			{
				AttachToEve();
				pEve.ActivateStationView();
			}

			[Test]
			[Category("Navigation")]
			public void IsWarpingTrue()
			{
				AttachToEve();
				Assert.True(pEve.IsWarping());
			}

			[Test]
			[Category("Navigation")]
			public void IsWarpingFalse()
			{
				AttachToEve();
				Assert.False(pEve.IsWarping());
			}

			[Test]
			[Category("Agent")]
			public void DockAtCourierMissionDestination()
			{
				AttachToEve();
				pEve.DockAtCourierMissionDestination();
			}

			[Test]
			[Category("Agent")]
			public void CompleteMission()
			{
				AttachToEve();
				pEve.CompleteMission();
			}
		}
	}
}
