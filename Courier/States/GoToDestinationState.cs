namespace Courier.States
{
	public class GoToDestinationState : EveCourierState
	{
		public override void Enter()
		{
			pMachine.LogAndDisplay("GoToDestinationState", "Enter");
		}
	}
}
