﻿using Oragon.Architecture.Logging;
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
		protected List<string> Arguments { get; private set; }

		protected List<ServiceHost> ServiceHosts { get; private set; }

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

		protected virtual bool HasServiceName
		{
			get
			{
				return this.Arguments.Contains("-servicename");
			}
		}

		protected virtual string ServiceName
		{
			get 
			{
				if (this.HasServiceName)
				{
					var parameterKeyIndex = this.Arguments.IndexOf("-servicename");
					var parameterValueIndex = parameterKeyIndex +1;
					if (this.Arguments.Count > parameterValueIndex)
						return this.Arguments[parameterValueIndex];
					else
						throw new ArgumentException("The argument -servicename must be before a name of service");
				}
				else
					return null;
			}
		}

		protected virtual bool HasServiceConfigurationFile
		{
			get
			{
				return this.Arguments.Contains("-serviceconfigurationfile");
			}
		}

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
					else
						throw new ArgumentException("The argument -servicename must be before a name of service");
				}
				else
					return null;
			}
		}

			



		public ApplicationHost(string[] arguments)
		{
			AppDomain.CurrentDomain.FirstChanceException += FirstChanceExceptionHandler;
			AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
			this.Arguments = new List<string>(arguments);
			this.ServiceHosts = new List<ServiceHost>();
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
			if(this.HasServiceConfigurationFile)
				pathQueue.Enqueue(this.ServiceConfigurationFile);
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


		private void FillServiceHosts(IApplicationContext descriptorsApplicationContext)
		{
			ServiceHost[] serviceHosts = descriptorsApplicationContext.GetObjects<ServiceHost>().Select(it => it.Value).ToArray();
			if (serviceHosts.Any())
				this.ServiceHosts.AddRange(serviceHosts);
			else
				throw new InvalidOperationException("DescriptorsApplicationContext dos not have any ServiceDescriptor defined");
		}


		public virtual void Run()
		{
			IApplicationContext descriptorsApplicationContext = this.BuildDescriptorsApplicationContext();
			this.FillServiceHosts(descriptorsApplicationContext);




		}

	}
}
