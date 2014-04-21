using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{

		public static string GetAssemblyVersion(this System.Reflection.Assembly @this)
		{
			AssemblyVersionAttribute attr = @this.GetCustomAttribute<AssemblyVersionAttribute>();
			string returnValue = null;
			if (attr != null)
				returnValue = attr.Version;
			return returnValue;
		}
	}
}
