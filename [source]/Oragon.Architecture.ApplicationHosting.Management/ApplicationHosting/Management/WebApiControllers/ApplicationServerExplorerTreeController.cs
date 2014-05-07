using Oragon.Architecture.ApplicationHosting.Management.ViewModel;
using Oragon.Architecture.ApplicationHosting.Management.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Oragon.Architecture.Extensions;
using Oragon.Architecture.ApplicationHosting.Management.Repository.Models.ApplicationModel;

namespace Oragon.Architecture.ApplicationHosting.Management.WebApiControllers
{
	public class ApplicationServerExplorerTreeController : ApiController
	{
		ApplicationRepository ApplicationRepository { get; set; }
		NotificationRepository NotificationRepository { get; set; }


		[HttpGet]
		public IEnumerable<TreeItem> Get(string node)
		{
			if (node == "root")
			{
				return new TreeItem[] { 
					
					new TreeItem(){ 
						id="/Servers/", 
						text = "Servers", 
						iconCls= "AppIcons-folder-brick", 
						leaf=false, 
						expanded = false, 
						children = null,
						menuItems = new MenuItem[]{
							new MenuItem(){ 
								text="Refresh", 
								iconCls="AppIcons-arrow-refresh-small", 
								handlerFunction="HomeController.refreshTreeNode",
								actionConfirmation = new ActionConfirmation("Atualizar", MessageBoxButtons.YESNO, MessageBoxIcon.INFO, "Are you shure?", MessageBoxButton.YES)
							} 
						}
					},
					new TreeItem(){ id="root/Repository",text = "Repository", iconCls= "AppIcons-information", leaf=false, expanded = true, children = null}
				};
			}
			else if (node == "/Servers/")
			{
				return this.ApplicationRepository.Machines.Select(
					it =>
					new TreeItem()
							{
								id = "/Server/{0}/".FormatWith(it.MachineDescriptor.MachineName),
								text = "Server {0}".FormatWith(it.MachineDescriptor.MachineName),
								iconCls = "AppIcons-server",
								leaf = false,
								expanded = false,
								children = null
							}
					);

			}
			else if (node.StartsWith("/Server/"))
			{
				string machineName = node.Split("/", StringSplitOptions.RemoveEmptyEntries).Last();
				Machine machine = this.ApplicationRepository.Machines.Single(it => it.MachineDescriptor.MachineName == machineName);

				return machine.Hosts.Select(it =>
					new TreeItem()
					{
						id = "/Host/{0}/".FormatWith(it.ID),
						text = "Host {0} :{1}".FormatWith(it.HostDescriptor.FriendlyName, it.HostDescriptor.PID),
						iconCls = "AppIcons-application-cascade",
						leaf = false,
						expanded = false,
						children = null
					}
				);
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
								children = null
							};

				return query.ToList();
			}
			

			return new TreeItem[] { };

		}
	}
}
