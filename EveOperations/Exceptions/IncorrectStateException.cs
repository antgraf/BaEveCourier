using System;

namespace EveOperations.Exceptions
{
	public class IncorrectStateException : ApplicationException
	{
		public IncorrectStateException()
		{}

		public IncorrectStateException(string msg)
			: base(msg)
		{}
	}
}
