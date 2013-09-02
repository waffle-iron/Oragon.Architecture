using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Sample01.ConsoleApp
{
	public class Pessoa
	{
		public string Nome { get; set; }
		public int Idade { get; set; }
		public List<IAnimal> Animais { get; set; }
	}
}
