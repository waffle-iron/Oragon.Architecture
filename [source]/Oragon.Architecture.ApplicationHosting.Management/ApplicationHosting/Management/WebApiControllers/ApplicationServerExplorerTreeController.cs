using Oragon.Architecture.ApplicationHosting.Management.Repository;
using Oragon.Architecture.ApplicationHosting.Management.ViewModel;
using Oragon.Architecture.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Oragon.Architecture.ApplicationHosting.Management.WebApiControllers
{
	[RoutePrefix("api/ApplicationServerExplorerTree")]
	public class ApplicationServerExplorerTreeController : ApiController
	{
		#region Private Properties

		private ApplicationRepository ApplicationRepository { get; set; }

		private NotificationRepository NotificationRepository { get; set; }

		#endregion Private Properties

		#region Public Methods

		[HttpGet]
		[Route("GetNodes")]
		public IEnumerable<TreeItem> GetNodes(string node)
		{
			var nodeRefresh = new MenuItem()
			{
				text = "Refresh",
				iconCls = "AppIcons-arrow-refresh-small",
				actionRoute = "Node|Refresh",
				actionConfirmation = null
			};

			if (node == "root")
			{
				return new TreeItem[]
				{
					new TreeItem()
					{
						id="/Servers/",
						text = "Servers",
						iconCls= "AppIcons-folder-brick",
						leaf=false,
						expanded = false,
						children = null,
						menuItems = new MenuItem[]
						{
							nodeRefresh
						}
					},
					new TreeItem()
					{
						id="/Repository",
						text = "Repository",
						iconCls= "AppIcons-information",
						leaf=false,
						expanded = false,
						children = null,
						menuItems = new MenuItem[]
						{
							nodeRefresh,
							new MenuItem()
							{
								text="Add New Application",
								iconCls="AppIcons-application-add",
								actionRoute="Repository|Add",
								actionConfirmation = null
							}
						}
					}
				};
			}
			else if (node == "/Servers/")
			{
				var query = from machine in this.ApplicationRepository.Machines
							select new TreeItem()
							{
								id = "/Server/{0}/".FormatWith(machine.MachineDescriptor.MachineName),
								text = "Server {0}".FormatWith(machine.MachineDescriptor.MachineName),
								iconCls = "AppIcons-server",
								leaf = false,
								expanded = false,
								children = null,
								menuItems = new MenuItem[]
								{
									nodeRefresh
								}
							};

				return query.ToList();
			}
			else if (node.StartsWith("/Server/"))
			{
				string machineName = node.Split("/", StringSplitOptions.RemoveEmptyEntries).Last();

				var query = from machine in this.ApplicationRepository.Machines
							from host in machine.Hosts
							where machine.MachineDescriptor.MachineName == machineName
							select new TreeItem()
							{
								id = "/Host/{0}/".FormatWith(host.ID),
								text = "Host {0} :{1}".FormatWith(host.HostDescriptor.FriendlyName, host.HostDescriptor.PID),
								iconCls = "AppIcons-application-cascade",
								leaf = false,
								expanded = false,
								children = null,
								menuItems = new MenuItem[]
								{
									nodeRefresh,
									new MenuItem()
									{
										text="Add Application",
										iconCls="AppIcons-application-add",
										actionRoute="Host|AddApplication",
										actionConfirmation = null,
									}
								}
							};

				return query.ToList();
			}
			else if (node.StartsWith("/Host/"))
			{
				string hostId = node.Split("/", StringSplitOptions.RemoveEmptyEntries).Last();

				var query = from machine in this.ApplicationRepository.Machines
							from host in machine.Hosts
							from app in host.HostDescriptor.Applications
							where
							host.ID.ToString("D") == hostId
							select new TreeItem()
							{
								id = "/Application/{0}/{1}/".FormatWith(host.ID, app.Name),
								text = app.FriendlyName,
								iconCls = "AppIcons-application",
								leaf = true,
								expanded = false,
								children = null,
								menuItems = new MenuItem[]
								{
									nodeRefresh,
									new MenuItem()
									{
										text="Start Application",
										iconCls="AppIcons-control-play-blue",
										actionRoute="Application|Start",
										actionConfirmation = new ActionConfirmation("Start Application", MessageBoxButtons.YESNO, MessageBoxIcon.QUESTION, "Please confirm start operation this application on server '{0}', process PID {1}.".FormatWith(machine.MachineDescriptor.MachineName, host.HostDescriptor.PID), MessageBoxButton.YES)
									} ,
									new MenuItem()
									{
										text="Stop Application",
										iconCls="AppIcons-control-stop-blue",
										actionRoute="Application|Stop",
										actionConfirmation = new ActionConfirmation("Stop Application", MessageBoxButtons.YESNOCANCEL, MessageBoxIcon.QUESTION, "Are you shure? This operation will stop this application on machine '{0}', process PID {1}.".FormatWith(machine.MachineDescriptor.MachineName, host.HostDescriptor.PID), MessageBoxButton.YES)
									} ,
									new MenuItem()
									{
										text="Remove Application",
										iconCls="AppIcons-application-delete",
										actionRoute="Application|Remove",
										actionConfirmation = new ActionConfirmation("Remove Application", MessageBoxButtons.YESNOCANCEL, MessageBoxIcon.WARNING, "Realy? Are you shure? This operation will REMOVE this application on machine '{0}', process PID {1}.".FormatWith(machine.MachineDescriptor.MachineName, host.HostDescriptor.PID), MessageBoxButton.YES)
									}
								}
							};

				return query.ToList();
			}

			return new TreeItem[] { };
		}

		#endregion Public Methods
	}
}