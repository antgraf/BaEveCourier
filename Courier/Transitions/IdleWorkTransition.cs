using ExecutionActors;
using Courier.States;

namespace Courier.Transitions
{
	class IdleWorkTransition : Transition<IdleState, WorkState>
	{
		public override bool CheckConstraints(IdleState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.Start;
		}
	}
}
