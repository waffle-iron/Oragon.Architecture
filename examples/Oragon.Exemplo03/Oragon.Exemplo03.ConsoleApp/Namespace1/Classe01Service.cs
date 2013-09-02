using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Exemplo03.ConsoleApp.Namespace1
{
	public class Classe01Service: IClasse01Service
	{
		public void Testar01()
		{
			Console.WriteLine("Classe01Service.Testar01");
		}

		public void Testar02()
		{
			Console.WriteLine("Classe01Service.Testar02");
			throw new Exception("Teste de Exception");
		}

		public string Teste03()
		{
			Console.WriteLine("Classe01Service.Teste03");
			return DateTime.Now.ToShortDateString();
		}
		
	}
}
