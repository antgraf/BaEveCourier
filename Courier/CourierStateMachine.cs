#define TurnOffHeartBeat

using System;
using System.Collections.Generic;
using System.Timers;
using ExecutionActors;
using Courier.States;
using Logger;
using EveOperations;

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
		Initialized,
		Autopilot,
		AutopilotStopped,
		DestinationReached,
		AgentReached,
		GoToDestination,
		RepeatAgent,
		NextAgent
	}

	public class CourierStateMachine : StateMachine
	{
		private const int pHeartBeatInterval = (int)(2.0 * 1000);	// milliseconds
		private const string pLogFormat = "[{0}] {1}";

		public const string Id = "antgraf.Eve.Courier";

		private readonly Timer pHeartBeat = new Timer(pHeartBeatInterval);
		private readonly FileLogger pLogger = null;
		private readonly IMessageHandler pMessager = null;
		private readonly ISettingsProvider pSettings = null;
		private readonly Eve pEve = null;

		public CourierStateMachine()
			: this(null, null, null)
		{}

		public CourierStateMachine(FileLogger logger, IMessageHandler messager, ISettingsProvider settings)
			: this(new IdleState(), logger, messager, settings)
		{}

		public CourierStateMachine(EveCourierState initialState, FileLogger logger, IMessageHandler messager, ISettingsProvider settings)
			: base(initialState)
		{
			SleepUntil = null;
			pLogger = logger;
			pMessager = messager;
			pSettings = settings;
			pEve = new Eve(pMessager);
			pHeartBeat.AutoReset = true;
			pHeartBeat.Elapsed += PHeartBeatElapsed;
#if !TurnOffHeartBeat
			pHeartBeat.Start();
#endif
		}

		public EveCourierState HandleEvent(CourierEvents eventId)
		{
			return (EveCourierState)base.HandleEvent((int)eventId);
		}

		public void PHeartBeatElapsed(object sender, ElapsedEventArgs e)
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

		public string Agent
		{
			get
			{
				string agent = (string)Settings[CourierSettings.LastAgent];
				List<string> agents = (List<string>)Settings[CourierSettings.Agents];
				if(string.IsNullOrEmpty(agent) && agents.Count > 0)
				{
					agent = agents[0];
				}
				if(!string.IsNullOrEmpty(agent))
				{
					DateTime t;
					Dictionary<string, DateTime> timers = (Dictionary<string, DateTime>)Settings[CourierSettings.AgentTimers];
					if(timers.TryGetValue(agent, out t))
					{
						if(t < DateTime.Now)
						{
							timers.Remove(agent);
							pSettings.SaveSettings();
						}
						else
						{
							agent = NextAgent;
						}
					}
				}
				return agent;
			}
		}

		public string NextAgent
		{
			get
			{
				string first = null;
				string next = null;
				bool currentFound = false;
				List<string> agents = (List<string>)Settings[CourierSettings.Agents];
				string lastagent = (string)Settings[CourierSettings.LastAgent];
				Dictionary<string, DateTime> timers = (Dictionary<string, DateTime>)Settings[CourierSettings.AgentTimers];
				foreach(string agent in agents)
				{
					bool ok = false;
					DateTime t;
					if(!currentFound && agent == lastagent)
					{
						currentFound = true;
						continue;
					}
					if(timers.TryGetValue(agent, out t))
					{
						if(t < DateTime.Now)
						{
							timers.Remove(agent);
							pSettings.SaveSettings();
							ok = true;
						}
					}
					else
					{
						ok = true;
					}
					if (!ok) continue;
					if(currentFound)
					{
						next = agent;
						break;
					}
					if(first == null)
					{
						first = agent;
					}
				}
				return next ?? first;
			}
		}

		public DateTime NextAgentAvailableTime
		{
			get
			{
				DateTime result = DateTime.MaxValue;
				List<string> agents = (List<string>)Settings[CourierSettings.Agents];
				Dictionary<string, DateTime> timers = (Dictionary<string, DateTime>)Settings[CourierSettings.AgentTimers];
				foreach(string agent in agents)
				{
					DateTime t;
					if(!timers.TryGetValue(agent, out t))
					{
						break;
					}
					if(t < result)
					{
						result = t;
					}
				}
				return result;
			}
		}

		public Eve Eve
		{
			get { return pEve; }
		}

		public Settings Settings
		{
			get
			{
				return pSettings.GetSettings();
			}
		}

		public DateTime? SleepUntil { get; set; }
	}
}
