using System;
using System.Runtime.Serialization;

namespace Oragon.Architecture.ApplicationHosting.Services.Contracts
{
	[Serializable]
	[DataContract(IsReference = true)]
	public class AssemblyDescriptor
	{
		#region Public Properties

		[DataMember]
		public string Name { get; set; }

		#endregion Public Properties
	}
}