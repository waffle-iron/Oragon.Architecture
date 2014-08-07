using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Oragon.Architecture.ApplicationHosting.Management.ViewModel
{
	public class ActionConfirmation
	{
		#region Public Constructors

		public ActionConfirmation(string title, MessageBoxButtons buttons, MessageBoxIcon icon, string text, MessageBoxButton performActionOnClickIn)
		{
			this.title = title;
			this.buttons = buttons;
			this.icon = icon;
			this.text = text;
			this.performActionOnClickIn = performActionOnClickIn;
		}

		#endregion Public Constructors

		#region Public Properties

		[JsonConverter(typeof(StringEnumConverter))]
		public MessageBoxButtons buttons { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public MessageBoxIcon icon { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public MessageBoxButton performActionOnClickIn { get; set; }

		public string text { get; set; }

		public string title { get; set; }

		#endregion Public Properties
	}
}