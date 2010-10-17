using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Courier.States
{
	public class InitializationState : EveCourierState
	{
		public InitializationState()
		{
			pCurrentSubState = new LaunchGameState();
		}
	}
}
