using ExecutionActors;
using System.Collections.Generic;
using System;

namespace Courier
{
	class CourierPlugin : PluginBase
	{
		public CourierPlugin()
		{
			pPluginName = "Eve Courier BA plug-in";
			// TODO check if already exists
			pActor = ActorsMan.NewActor(typeof(CourierActor), this, this);
		}

		public override void ShowUI()
		{
			try
			{
				SettingsForm form = new SettingsForm(this);
				form.ShowDialog();
				switch (form.Result)
				{
					case SettingsFormResult.Run:
						{
							((CourierActor) pActor).Setup = false;
							Run();
							break;
						}
					case SettingsFormResult.Setup:
						{
							((CourierActor) pActor).Setup = true;
							Run();
							break;
						}
				}
			}
			catch(Exception e)
			{
				Notify(pActor, "Error: " + e);
			}
		}

		public override void SaveSettings(System.Type[] types = null)
		{
			base.SaveSettings(new[] { typeof(Dictionary<string, DateTime>) });
		}

		public override void LoadSettings(System.Type[] types = null)
		{
			base.LoadSettings(new[] { typeof(Dictionary<string, DateTime>) });
		}
	}
}
