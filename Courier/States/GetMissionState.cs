namespace Courier.States
{
	public class GetMissionState : EveCourierState
	{
		public override void Enter()
		{
			pMachine.LogAndDisplay("GetMissionState", "Enter");
		}
	}
}
