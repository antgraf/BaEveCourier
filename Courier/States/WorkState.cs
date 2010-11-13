using Courier.Transitions;

namespace Courier.States
{
	public class WorkState : EveCourierState
	{
		public WorkState()
		{
			pTransitions.Add(new WorkIdleTransition());
			pTransitions.Add(new WorkSleepTransition());
			pCurrentSubState = new InitializationState();
		}
	}
}
