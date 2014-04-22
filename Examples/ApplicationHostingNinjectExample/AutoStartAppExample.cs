using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationHostingNinjectExample
{

	public interface IAutoStartAppExample
	{
		void Start();

		void Stop();
	}


	public class AutoStartAppExample : IAutoStartAppExample
	{
		public void Start()
		{
			Console.WriteLine("AutoStartAppExample Start");
		}

		public void Stop()
		{
			Console.WriteLine("AutoStartAppExample Stop");
		}


	}
}
