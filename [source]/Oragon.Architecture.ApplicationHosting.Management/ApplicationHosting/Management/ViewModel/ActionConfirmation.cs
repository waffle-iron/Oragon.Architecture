using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.Management.ViewModel
{
	public class ActionConfirmation
	{
		public string title { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public MessageBoxButtons buttons { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public MessageBoxIcon icon { get; set; }

		public string text { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public MessageBoxButton performActionOnClickIn { get; set; }


		public ActionConfirmation(string title, MessageBoxButtons buttons, MessageBoxIcon icon, string text, MessageBoxButton performActionOnClickIn)
		{
			this.title = title;
			this.buttons = buttons;
			this.icon = icon;
			this.text = text;
			this.performActionOnClickIn = performActionOnClickIn;
		}
	}
}
