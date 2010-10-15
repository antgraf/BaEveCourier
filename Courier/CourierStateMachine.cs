using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using ExecutionActors;
using Courier.States;

namespace Courier
{
	public enum CourierEvents
	{
		Start,
		End,
		GoToFinalState,
		Sleep,
		HeartBeat,
		EveLaunched,
		LoggedIn
	}

	public class CourierStateMachine : StateMachine
	{
		private const int pHeartBeatInterval = 5000;

		public const string Id = "antgraf.Eve.Courier";

		private Timer pHeartBeat = new Timer(pHeartBeatInterval);

		public CourierStateMachine()
			: this(new IdleState())
		{}

		public CourierStateMachine(EveCourierState initialState)
			: base(initialState)
		{
			pHeartBeat.Elapsed += new ElapsedEventHandler(pHeartBeat_Elapsed);
			pHeartBeat.Start();
		}

		public EveCourierState HandleEvent(CourierEvents eventId)
		{
			return (EveCourierState)base.HandleEvent((int)eventId);
		}

		public void pHeartBeat_Elapsed(object sender, ElapsedEventArgs e)
		{
			HandleEvent(CourierEvents.HeartBeat);
		}
	}
}
