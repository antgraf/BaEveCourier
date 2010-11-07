#define TurnOffHeartBeat

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
		NextAgent
	}

	public class CourierStateMachine : StateMachine
	{
		private const int pHeartBeatInterval = 5000;
		private const string pLogFormat = "[{0}] {1}";

		public const string Id = "antgraf.Eve.Courier";

		private Timer pHeartBeat = new Timer(pHeartBeatInterval);
		private FileLogger pLogger = null;
		private IMessageHandler pMessager = null;
		private ISettingsProvider pSettings = null;
		private Eve pEve = null;
		private DateTime? pSleepUntil = null;

		public CourierStateMachine()
			: this(null, null, null)
		{}

		public CourierStateMachine(FileLogger logger, IMessageHandler messager, ISettingsProvider settings)
			: this(new IdleState(), logger, messager, settings)
		{}

		public CourierStateMachine(EveCourierState initialState, FileLogger logger, IMessageHandler messager, ISettingsProvider settings)
			: base(initialState)
		{
			pLogger = logger;
			pMessager = messager;
			pSettings = settings;
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

		public string Agent
		{
			get
			{
				string agent = (string)this.Settings[CourierSettings.LastAgent];
				List<string> agents = (List<string>)this.Settings[CourierSettings.Agents];
				if(string.IsNullOrEmpty(agent) && agents.Count > 0)
				{
					agent = agents[0];
				}
				if(!string.IsNullOrEmpty(agent))
				{
					DateTime t;
					Dictionary<string, DateTime> timers = (Dictionary<string, DateTime>)this.Settings[CourierSettings.AgentTimers];
					if(timers.TryGetValue(agent, out t))
					{
						if(t < DateTime.Now)
						{
							timers.Remove(agent);
							this.pSettings.SaveSettings();
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
				bool current_found = false;
				List<string> agents = (List<string>)this.Settings[CourierSettings.Agents];
				string lastagent = (string)this.Settings[CourierSettings.LastAgent];
				Dictionary<string, DateTime> timers = (Dictionary<string, DateTime>)this.Settings[CourierSettings.AgentTimers];
				foreach(string agent in agents)
				{
					bool ok = false;
					DateTime t;
					if(!current_found && agent == lastagent)
					{
						current_found = true;
						continue;
					}
					if(timers.TryGetValue(agent, out t))
					{
						if(t < DateTime.Now)
						{
							timers.Remove(agent);
							this.pSettings.SaveSettings();
							ok = true;
						}
					}
					else
					{
						ok = true;
					}
					if(ok)
					{
						if(current_found)
						{
							next = agent;
							break;
						}
						if(first == null)
						{
							first = agent;
						}
					}
				}
				if(next == null) next = first;
				return next;
			}
		}

		public DateTime NextAgentAvailableTime
		{
			get
			{
				DateTime result = DateTime.MaxValue;
				DateTime t;
				List<string> agents = (List<string>)this.Settings[CourierSettings.Agents];
				Dictionary<string, DateTime> timers = (Dictionary<string, DateTime>)this.Settings[CourierSettings.AgentTimers];
				foreach(string agent in agents)
				{
					if(!timers.TryGetValue(agent, out t))
					{
						t = DateTime.MinValue;
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

		public DateTime? SleepUntil
		{
			get { return pSleepUntil; }
			set { pSleepUntil = value; }
		}
	}
}
