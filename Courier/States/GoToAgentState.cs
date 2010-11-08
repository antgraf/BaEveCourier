using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BACommon;
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
				pMachine.HandleEvent(CourierEvents.GoToDestination);
			}
			else
			{
				string agent = pMachine.Agent;
				if(string.IsNullOrEmpty(agent))
				{
					pMachine.SleepUntil = pMachine.NextAgentAvailableTime;
					pMachine.LogAndDisplay("GoToAgentState", "No active agents. Sleeping until " + pMachine.SleepUntil.ToString());
					pMachine.HandleEvent(CourierEvents.Sleep);
				}
				else
				{
					pMachine.LogAndDisplay("GoToAgentState", "CheckIfAtAgentsStation");
					if(pMachine.Eve.CheckIfAtAgentsStation(agent))
					{
						pMachine.HandleEvent(CourierEvents.AgentReached);
					}
					else
					{
						try
						{
							pMachine.LogAndDisplay("GoToAgentState", "SetDestinationToAgent");
							if(pMachine.Eve.SetDestinationToAgent(pMachine.Eve.GetAgent(), true))
							{
								pCurrentSubState = new AutopilotState();
								pMachine.HandleEvent(CourierEvents.Autopilot);
							}
							else
							{
								pMachine.LogAndDisplay("GoToAgentState", "Cannot reach agent. Trying another one.");
								pMachine.HandleEvent(CourierEvents.NextAgent);
							}
						}
						catch(AgentHasNoMissionsAvailableException)
						{
							pMachine.LogAndDisplay("GoToAgentState", "Agent has no missions available");
							pMachine.HandleEvent(CourierEvents.NextAgent);
						}
					}
				}
			}
		}
	}
}
