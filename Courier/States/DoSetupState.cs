using System;

namespace Courier.States
{
	public class DoSetupState : EveCourierState
	{
		public DoSetupState()
		{
			pCurrentSubState = new InitializationState(true);
		}

		public override void Enter()
		{
			pMachine.LogAndDisplay("DoSetupState", "Enter");
		}
	}
}
