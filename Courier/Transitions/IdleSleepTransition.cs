using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExecutionActors;
using Courier;
using Courier.States;

namespace Courier.Transitions
{
	class IdleSleepTransition : Transition<IdleState, SleepState>
	{
		public override bool CheckConstraints(IdleState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.Sleep;
		}
	}
}
