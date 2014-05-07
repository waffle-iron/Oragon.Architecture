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
	public class ApplicationDescriptor
	{
		[DataMember]
		public string Name { get; set; }
		[DataMember]
		public string FriendlyName { get; set; }
		[DataMember]
		public string Description { get; set; }
		[DataMember]
		public string FactoryType { get; set; }
		[DataMember]
		public string ApplicationConfigurationFile { get; set; }
		[DataMember]
		public string ApplicationBaseDirectory { get; set; }
		[DataMember]
		public string TypeName { get; set; }
	}
}
