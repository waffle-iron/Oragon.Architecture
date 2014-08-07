using System.Collections.Generic;

namespace Oragon.Architecture.ApplicationHosting.Management.ViewModel
{
	public class TreeItem
	{
		#region Public Properties

		public IEnumerable<TreeItem> children { get; set; }

		public bool expanded { get; set; }

		public string iconCls { get; set; }

		public string id { get; set; }

		public bool leaf { get; set; }

		public IEnumerable<MenuItem> menuItems { get; set; }

		public string text { get; set; }

		#endregion Public Properties
	}
}