using Spring.Objects.Factory;
using System;

namespace ApplicationHostingSpringNetExample
{
	public class AutoStartAppExample : IInitializingObject, IDisposable
	{
		#region Public Methods

		public void AfterPropertiesSet()
		{
			Console.WriteLine("AutoStartAppExample Start");
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		#endregion Public Methods

		#region Protected Methods

		protected virtual void Dispose(bool disposing)
		{
			Console.WriteLine("AutoStartAppExample Stop");
		}

		#endregion Protected Methods
	}
}