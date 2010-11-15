using ExecutionActors;

namespace Courier.States
{
	public class EveCourierState : State
	{
		public EveCourierState()
		{
			pStateMachineId = CourierStateMachine.Id;
		}

		protected void SendEvent(CourierEvents eventId)
		{
			SendEvent((int)eventId);
		}

// ReSharper disable InconsistentNaming
		protected static CourierStateMachine pMachine
// ReSharper restore InconsistentNaming
		{
			get
			{
				return (CourierStateMachine)StateMachine.GetInstance(CourierStateMachine.Id);
			}
		}
	}
}
