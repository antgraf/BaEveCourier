using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExecutionActors;

namespace Courier
{
	class CourierActor : Actor, IMessageHandler
	{
		protected CourierPlugin pPlugin = null;

		protected override void Init(object data)
		{
			pPlugin = (CourierPlugin)data;
			StateMachine.Register(CourierStateMachine.Id, new CourierStateMachine(pLog, this));
			pObserver.Notify(this, "Initialization", 100, "Eve Courier actor initialized");
		}

		protected override void Worker()
		{
			pObserver.Notify(this, "Start", 100, "Eve Courier actor started");
			CourierStateMachine machine = (CourierStateMachine)StateMachine.GetInstance(CourierStateMachine.Id);
			machine.Eve.PathToEve = (string)pPlugin.Settings[CourierSettings.Path];
			machine.Eve.Login = (string)pPlugin.Settings[CourierSettings.Login];
			machine.Eve.Password = (string)pPlugin.Settings[CourierSettings.Password];
			machine.Eve.Position = (CharacterPosition)pPlugin.Settings[CourierSettings.Position];
			machine.Eve.Agents = (List<string>)pPlugin.Settings[CourierSettings.Agents];
			machine.Eve.CurrentAgent = (string)pPlugin.Settings[CourierSettings.CurrentAgent];
			machine.Eve.CircleAgents = (bool)pPlugin.Settings[CourierSettings.CircleAgents];
			// TODO: transfer other settings
			machine.HandleEvent(CourierEvents.Start);
			pObserver.Notify(this, "End", 100, "Eve Courier actor returned");
		}

		#region IMessageHandler Members

		public void SendMessage(string stage, string msg)
		{
			pObserver.Notify(this, stage, 0, msg);
		}

		#endregion
	}
}
