using ExecutionActors;
using Courier.States;

namespace Courier.Transitions
{
	class WorkIdleTransition : Transition<WorkState, IdleState>
	{
		public override bool CheckConstraints(WorkState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.End || eventId == (int)CourierEvents.Error ||
				eventId == (int)CourierEvents.FatalError || eventId == (int)CourierEvents.RescueLogoff;
		}
	}
}
