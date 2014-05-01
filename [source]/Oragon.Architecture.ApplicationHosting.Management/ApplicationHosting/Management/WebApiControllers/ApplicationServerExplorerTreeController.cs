using Oragon.Architecture.ApplicationHosting.Management.Model;
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

		[HttpGet]
		public IEnumerable<TreeItem> Get(string node)
		{
			if (node == "root")
			{
				return new TreeItem[] { 
					
					new TreeItem(){ 
						id="root/Servers", 
						text = "Servers", 
						iconCls= "AppIcons-folder-brick", 
						leaf=false, 
						expanded = false, 
						children = null,
					},
					new TreeItem(){ id="root/Repository",text = "Repository", iconCls= "AppIcons-information", leaf=false, expanded = true, children = null}
				};
			}
			else if (node == "root/Servers")
			{
				var query = from client in this.ApplicationRepository.Clients
							group client by new
							{
								client.MachineName
							} into grouper
							select new TreeItem()
							{
								id = "Server/{0}".FormatWith(grouper.Key.MachineName),
								text = "Server {0}".FormatWith(grouper.Key.MachineName),
								iconCls = "AppIcons-server",
								leaf = false,
								expanded = true,
								children = grouper.Select( it =>
									new TreeItem()
									{
										id = "root/Servers/Server/{0}/Host/{1}".FormatWith(grouper.Key.MachineName, it.ID),
										text = "Host {0} :{1}".FormatWith(it.FriendlyName, it.PID),
										iconCls = "AppIcons-application-xp-terminal",
										leaf = false,
										expanded = false,
										children = null
									}
								).ToList()
							};

				return query;
				
			}
			else
			{
				return new TreeItem[] { 
			
				};
			}

		}
	}
}
