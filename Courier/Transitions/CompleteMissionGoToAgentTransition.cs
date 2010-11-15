using ExecutionActors;
using Courier.States;

namespace Courier.Transitions
{
	class CompleteMissionGoToAgentTransition : Transition<CompleteMissionState, GoToAgentState>
	{
		public override bool CheckConstraints(CompleteMissionState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.NextAgent || eventId == (int)CourierEvents.RepeatAgent;
		}
	}
}
