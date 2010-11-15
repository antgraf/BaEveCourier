namespace Courier.States
{
	public class CompleteMissionState : EveCourierState
	{
		public override void Enter()
		{
			pMachine.LogAndDisplay("CompleteMissionState", "Enter");
		}
	}
}
