using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Courier
{
	public interface IMessageHandler
	{
		void SendMessage(string stage, string msg);
	}
}
