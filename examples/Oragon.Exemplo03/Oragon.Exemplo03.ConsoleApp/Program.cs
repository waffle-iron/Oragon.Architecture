using Oragon.Exemplo03.ConsoleApp.Namespace1;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Exemplo03.ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Spring.Context.IApplicationContext context = Spring.Context.Support.ContextRegistry.GetContext();
			IClasse01Service classe01Service = context.GetObject<IClasse01Service>("Classe01Service_Instance01");

			string valor = classe01Service.Teste03();
		}
	}
}
