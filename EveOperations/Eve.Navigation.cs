using WindowEntity;

namespace EveOperations
{
	public partial class Eve
	{
		private bool CheckInDock()
		{
			// POINT: top-left corner to search undock button
			Coordinate tlPt = new Coordinate(
				new StretchedPoint() { X = 0.0058252427184466, Y = 0.921815889029004 });
			// POINT: bottom-right corner to search undock button
			Coordinate brPt = new Coordinate(
				new StretchedPoint() { X = 0.0359223300970874, Y = 0.952080706179067 });
			return FindImage(tlPt, brPt, pImageUnDock);
		}

		private void UnDock()
		{
			// POINT: undock button
			Coordinate undock = new Coordinate(
				new StretchedPoint() { X = 0.0194174757281553, Y = 0.934426229508197 });
			pEveWindow.LeftClick(undock);
			WaitRandom();
		}

		private bool IsWarping()
		{
			// POINT: top-left corner to search for warping text
			Coordinate tlPt = new Coordinate(
			new StretchedPoint() { X = 0.475728155339806, Y = 0.92433795712484 });
			// POINT: bottom-right corner to search for warping text
			Coordinate brPt = new Coordinate(
			new StretchedPoint() { X = 0.511941747572816, Y = 0.944514501891551 });
			return FindImage(tlPt, brPt, pImageWarpGrey) || FindImage(tlPt, brPt, pImageWarpBlue);
		}

		private bool CheckAndSelectDestinationGate()
		{
			ActivateOverview();
			// POINT: top-left corner to search for gate
			Coordinate tlPt = new Coordinate(
				new StretchedPoint() { X = 0.723300970873786, Y = 0.171500630517024 });
			// POINT: bottom-right corner to search for gate
			Coordinate brPt = new Coordinate(
				new StretchedPoint() { X = 0.77378640776699, Y = 0.735182849936948 });
			Coordinate gate = FindImageCoordinate(tlPt, brPt, pImageSelectedGate) ??
			                  FindImageCoordinate(tlPt, brPt, pImageUnSelectedGate);
			if(gate != null)
			{
				pEveWindow.LeftClick(gate);
				WaitRandom();
			}
			return gate != null;
		}

		private void Align()
		{
			ActivateOverview();
			// POINT: align button
			Coordinate button = new Coordinate(
				new StretchedPoint() { X = 0.752427184466019, Y = 0.141235813366961 });
			pEveWindow.LeftClick(button);
			WaitRandom();
		}

		private void Warp()
		{
			ActivateOverview();
			// POINT: warp button
			Coordinate button = new Coordinate(
				new StretchedPoint() { X = 0.777669902912621, Y = 0.139974779319042 });
			pEveWindow.LeftClick(button);
			WaitRandom();
		}

		private void Activate()
		{
			ActivateOverview();
			// POINT: activate button
			Coordinate button = new Coordinate(
				new StretchedPoint() { X = 0.799029126213592, Y = 0.141235813366961 });
			pEveWindow.LeftClick(button);
			WaitRandom();
		}

		private bool CheckWarpButtonActive()
		{
			// POINT: top-left corner to search for warp button
			Coordinate tlPt = new Coordinate(
				new StretchedPoint() { X = 0.729126213592233, Y = 0.127364438839849 });
			// POINT: bottom-right corner to search for warp button
			Coordinate brPt = new Coordinate(
				new StretchedPoint() { X = 0.972815533980583, Y = 0.168978562421185 });
			return FindImage(tlPt, brPt, pImageActiveWarpButton);
		}

		private bool CheckActivateButtonActive()
		{
			// POINT: top-left corner to search for activate button
			Coordinate tlPt = new Coordinate(
				new StretchedPoint() { X = 0.729126213592233, Y = 0.127364438839849 });
			// POINT: bottom-right corner to search for activate button
			Coordinate brPt = new Coordinate(
				new StretchedPoint() { X = 0.972815533980583, Y = 0.168978562421185 });
			return FindImage(tlPt, brPt, pImageActiveActivateButton);
		}
	}
}
