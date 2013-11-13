using Oragon.Architecture.Services;
using System.Linq;
using System;
using NLog;

namespace Oragon.Architecture.GenericServiceHost
{
	internal static class Program
	{
		volatile static Logger Logger;

		private static void Main(params string[] args)
		{
			Program.Logger = LogManager.GetCurrentClassLogger();
			AppDomain.CurrentDomain.FirstChanceException += CurrentDomain_FirstChanceException;
			AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;

			string startPoint = typeof(Program).Assembly.EscapedCodeBase;

			ServiceProcessEntryPoint.Run(startPoint, args);
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
