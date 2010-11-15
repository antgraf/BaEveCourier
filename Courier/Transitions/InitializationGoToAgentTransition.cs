using ExecutionActors;
using Courier.States;

namespace Courier.Transitions
{
	public class InitializationGoToAgentTransition : Transition<InitializationState, GoToAgentState>
	{
		public override bool CheckConstraints(InitializationState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.Initialized;
		}
	}
}
