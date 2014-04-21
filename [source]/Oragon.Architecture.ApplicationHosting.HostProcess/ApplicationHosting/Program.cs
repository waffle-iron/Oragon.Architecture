using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;

namespace Oragon.Architecture.ApplicationHosting
{
	public class Program
	{
		public static void Main(string[] args)
		{
			ApplicationHost host = new ApplicationHost(args);
			TopshelfExitCode exitCode = host.Run();
			System.Environment.Exit((int)exitCode);
		}
	}
}
