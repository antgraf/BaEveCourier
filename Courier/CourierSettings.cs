using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EveOperations;

namespace Courier
{
	public static class CourierSettings
	{
		public const string Version = "1.0.0";

		public const string Path = "Path";
		public const string Login = "Login";
		public const string Password = "Password";
		public const string Position = "Position";
		public const string Agents = "Agents";
		public const string LastAgent = "LastAgent";
		public const string CurrentAgent = "CurrentAgent";
		public const string CurrentCargo = "CurrentAgentGotCargo";
		public const string CircleAgents = "CircleAgents";
		public const string AgentTimers = "AgentTimers";

		public const string DefaultPath = @"C:\Program Files\CCP\EVE\eve.exe";
		public const string DefaultLogin = "";
		public const string DefaultPassword = "";
		public static string[] DefaultAgents = new string[0];
		public const string DefaultLastAgent = "";
		public const string DefaultCurrentAgent = "";
		public const bool DefaultCurrentCargo = false;
		public const bool DefaultCircleAgents = false;
		public const CharacterPosition DefaultPosition = CharacterPosition.Main;
		public static Dictionary<string, DateTime> DefaultAgentTimers = new Dictionary<string, DateTime>();
	}
}
