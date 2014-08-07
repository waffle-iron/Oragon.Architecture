using System;
using System.Runtime.Serialization;

namespace Oragon.Architecture.ApplicationHosting.Services.Contracts
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class ApplicationStatistic
	{
		#region Public Properties

		[DataMember]
		public AppDomainStatistic AppDomainStatistic { get; set; }

		[DataMember]
		public ApplicationDescriptor ApplicationDescriptor { get; set; }

		#endregion Public Properties
	}
}