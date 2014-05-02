using Oragon.Architecture.ApplicationHosting.Management.ViewModel;
using Oragon.Architecture.ApplicationHosting.Management.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Oragon.Architecture.Extensions;

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
				var query = from client in this.ApplicationRepository.Clients
							group client by new
							{
								client.MachineName
							} into grouper
							select new TreeItem()
							{
								id = "/Server/{0}".FormatWith(grouper.Key.MachineName),
								text = "Server {0}".FormatWith(grouper.Key.MachineName),
								iconCls = "AppIcons-server",
								leaf = false,
								expanded = true,
								children = grouper.Select(it =>
									new TreeItem()
									{
										id = "/Host/{0}".FormatWith(it.ID),
										text = "Host {0} :{1}".FormatWith(it.FriendlyName, it.PID),
										iconCls = "AppIcons-application-cascade",
										leaf = false,
										expanded = false,
										children = null
									}
								).ToList()
							};
				return query;
			}
			else if (node.StartsWith("/Host/"))
			{
				var id = node.Substring("/Host/".Length);
				Oragon.Architecture.ApplicationHosting.Model.HostDescriptor hostDescriptor = this.ApplicationRepository.Clients.SingleOrDefault(it => it.ID == Guid.Parse(id));
				if (hostDescriptor != null)
				{
					int index = 0;
					return hostDescriptor.Applications.Select(it =>
						new TreeItem()
						{
							id = "Application/{0}/{1}/".FormatWith(id, ++index),
							text = it.FriendlyName,
							iconCls = "AppIcons-application",
							leaf = true,
							expanded = false,
							children = null
						}
					);
				}
			}

			return new TreeItem[] { };

		}
	}
}
