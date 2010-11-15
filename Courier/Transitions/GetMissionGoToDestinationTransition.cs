using ExecutionActors;
using Courier.States;

namespace Courier.Transitions
{
	class GetMissionGoToDestinationTransition : Transition<GetMissionState, GoToDestinationState>
	{
		public override bool CheckConstraints(GetMissionState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.GotMission;
		}
	}
}
