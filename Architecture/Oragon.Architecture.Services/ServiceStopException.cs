using System;
using System.Runtime.Serialization;
namespace Oragon.Architecture.Services
{
	[Serializable]
	public class ServiceStopException : Exception
	{
		public ServiceStopException()
		{
		}
		public ServiceStopException(string message) : base(message)
		{
		}
		public ServiceStopException(string message, Exception inner) : base(message, inner)
		{
		}
		protected ServiceStopException(SerializationInfo info, StreamingContext context) : base(info, context)
		{
		}
	}
}
