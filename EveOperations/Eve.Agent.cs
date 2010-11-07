using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using WindowEntity;
using System.Diagnostics;
using Logger;
using System.Drawing;
using BACommon;
using System.Reflection;
using System.IO;

namespace EveOperations
{
	public partial class Eve
	{
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
			pEveWindow.Wait(pLoadWaitTime);
			CheckAndCloseAgentWarning();
			WaitRandom();
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
				new StretchedPoint() { X = 0.107766990291262, Y = 0.406052963430013 });
			// POINT: bottom-right corner to search text
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.262135922330097, Y = 0.480453972257251 });
			return FindImage(tl_pt, br_pt, pImageNoMissions) || FindImage(tl_pt, br_pt, pImageNoMissions2);
		}

		private bool CheckRemoteAgentHasMission()
		{
			ActivateAgentWindow();
			// POINT: top-left corner to search text
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.107766990291262, Y = 0.406052963430013 });
			// POINT: bottom-right corner to search text
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.262135922330097, Y = 0.480453972257251 });
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

		private bool CheckAndGoAgentLocationDestinationMenuItem()
		{
			// POINT: top-left corner to search menu item
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.266990291262136, Y = 0.320302648171501 });
			// POINT: bottom-right corner to search menu item
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.448543689320388, Y = 0.539722572509458 });
			return FindImage(tl_pt, br_pt, pImageSetDestination);
		}

		private bool CheckAndGoAgentLocationDockMenuItem()
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

		private bool CheckAndCloseAgentWarning()
		{
			// POINT: top-left corner to search warning text
			Coordinate tl_pt = new Coordinate(
				new StretchedPoint() { X = 0.346601941747573, Y = 0.469104665825977 });
			// POINT: bottom-right corner to search warning text
			Coordinate br_pt = new Coordinate(
				new StretchedPoint() { X = 0.406796116504854, Y = 0.519546027742749 });
			bool found = FindImage(tl_pt, br_pt, pImageAgentWarning);
			if(found)
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
			return found;
		}

		private void SetCourierMissionDestination()
		{
			ActivateAgentWindow();
			// POINT: destination hyperlink
			Coordinate destination = new Coordinate(
				new StretchedPoint() { X = 0.713592233009709, Y = 0.369482976040353 });
			pEveWindow.RightClick(destination);
			WaitRandom();
			// POINT: "set destination" menu item
			Coordinate menu = new Coordinate(
				new StretchedPoint() { X = 0.737864077669903, Y = 0.382093316519546 });
			pEveWindow.LeftClick(menu);
			WaitRandom();
		}

		private void CloseAgentWindow()
		{
			ActivateAgentWindow();
			// POINT: close button
			Coordinate button = new Coordinate(
				new StretchedPoint() { X = 0.898058252427185, Y = 0.194199243379571 });
			pEveWindow.LeftClick(button);
			WaitRandom();
		}

		private void DockAtCourierMissionDestination()
		{
			ActivateAgentWindow();
			// POINT: destination hyperlink
			Coordinate destination = new Coordinate(
				new StretchedPoint() { X = 0.713592233009709, Y = 0.370744010088272 });
			pEveWindow.RightClick(destination);
			WaitRandom();
			// POINT: dock menu item
			Coordinate dock = new Coordinate(
				new StretchedPoint() { X = 0.73495145631068, Y = 0.440100882723834 });
			pEveWindow.LeftClick(dock);
			WaitRandom();
		}

		private void CompleteMission()
		{
			ActivateAgentWindow();
			// POINT: complete button
			Coordinate button = new Coordinate(
				new StretchedPoint() { X = 0.595145631067961, Y = 0.848675914249685 });
			pEveWindow.LeftClick(button);
			WaitRandom();
		}
	}
}
