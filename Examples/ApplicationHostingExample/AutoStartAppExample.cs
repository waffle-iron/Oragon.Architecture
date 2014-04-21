using Spring.Context;
using Spring.Objects.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationHostingExample
{
	public class AutoStartAppExample : IInitializingObject, IDisposable
	{
		public void AfterPropertiesSet()
		{
			Console.WriteLine("AutoStartAppExample Start");
		}

		public void Dispose()
		{
			Console.WriteLine("AutoStartAppExample Stop");
		}


	}
}
