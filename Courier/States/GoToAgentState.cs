using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BACommon;
using EveOperations.Exceptions;

namespace Courier.States
{
	public class GoToAgentState : EveCourierState
	{
		public GoToAgentState()
		{
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
						if(pMachine.Eve.CheckIfAtAgentsStation(agent))
						{
							pMachine.HandleEvent(CourierEvents.AgentReached);
						}
						else
						{
							try
							{
								pMachine.Eve.SetDestinationToAgent(pMachine.Eve.GetAgent(), true);
								pMachine.HandleEvent(CourierEvents.Autopilot);
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
