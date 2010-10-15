using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExecutionActors;
using Courier;
using Courier.States;

namespace Courier.Transitions
{
	class WorkSleepTransition : Transition<WorkState, SleepState>
	{
		public override bool CheckConstraints(WorkState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.Sleep;
		}
	}
}
