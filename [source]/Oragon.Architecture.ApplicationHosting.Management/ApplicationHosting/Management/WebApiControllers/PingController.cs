using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
﻿using System;
using System.Web.Http;
using Oragon.Architecture.ApplicationHosting.Management.Model;

namespace Oragon.Architecture.ApplicationHosting.Management.WebApiControllers
{
	public class PingController : ApiController
	{
		public PingController()
		{
		}

		[HttpGet]
		public PingResponse Get()
		{
			return new PingResponse()
			{
				Result = "pong"
			};
		}
	}
}
