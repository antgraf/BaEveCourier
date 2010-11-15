namespace Courier.States
{
	public class SetOptionsState : EveCourierState
	{
		public override void Enter()
		{
			pMachine.LogAndDisplay("SetOptionsState", "Enter");
			pMachine.Eve.SetEveSettings();
			pMachine.LogAndDisplay("SetOptionsState", "Complete");
			SendEvent(CourierEvents.End);
		}
	}
}
