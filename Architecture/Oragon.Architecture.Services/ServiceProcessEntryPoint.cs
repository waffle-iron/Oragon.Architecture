using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using NLog;
using Oragon.Architecture.Services.Descriptor;
using Topshelf;
using Topshelf.HostConfigurators;
using Topshelf.Configurators;
using Topshelf.Logging;
using Topshelf.Options;
using Topshelf.Runtime;
using Topshelf.ServiceConfigurators;
using ContextRegistry = Spring.Context.Support.ContextRegistry;
using IApplicationContext = Spring.Context.IApplicationContext;

namespace Oragon.Architecture.Services
{
	public static class ServiceProcessEntryPoint
	{
		public static void Run(params string[] args)
		{
			bool isDebug = (args != null && args.Contains("debug"));
			bool isConsole = (args != null && args.Contains("console"));

			if (isDebug)
				System.Diagnostics.Debugger.Launch();

			IApplicationContext applicationContext = new Spring.Context.Support.XmlApplicationContext("file://~/IoC.WindowsService.xml");
			ServiceDescriptor serviceDescriptor = applicationContext.GetObject<ServiceDescriptor>();

			if (isConsole)
			{
				var serviceManager = applicationContext.GetObject<ServiceManager>("ServiceManager");
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Iniciando em modo console");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ResetColor();
				}
				serviceManager.Start(null);
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Rodando... pressione qualquer tecla para finalizar...");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ResetColor();
				}
				Console.ReadKey();
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Finalizando...");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ResetColor();
				}
				serviceManager.Stop(null);
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
			else
			{
				TopshelfExitCode exitCode = HostFactory.Run(hostConfig =>
				{
					hostConfig.UseNLog();
					hostConfig.Service<Oragon.Architecture.Services.ServiceManager>(serviceConfigurator =>
					{
						serviceConfigurator.ConstructUsing(() => applicationContext.GetObject<Oragon.Architecture.Services.ServiceManager>());
						serviceConfigurator.WhenStarted((serviceManagerInstance, hostControl) => serviceManagerInstance.Start(hostControl));
						serviceConfigurator.WhenStopped((serviceManagerInstance, hostControl) => serviceManagerInstance.Stop(hostControl));
					});

					if (serviceDescriptor.Dependences != null)
					{
						foreach (string dependency in serviceDescriptor.Dependences)
						{
							hostConfig.AddDependency(dependency);
						}
					}
					switch (serviceDescriptor.StartMode)
					{
						case StartMode.Automatically: hostConfig.StartAutomatically(); break;
						case StartMode.AutomaticallyDelayed: hostConfig.StartAutomaticallyDelayed(); break;
						case StartMode.Disabled: hostConfig.Disabled(); break;
						case StartMode.Manually: hostConfig.StartManually(); break;
					}
					switch (serviceDescriptor.IdentityType)
					{
						case AccountType.LocalService: hostConfig.RunAsLocalService(); break;
						case AccountType.LocalSystem: hostConfig.RunAsLocalSystem(); break;
						case AccountType.NetworkService: hostConfig.RunAsNetworkService(); break;
						case AccountType.Prompt: hostConfig.RunAsPrompt(); break;
						case AccountType.Custom: hostConfig.RunAs(serviceDescriptor.CustomIdentityCredential.Username, serviceDescriptor.CustomIdentityCredential.Password); break;
					}
					hostConfig.SetServiceName(serviceDescriptor.Name);
					hostConfig.SetDisplayName(serviceDescriptor.FriendlyName);
					hostConfig.SetDescription(serviceDescriptor.Description);
				});
			}


		}








	}
}
