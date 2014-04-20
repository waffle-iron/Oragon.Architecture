using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting
{
	public class Program
	{
		static void Main(string[] args)
		{
			string hostProcessPath = (new Uri(typeof(Program).Assembly.CodeBase)).LocalPath;
			ApplicationHost host = new ApplicationHost(hostProcessPath, args);
			host.Run();
		}
	}
}
