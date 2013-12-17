using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Merging
{
	public class MergeResult<T>
	{
		public List<T> ItemsToInsert { get; set; }
		public List<ItemToUpdate<T>> ItemsToUpdate { get; set; }
		public List<T> ItemsToDelete { get; set; }
	}
}
