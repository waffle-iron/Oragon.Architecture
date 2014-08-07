using System.Net;
using System.Net.Sockets;

namespace Oragon.Architecture.Networking
{
	public static class DynamicPort
	{
		#region Public Methods

		public static int GetFreePort()
		{
			TcpListener listener = new TcpListener(IPAddress.Loopback, 0);
			listener.Start();
			int port = ((IPEndPoint)listener.LocalEndpoint).Port;
			listener.Stop();
			return port;
		}

		#endregion Public Methods
	}
}