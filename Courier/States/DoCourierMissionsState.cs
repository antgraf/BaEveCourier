namespace Courier.States
{
	public class DoCourierMissionsState : EveCourierState
	{
		public DoCourierMissionsState()
		{
			pCurrentSubState = new InitializationState(false);
		}

		public override void Enter()
		{
			pMachine.LogAndDisplay("DoCourierMissionsState", "Enter");
		}
	}
}
