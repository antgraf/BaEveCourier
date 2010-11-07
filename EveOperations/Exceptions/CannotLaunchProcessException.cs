using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveOperations.Exceptions
{
	public class CannotLaunchProcessException : ApplicationException
	{
		public CannotLaunchProcessException()
			: base()
		{}

		public CannotLaunchProcessException(string path)
			: base(path)
		{}
	}
}
