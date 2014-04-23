using Spring.Context;
using Spring.Objects.Factory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationHostingSpringNetExample
{
	public class AutoStartAppExample : IInitializingObject, IDisposable
	{
		public void AfterPropertiesSet()
		{
			Console.WriteLine("AutoStartAppExample Start");
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		protected virtual void Dispose(bool disposing)
		{
			Console.WriteLine("AutoStartAppExample Stop");
		}


	}
}
