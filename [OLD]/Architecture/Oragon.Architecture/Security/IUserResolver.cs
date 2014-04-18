using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Oragon.Architecture.Security
{
	/// <summary>
	/// Interface se prop�e a resolver uma entidade de neg�cio com base no IIdentity informado
	/// </summary>
	public interface IUserResolver
	{
		object Resolve(IIdentity identity);
	}
}
