using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Services.Contracts
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class HostStatistic
	{
		[DataMember]
		public List<ApplicationStatistic> ApplicationStatistics { get; set; }

	}
}
