using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Courier.Transitions;

namespace Courier.States
{
	public class SleepState : EveCourierState
	{
		public SleepState()
		{
			pTransitions.Add(new SleepIdleTransition());
			pTransitions.Add(new SleepWorkTransition());
		}

		public override void Enter()
		{
			pMachine.Eve.Close();
		}
	}
}
