using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExecutionActors;

namespace Courier.States
{
	public class EveCourierState : State
	{
		public EveCourierState()
		{
			pStateMachineId = CourierStateMachine.Id;
		}

		protected static CourierStateMachine pMachine
		{
			get
			{
				return (CourierStateMachine)StateMachine.GetInstance(CourierStateMachine.Id);
			}
		}
	}
}
