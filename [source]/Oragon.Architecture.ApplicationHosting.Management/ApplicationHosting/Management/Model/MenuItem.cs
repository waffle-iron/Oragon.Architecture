using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.Model
{
	public class MenuItem
	{
		public string text { get; set; }
		public string iconCls { get; set; }
		public string handlerFunction { get; set; }
		public ActionConfirmation actionConfirmation { get; set; }
		public bool disabled { get; set; }
	}
	
}