using Courier.Transitions;

namespace Courier.States
{
	public class InitializationState : EveCourierState
	{
		public InitializationState()
			: this(false)
		{}

		public InitializationState(bool setup)
		{
			pCurrentSubState = new LaunchGameState();
			if(setup)
			{
				pTransitions.Add(new InitializationSetOptionsTransitions());
			}
			else
			{
				pTransitions.Add(new InitializationGoToAgentTransition());
			}
		}
	}
}
