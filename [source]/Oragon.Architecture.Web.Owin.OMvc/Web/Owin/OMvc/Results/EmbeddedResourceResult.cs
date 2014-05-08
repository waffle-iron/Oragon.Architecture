using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;
using Microsoft.Owin;

namespace Oragon.Architecture.Web.Owin.OMvc.Results
{
	public static class EmbeddedResourceResultExtensions
	{
		public static EmbeddedResourceResult EmbeddedResource(this OMvcController @this, IEnumerable<Oragon.Architecture.Web.Owin.OMvc.Results.EmbeddedResourceResult.AssemblyMapping> mappings)
		{
			return new EmbeddedResourceResult(mappings);
		}

	}



	public class EmbeddedResourceResult : MvcResult
	{
		public class AssemblyMapping
		{
			public string VirtualFolder { get; private set; }

			public string AssemblyName { get; private set; }

			public System.Reflection.Assembly Assembly { get; private set; }

			public Action<IOwinContext> AfterResultExecution { get; set; }
			public Func<IOwinContext, bool> BeforeResultExecution { get; set; }

			public AssemblyMapping(string virtualFolder, string assemblyName, Func<IOwinContext, bool> beforeResultExecution = null, Action<IOwinContext> afterResultExecution = null)
			{
				this.Assembly = System.Reflection.Assembly.Load(assemblyName);
				this.VirtualFolder = virtualFolder;
				this.AssemblyName = assemblyName;
				this.BeforeResultExecution = beforeResultExecution;
				this.AfterResultExecution = afterResultExecution;
			}
		}

		List<AssemblyMapping> mappings;

		public EmbeddedResourceResult(IEnumerable<AssemblyMapping> mappings)
		{
			this.mappings = mappings.ToList();
		}

		public override void Execute(IOwinContext context)
		{
			string pathAndQuery = context.Request.Uri.PathAndQuery;
			string[] parts = pathAndQuery.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

			string contentType = MimeTypeResolver.ResolveMimeType(context.Request.Uri);
			foreach (AssemblyMapping assemblyMapping in this.mappings)
			{
				if (pathAndQuery.ToLower().StartsWith(assemblyMapping.VirtualFolder.ToLower()))
				{
					int startAt = assemblyMapping.VirtualFolder.Split(new char[] { '/' }, StringSplitOptions.RemoveEmptyEntries).Count();
					string fullName = assemblyMapping.AssemblyName;
					for (int i = startAt; i < parts.Length; i++)
					{
						if (i < parts.Length - 1)
							fullName += "." + parts[i].Replace(@"/", ".").Replace(@"-", "_");
						else
							fullName += "." + parts[i];
					}
					System.IO.Stream stream = assemblyMapping.Assembly.GetManifestResourceStream(fullName);
					if (stream != null)
					{
						var streamResult = new StreamResult() { Stream = stream, ContentType = contentType };
						bool canExecute = true;
						if (assemblyMapping.BeforeResultExecution != null)
						{
							canExecute = assemblyMapping.BeforeResultExecution(context);
						}
						if (canExecute)
						{
							streamResult.Execute(context);
							if (assemblyMapping.AfterResultExecution != null)
							{
								assemblyMapping.AfterResultExecution(context);
							}
						}
						return;
					}
				}
			}

			throw new System.IO.FileNotFoundException("Resource binded with '{0}' could not be found.".FormatWith(pathAndQuery));
		}


	}
}
