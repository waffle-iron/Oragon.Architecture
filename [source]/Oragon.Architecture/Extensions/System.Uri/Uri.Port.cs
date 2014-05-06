using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		public static Uri Port(this Uri @this, int newPort)
		{
			var builder = new UriBuilder(@this);
			builder.Port = newPort;
			return builder.Uri;
		}
	}
}
