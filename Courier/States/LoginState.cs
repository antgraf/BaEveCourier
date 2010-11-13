using Courier.Transitions;

namespace Courier.States
{
	public class LoginState : EveCourierState
	{
		public LoginState()
		{
			pTransitions.Add(new LoginSelectCharacterTransition());
		}

		public override void Enter()
		{
			pMachine.LogAndDisplay("LoginState", "Enter");
			pMachine.HandleEvent(pMachine.Eve.DoLogin((string) pMachine.Settings[CourierSettings.Login],
			                                          (string) pMachine.Settings[CourierSettings.Password])
			                     	? CourierEvents.LoggedIn
			                     	: CourierEvents.End);
		}
	}
}
