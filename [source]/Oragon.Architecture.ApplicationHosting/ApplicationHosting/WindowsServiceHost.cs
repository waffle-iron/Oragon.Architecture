using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Oragon.Architecture.Extensions;
using NDepend.Helpers;
using NDepend.Path;
using System.Diagnostics.Contracts;

namespace Oragon.Architecture.ApplicationHosting
{
	public class WindowsServiceHost : ServiceControl
	{
		private Guid clientID;

		public string Name { get; set; }

		public string FriendlyName { get; set; }

		public string Description { get; set; }

		public Uri MonitoringEndPoint { get; set; }

		public WindowsServiceConfiguration WindowsServiceConfiguration { get; set; }

		public List<ApplicationHost> Applications { get; set; }

		public IAbsoluteFilePath ConfigurationFilePath { get; private set; }

		public void Configure(Topshelf.HostConfigurators.HostConfigurator hostConfig, string configurationFileName)
		{
			hostConfig.Service<WindowsServiceHost>(serviceConfigurator =>
			{
				serviceConfigurator.ConstructUsing(() => this);
				serviceConfigurator.WhenStarted((serviceManagerInstance, hostControl) => serviceManagerInstance.Start(hostControl));
				serviceConfigurator.WhenStopped((serviceManagerInstance, hostControl) => serviceManagerInstance.Stop(hostControl));
				serviceConfigurator.WhenShutdown((serviceManagerInstance, hostControl) => serviceManagerInstance.Shutdown(hostControl));

			});

			hostConfig.EnableShutdown();

			hostConfig.SetServiceName(this.Name);
			hostConfig.SetDisplayName(this.FriendlyName);
			hostConfig.SetDescription(this.Description);

			switch (this.WindowsServiceConfiguration.IdentityType)
			{
				case AccountType.LocalService: hostConfig.RunAsLocalService(); break;
				case AccountType.LocalSystem: hostConfig.RunAsLocalSystem(); break;
				case AccountType.NetworkService: hostConfig.RunAsNetworkService(); break;
				case AccountType.Prompt: hostConfig.RunAsPrompt(); break;
				case AccountType.Custom: hostConfig.RunAs(this.WindowsServiceConfiguration.CustomIdentityCredential.Username, this.WindowsServiceConfiguration.CustomIdentityCredential.Password); break;
			}

			switch (this.WindowsServiceConfiguration.StartMode)
			{
				case StartMode.Automatically: hostConfig.StartAutomatically(); break;
				case StartMode.AutomaticallyDelayed: hostConfig.StartAutomaticallyDelayed(); break;
				case StartMode.Disabled: hostConfig.Disabled(); break;
				case StartMode.Manually: hostConfig.StartManually(); break;
			}
			if (this.WindowsServiceConfiguration.Dependences != null)
			{
				foreach (string dependency in this.WindowsServiceConfiguration.Dependences)
				{
					hostConfig.AddDependency(dependency);
				}
			}

			this.ConfigurationFilePath = configurationFileName.ToAbsoluteFilePath();
		}

		public bool Start(HostControl hostControl)
		{
			Contract.Requires(this.Applications != null && this.Applications.Count > 0, "Invalid Application configuration, has no Application defined.");
			Contract.Requires(this.ConfigurationFilePath.Exists, "Configuration FilePath cannot be found in disk");

			if (this.MonitoringEndPoint != null)
			{
				ClientApiProxy proxy = new ClientApiProxy(this.MonitoringEndPoint);
				proxy.RegisterHost(this)
					.ContinueWith(it => this.clientID = it.Result);
			}


			List<ApplicationHost> tmpApplicationList = new List<ApplicationHost>(this.Applications);
			foreach (var application in tmpApplicationList)
			{
				application.Start(this.ConfigurationFilePath.ParentDirectoryPath);
			}
			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			if (this.MonitoringEndPoint != null)
			{
				ClientApiProxy proxy = new ClientApiProxy(this.MonitoringEndPoint);
				proxy.UnregisterHost(this.clientID);
			}

			List<ApplicationHost> tmpApplicationList = new List<ApplicationHost>(this.Applications);
			tmpApplicationList.Reverse();
			foreach (var application in tmpApplicationList)
			{
				application.Stop();
			}
			return true;
		}

		public void Shutdown(HostControl hostControl)
		{
			return;
		}

		public TopshelfExitCode RunConsoleMode(List<string> arguments, string configurationFileName)
		{
			IFilePath filePath = null;
			if (configurationFileName.TryGetFilePath(out filePath))
			{
				if (filePath.IsAbsolutePath)
				{
					this.ConfigurationFilePath = configurationFileName.ToAbsoluteFilePath();
				}
				else if (filePath.IsRelativePath)
				{
					this.ConfigurationFilePath = configurationFileName.ToRelativeFilePath().GetAbsolutePathFrom(Environment.CurrentDirectory.ToAbsoluteDirectoryPath());
				}
				if (this.ConfigurationFilePath.Exists == false)
					throw new System.IO.FileNotFoundException("ConfigurationFileName cannot be found", configurationFileName);
			}
			else
				throw new InvalidOperationException("ConfigurationFileName '{0}' is not a valid path.".FormatWith(configurationFileName));

			this.WriteHeader();

			this.WriteBeforeStart();
			this.Start(null);
			this.WriteAfterStart();

			this.WaitKeys(ConsoleKey.Escape, ConsoleKey.End);

			this.WriteBeforeStop();
			this.Stop(null);
			this.WriteAfterStop();

			return TopshelfExitCode.Ok;
		}

		protected virtual void WriteHeader()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Oragon Architcture Application Hosting " + this.GetType().Assembly.GetAssemblyInformationalVersion());
			Console.WriteLine("AssemblyVersion : " + this.GetType().Assembly.GetAssemblyVersion());
			Console.WriteLine("AssemblyFileVersion : " + this.GetType().Assembly.GetAssemblyFileVersion());
			Console.Write("Windows Service: ");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write(this.Name + " ");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("( ");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write(this.FriendlyName);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(" )");
			Console.Write("Description: ");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(this.Description);
			Console.WriteLine(string.Format("Start Timeout: {0}  Stop TimeOut: {1}", this.WindowsServiceConfiguration.StartTimeOut.ToString(), this.WindowsServiceConfiguration.StopTimeOut.ToString()));
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ResetColor();
		}

		protected virtual void WriteBeforeStart()
		{
		}
		protected virtual void WriteAfterStart()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Rodando... pressione a tecla 'ESC' tecla para finalizar...");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ResetColor();
		}

		protected virtual void WaitKeys(params ConsoleKey[] keys)
		{
			ConsoleKeyInfo keyInfo;
			do
			{
				keyInfo = Console.ReadKey();
			} while (keys.Length != 0 && keys.Contains(keyInfo.Key) == false);
		}

		protected virtual void WriteBeforeStop()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Finalizando...");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ResetColor();
		}
		protected virtual void WriteAfterStop()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Finalizado!! Pressione qualquer tecla para sair!");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ResetColor();
			for (int i = 1; i <= 60; i++)
			{
				System.Threading.Thread.Sleep(new TimeSpan(0, 0, 0, 0, 100));
				Console.Write("-");
			}
		}
	}
}
