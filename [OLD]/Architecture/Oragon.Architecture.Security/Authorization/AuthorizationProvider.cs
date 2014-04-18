using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Security.Authorization
{
	public abstract class AuthorizationProvider
	{
		public virtual bool Authorize(string action)
		{
			return true;
		}

		public virtual bool Authorize(string action, string contextName, int contextData)
		{
			return true;
		}

		public virtual bool Authorize(string action, string contextName, string contextData)
		{
			return true;
		}

	}
}
