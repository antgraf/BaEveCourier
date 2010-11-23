using Courier.Transitions;

namespace Courier.States
{
	public class LaunchGameState : EveCourierState
	{
		public LaunchGameState()
		{
			pTransitions.Add(new LaunchGameLoginTransition());
		}

		public override void Enter()
		{
			pMachine.LogAndDisplay("LaunchGameState", "Enter");
			if(pMachine.Eve.Launch((string)pMachine.Settings[CourierSettings.Path]))
			{
				SendEvent(CourierEvents.EveLaunched);
			}
			else
			{
				pMachine.LogAndDisplay("LaunchGameState", "Cannot launch game.");
				SendEvent(CourierEvents.FatalError);
			}
		}
	}
}
