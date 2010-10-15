using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExecutionActors;
using Courier;
using Courier.States;

namespace Courier.Transitions
{
	class WorkIdleTransition : Transition<WorkState, IdleState>
	{
		public override bool CheckConstraints(WorkState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.End;
		}
	}
}
