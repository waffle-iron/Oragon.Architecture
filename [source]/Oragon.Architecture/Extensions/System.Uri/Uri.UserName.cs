using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		public static Uri UserName(this Uri @this, string newUserName)
		{
			var builder = new UriBuilder(@this);
			builder.UserName = newUserName;
			return builder.Uri;
		}
	}
}
