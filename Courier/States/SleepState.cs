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
			pMachine.LogAndDisplay("SleepState", "Enter");
			pMachine.Eve.Close();
		}
	}
}
