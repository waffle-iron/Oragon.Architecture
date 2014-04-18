using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Services
{
	
	[Serializable]
	public class EnvelopeNullException : Exception
	{
		public EnvelopeNullException() { }
		public EnvelopeNullException(string message) : base(message) { }
		public EnvelopeNullException(string message, Exception inner) : base(message, inner) { }
		protected EnvelopeNullException(
		  System.Runtime.Serialization.SerializationInfo info,
		  System.Runtime.Serialization.StreamingContext context)
			: base(info, context) { }
	}
}
