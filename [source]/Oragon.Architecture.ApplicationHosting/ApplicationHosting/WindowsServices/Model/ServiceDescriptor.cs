﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.WindowsServices.Model
{
	public class ServiceDescriptor
	{
		public string Name { get; set; }
		public string FriendlyName { get; set; }
		public string Description { get; set; }
		public StartMode StartMode { get; set; }
		public AccountType IdentityType { get; set; }
		public Credential CustomIdentityCredential { get; set; }
		public List<string> Dependences { get; set; }
		public string ConfigurationFile { get; set; }
	}
}
