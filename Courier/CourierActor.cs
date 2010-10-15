using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExecutionActors;

namespace Courier
{
	class CourierActor : Actor
	{
		protected override void Init(object data)
		{
			pObserver.Notify(this, "Initialization", 100, "Eve Courier actor initialized");
		}

		protected override void Worker()
		{
			pObserver.Notify(this, "Start", 100, "Eve Courier actor started");
			pObserver.Notify(this, "TODO", 0, "NOT IMPLEMENTED");
			pObserver.Notify(this, "End", 100, "Eve Courier actor finished");
		}
	}
}
