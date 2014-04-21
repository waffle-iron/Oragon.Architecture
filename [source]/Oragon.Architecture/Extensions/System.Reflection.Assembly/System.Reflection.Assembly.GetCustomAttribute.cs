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

		public static T GetCustomAttribute<T>(this System.Reflection.Assembly @this)
			where T: Attribute
		{
			T attr = (T)@this.GetCustomAttributes(typeof(T), true).FirstOrDefault();
			return attr;
		}
	}
}
