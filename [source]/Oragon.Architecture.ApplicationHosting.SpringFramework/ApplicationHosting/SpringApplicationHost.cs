using Spring.Objects.Factory.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NDepend.Helpers;
using NDepend.Path;
using System.Diagnostics.Contracts;
using System.Security.Policy;
using System.Security;
using System.Security.Permissions;

namespace Oragon.Architecture.ApplicationHosting
{
	public class SpringApplicationHost : ApplicationHost
	{
		[Required]
		public string ApplicationConfigurationFile { get; set; }
		[Required]
		public string ApplicationBaseDirectory { get; set; }

		private AppDomain privateAppDomain;

		private SpringApplicationHostController applicationHostController;

		private AppDomain CreateDomain(string appDomainName, IAbsoluteDirectoryPath absoluteApplicationBaseDirectory, IAbsoluteFilePath absoluteApplicationConfigurationFile)
		{
			Evidence domainEvidence = new Evidence();
			domainEvidence.AddHostEvidence(new Zone(SecurityZone.Trusted));
			
			//PermissionSet permissions = SecurityManager.GetStandardSandbox(domainEvidence);
			PermissionSet permissions = new PermissionSet( PermissionState.Unrestricted);
			permissions.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
			permissions.AddPermission(new System.Configuration.ConfigurationPermission(PermissionState.Unrestricted));
			permissions.AddPermission(new SecurityPermission(PermissionState.Unrestricted));

			//[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]

			// all assemblies need Execution permission to run at all
			permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
			// grant permission to perform DNS lookups
			permissions.AddPermission(new System.Net.DnsPermission(PermissionState.Unrestricted));

			AppDomainSetup domainSetup = new AppDomainSetup()
			{
				ApplicationBase = absoluteApplicationBaseDirectory.DirectoryInfo.FullName,
				ConfigurationFile = absoluteApplicationConfigurationFile.FileInfo.FullName
			};

			AppDomain appDomain = AppDomain.CreateDomain(appDomainName, domainEvidence, domainSetup, permissions);
			return appDomain;
		}

		private IAbsoluteDirectoryPath GetAbsoluteDirectoryPath(NDepend.Path.IAbsoluteDirectoryPath baseDirectory)
		{
			IRelativeDirectoryPath relativeApplicationBaseDirectory = this.ApplicationBaseDirectory.ToRelativeDirectoryPath();
			IAbsoluteDirectoryPath absoluteApplicationBaseDirectory = relativeApplicationBaseDirectory.GetAbsolutePathFrom(baseDirectory);
			if (absoluteApplicationBaseDirectory.Exists == false)
				throw new InvalidOperationException(string.Format("ApplicationBaseDirectory could not be resolved '{0}'", this.ApplicationBaseDirectory));

			return absoluteApplicationBaseDirectory;
		}

		private IAbsoluteFilePath GetAbsoluteFilePath(NDepend.Path.IAbsoluteDirectoryPath baseDirectory)
		{
			IRelativeFilePath relativeApplicationConfigurationFile = this.ApplicationConfigurationFile.ToRelativeFilePath();
			IAbsoluteFilePath absoluteApplicationConfigurationFile = relativeApplicationConfigurationFile.GetAbsolutePathFrom(baseDirectory);
			if (absoluteApplicationConfigurationFile.Exists == false)
				throw new InvalidOperationException(string.Format("ApplicationConfigurationFile could not be resolved using '{0}'", this.ApplicationConfigurationFile));
			return absoluteApplicationConfigurationFile;
		}



		public override void Start(NDepend.Path.IAbsoluteDirectoryPath baseDirectory)
		{
			Contract.Requires(baseDirectory != null && baseDirectory.Exists);

			IAbsoluteDirectoryPath absoluteApplicationBaseDirectory = this.GetAbsoluteDirectoryPath(baseDirectory);
			IAbsoluteFilePath absoluteApplicationConfigurationFile = this.GetAbsoluteFilePath(baseDirectory);
			this.privateAppDomain = this.CreateDomain(this.Name, absoluteApplicationBaseDirectory, absoluteApplicationConfigurationFile);

			Type typeOfApplicationController = typeof(SpringApplicationHostController);
			this.applicationHostController = (SpringApplicationHostController)this.privateAppDomain.CreateInstanceAndUnwrap(typeOfApplicationController.Assembly.FullName, typeOfApplicationController.FullName);
			this.applicationHostController.Start();
		}



		public override void Stop()
		{
			this.applicationHostController.Stop();
			AppDomain.Unload(this.privateAppDomain);
			this.applicationHostController = null;
			this.privateAppDomain = null;
		}
	}
}
