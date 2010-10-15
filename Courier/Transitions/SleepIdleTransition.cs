using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExecutionActors;
using Courier;
using Courier.States;

namespace Courier.Transitions
{
	class SleepIdleTransition : Transition<SleepState, IdleState>
	{
		public override bool CheckConstraints(SleepState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.End;
		}
	}
}
