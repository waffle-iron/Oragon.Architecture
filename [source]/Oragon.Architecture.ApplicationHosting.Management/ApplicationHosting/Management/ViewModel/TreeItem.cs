using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.ViewModel
{
	public class TreeItem
	{
		public string text { get; set; }
		public string iconCls { get; set; }
		public bool leaf { get; set; }
		public bool expanded { get; set; }
		public IEnumerable<TreeItem> children { get; set; }

		public IEnumerable<MenuItem> menuItems { get; set; }

		public string id { get; set; }
	}
}
