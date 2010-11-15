using Courier.Transitions;

namespace Courier.States
{
	public class WorkState : EveCourierState
	{
		public WorkState()
		{
			pTransitions.Add(new WorkIdleTransition());
			pTransitions.Add(new WorkSleepTransition());
		}

		public override void Init(object arg)
		{
			if(arg is bool ? (bool)arg : false)
			{
				pCurrentSubState = new DoSetupState();
			}
			else
			{
				pCurrentSubState = new DoCourierMissionsState();
			}
			base.Init(arg);
		}
	}
}
