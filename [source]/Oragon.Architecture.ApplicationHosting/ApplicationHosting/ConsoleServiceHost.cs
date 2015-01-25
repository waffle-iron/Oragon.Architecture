using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using Oragon.Architecture.Extensions;
using Oragon.Architecture.IO.Path;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Topshelf;

namespace Oragon.Architecture.ApplicationHosting
{
	public class ConsoleServiceHost : IHostProcessService
	{
		#region Private Fields

		private readonly Action<string> _green = (text) => { Console.ForegroundColor = ConsoleColor.Green; Console.Write(text); };

		private Microsoft.AspNet.SignalR.Client.IHubProxy _hostHubProxy;

		private Microsoft.AspNet.SignalR.Client.HubConnection _hubConnection;
		
		private readonly Action<string> _red = (text) => { Console.ForegroundColor = ConsoleColor.Red; Console.Write(text); };

		private readonly Action _set0X0 = () => Console.SetCursorPosition(0, 0);

		private readonly Action<int> _setLeft = (line) => Console.SetCursorPosition(0, line);

		private readonly Action<int> _setRight = (line) => Console.SetCursorPosition(Console.WindowWidth - 1, line);

		#endregion Private Fields

		#region Public Properties

		public List<ApplicationHost> Applications { get; set; }

		public Uri ApplicationServerEndPoint { get; set; }

		public Guid ClientId { get; set; }

		public IAbsoluteFilePath ConfigurationFilePath { get; protected set; }

		public string Description { get; set; }

		public string FriendlyName { get; set; }

		public string Name { get; set; }

		#endregion Public Properties

		#region Public Methods

		public TopshelfExitCode RunConsoleMode(List<string> arguments, string configurationFileName)
		{
			IFilePath filePath = null;
			if (configurationFileName.TryGetFilePath(out filePath))
			{
				if (filePath.IsAbsolutePath)
				{
					this.ConfigurationFilePath = configurationFileName.ToAbsoluteFilePath();
				}
				else if (filePath.IsRelativePath)
				{
					this.ConfigurationFilePath = configurationFileName.ToRelativeFilePath().GetAbsolutePathFrom(Environment.CurrentDirectory.ToAbsoluteDirectoryPath());
				}
				if (this.ConfigurationFilePath.Exists == false)
					throw new System.IO.FileNotFoundException("ConfigurationFileName cannot be found", configurationFileName);
			}
			else
				throw new InvalidOperationException("ConfigurationFileName '{0}' is not a valid path.".FormatWith(configurationFileName));

			this.WriteHeader();

			this.WriteBeforeStart();
			this.Start();
			this.WriteAfterStart();

			this.WaitKeys(ConsoleKey.Escape, ConsoleKey.End);

			this.WriteBeforeStop();
			this.Stop();
			this.WriteAfterStop();

			return TopshelfExitCode.Ok;
		}

		public virtual void Start()
		{
			Contract.Requires(this.Applications != null && this.Applications.Count > 0, "Invalid Application configuration, has no Application defined.");
			Contract.Requires(this.ConfigurationFilePath.Exists, "Configuration FilePath cannot be found in disk");

			var tmpApplicationList = new List<ApplicationHost>(this.Applications);
			foreach (var application in tmpApplicationList)
			{
				Oragon.Architecture.Threading.ThreadRunner.RunTask(() =>
					application.Start(this.ConfigurationFilePath.ParentDirectoryPath)
				);
			}
			this.ConnectToApplicationServer().Wait();
		}

		public virtual void Stop()
		{
			this.DisconnectFromApplicationServer().Wait();

			var tmpApplicationList = new List<ApplicationHost>(this.Applications);
			tmpApplicationList.Reverse();
			foreach (var application in tmpApplicationList)
			{
				application.Stop();
			}
		}

		#endregion Public Methods

		#region Application Server Integration

		private async Task ConnectToApplicationServer()
		{
			this._hubConnection = new Microsoft.AspNet.SignalR.Client.HubConnection(this.ApplicationServerEndPoint.ToString());
			this._hostHubProxy = _hubConnection.CreateHubProxy("HostHub");
			this._hubConnection.Start().Wait();

			var requestMessage = new RegisterHostRequestMessage()
			{
				MachineDescriptor = new MachineDescriptor()
				{
					IpAddressList = GetAllIpAddresses(),
					MachineName = Environment.MachineName
				},
				HostDescriptor = new HostDescriptor()
				{
					Pid = System.Diagnostics.Process.GetCurrentProcess().Id,
					Description = this.Description,
					FriendlyName = this.FriendlyName,
					Name = this.Name,
					Applications = this.Applications.ToList(it => it.ToDescriptor())
				}
			};

			RegisterHostResponseMessage responseMessage = await this._hostHubProxy.Invoke<RegisterHostResponseMessage>("RegisterHost", requestMessage);
			this.ClientId = responseMessage.ClientId;
		}

