using Oragon.Architecture.Logging;
using Spring.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using Topshelf;

namespace Oragon.Architecture.ApplicationHosting
{
	public class HostProcessRunner
	{
		#region Public Constructors

		public HostProcessRunner(IEnumerable<string> arguments)
		{
			AppDomain.CurrentDomain.FirstChanceException += FirstChanceExceptionHandler;
			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
			this.Arguments = new List<string>(arguments);
			this.Logger = new Oragon.Architecture.Logging.Loggers.DiagnosticsLogger();
		}

		#endregion Public Constructors

		#region Protected Properties

		protected List<string> Arguments { get; private set; }

		protected virtual bool HasServiceConfigurationFile
		{
			get
			{
				return this.Arguments.Contains("-serviceconfigurationfile");
			}
		}

		protected virtual bool IsConsole
		{
			get
			{
				return this.Arguments.Contains("console");
			}
		}

		protected virtual bool IsDebug
		{
			get
			{
				return this.Arguments.Contains("debug");
			}
		}

		protected ILogger Logger { get; set; }

		protected virtual string ServiceConfigurationFile
		{
			get
			{
				if (this.HasServiceConfigurationFile)
				{
					var parameterKeyIndex = this.Arguments.IndexOf("-serviceconfigurationfile");
					var parameterValueIndex = parameterKeyIndex + 1;
					if (this.Arguments.Count > parameterValueIndex)
						return this.Arguments[parameterValueIndex];
				}
				return null;
			}
		}

		protected WindowsServiceHost ServiceHost { get; private set; }

		#endregion Protected Properties

		#region Public Methods

		public virtual TopshelfExitCode Run()
		{
			if (this.IsDebug)
			{
				System.Diagnostics.Debugger.Launch();
			}

			AppDomain.MonitoringIsEnabled = true;
			using (IApplicationContext descriptorsApplicationContext = this.BuildDescriptorsApplicationContext())
			{
				this.FillServiceHosts(descriptorsApplicationContext);
			}

			TopshelfExitCode exitCode = TopshelfExitCode.Ok;


			if (this.IsConsole)
			{
				exitCode = this.ServiceHost.RunConsoleMode(this.Arguments, this.ServiceConfigurationFile);
			}
			else
			{
				Host host = HostFactory.New(hostConfig =>
				{
					this.ServiceHost.Configure(hostConfig, this.ServiceConfigurationFile);
				});
				exitCode = host.Run();
			}
			return exitCode;
		}

		#endregion Public Methods

		#region Protected Methods

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

		protected virtual Queue<string> BuildPathQueue()
		{
			Queue<string> pathQueue = new Queue<string>();
			if (this.HasServiceConfigurationFile)
				pathQueue.Enqueue(this.ServiceConfigurationFile);
			return pathQueue;
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

		#endregion Protected Methods

		#region Private Methods

		private void FillServiceHosts(IApplicationContext descriptorsApplicationContext)
		{
			WindowsServiceHost[] serviceHosts = descriptorsApplicationContext.GetObjects<WindowsServiceHost>().Select(it => it.Value).ToArray();
			if (serviceHosts.Length == 1)
				this.ServiceHost = serviceHosts.First();
			else if (serviceHosts.Length > 1)
				throw new InvalidOperationException("DescriptorsApplicationContext has many ServiceDescriptors defined. Expected: One");
			else if (serviceHosts.Length < 1)
				throw new InvalidOperationException("DescriptorsApplicationContext dos not have any ServiceDescriptor defined. Expected: One");
		}

		#endregion Private Methods
	}
}