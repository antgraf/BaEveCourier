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
		Idle,
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
		private const string pLogFormat = "[{0}] {1}";

		public const string Id = "antgraf.Eve.Courier";

		private Timer pHeartBeat = new Timer(pHeartBeatInterval);
		private FileLogger pLogger = null;
		private IMessageHandler pMessager = null;
		private Eve pEve = null;

		public CourierStateMachine()
			: this(null, null)
		{}

		public CourierStateMachine(FileLogger logger, IMessageHandler messager)
			: this(new IdleState(), logger, messager)
		{}

		public CourierStateMachine(EveCourierState initialState, FileLogger logger, IMessageHandler messager)
			: base(initialState)
		{
			pLogger = logger;
			pMessager = messager;
			pEve = new Eve(pMessager);
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

		public void LogAndDisplay(string stage, string msg)
		{
			Log(string.Format(pLogFormat, stage, msg));
			if(pMessager != null)
			{
				pMessager.SendMessage(stage, msg);
			}
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
