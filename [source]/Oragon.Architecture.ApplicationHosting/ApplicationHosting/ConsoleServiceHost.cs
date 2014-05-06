﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Topshelf;
using Oragon.Architecture.Extensions;
using NDepend.Helpers;
using NDepend.Path;
using System.Diagnostics.Contracts;
using Oragon.Architecture.ApplicationHosting.Services.Contracts;

namespace Oragon.Architecture.ApplicationHosting
{
	public class ConsoleServiceHost
	{
		Action<string> Green = (text) => { Console.ForegroundColor = ConsoleColor.Green; Console.Write(text); };

		Action<string> Red = (text) => { Console.ForegroundColor = ConsoleColor.Red; Console.Write(text); };

		Action Set0x0 = () => { Console.SetCursorPosition(0, 0); };

		Action<int> SetLeft = (line) => { Console.SetCursorPosition(0, line); };

		Action<int> SetRight = (line) => { Console.SetCursorPosition(Console.WindowWidth - 1, line); };

		public List<ApplicationHost> Applications { get; set; }

		public IApplicationServerService ApplicationServer { get; set; }

		public Guid ClientID { get; set; }

		public IAbsoluteFilePath ConfigurationFilePath { get; protected set; }

		public string Description { get; set; }

		public string FriendlyName { get; set; }

		public Uri MonitoringEndPoint { get; set; }

		public string Name { get; set; }
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

			List<ApplicationHost> tmpApplicationList = new List<ApplicationHost>(this.Applications);
			foreach (var application in tmpApplicationList)
			{
				application.Start(this.ConfigurationFilePath.ParentDirectoryPath);
			}

			if (this.ApplicationServer == null)
			{
				var registerHostRequestMessage = new RegisterHostRequestMessage()
				{
					MachineDescriptor = new MachineDescriptor()
					{
						IPAddressList = this.GetAllIPAddresses(),
						MachineName = Environment.MachineName
					},
					HostDescriptor = new HostDescriptor() {
						PID = System.Diagnostics.Process.GetCurrentProcess().Id,
						Description = this.Description,
						FriendlyName = this.FriendlyName,
						Name = this.Name,
						Applications = this.Applications.ToList(it =>
							new ApplicationDescriptor()
							{
								Name = it.Name,
								FriendlyName = it.FriendlyName,
								Description = it.Description,
								FactoryType = it.FactoryType,
								ApplicationConfigurationFile = it.ApplicationConfigurationFile,
								ApplicationBaseDirectory = it.ApplicationBaseDirectory								
								
							}
						).ToList()					
					}
				};
				RegisterHostResponseMessage registerHostResponseMessage =  this.ApplicationServer.RegisterHost(registerHostRequestMessage);
				this.ClientID = registerHostResponseMessage.ClientID;
			}
		}


		public virtual void Stop()
		{
			if (this.MonitoringEndPoint != null)
			{
				ClientApiProxy proxy = new ClientApiProxy(this.MonitoringEndPoint);
				proxy.UnregisterHost(this.ClientID);
			}

			List<ApplicationHost> tmpApplicationList = new List<ApplicationHost>(this.Applications);
			tmpApplicationList.Reverse();
			foreach (var application in tmpApplicationList)
			{
				application.Stop();
			}
		}

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
			Queue<string> asciiArt = new Queue<string>();

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
				Set0x0();
				System.Threading.Thread.Sleep(new TimeSpan(0, 0, 0, 0, 15));
			}
			Set0x0();
			var headerText = "Oragon Architecture Application Hosting";

			Console.BackgroundColor = ConsoleColor.DarkGray;
			Console.ForegroundColor = ConsoleColor.Black;
			for (int i = 0; i < Console.WindowWidth; i++)
				Console.Write(" ");

			var position = Math.Abs((Console.WindowWidth - headerText.Length) / 2);
			Console.SetCursorPosition(position, 0);
			Console.Write(headerText);

			int headerSize = 5;
			for (int i = 1; i < headerSize + 1; i++)
			{
				SetLeft(i);
				Console.Write(" ");
				SetRight(i);
				Console.Write(" ");
			}
			SetLeft(headerSize + 1);
			for (int i = 0; i < Console.WindowWidth; i++)
				Console.Write(" ");




			Console.ResetColor();
			Console.ForegroundColor = ConsoleColor.Red;
			Console.SetCursorPosition(1, 1); Red("Version: "); Green(this.GetType().Assembly.GetAssemblyInformationalVersion());
			Console.SetCursorPosition(1, 2); Red("AssemblyFileVersion: "); Green(this.GetType().Assembly.GetAssemblyFileVersion());
			Console.SetCursorPosition(1, 3); Red("ServiceName: "); Green(this.Name);
			Console.SetCursorPosition(1, 4); Red("FriendlyName: "); Green(this.FriendlyName);
			Console.SetCursorPosition(1, 5); Red("Description: "); Green(this.Description);
			Console.ResetColor();

			SetLeft(headerSize + 2);
		}

		private List<string> GetAllIPAddresses()
		{
			IEnumerable<string> iplist = System.Net.Dns.GetHostEntry(Environment.MachineName).AddressList.Select(it => it.ToString());
			iplist = iplist.Where(it => it.Contains(":") == false).ToArray(); //Only IPV4 IP`s
			return iplist.ToList();
		}
	}
}
