#define TurnOffHeartBeat

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Timers;
using ExecutionActors;
using Courier.States;
using Logger;

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
		LoggedIn,
		CharacterSelected,
		Initialized
	}

	public class CourierStateMachine : StateMachine
	{
		private const int pHeartBeatInterval = 5000;

		public const string Id = "antgraf.Eve.Courier";

		private Timer pHeartBeat = new Timer(pHeartBeatInterval);
		private FileLogger pLogger = null;
		private Eve pEve = null;

		public CourierStateMachine()
			: this(null)
		{}

		public CourierStateMachine(FileLogger logger)
			: this(new IdleState(), logger)
		{}

		public CourierStateMachine(EveCourierState initialState, FileLogger logger)
			: base(initialState)
		{
			pLogger = logger;
			pEve = new Eve(pLogger);
			pHeartBeat.AutoReset = true;
			pHeartBeat.Elapsed += new ElapsedEventHandler(pHeartBeat_Elapsed);
#if !TurnOffHeartBeat
			pHeartBeat.Start();
#endif
		}

		public EveCourierState HandleEvent(CourierEvents eventId)
		{
			return (EveCourierState)base.HandleEvent((int)eventId);
		}

		public void pHeartBeat_Elapsed(object sender, ElapsedEventArgs e)
		{
			HandleEvent(CourierEvents.HeartBeat);
		}

		public void Log(string msg)
		{
			if(pLogger != null)
			{
				pLogger.Log(msg);
			}
		}

		public void Stop()
		{
			pHeartBeat.Stop();
		}

		public Eve Eve
		{
			get { return pEve; }
		}
	}
}
