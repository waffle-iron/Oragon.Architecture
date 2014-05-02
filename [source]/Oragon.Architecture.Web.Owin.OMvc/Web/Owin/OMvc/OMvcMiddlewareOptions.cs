using Microsoft.Owin;
using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Web.Owin.OMvc
{
	public class OMvcMiddlewareOptions
	{
		public IApplicationContext ApplicationContext { get; private set; }

		public OMvcMiddlewareOptions(IApplicationContext applicationContext)
		{
			this.ApplicationContext = applicationContext;
		}

	}
}
