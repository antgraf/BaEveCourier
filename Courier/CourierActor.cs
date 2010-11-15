using ExecutionActors;
using EveOperations;

namespace Courier
{
	class CourierActor : Actor, IMessageHandler, ISettingsProvider
	{
// ReSharper disable InconsistentNaming
		protected CourierPlugin pPlugin = null;
// ReSharper restore InconsistentNaming

		public CourierActor()
		{
			Setup = false;
		}

		protected override void Init(object data)
		{
			pPlugin = (CourierPlugin)data;
			StateMachine.Register(CourierStateMachine.Id, new CourierStateMachine(pLog, this, this));
			pObserver.Notify(this, "Initialization", 100, "Eve Courier actor initialized");
		}

		protected override void Worker()
		{
			pObserver.Notify(this, "Start", 100, "Eve Courier actor started");
			CourierStateMachine machine = (CourierStateMachine)StateMachine.GetInstance(CourierStateMachine.Id);
			if(Setup)
			{
				pObserver.Notify(this, "Start", 100, "Setup started");
				machine.Setup();
				pObserver.Notify(this, "End", 100, "Setup completed");
			}
			else
			{
				pObserver.Notify(this, "Start", 100, "Execution started");
				machine.Start();
				pObserver.Notify(this, "End", 100, "Eve Courier actor started");
			}
			pObserver.Notify(this, "End", 100, "Execution completed");
		}

		#region IMessageHandler Members

		public void SendMessage(string stage, string msg)
		{
			pObserver.Notify(this, stage, 0, msg);
		}

		#endregion

		#region ISettingsProvider Members

		public Settings GetSettings()
		{
			return pPlugin.Settings;
		}

		public void SaveSettings()
		{
			pPlugin.SaveSettings();
		}

		#endregion

		public bool Setup { get; set; }
	}
}
