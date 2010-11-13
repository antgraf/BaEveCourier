using System;
using ExecutionActors;

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
			SettingsForm form = new SettingsForm(this);
			form.ShowDialog();
			switch(form.Result)
			{
				case SettingsFormResult.Run:
					Run();
					break;
				case SettingsFormResult.Setup:
					throw new NotImplementedException();
			}
		}
	}
}
