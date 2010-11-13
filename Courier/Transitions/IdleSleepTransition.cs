using ExecutionActors;
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
