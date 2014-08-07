using System;
using System.Runtime.Serialization;

namespace Oragon.Architecture.ApplicationHosting.Services.Contracts
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class ApplicationDescriptor
	{
		#region Public Properties

		[DataMember]
		public string ApplicationBaseDirectory { get; set; }

		[DataMember]
		public string ApplicationConfigurationFile { get; set; }

		[DataMember]
		public string Description { get; set; }

		[DataMember]
		public string FactoryType { get; set; }

		[DataMember]
		public string FriendlyName { get; set; }

		[DataMember]
		public string Name { get; set; }

		[DataMember]
		public string TypeName { get; set; }

		#endregion Public Properties
	}
}