using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.Model
{
	[DataContract]
	public class PingResponse
	{
		[DataMember]
		public string Result { get; set; }
	}
}
