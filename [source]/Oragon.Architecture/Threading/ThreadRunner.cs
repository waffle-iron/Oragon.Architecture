using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Threading
{
	public static class ThreadRunner
	{

		public static Task RunTask(Action method)
		{
			Task task = new Task(method);
			task.Start();
			return task;
		}


		public static void RunSta(Action method)
		{
			Run(method, System.Threading.ApartmentState.STA);
		}

		public static void RunMta(Action method)
		{
			Run(method, System.Threading.ApartmentState.MTA);
		}

		public static void RunUnknown(Action method)
		{
			Run(method, System.Threading.ApartmentState.Unknown);
		}

		private static void Run(Action method, System.Threading.ApartmentState apartmentState)
		{
			System.Threading.ManualResetEvent evnt = new System.Threading.ManualResetEvent(false);
			System.Threading.Thread thread = new System.Threading.Thread(delegate()
			{
				method();
				evnt.Set();
			});
			thread.SetApartmentState(apartmentState);
			thread.Start();
			evnt.WaitOne();
		}
	}
}
