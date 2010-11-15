using EveOperations;

namespace Courier.States
{
	public class CharacterSelectState : EveCourierState
	{
		private const int pCharacterSelectWaitTime = 15 * 1000;

		public override void Enter()
		{
			pMachine.LogAndDisplay("CharacterSelectState", "Enter");
			if(pMachine.Eve.SelectCharacter((CharacterPosition)pMachine.Settings[CourierSettings.Position]))
			{
				SendEvent(CourierEvents.CharacterSelected);
				pMachine.Eve.EveWindow.Wait(pCharacterSelectWaitTime);
				SendEvent(CourierEvents.Initialized);
			}
			else
			{
				pMachine.LogAndDisplay("CharacterSelectState", "Cannot select character.");
				SendEvent(CourierEvents.End);
			}
		}
	}
}
