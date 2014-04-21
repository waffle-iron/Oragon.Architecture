using Spring.Objects.Factory.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting
{
	public class WindowsServiceConfiguration
	{
		[Required]
		public StartMode StartMode { get; set; }
		[Required]
		public AccountType IdentityType { get; set; }
		public Credential CustomIdentityCredential { get; set; }
		[Required]
		public List<string> Dependences { get; set; }
		[Required]
		public TimeSpan StartTimeOut { get; set; }
		[Required]
		public TimeSpan StopTimeOut { get; set; }
	}
}
