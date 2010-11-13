using ExecutionActors;
using Courier.States;

namespace Courier.Transitions
{
	class SleepWorkTransition : Transition<SleepState, WorkState>
	{
		public override bool CheckConstraints(SleepState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.Start;
		}
	}
}
