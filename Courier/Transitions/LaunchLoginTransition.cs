using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExecutionActors;
using Courier;
using Courier.States;

namespace Courier.Transitions
{
	class LaunchLoginTransition : Transition<LaunchGameState, LoginState>
	{
		public override bool CheckConstraints(LaunchGameState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.EveLaunched;
		}
	}
}
