using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		public static Uri Fragment(this Uri @this, string newFragment)
		{
			var builder = new UriBuilder(@this);
			builder.Fragment = newFragment;
			return builder.Uri;
		}
	}
}
