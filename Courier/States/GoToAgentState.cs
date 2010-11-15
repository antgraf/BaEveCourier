using EveOperations.Exceptions;
using Courier.Transitions;

namespace Courier.States
{
	public class GoToAgentState : EveCourierState
	{
		public GoToAgentState()
		{
			pTransitions.Add(new GoToAgentGoToDestinationTransition());
		}

		public override void Enter()
		{
			pMachine.LogAndDisplay("GoToAgentState", "Enter");
			if(!string.IsNullOrEmpty((string)pMachine.Settings[CourierSettings.CurrentAgent])
				&& (bool)pMachine.Settings[CourierSettings.CurrentCargo])
			{
				SendEvent(CourierEvents.GoToDestination);
			}
			else
			{
				string agent = pMachine.Agent;
				if(string.IsNullOrEmpty(agent))
				{
					pMachine.SleepUntil = pMachine.NextAgentAvailableTime;
					pMachine.LogAndDisplay("GoToAgentState", "No active agents. Sleeping until " + pMachine.SleepUntil);
					SendEvent(CourierEvents.Sleep);
				}
				else
				{
					pMachine.LogAndDisplay("GoToAgentState", "CheckIfAtAgentsStation");
					if(pMachine.Eve.CheckIfAtAgentsStation(agent))
					{
						SendEvent(CourierEvents.AgentReached);
					}
					else
					{
						try
						{
							pMachine.LogAndDisplay("GoToAgentState", "SetDestinationToAgent");
							if(pMachine.Eve.SetDestinationToAgent(pMachine.Eve.GetAgent(), true))
							{
								pCurrentSubState = new AutopilotState();
								SendEvent(CourierEvents.Autopilot);
							}
							else
							{
								pMachine.LogAndDisplay("GoToAgentState", "Cannot reach agent. Trying another one.");
								SendEvent(CourierEvents.NextAgent);
							}
						}
						catch(AgentHasNoMissionsAvailableException)
						{
							pMachine.LogAndDisplay("GoToAgentState", "Agent has no missions available");
							SendEvent(CourierEvents.NextAgent);
						}
					}
				}
			}
		}
	}
}
