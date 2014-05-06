using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		public static Uri Host(this Uri @this, string newHost)
		{
			var builder = new UriBuilder(@this);
			builder.Host = newHost;
			return builder.Uri;
		}
	}
}
