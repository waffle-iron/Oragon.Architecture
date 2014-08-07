namespace Oragon.Architecture.ApplicationHosting.Management.ViewModel
{
	public class MenuItem
	{
		#region Public Properties

		public ActionConfirmation actionConfirmation { get; set; }

		public string actionRoute { get; set; }

		public bool disabled { get; set; }

		public string iconCls { get; set; }

		public string text { get; set; }

		#endregion Public Properties
	}
}