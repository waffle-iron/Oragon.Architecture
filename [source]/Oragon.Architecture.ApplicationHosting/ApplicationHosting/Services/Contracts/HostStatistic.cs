using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Oragon.Architecture.ApplicationHosting.Services.Contracts
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class HostStatistic
	{
		#region Public Properties

		[DataMember]
		public List<ApplicationStatistic> ApplicationStatistics { get; set; }

		#endregion Public Properties
	}
}