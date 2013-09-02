using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Threading
{
	public class SameKeyParser : IKeyParser
	{
		public string GetName(string name)
		{
			return name;
		}
	}
}
