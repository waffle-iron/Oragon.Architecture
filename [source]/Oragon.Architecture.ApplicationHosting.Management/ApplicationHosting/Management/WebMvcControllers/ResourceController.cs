﻿using Oragon.Architecture.ApplicationHosting.Management.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;
using Oragon.Architecture.Web.Owin.OMvc.Results;
using Oragon.Architecture.Web.Owin.OMvc;
using AssemblyMapping = Oragon.Architecture.Web.Owin.OMvc.Results.EmbeddedResourceResult.AssemblyMapping;

namespace Oragon.Architecture.ApplicationHosting.Management.WebMvcControllers
{
	public class ResourceController : OMvcController
	{
		//private class AssemblyMapping
		//{
		//	public string Folder { get; set; }

		//	public System.Reflection.Assembly Assembly { get; set; }

		//	public string ResourceRootPath { get; set; }
		//}

		//private List<AssemblyMapping> resourceAssemblies;

		//public ResourceController()
		//{
		//	this.resourceAssemblies = new List<AssemblyMapping>();
		//	this.resourceAssemblies.Add(new AssemblyMapping() { Folder = @"extjs", Assembly = System.Reflection.Assembly.LoadFrom("Oragon.Architecture.ExtJS.dll"), ResourceRootPath = "Oragon.Architecture.ExtJS" });
		//	this.resourceAssemblies.Add(new AssemblyMapping() { Folder = @"script", Assembly = typeof(ResourceController).Assembly, ResourceRootPath = "Oragon.Architecture.ApplicationHosting.Management.Script" });
		//	this.resourceAssemblies.Add(new AssemblyMapping() { Folder = @"icons", Assembly = System.Reflection.Assembly.LoadFrom("Oragon.Architecture.Icons.dll"), ResourceRootPath = "Oragon.Architecture.Icons" });
		//}

		private List<AssemblyMapping> resourceAssemblies;
		public ResourceController()
		{
		}

		public override void Initialize()
		{
			base.Initialize();

			this.resourceAssemblies = new List<AssemblyMapping>();
			this.resourceAssemblies.Add(new AssemblyMapping(virtualFolder: @"/dynRes/extjs/", assemblyName: "Oragon.Architecture.ExtJS"));
			this.resourceAssemblies.Add(new AssemblyMapping(virtualFolder: @"/dynRes/ApplicationHosting/Management/", assemblyName: "Oragon.Architecture.ApplicationHosting.Management"));
			this.resourceAssemblies.Add(new AssemblyMapping(virtualFolder: @"/dynRes/icons/", assemblyName: "Oragon.Architecture.Icons"));
			this.resourceAssemblies.Add(new AssemblyMapping(virtualFolder: @"/dynRes/bootstrap/", assemblyName: "Oragon.Architecture.Bootstrap"));
		}

//		public MvcResult LoadFrom(string resourceName)
//		{

//			foreach (AssemblyMapping assemblyMapping in this.resourceAssemblies)
//			{
//				if (parts[0].ToLower() == assemblyMapping.Folder.ToLower())
//				{
//					string fullName = assemblyMapping.ResourceRootPath;
//					for (int i = 1; i < parts.Length; i++)
//					{
//						if (i < parts.Length - 1)
//							fullName += "." + parts[i].Replace(@"/", ".").Replace(@"-", "_");
//						else
//							fullName += "." + parts[i];
//					}
//					System.IO.Stream stream = assemblyMapping.Assembly.GetManifestResourceStream(fullName);
//					return new StreamResult() { Stream = stream, ContentType = contentType };
//				}
//			}
//			throw new DllNotFoundException("Could not be found assembly binded with this resource");
//		}
		public MvcResult LoadFrom(string resourceName)
		{
#if DEBUG
			string startConst = @"ApplicationHosting/Management/";
			if (resourceName.ToLower().StartsWith(startConst.ToLower()))
			{
				var fileName = @"D:\Projetos\Oragon.Architecture\[source]\Oragon.Architecture.ApplicationHosting.Management\ApplicationHosting\Management\" + resourceName.Substring(startConst.Length);
				string fileContent = System.IO.File.ReadAllText(fileName);
				return new ContentResult() { Content = fileContent, ContentType = MimeTypeResolver.ResolveMimeType(this.Request.Uri) };
			}
#endif
			return new EmbeddedResourceResult(this.resourceAssemblies);
		}
	}
}
