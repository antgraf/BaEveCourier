using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BACommon;

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
				// TODO
			}
		}
	}
}
