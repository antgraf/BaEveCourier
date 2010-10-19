using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Courier.Transitions;

namespace Courier.States
{
	public class LaunchGameState : EveCourierState
	{
		public LaunchGameState()
		{
			pTransitions.Add(new LaunchLoginTransition());
		}

		public override void Enter()
		{
			pMachine.LogAndDisplay("LaunchGameState", "Enter");
			if(pMachine.Eve.Launch(pMachine.Eve.PathToEve))
			{
				pMachine.HandleEvent(CourierEvents.EveLaunched);
			}
			else
			{
				pMachine.HandleEvent(CourierEvents.End);
			}
		}
	}
}
