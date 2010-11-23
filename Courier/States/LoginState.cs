using Courier.Transitions;

namespace Courier.States
{
	public class LoginState : EveCourierState
	{
		public LoginState()
		{
			pTransitions.Add(new LoginCharacterSelectTransition());
		}

		public override void Enter()
		{
			pMachine.LogAndDisplay("LoginState", "Enter");
			if(pMachine.Eve.DoLogin((string)pMachine.Settings[CourierSettings.Login],
				(string)pMachine.Settings[CourierSettings.Password]))
			{
				SendEvent(CourierEvents.LoggedIn);
			}
			else
			{
				pMachine.LogAndDisplay("LaunchGameState", "Cannot login.");
				SendEvent(CourierEvents.FatalError);
			}
		}
	}
}
