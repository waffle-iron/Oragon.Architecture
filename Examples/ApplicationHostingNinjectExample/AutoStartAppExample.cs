using System;

namespace ApplicationHostingNinjectExample
{
	public interface IAutoStartAppExample
	{
		#region Public Methods

		void Start();

		void Stop();

		#endregion Public Methods
	}

	public class AutoStartAppExample : IAutoStartAppExample
	{
		#region Public Methods

		public void Start()
		{
			Console.WriteLine("AutoStartAppExample Start");
		}

		public void Stop()
		{
			Console.WriteLine("AutoStartAppExample Stop");
		}

		#endregion Public Methods
	}
}