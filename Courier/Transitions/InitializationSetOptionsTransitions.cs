using ExecutionActors;
using Courier.States;

namespace Courier.Transitions
{
	public class InitializationSetOptionsTransitions : Transition<InitializationState, SetOptionsState>
	{
		public override bool CheckConstraints(InitializationState currentState, int eventId)
		{
			return eventId == (int)CourierEvents.Initialized;
		}
	}
}
