using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Courier.States
{
	public class GoToDestinationState : EveCourierState
	{
		public GoToDestinationState()
		{
		}

		public override void Enter()
		{
			pMachine.LogAndDisplay("GoToDestinationState", "Enter");
		}
	}
}
