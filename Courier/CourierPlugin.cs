using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExecutionActors;
using System.Windows.Forms;

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
			if(form.Result == SettingsFormResult.Run)
			{
				Run();
			}
			else if(form.Result == SettingsFormResult.Setup)
			{
				throw new NotImplementedException();
			}
		}
	}
}
