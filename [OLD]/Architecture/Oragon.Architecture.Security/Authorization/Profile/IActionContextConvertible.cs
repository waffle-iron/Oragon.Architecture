using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Security.Authorization.Profile
{
	public interface IActionContextConvertible
	{
		ActionContext ToActionContext();
	}
}
