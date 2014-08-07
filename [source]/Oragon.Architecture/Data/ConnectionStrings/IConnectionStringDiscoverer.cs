using System.Configuration;

namespace Oragon.Architecture.Data.ConnectionStrings
{
	public interface IConnectionStringDiscoverer
	{
		#region Public Methods

		ConnectionStringSettings GetConnectionString();

		#endregion Public Methods
	}
}