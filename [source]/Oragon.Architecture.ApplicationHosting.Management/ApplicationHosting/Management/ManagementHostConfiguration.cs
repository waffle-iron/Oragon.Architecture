using Spring.Objects.Factory.Attributes;

namespace Oragon.Architecture.ApplicationHosting.Management
{
	public class ManagementHostConfiguration
	{
		#region Public Properties

		[Required]
		public bool AllowRemoteMonitoring { get; set; }

		[Required]
		public int ManagementPort { get; set; }

		#endregion Public Properties
	}
}