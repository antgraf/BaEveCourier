using ExecutionActors;
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
