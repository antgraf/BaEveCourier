using System;

namespace EveOperations.Exceptions
{
	public class CannotLaunchProcessException : ApplicationException
	{
		public CannotLaunchProcessException()
		{}

		public CannotLaunchProcessException(string path)
			: base(path)
		{}
	}
}
