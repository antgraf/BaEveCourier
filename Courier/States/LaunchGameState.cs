using Courier.Transitions;

namespace Courier.States
{
	public class LaunchGameState : EveCourierState
	{
		public LaunchGameState()
		{
			pTransitions.Add(new LaunchLoginTransition());
		}

		public override void Enter()
		{
			pMachine.LogAndDisplay("LaunchGameState", "Enter");
			pMachine.HandleEvent(pMachine.Eve.Launch((string) pMachine.Settings[CourierSettings.Path])
			                     	? CourierEvents.EveLaunched
			                     	: CourierEvents.End);
		}
	}
}
