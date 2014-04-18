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
using System.Collections.Generic;

namespace Oragon.Architecture.Services
{
	public static class ServiceProcessEntryPoint
	{
		public static void Run(string executable, params string[] args)
		{
			bool isDebug = (args != null && args.Contains("debug"));
			bool isConsole = (args != null && args.Contains("console"));

			if (isDebug)
				System.Diagnostics.Debugger.Launch();

			IApplicationContext applicationContext = null;
			Queue<string> paths = new Queue<string>();
			paths.Enqueue(executable + ".xml");
			paths.Enqueue("file://~/IoC.WindowsService.xml");
			RetryManager.Try(delegate
			{
				applicationContext = new Spring.Context.Support.XmlApplicationContext(paths.Dequeue());
			}, paths.Count, true, 10);

			//if (applicationContext == null)
			//{
			//	throw new System.IO.FileNotFoundException("XML de configuracao do servico windows nao foi encontrado nos paths '" + string.Join("','", paths.ToArray()) + "'");
			//}

			ServiceDescriptor serviceDescriptor = applicationContext.GetObject<ServiceDescriptor>();

			if (isConsole)
			{
				var serviceManager = applicationContext.GetObject<ServiceManager>("ServiceManager");
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Iniciando em modo console");
					Console.Write("Serviço: ");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Write(serviceDescriptor.Name + " ");
					Console.ForegroundColor = ConsoleColor.Red;
					Console.Write("( ");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.Write(serviceDescriptor.FriendlyName);
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine(" )");
					Console.Write("Descrição: ");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine(serviceDescriptor.Description);
					Console.WriteLine(string.Format("Timeout de Inicio: {0}  Timeout de Finaliação: {1}", serviceManager.StartTimeOut.Value.ToString(), serviceManager.StopTimeOut.Value.ToString()));
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ResetColor();
				}
				serviceManager.Start(null);
				{
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ForegroundColor = ConsoleColor.Red;
					Console.WriteLine("Rodando... pressione a tecla 'ESC' tecla para finalizar...");
					Console.ForegroundColor = ConsoleColor.Green;
					Console.WriteLine("#######################################################");
					Console.ResetColor();
				}
				ConsoleKeyInfo keyInfo;
				do
				{
					keyInfo = Console.ReadKey();
				} while (keyInfo.Key != ConsoleKey.Escape);
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
						serviceConfigurator.WhenStopped((serviceManagerInstance, hostControl) =>
						{
							var returnValue = serviceManagerInstance.Stop(hostControl);
							AppDomain.Unload(AppDomain.CurrentDomain);
							return returnValue;
						});
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
