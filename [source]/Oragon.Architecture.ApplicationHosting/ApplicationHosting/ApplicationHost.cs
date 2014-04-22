using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting
{
	public abstract class ApplicationHost
	{
		public string Name { get; set; }
		public string FriendlyName { get; set; }
		public string Description { get; set; }

		public abstract void Start(NDepend.Path.IAbsoluteDirectoryPath baseDirectory);

		public abstract void Stop();
	}
}