		private async Task DisconnectFromApplicationServer()
		{
			var requestMessage = new UnregisterHostRequestMessage()
			{
				ClientId = this.ClientId
			};
			UnregisterHostResponseMessage responseMessage = await this._hostHubProxy.Invoke<UnregisterHostResponseMessage>("UnregisterHost", requestMessage);
			this._hubConnection.Stop();
		}

		#endregion Application Server Integration

		#region Console Management

		protected virtual void WaitKeys(params ConsoleKey[] keys)
		{
			ConsoleKeyInfo keyInfo;
			do
			{
				Console.Write("Press 'ESC' or 'END' keys to stop...");
				Console.ResetColor();
				Console.ForegroundColor = Console.BackgroundColor;
				keyInfo = Console.ReadKey();
				Console.WriteLine(string.Empty);
				Console.ResetColor();
			} while (keys.Length != 0 && keys.Contains(keyInfo.Key) == false);
		}

		protected virtual void WriteAfterStart()
		{
			Console.WriteLine("Running!");
			Console.ResetColor();
		}

		protected virtual void WriteAfterStop()
		{
			Console.WriteLine("All itens of pipeline are stopped!");
			Console.WriteLine("See you late!");
			Console.ResetColor();
			for (int i = 1; i <= 15; i++)
			{
				System.Threading.Thread.Sleep(new TimeSpan(0, 0, 0, 0, 100));
				Console.Write(".");
			}
		}

		protected virtual void WriteBeforeStart()
		{
		}

		protected virtual void WriteBeforeStop()
		{
			Console.WriteLine("Stoping...");
			Console.ResetColor();
		}

