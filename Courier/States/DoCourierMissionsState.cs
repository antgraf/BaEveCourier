namespace Courier.States
{
	public class DoCourierMissionsState : EveCourierState
	{
		public DoCourierMissionsState()
		{
			pCurrentSubState = new GoToAgentState();
		}

		public override void Enter()
		{
			pMachine.LogAndDisplay("DoCourierMissionsState", "Enter");
		}
	}
}
