using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Security.Authentication
{
	public class Credential
	{
		public virtual string Username { get; set; }
		public virtual string Password { get; set; }
	}
}
