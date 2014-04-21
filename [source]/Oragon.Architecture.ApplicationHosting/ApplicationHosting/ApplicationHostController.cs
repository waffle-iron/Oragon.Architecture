using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting
{
	public abstract class ApplicationHostController : MarshalByRefObject
	{
		public abstract void Start();

		public abstract void Stop();
	}
}
