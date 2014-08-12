namespace Oragon.Architecture.Data.ConnectionStrings
{
	public class ConfigurableConnectionStringDiscoverer : IConnectionStringDiscoverer
	{
		#region Public Properties

		public string Value { get; set; }

		public string Name { get; set; }

		public string ProviderName { get; set; }

		#endregion Public Properties

		#region Public Methods

		public System.Configuration.ConnectionStringSettings GetConnectionString()
		{
			return new System.Configuration.ConnectionStringSettings(this.Name, this.Value, this.ProviderName);
		}

		#endregion Public Methods
	}
}