using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Sample01.ConsoleApp
{
	public class Calopsita: IAnimal
	{
		public string Nome { get; set; }

		public void FazerBarulho()
		{
			Console.WriteLine("Eu sou {0} e estou piando", this.Nome);
		}
	}
}
