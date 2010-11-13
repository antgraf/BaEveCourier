namespace Courier.States
{
	public class InitializationState : EveCourierState
	{
		public InitializationState()
		{
			pCurrentSubState = new LaunchGameState();
		}
	}
}
