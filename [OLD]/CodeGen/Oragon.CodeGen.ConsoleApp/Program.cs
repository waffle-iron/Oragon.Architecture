using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.CodeGen.Templating;

namespace Oragon.CodeGen.ConsoleApp
{
	class Program
	{
		volatile static bool canclose = false;

		static void Main(string[] args)
		{
			bool isDebug = (args != null && args.Contains("/debug"));
			bool isConsole = (args != null && args.Contains("/console"));

			if(isDebug)
				System.Diagnostics.Debugger.Launch();

			Spring.Context.IApplicationContext springContext = Spring.Context.Support.ContextRegistry.GetContext();
			GenerationContainer generationContainer = (GenerationContainer)springContext.GetObject("Main");
			if (isConsole)
			{
				Console.WriteLine("Oragon CodeGen App - Executando em modo console");
				Program.canclose = false;
				generationContainer.EndOfProcess += new EventHandler(generationContainer_EndOfProcess);
				foreach (T4Template template in generationContainer.Templates)
				{
					template.GenerationStart += new Action<T4Template>(template_GenerationStart);
					template.GenerationError += new Action<T4Template, Exception>(template_GenerationError);
					template.GenerationSucess += new Action<T4Template>(template_GenerationSucess);
				}
				Console.WriteLine("Cleanup");
				generationContainer.CleanUp();
				Console.WriteLine("Iniciando Execução");
				generationContainer.Run();
				Console.WriteLine("Executando...");
				while (Program.canclose == false)
				{
					//System.Threading.Thread.Sleep(250);
				}
				Console.WriteLine("Fechando...");
			}
			else
			{
				MainForm mainForm = new MainForm(generationContainer);
				mainForm.ShowDialog();
			}
		}

		static void template_GenerationSucess(T4Template template)
		{
			Console.WriteLine("Template processado com sucesso. Output em {0}", template.OutputPath);
		}

		static void template_GenerationError(T4Template template, Exception exception)
		{
			Console.WriteLine("Erro ao executar template {0}", exception.ToString());
		}

		static void template_GenerationStart(T4Template template)
		{
			Console.WriteLine("Iniciando Execução do Template '{0}'", template.TemplatePath);
		}

		static void generationContainer_EndOfProcess(object sender, EventArgs e)
		{
			Console.WriteLine("Finalizado");
			Program.canclose = true;
		}
	}
}
