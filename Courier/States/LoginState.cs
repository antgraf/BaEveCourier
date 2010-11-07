using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
			if(pMachine.Eve.DoLogin((string)pMachine.Settings[CourierSettings.Login], (string)pMachine.Settings[CourierSettings.Password]))
			{
				pMachine.HandleEvent(CourierEvents.LoggedIn);
			}
			else
			{
				pMachine.HandleEvent(CourierEvents.End);
			}
		}
	}
}
