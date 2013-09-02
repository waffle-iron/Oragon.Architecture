using Oragon.Architecture.Services;
using System.Linq;
using System;
using NLog;

namespace Oragon.Architecture.GenericServiceHost
{
	internal static class Program
	{
		volatile static Logger Logger = LogManager.GetCurrentClassLogger();

		private static void Main(params string[] args)
		{
			AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
			ServiceProcessEntryPoint.Run(args);
		}

		static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.IsTerminating)
				Program.Logger.Fatal((e.ExceptionObject as Exception).ToString());
			else
				Program.Logger.Error((e.ExceptionObject as Exception).ToString());
		}

		static void CurrentDomain_FirstChanceException(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
		{
			Program.Logger.Warn("FirstChanceException: " + e.Exception.ToString());
		}
	}
}
