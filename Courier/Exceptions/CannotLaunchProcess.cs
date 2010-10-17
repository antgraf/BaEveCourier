using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Courier.Exceptions
{
	public class CannotLaunchProcess : ApplicationException
	{
		public CannotLaunchProcess()
			: base()
		{}

		public CannotLaunchProcess(string path)
			: base(path)
		{}
	}
}
