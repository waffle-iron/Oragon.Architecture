using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		public static Uri Path(this Uri @this, string newPath)
		{
			var builder = new UriBuilder(@this);
			builder.Path = newPath;
			return builder.Uri;
		}
	}
}
