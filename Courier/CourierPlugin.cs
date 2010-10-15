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
			pActor = ActorsMan.NewActor(typeof(CourierActor), this);
		}

		public override void ShowUI()
		{
			if(MessageBox.Show("NOT IMPLEMENTED\r\nStart worker?", "TODO", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation)
				== DialogResult.Yes)
			{
				Run();
			}
		}
	}
}
