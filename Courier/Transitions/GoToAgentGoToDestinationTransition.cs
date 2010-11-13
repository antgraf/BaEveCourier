using ExecutionActors;
using Courier.States;

namespace Courier.Transitions
{
	class GoToAgentGoToDestinationTransition : Transition<GoToAgentState, GoToDestinationState>
	{
		public override bool CheckConstraints(GoToAgentState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.GoToDestination;
		}
	}
}
