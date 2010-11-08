using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExecutionActors;
using Courier;
using Courier.States;

namespace Courier.Transitions
{
	class GoToAgentGoToDestinationTransition : Transition<GoToAgentState, GoToDestinationState>
	{
		public override bool CheckConstraints(GoToAgentState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.GoToDestination;
		}
	}
}
