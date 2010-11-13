using ExecutionActors;
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
