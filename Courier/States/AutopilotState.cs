using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Courier.States
{
	public class AutopilotState : EveCourierState
	{
		public AutopilotState()
		{
		}

		public override void Enter()
		{
			pMachine.LogAndDisplay("AutopilotState", "Enter");
		}
	}
}
