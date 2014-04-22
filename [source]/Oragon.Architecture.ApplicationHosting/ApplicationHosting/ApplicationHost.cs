﻿using NDepend.Path;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting
{

	public abstract class ApplicationHost
	{
		public string Name { get; set; }
		public string FriendlyName { get; set; }
		public string Description { get; set; }

		public abstract void Start(NDepend.Path.IAbsoluteDirectoryPath baseDirectory);
		public abstract void Stop();

		public string ApplicationConfigurationFile { get; set; }

		public string ApplicationBaseDirectory { get; set; }



		protected AppDomain CreateDomain(string appDomainName, IAbsoluteDirectoryPath absoluteApplicationBaseDirectory, IAbsoluteFilePath absoluteApplicationConfigurationFile)
		{
			Evidence domainEvidence = new Evidence();
			domainEvidence.AddHostEvidence(new Zone(SecurityZone.Trusted));

			//PermissionSet permissions = SecurityManager.GetStandardSandbox(domainEvidence);
			PermissionSet permissions = new PermissionSet(PermissionState.Unrestricted);
			permissions.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
			permissions.AddPermission(new ConfigurationPermission(PermissionState.Unrestricted));
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

		protected IAbsoluteDirectoryPath GetAbsoluteDirectoryPath(NDepend.Path.IAbsoluteDirectoryPath baseDirectory)
		{
			IRelativeDirectoryPath relativeApplicationBaseDirectory = this.ApplicationBaseDirectory.ToRelativeDirectoryPath();
			IAbsoluteDirectoryPath absoluteApplicationBaseDirectory = relativeApplicationBaseDirectory.GetAbsolutePathFrom(baseDirectory);
			if (absoluteApplicationBaseDirectory.Exists == false)
				throw new InvalidOperationException(string.Format("ApplicationBaseDirectory could not be resolved '{0}'", this.ApplicationBaseDirectory));

			return absoluteApplicationBaseDirectory;
		}

		protected IAbsoluteFilePath GetAbsoluteFilePath(NDepend.Path.IAbsoluteDirectoryPath baseDirectory)
		{
			IRelativeFilePath relativeApplicationConfigurationFile = this.ApplicationConfigurationFile.ToRelativeFilePath();
			IAbsoluteFilePath absoluteApplicationConfigurationFile = relativeApplicationConfigurationFile.GetAbsolutePathFrom(baseDirectory);
			if (absoluteApplicationConfigurationFile.Exists == false)
				throw new InvalidOperationException(string.Format("ApplicationConfigurationFile could not be resolved using '{0}'", this.ApplicationConfigurationFile));
			return absoluteApplicationConfigurationFile;
		}

	}

	public abstract class ApplicationHost<T> : ApplicationHost
		where T : ApplicationHostController
	{
		private T applicationHostController;

		private AppDomain privateAppDomain;

		public override void Start(NDepend.Path.IAbsoluteDirectoryPath baseDirectory)
		{
			Contract.Requires(baseDirectory != null && baseDirectory.Exists);

			IAbsoluteDirectoryPath absoluteApplicationBaseDirectory = this.GetAbsoluteDirectoryPath(baseDirectory);
			IAbsoluteFilePath absoluteApplicationConfigurationFile = this.GetAbsoluteFilePath(baseDirectory);
			this.privateAppDomain = this.CreateDomain(this.Name, absoluteApplicationBaseDirectory, absoluteApplicationConfigurationFile);

			Type typeOfApplicationController = typeof(T);
			this.applicationHostController = (T)this.privateAppDomain.CreateInstanceAndUnwrap(typeOfApplicationController.Assembly.FullName, typeOfApplicationController.FullName);
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
