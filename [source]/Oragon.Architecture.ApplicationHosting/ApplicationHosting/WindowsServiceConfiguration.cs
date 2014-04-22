using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting
{
	public class WindowsServiceConfiguration
	{
		public StartMode StartMode { get; set; }
		public AccountType IdentityType { get; set; }
		public Credential CustomIdentityCredential { get; set; }
		public List<string> Dependences { get; set; }
		public TimeSpan StartTimeOut { get; set; }
		public TimeSpan StopTimeOut { get; set; }
	}
}
