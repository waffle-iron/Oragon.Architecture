using Microsoft.Owin;
using Oragon.Architecture.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oragon.Architecture.Web.Owin.OMvc.Results
{
	public static class EmbeddedResourceResultExtensions
	{
		#region Public Methods

		public static EmbeddedResourceResult EmbeddedResource(this OMvcController @this, IEnumerable<Oragon.Architecture.Web.Owin.OMvc.Results.EmbeddedResourceResult.AssemblyMapping> mappings)
		{
			return new EmbeddedResourceResult(mappings);
		}

		#endregion Public Methods
	}

	public class EmbeddedResourceResult : MvcResult
	{
		#region Private Fields

		private List<AssemblyMapping> mappings;

		#endregion Private Fields

		#region Public Constructors

		public EmbeddedResourceResult(IEnumerable<AssemblyMapping> mappings)
		{
			this.mappings = mappings.ToList();
		}

		#endregion Public Constructors

		#region Public Methods

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

		#endregion Public Methods

		#region Public Classes

		public class AssemblyMapping
		{
			#region Public Constructors

			public AssemblyMapping(string virtualFolder, string assemblyName, Func<IOwinContext, bool> beforeResultExecution = null, Action<IOwinContext> afterResultExecution = null)
			{
				this.Assembly = System.Reflection.Assembly.Load(assemblyName);
				this.VirtualFolder = virtualFolder;
				this.AssemblyName = assemblyName;
				this.BeforeResultExecution = beforeResultExecution;
				this.AfterResultExecution = afterResultExecution;
			}

			#endregion Public Constructors

			#region Public Properties

			public Action<IOwinContext> AfterResultExecution { get; set; }

			public System.Reflection.Assembly Assembly { get; private set; }

			public string AssemblyName { get; private set; }

			public Func<IOwinContext, bool> BeforeResultExecution { get; set; }

			public string VirtualFolder { get; private set; }

			#endregion Public Properties
		}

		#endregion Public Classes
	}
}