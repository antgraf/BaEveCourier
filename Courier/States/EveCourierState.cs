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
	}
}
