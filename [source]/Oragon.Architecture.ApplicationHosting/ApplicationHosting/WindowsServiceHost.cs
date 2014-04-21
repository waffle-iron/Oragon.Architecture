using Spring.Objects.Factory.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Oragon.Architecture.ApplicationHosting
{
	public class WindowsServiceHost : ServiceControl
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string FriendlyName { get; set; }
		[Required]
		public string Description { get; set; }

		[Required]
		public WindowsServiceConfiguration WindowsServiceConfiguration { get; set; }

		[Required]
		public ApplicationHostingConfiguration ApplicationHostingConfiguration { get; set; }


		public void Configure(Topshelf.HostConfigurators.HostConfigurator hostConfig)
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
		}

		public bool Start(HostControl hostControl)
		{
			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			return true;
		}

		public void Shutdown(HostControl hostControl)
		{
			return;
		}

		internal void RunConsoleMode(List<string> arguments)
		{
			this.WriteHeader();

			this.WriteBeforeStart();
			this.Start(null);
			this.WriteAfterStart();
			this.WaitKeys(ConsoleKey.Escape);
			this.WriteBeforeStop();
			this.Stop(null);
			this.WriteAfterStop();

		}

		private void WriteHeader()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Iniciando em modo console");
			Console.Write("Serviço: ");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write(this.Name + " ");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.Write("( ");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.Write(this.FriendlyName);
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine(" )");
			Console.Write("Descrição: ");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine(this.Description);
			Console.WriteLine(string.Format("Timeout de Inicio: {0}  Timeout de Finaliação: {1}", this.WindowsServiceConfiguration.StartTimeOut.ToString(), this.WindowsServiceConfiguration.StopTimeOut.ToString()));
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ResetColor();
		}

		private void WriteBeforeStart()
		{
		}
		private void WriteAfterStart()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Rodando... pressione a tecla 'ESC' tecla para finalizar...");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ResetColor();
		}

		private void WaitKeys(params ConsoleKey[] keys)
		{
			ConsoleKeyInfo keyInfo;
			do
			{
				keyInfo = Console.ReadKey();
			} while (keys.Contains(keyInfo.Key) == false);
		}

		private void WriteBeforeStop()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Finalizando...");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ResetColor();
		}
		private void WriteAfterStop()
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine("Finalizado!! Pressione qualquer tecla para sair!");
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine("#######################################################");
			Console.ResetColor();
		}
	}
}
