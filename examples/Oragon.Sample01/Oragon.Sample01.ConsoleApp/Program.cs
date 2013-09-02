using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Sample01.ConsoleApp
{
	class Program
	{
		static void Main(string[] args)
		{
			Spring.Context.IApplicationContext context = Spring.Context.Support.ContextRegistry.GetContext();
			Pessoa pessoa = context.GetObject<Pessoa>("Teste1");
			Console.WriteLine("Eu sou {0}", pessoa.Nome); 
			foreach(IAnimal animal in pessoa.Animais)
			{
				Console.WriteLine("Eu tenho um(a) {0} que se chama {1}", animal.GetType().Name, animal.Nome); 
				animal.FazerBarulho();
			}
		}
	}
}
