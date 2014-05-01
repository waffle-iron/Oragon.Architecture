using Oragon.Architecture.ApplicationHosting.Management.Middleware;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.ApplicationHosting.Management.WebMvcControllers
{
	public class ResourceController : Controller
	{
		private class AssemblyMapping
		{
			public string Folder { get; set; }

			public System.Reflection.Assembly Assembly { get; set; }

			public string ResourceRootPath { get; set; }
		}

		private List<AssemblyMapping> resourceAssemblies;

		public ResourceController()
		{
			this.resourceAssemblies = new List<AssemblyMapping>();
			this.resourceAssemblies.Add(new AssemblyMapping() { Folder = @"extjs", Assembly = System.Reflection.Assembly.LoadFrom("Oragon.Architecture.ExtJS.dll"), ResourceRootPath = "Oragon.Architecture.ExtJS" });
			this.resourceAssemblies.Add(new AssemblyMapping() { Folder = @"script", Assembly = typeof(ResourceController).Assembly, ResourceRootPath = "Oragon.Architecture.ApplicationHosting.Management.Script" });
			this.resourceAssemblies.Add(new AssemblyMapping() { Folder = @"icons", Assembly = System.Reflection.Assembly.LoadFrom("Oragon.Architecture.Icons.dll"), ResourceRootPath = "Oragon.Architecture.Icons" });
		}

		public MvcResult LoadFrom(string resourceName)
		{
			//extjs/build/ext-all.js
			string[] parts = resourceName.Split('/');
			string contentType = base.ResolveMimeType(this.Request.Uri);


#if DEBUG
			if (parts[0].ToLower() == "script")
			{
				var fileName = @"D:\Projetos\Oragon.Architecture\[source]\Oragon.Architecture.ApplicationHosting.Management\ApplicationHosting\Management\" + resourceName;
				string fileContent = System.IO.File.ReadAllText(fileName);
				return new ContentResult() { Content = fileContent, ContentType = contentType };
			}
#endif
			foreach (AssemblyMapping assemblyMapping in this.resourceAssemblies)
			{
				if (parts[0].ToLower() == assemblyMapping.Folder.ToLower())
				{
					string fullName = assemblyMapping.ResourceRootPath;
					for (int i = 1; i < parts.Length; i++)
					{
						if (i < parts.Length - 1)
							fullName += "." + parts[i].Replace(@"/", ".").Replace(@"-", "_");
						else
							fullName += "." + parts[i];
					}
					System.IO.Stream stream = assemblyMapping.Assembly.GetManifestResourceStream(fullName);
					return new StreamResult() { Stream = stream, ContentType = contentType };
				}
			}
			throw new DllNotFoundException("Could not be found assembly binded with this resource");
		}
	}
}
