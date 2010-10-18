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
			if(pMachine.Eve.DoLogin(pMachine.Eve.Login, pMachine.Eve.Password))
			{
				pMachine.HandleEvent(CourierEvents.EveLaunched);
			}
			else
			{
				pMachine.HandleEvent(CourierEvents.End);
			}
		}
	}
}
