using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExecutionActors;

namespace Courier
{
	public interface ISettingsProvider
	{
		Settings GetSettings();
		void SaveSettings();
	}
}
