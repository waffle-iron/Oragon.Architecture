using Oragon.Architecture.ApplicationHosting.WindowsServices.Model;
using Oragon.Architecture.Logging;
using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting
{
	public class ApplicationHost
	{
		protected string HostProcessPath { get; private set; }

		protected List<string> Arguments { get; private set; }

		protected List<ServiceDescriptor> ServiceDescriptors { get; private set; }

		protected ILogger Logger { get; set; }

		protected virtual bool IsDebug
		{
			get
			{
				return this.Arguments.Contains("debug");
			}
		}

		protected virtual bool IsConsole
		{
			get
			{
				return this.Arguments.Contains("console");
			}
		}


		public ApplicationHost(string hostProcessPath, string[] arguments)
		{
			AppDomain.CurrentDomain.FirstChanceException += FirstChanceExceptionHandler;
			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
			this.HostProcessPath = hostProcessPath;
			this.Arguments = new List<string>(arguments);
			this.ServiceDescriptors = new List<ServiceDescriptor>();
			this.Logger = new Oragon.Architecture.Logging.Loggers.DiagnosticsLogger();
		}



		protected virtual void FirstChanceExceptionHandler(object sender, System.Runtime.ExceptionServices.FirstChanceExceptionEventArgs e)
		{
			this.Logger.Warn("ApplicationHost", "FirstChanceException: " + e.Exception.ToString());
		}

		protected virtual void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
		{
			if (e.IsTerminating)
				this.Logger.Fatal("ApplicationHost", (e.ExceptionObject as Exception).ToString());
			else
				this.Logger.Error("ApplicationHost", (e.ExceptionObject as Exception).ToString());
		}

		protected virtual Queue<string> BuildPathQueue()
		{
			Queue<string> pathQueue = new Queue<string>();
			pathQueue.Enqueue(this.HostProcessPath + ".xml");
			return pathQueue;
		}

		protected virtual IApplicationContext BuildDescriptorsApplicationContext()
		{
			Queue<string> paths = this.BuildPathQueue();
			IApplicationContext applicationContext = null;
			RetryHelper.Try(delegate
			{
				applicationContext = new Spring.Context.Support.XmlApplicationContext(paths.Dequeue());
			}, paths.Count, true, 0);

			return applicationContext;
		}


		private void FillServiceDescriptors(IApplicationContext descriptorsApplicationContext)
		{
			ServiceDescriptor[] serviceDescriptors = descriptorsApplicationContext.GetObjects<ServiceDescriptor>().Select(it => it.Value).ToArray();
			if (serviceDescriptors.Any())
				this.ServiceDescriptors.AddRange(serviceDescriptors);
			else
				throw new InvalidOperationException("DescriptorsApplicationContext dos not have any ServiceDescriptor defined");
		}






		public virtual void Run()
		{
			IApplicationContext descriptorsApplicationContext = this.BuildDescriptorsApplicationContext();
			this.FillServiceDescriptors(descriptorsApplicationContext);
		}

	}
}
