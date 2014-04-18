using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Oragon.Architecture.Log
{
	[DataContract]
	public enum LogLevel
	{
		[EnumMember(Value = "Debug")]
		Debug = 1,

		[EnumMember(Value = "Trace")]
		Trace = 2,

		[EnumMember(Value = "Warn")]
		Warn = 3,

		[EnumMember(Value = "Error")]
		Error = 4,

		[EnumMember(Value = "Fatal")]
		Fatal = 5,

		[EnumMember(Value = "Audit")]
		Audit = 6
	}
}
