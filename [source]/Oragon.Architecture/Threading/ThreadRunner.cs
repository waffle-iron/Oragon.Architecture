using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Threading
{
	public static class ThreadRunner
	{

		public static void RunAndWaitTasks(params Action[] actions)
		{
			Task[] taskArray = new Task[actions.Length];
			for (int i = 0; i < actions.Length; i++)
			{
				taskArray[i] = Task.Factory.StartNew(actions[i]);
			}
			Task.WaitAll(taskArray);
		}
		public static T[] RunAndWaitTaskResults<T>(params Func<T>[] funcs)
		{
			Task<T>[] taskArray = new Task<T>[funcs.Length];
			for (int i = 0; i < funcs.Length; i++)
			{
				taskArray[i] = Task.Factory.StartNew<T>(funcs[i]);
			}
			Task.WaitAll(taskArray);
			return taskArray.Select(it => it.Result).ToArray();
		}

		public static async Task RunAsync(Action action)
		{
			await RunTask(action);
		}
		public static async Task<T> RunAsync<T>(Func<T> func)
		{
			return await RunTask(func);
		}

		public static Task RunTask(Action action)
		{
			Task task = Task.Factory.StartNew(action);
			return task;
		}
		public static Task<T> RunTask<T>(Func<T> func)
		{
			Task<T> task = Task.Factory.StartNew<T>(func);
			return task;
		}

		public static void RunStaThread(Action action)
		{
			Run(action, System.Threading.ApartmentState.STA);
		}

		public static void RunMtaThread(Action action)
		{
			Run(action, System.Threading.ApartmentState.MTA);
		}

		public static void RunThread(Action action)
		{
			Run(action, System.Threading.ApartmentState.Unknown);
		}

		private static void Run(Action acton, System.Threading.ApartmentState apartmentState)
		{
			System.Threading.ManualResetEvent evnt = new System.Threading.ManualResetEvent(false);
			System.Threading.Thread thread = new System.Threading.Thread(delegate()
			{
				acton();
				evnt.Set();
			});
			thread.SetApartmentState(apartmentState);
			thread.Start();
			evnt.WaitOne();
		}

	}
}
