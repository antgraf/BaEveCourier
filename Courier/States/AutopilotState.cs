namespace Courier.States
{
	public class AutopilotState : EveCourierState
	{
		//private const int pLoadWaitTimeMSec = (int)(2.5 * 1000);
		private const int pLoadWaitAttempts = 5;
		//private const int pTimeoutSec = 20;

		public override void Enter()
		{
			bool gatefound = false;
			pMachine.LogAndDisplay("AutopilotState", "Enter");
			pMachine.Eve.WaitWhileWarping();
			if(pMachine.Eve.CheckIfAtAgentsStation(pMachine.Agent))
			{
				SendEvent(CourierEvents.AgentReached);
			}
			else
			{
				for(int attempts = pLoadWaitAttempts; attempts > 0; attempts--)
				{
					if(pMachine.Eve.WarpThroughActiveGate())
					{
						gatefound = true;
						break;
					}
				}
				if(!gatefound)
				{
					SendEvent(CourierEvents.AutopilotStopped);
				}
			}
		}
	}
}
