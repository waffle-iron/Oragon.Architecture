using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.Architecture.Log;


namespace Oragon.Architecture.LogServer.Host.TestClient
{
	class Program
	{
		private volatile static int QtdProcessados;

		static void Main(string[] args)
		{
			Console.ForegroundColor = ConsoleColor.Gray;
			Console.WriteLine("#######################################################");
			Console.WriteLine("Cliente de Log");
			Console.WriteLine("#######################################################");


			Spring.Context.IApplicationContext appContext = Spring.Context.Support.ContextRegistry.GetContext();
			ILogger logEngineClient = (ILogger)appContext.GetObject("Logger");

			//System.Timers.Timer timer = new System.Timers.Timer(1000 * 10);
			//timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
			//timer.Start();
			int qtd = 500;
			for (int i = 1; i <= qtd; i++)
			{
				if (i % 2 == 0)
					logEngineClient.Error("Program.Main", "Log Error " + DateTime.Now.ToString(), "Tipo", "ConsoleApplication", "Login", "usuario0" + i.ToString());
				else
					logEngineClient.Audit("Program.Main", "Log Error " + DateTime.Now.ToString(), "Tipo", "ConsoleApplication", "Login", "usuario0" + i.ToString());
				QtdProcessados++;
			}
			//timer.Stop();
			Console.WriteLine("Foram enviadas {0} mensagens para a fila!", qtd);
			Console.ReadKey();
		}

		static int valorAntigo;
		static void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
		{
			int valorAtual = QtdProcessados;
			int delta = (valorAtual - valorAntigo);
			valorAntigo = valorAtual;
			Console.WriteLine("Foram enviadas mais {1} mensagens para a fila, ao todo foram enviados {0} itens.", valorAtual, delta);
		}
	}
}
