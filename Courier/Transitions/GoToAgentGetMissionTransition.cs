using ExecutionActors;
using Courier.States;

namespace Courier.Transitions
{
	public class GoToAgentGetMissionTransition : Transition<GoToAgentState, GetMissionState>
	{
		public override bool CheckConstraints(GoToAgentState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.AgentReached;
		}
	}
}
