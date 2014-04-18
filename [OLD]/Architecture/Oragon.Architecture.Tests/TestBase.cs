using Spring.Testing.Microsoft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Tests
{
	public abstract class TestBase : AbstractDependencyInjectionSpringContextTests
	{
		protected override string[] ConfigLocations
		{
			get
			{
				var currentType = this.GetType();
				var text = string.Format(@"assembly://{0}/{1}/{2}.xml",
					currentType.Assembly.GetName().Name,
					currentType.Namespace,
					currentType.Name
				);
				return new String[] { text };
			}
		}
	}
}
