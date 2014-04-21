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

		public static string GetAssemblyInformationalVersion(this System.Reflection.Assembly @this)
		{
			AssemblyInformationalVersionAttribute attr = @this.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
			string returnValue = null;
			if (attr != null)
				returnValue = attr.InformationalVersion;
			return returnValue;
		}
	}
}
