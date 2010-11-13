using ExecutionActors;

namespace Courier.States
{
	public class EveCourierState : State
	{
		public EveCourierState()
		{
			pStateMachineId = CourierStateMachine.Id;
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
