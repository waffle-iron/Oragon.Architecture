using Oragon.Architecture.ApplicationHosting.Management.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Oragon.Architecture.ApplicationHosting.Management.WebApiControllers
{
	public class ApplicationServerExplorerTreeController : ApiController
	{
		[HttpGet]
		public IEnumerable<TreeItem> Get(string node)
		{
			if (node == "root")
			{
				return new TreeItem[] { 
					
					new TreeItem(){ 
						id="root/Servers", 
						text = "Servers", 
						iconCls= "AppIcons-server", 
						leaf=false, 
						expanded = true, 
						children = new TreeItem[]{
							new TreeItem(){ 
									id="root/Servers/Applications", 
									text = "Server ", 
									iconCls= "AppIcons-folder-brick", 
									leaf=false, 
									expanded = false, 
									children = null
							},
						}
					},
					new TreeItem(){ id="root/Repository",text = "Repository", iconCls= "AppIcons-information", leaf=false, expanded = true, children = null}
				};
			}
			else
			{



				return new TreeItem[] { 
			
				};
			}

		}
	}
}
