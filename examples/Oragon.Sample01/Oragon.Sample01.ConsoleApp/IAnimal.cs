using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Sample01.ConsoleApp
{
	public interface IAnimal
	{
		string Nome { get; }
		void FazerBarulho();
	}
}
