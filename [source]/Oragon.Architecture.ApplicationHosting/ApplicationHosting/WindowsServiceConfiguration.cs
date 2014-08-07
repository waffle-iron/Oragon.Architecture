using System;
using System.Collections.Generic;

namespace Oragon.Architecture.ApplicationHosting
{
	public class WindowsServiceConfiguration
	{
		#region Public Properties

		public Credential CustomIdentityCredential { get; set; }

		public List<string> Dependences { get; set; }

		public AccountType IdentityType { get; set; }

		public StartMode StartMode { get; set; }

		public TimeSpan StartTimeOut { get; set; }

		public TimeSpan StopTimeOut { get; set; }

		#endregion Public Properties
	}
}