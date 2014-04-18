using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Principal;

namespace Oragon.Architecture.Security
{
	/// <summary>
	/// Interface se propõe a resolver uma entidade de negócio com base no IIdentity informado
	/// </summary>
	public interface IUserResolver
	{
		object Resolve(IIdentity identity);
	}
}
