using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Courier.Transitions;

namespace Courier.States
{
	public class IdleState : EveCourierState
	{
		public IdleState()
		{
			pTransitions.Add(new IdleSleepTransition());
			pTransitions.Add(new IdleWorkTransition());
		}

		public override void Enter()
		{
			pMachine.LogAndDisplay("IdleState", "Enter");
			pMachine.Eve.Close();
			pMachine.HandleEvent(CourierEvents.Idle);
		}
	}
}
