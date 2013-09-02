using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using CommandLine.Text;
using System.ServiceProcess; // if you want text formatting helpers (recommended)

namespace Oragon.JenkinsTasks.WindowsServices
{
	public class Program
	{
		private static readonly HeadingInfo HeadingInfo = new HeadingInfo("JenkinsTasks.WindowsServices", "1.0");

		static void Main(string[] args)
		{

			var options = new Options();
			var parser = new CommandLine.Parser(with => with.HelpWriter = Console.Error);
			if (parser.ParseArgumentsStrict(args, options, () => { Console.ReadKey(); Environment.Exit(-2); }))
			{
				Run(options);
			}

		}

		private static void Run(Options options)
		{
			bool[] itens = new bool[] { options.start, options.stop, options.exists };
			if (itens.Where(it => it).Count() != 1)
			{
				HeadingInfo.WriteMessage(string.Format("Somente uma operaao START|STOP|EXISTS"));
				Environment.Exit(-1);
			}
			if (options.exists)
				HandleExists(options);
			else if (options.start)
				HandleStart(options);
			else if (options.stop)
				HandleStop(options);

			Console.ReadKey();
		}

		private static void HandleStart(Options options)
		{
			ServiceController service = GetServiceByName(options.ServiceName);
			if (service != null)
			{
				HeadingInfo.WriteMessage(string.Format("Iniciando o servico '{0}'", options.ServiceName));
				Do(options, delegate { service.Start(); }, service, ServiceControllerStatus.Stopped, ServiceControllerStatus.Running);
			}
			else if (options.ifExists)
				Environment.Exit(0);
			else
				Environment.Exit(1);
		}

		private static void HandleStop(Options options)
		{
			ServiceController service = GetServiceByName(options.ServiceName);
			if (service != null)
			{
				HeadingInfo.WriteMessage(string.Format("Parando o servico '{0}'", options.ServiceName));
				Do(options, delegate { service.Stop(); }, service, ServiceControllerStatus.Running, ServiceControllerStatus.Stopped);
			}
			else if (options.ifExists)
				Environment.Exit(0);
			else
				Environment.Exit(1);
		}

		private static void Do(Options options, Action operationAction, ServiceController service, ServiceControllerStatus expectedStartStatus, ServiceControllerStatus expectedFinalStatus)
		{
			if (service.Status == expectedStartStatus)
			{
				try
				{
					operationAction();
					if (options.Timeout > default(int))
					{
						DateTime startTime = DateTime.Now;
						DateTime startWaitTo = startTime.AddSeconds(Convert.ToDouble(options.Timeout));
						while (DateTime.Now < startWaitTo && service.Status != expectedFinalStatus)
						{
							HeadingInfo.WriteMessage(string.Format("Servico {0} possui status {1}. Aguarde...", service.ServiceName, service.Status.ToString()));
							System.Threading.Thread.Sleep(new TimeSpan(0, 0, 2));
							service.Refresh();
						}
						if (service.Status != expectedFinalStatus)
						{
							HeadingInfo.WriteMessage(string.Format("Tempo excedido, o processo falhou, o servico {0} possui status {1}.", service.ServiceName, service.Status.ToString()));
							Environment.Exit(2);
						}
						else
						{
							HeadingInfo.WriteMessage(string.Format("Sucesso, o servico {0} possui status {1}.", service.ServiceName, service.Status.ToString()));
							Environment.Exit(0);
						}
					}
				}
				catch (Exception)
				{
					throw;
				}
				Environment.Exit(0);
			}
			else if (service.Status == expectedFinalStatus)
			{
				HeadingInfo.WriteMessage(string.Format("O servico '{0}' ja esta com estado '{1}'.", options.ServiceName, service.Status.ToString()));
				Environment.Exit(0);
			}
			else
			{
				HeadingInfo.WriteMessage(string.Format("Status invalido para iniciar servico.", options.ServiceName));
				Environment.Exit(2);
			}
		}

		private static void HandleExists(Options options)
		{
			ServiceController service = GetServiceByName(options.ServiceName);
			if (service != null)
				Environment.Exit(0);
			else
				Environment.Exit(1);
		}

		private static ServiceController GetServiceByName(string serviceName)
		{
			HeadingInfo.WriteMessage(string.Format("Verificando se servico '{0}' existe.", serviceName));
			ServiceController returnValue = ServiceController.GetServices().Where(it => it.ServiceName == serviceName).FirstOrDefault();
			if (returnValue == null)
				HeadingInfo.WriteMessage(string.Format("O servico servico '{0}' nao esta instalado.", serviceName));
			else
				HeadingInfo.WriteMessage(string.Format("O servico servico '{0}' esta instalado e seu status e {1}", serviceName, returnValue.Status.ToString()));
			return returnValue;
		}

	}
}
