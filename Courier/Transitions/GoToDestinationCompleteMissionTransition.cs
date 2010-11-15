using ExecutionActors;
using Courier.States;

namespace Courier.Transitions
{
	class GoToDestinationCompleteMissionTransition : Transition<GoToDestinationState, CompleteMissionState>
	{
		public override bool CheckConstraints(GoToDestinationState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.DestinationReached;
		}
	}
}
