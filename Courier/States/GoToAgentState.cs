using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BACommon;
using Courier.Exceptions;

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
			if(!StringUtils.IsEmpty(pMachine.Eve.CurrentAgent))
			{
				pMachine.HandleEvent(CourierEvents.GoToDestination);
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