		protected virtual void WriteHeader()
		{
			var asciiArt = new Queue<string>();

			for (int i = 0; i < Console.WindowHeight; i++)
			{
				asciiArt.Enqueue(string.Empty.PadRight(Console.WindowWidth - 1, ' '));
			}
			asciiArt.Enqueue(@"																                                 ");
			asciiArt.Enqueue(@" ██████╗ ██████╗  █████╗  ██████╗  ██████╗ ███╗   ██╗                                         ");
			asciiArt.Enqueue(@"██╔═══██╗██╔══██╗██╔══██╗██╔════╝ ██╔═══██╗████╗  ██║                                         ");
			asciiArt.Enqueue(@"██║   ██║██████╔╝███████║██║  ███╗██║   ██║██╔██╗ ██║                                         ");
			asciiArt.Enqueue(@"██║   ██║██╔══██╗██╔══██║██║   ██║██║   ██║██║╚██╗██║                                         ");
			asciiArt.Enqueue(@"╚██████╔╝██║  ██║██║  ██║╚██████╔╝╚██████╔╝██║ ╚████║                                         ");
			asciiArt.Enqueue(@" ╚═════╝ ╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝  ╚═════╝ ╚═╝  ╚═══╝                                         ");
			asciiArt.Enqueue(@"                                                                                              ");
			asciiArt.Enqueue(@" █████╗ ██████╗  ██████╗██╗  ██╗██╗████████╗███████╗ ██████╗████████╗██╗   ██╗██████╗ ███████╗");
			asciiArt.Enqueue(@"██╔══██╗██╔══██╗██╔════╝██║  ██║██║╚══██╔══╝██╔════╝██╔════╝╚══██╔══╝██║   ██║██╔══██╗██╔════╝");
			asciiArt.Enqueue(@"███████║██████╔╝██║     ███████║██║   ██║   █████╗  ██║        ██║   ██║   ██║██████╔╝█████╗  ");
			asciiArt.Enqueue(@"██╔══██║██╔══██╗██║     ██╔══██║██║   ██║   ██╔══╝  ██║        ██║   ██║   ██║██╔══██╗██╔══╝  ");
			asciiArt.Enqueue(@"██║  ██║██║  ██║╚██████╗██║  ██║██║   ██║   ███████╗╚██████╗   ██║   ╚██████╔╝██║  ██║███████╗");
			asciiArt.Enqueue(@"╚═╝  ╚═╝╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚═╝   ╚═╝   ╚══════╝ ╚═════╝   ╚═╝    ╚═════╝ ╚═╝  ╚═╝╚══════╝");
			asciiArt.Enqueue(@"                                                                                              ");
			asciiArt.Enqueue(@" █████╗ ██████╗ ██████╗ ██╗     ██╗ ██████╗ █████╗ ████████╗██╗ ██████╗ ███╗   ██╗            ");
			asciiArt.Enqueue(@"██╔══██╗██╔══██╗██╔══██╗██║     ██║██╔════╝██╔══██╗╚══██╔══╝██║██╔═══██╗████╗  ██║            ");
			asciiArt.Enqueue(@"███████║██████╔╝██████╔╝██║     ██║██║     ███████║   ██║   ██║██║   ██║██╔██╗ ██║            ");
			asciiArt.Enqueue(@"██╔══██║██╔═══╝ ██╔═══╝ ██║     ██║██║     ██╔══██║   ██║   ██║██║   ██║██║╚██╗██║            ");
			asciiArt.Enqueue(@"██║  ██║██║     ██║     ███████╗██║╚██████╗██║  ██║   ██║   ██║╚██████╔╝██║ ╚████║            ");
			asciiArt.Enqueue(@"╚═╝  ╚═╝╚═╝     ╚═╝     ╚══════╝╚═╝ ╚═════╝╚═╝  ╚═╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝            ");
			asciiArt.Enqueue(@"                                                                                              ");
			asciiArt.Enqueue(@"███████╗███████╗██████╗ ██╗   ██╗███████╗██████╗                                              ");
			asciiArt.Enqueue(@"██╔════╝██╔════╝██╔══██╗██║   ██║██╔════╝██╔══██╗                                             ");
			asciiArt.Enqueue(@"███████╗█████╗  ██████╔╝██║   ██║█████╗  ██████╔╝                                             ");
			asciiArt.Enqueue(@"╚════██║██╔══╝  ██╔══██╗╚██╗ ██╔╝██╔══╝  ██╔══██╗                                             ");
			asciiArt.Enqueue(@"███████║███████╗██║  ██║ ╚████╔╝ ███████╗██║  ██║                                             ");
			asciiArt.Enqueue(@"╚══════╝╚══════╝╚═╝  ╚═╝  ╚═══╝  ╚══════╝╚═╝  ╚═╝                                             ");
			for (int i = 0; i < Console.WindowHeight; i++)
			{
				asciiArt.Enqueue(string.Empty.PadRight(Console.WindowWidth - 1, ' '));
			}
			while (asciiArt.Count > 0)
			{
				string[] array = asciiArt.ToArray();
				asciiArt.Dequeue();
				for (int i = 0; i < array.Length && i < Console.WindowHeight; i++)
				{
					Console.WriteLine(array[i].PadRight(Console.WindowWidth - 1, ' '));
				}
				_set0X0();
				System.Threading.Thread.Sleep(new TimeSpan(0, 0, 0, 0, 15));
			}
			_set0X0();
			const string headerText = "Oragon Architecture Application Hosting";

			Console.BackgroundColor = ConsoleColor.DarkGray;
			Console.ForegroundColor = ConsoleColor.Black;
			for (int i = 0; i < Console.WindowWidth; i++)
				Console.Write(" ");

			var position = Math.Abs((Console.WindowWidth - headerText.Length) / 2);
			Console.SetCursorPosition(position, 0);
			Console.Write(headerText);

			const int headerSize = 5;
			for (int i = 1; i < headerSize + 1; i++)
			{
				_setLeft(i);
				Console.Write(" ");
				_setRight(i);
				Console.Write(" ");
			}
			_setLeft(headerSize + 1);
			for (int i = 0; i < Console.WindowWidth; i++)
				Console.Write(" ");

			Console.ResetColor();
			Console.ForegroundColor = ConsoleColor.Red;
			Console.SetCursorPosition(1, 1); _red("Version: "); _green(this.GetType().Assembly.GetAssemblyInformationalVersion());
			Console.SetCursorPosition(1, 2); _red("AssemblyFileVersion: "); _green(this.GetType().Assembly.GetAssemblyFileVersion());
			Console.SetCursorPosition(1, 3); _red("ServiceName: "); _green(this.Name);
			Console.SetCursorPosition(1, 4); _red("FriendlyName: "); _green(this.FriendlyName);
			Console.SetCursorPosition(1, 5); _red("Description: "); _green(this.Description);
			Console.ResetColor();

			_setLeft(headerSize + 2);
		}

		#endregion Console Management

		#region Private Methods

		private static List<string> GetAllIpAddresses()
		{
			IEnumerable<string> iplist = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList.Select(it => it.ToString());
			iplist = iplist.Where(it => it.Contains(":") == false).ToArray(); //Only IPV4 IP`s
			return iplist.ToList();
		}

		#endregion Private Methods

		#region IHostProcessService Methods

		public virtual void AddApplication()
		{
			throw new NotImplementedException();
		}

		public HostStatistic CollectStatistics()
		{
			var tmpApplicationList = new List<ApplicationHost>(this.Applications);

			var returnValue = new HostStatistic()
			{
				ApplicationStatistics = new List<ApplicationStatistic>()
			};

			foreach (var application in tmpApplicationList)
			{
				returnValue.ApplicationStatistics.Add(new ApplicationStatistic()
				{
					AppDomainStatistic = application.GetAppDomainStatistics(),
					ApplicationDescriptor = application.ToDescriptor()
				});
			}
			return returnValue;
		}

		public virtual void HeartBeat()
		{
			System.Diagnostics.Debug.WriteLine("{0} receive heartbeat ping.".FormatWith(this.Name));
		}

		public virtual void StartApplication()
		{
			throw new NotImplementedException();
		}

		public virtual void StopApplication()
		{
			throw new NotImplementedException();
		}

		#endregion IHostProcessService Methods
	}
}