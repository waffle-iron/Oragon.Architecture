using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Merging
{
	public class ItemToUpdate<T>
	{
		public T Original { get; set; }
		public T Modified { get; set; }
	}
}
