using Oragon.Architecture.Samples02.Data.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Samples02.Data
{
	class Program
	{
		static void Main(string[] args)
		{
			Spring.Context.IApplicationContext appContext = Spring.Context.Support.ContextRegistry.GetContext();
			ITestService testeService = appContext.GetObject<ITestService>("TestService");
			testeService.TestarAssociacaoAlunosXTurmas();
		}
	}
}
