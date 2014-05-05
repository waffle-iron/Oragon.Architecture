//using Oragon.Architecture.ApplicationHosting.Model;
//using Oragon.Architecture.ApplicationHosting.Services.Contracts;
//using Oragon.Architecture.Extensions;
//using System;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Net.Http.Formatting;
//using System.Net.Http.Headers;
//using System.Text;
//using System.Threading.Tasks;

//namespace Oragon.Architecture.ApplicationHosting
//{
//	public class ClientApiProxy
//	{
//		public Uri EndPointUri { get; private set; }

//		public ClientApiProxy(Uri endPointUri)
//		{
//			this.EndPointUri = endPointUri;
//		}

//		private HttpClient BuildRequest()
//		{
//			var client = new HttpClient();
//			client.BaseAddress = this.EndPointUri;
//			client.DefaultRequestHeaders.Accept.Clear();
//			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
//			return client;
//		}

//		private List<MediaTypeFormatter> GetFormatters()
//		{
//			var formatters = new List<MediaTypeFormatter>() {
//				new JsonMediaTypeFormatter(),
//				new XmlMediaTypeFormatter()
//			};
//			return formatters;
//		}

//		private List<string> GetAllIPAddresses()
//		{
//			IEnumerable<string> iplist = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList.Select(it => it.ToString());
//			iplist = iplist.Where(it => it.Contains(":") == false).ToArray(); //Only IPV4 IP`s
//			return iplist.ToList();
//		}

//		public async Task<Guid> RegisterHost(ConsoleServiceHost windowsServiceHost)
//		{
//			HostDescriptor hostDescriptor = new HostDescriptor()
//			{
//				ID = windowsServiceHost.ClientID,
//				PID = System.Diagnostics.Process.GetCurrentProcess().Id,
//				Description = windowsServiceHost.Description,
//				FriendlyName = windowsServiceHost.FriendlyName,
//				Name = windowsServiceHost.Name,
//				MachineName = Environment.MachineName,
//				IPAddressList = this.GetAllIPAddresses(),
//				Applications = windowsServiceHost.Applications.Select(it =>
//					new ApplicationDescriptor()
//					{
//						Name = it.Name,
//						FriendlyName = it.FriendlyName,
//						Description = it.Description,
//						FactoryType = it.FactoryType,
//						ApplicationConfigurationFile = it.ApplicationConfigurationFile,
//						ApplicationBaseDirectory = it.ApplicationBaseDirectory
//					}
//				).ToList()
//			};

//			using (HttpClient client = this.BuildRequest())
//			{
//				HttpResponseMessage response = await client.PostAsJsonAsync("api/RegisterService/Register", hostDescriptor);
//				if (response.IsSuccessStatusCode)
//				{
//					Guid clientGUID = await response.Content.ReadAsAsync<Guid>(this.GetFormatters());
//					return clientGUID;
//				}
//			}
//			return Guid.Empty;
//		}


//		internal async Task<int> UnregisterHost(System.Guid id)
//		{
//			HostDescriptor hostDescriptor = new HostDescriptor()
//			{
//				ID = id
//			};

//			using (HttpClient client = this.BuildRequest())
//			{
//				HttpResponseMessage response = await client.PostAsJsonAsync("api/RegisterService/Unregister", hostDescriptor);
//				if (response.IsSuccessStatusCode)
//				{
//					return 0;
//				}
//			}
//			return -1;
//		}
//	}
//}
