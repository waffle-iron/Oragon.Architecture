using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Extensions
{
	public static partial class OragonExtensions
	{
		public static Uri Password(this Uri @this, string newPassword)
		{
			var builder = new UriBuilder(@this);
			builder.Password = newPassword;
			return builder.Uri;
		}
	}
}
