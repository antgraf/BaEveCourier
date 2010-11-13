using ExecutionActors;
using Courier.States;

namespace Courier.Transitions
{
	public class InitializationDoCourierMissionsTransition : Transition<InitializationState, DoCourierMissionsState>
	{
		public override bool CheckConstraints(InitializationState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.Initialized;
		}
	}
}
