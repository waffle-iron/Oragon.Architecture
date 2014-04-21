using Spring.Objects.Factory.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting
{
	public abstract class ApplicationHost
	{
		[Required]
		public string Name { get; set; }
		[Required]
		public string FriendlyName { get; set; }
		[Required]
		public string Description { get; set; }

		public abstract void Start(NDepend.Path.IAbsoluteDirectoryPath baseDirectory);

		public abstract void Stop();
	}
}
