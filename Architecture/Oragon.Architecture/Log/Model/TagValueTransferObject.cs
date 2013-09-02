using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.Log.Model
{
	public class TagValueTransferObject
	{
		public long TagValueID { get; set; }
		public long TagID { get; set; }
		public string Value { get; set; }
	}
}
