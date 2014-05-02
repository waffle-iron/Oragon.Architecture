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
		public string FactoryType { get; set; }
		public abstract void Start(NDepend.Path.IAbsoluteDirectoryPath baseDirectory);
		public abstract void Stop();
		public string ApplicationConfigurationFile { get; set; }
		public string ApplicationBaseDirectory { get; set; }

		public bool EnableShadowCopy { get; set; }

		public List<AppDomainStatistic> AppDomainStatisticHistory { get; set; }

		public ApplicationHost()
		{
			this.AppDomainStatisticHistory = new List<AppDomainStatistic>();
		}

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
				//PrivateBinPath = absoluteApplicationBaseDirectory.DirectoryInfo.FullName,
				//PrivateBinPathProbe = absoluteApplicationBaseDirectory.DirectoryInfo.FullName,
				ConfigurationFile = absoluteApplicationConfigurationFile.FileInfo.FullName,
				ShadowCopyFiles = this.EnableShadowCopy.ToString()
			};

			AppDomain appDomain = AppDomain.CreateDomain(appDomainName, domainEvidence, domainSetup, permissions);			
			//Console.WriteLine("BaseDirectory " + appDomain.BaseDirectory);
			//Console.WriteLine("DynamicDirectory " + appDomain.DynamicDirectory);
			//Console.WriteLine("FriendlyName " + appDomain.FriendlyName);
			//Console.WriteLine("Id " + appDomain.Id.ToString());
			//Console.WriteLine("IsFullyTrusted " + appDomain.IsFullyTrusted.ToString());
			//Console.WriteLine("RelativeSearchPath " + appDomain.RelativeSearchPath);
			//Console.WriteLine("SetupInformation.CachePath " + appDomain.SetupInformation.CachePath);
			//Console.WriteLine("SetupInformation.PrivateBinPath " + appDomain.SetupInformation.PrivateBinPath);
			//Console.WriteLine("SetupInformation.PrivateBinPathProbe " + appDomain.SetupInformation.PrivateBinPathProbe);

			//appDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs args)
			//{
			//	Console.WriteLine(args.Name);
			//	return null;
			//};
			//appDomain.TypeResolve += delegate(object sender, ResolveEventArgs args)
			//{
			//	Console.WriteLine(args.Name);
			//	return null;
			//};
			//appDomain.ReflectionOnlyAssemblyResolve += delegate(object sender, ResolveEventArgs args)
			//{
			//	Console.WriteLine(args.Name);
			//	return null;
			//}; 

			//AppDomain.CurrentDomain.AssemblyResolve += delegate(object sender, ResolveEventArgs args)
			//{
			//	Console.WriteLine(args.Name);
			//	return null;
			//};
			//AppDomain.CurrentDomain.TypeResolve += delegate(object sender, ResolveEventArgs args)
			//{
			//	Console.WriteLine(args.Name);
			//	return null;
			//};
			//AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += delegate(object sender, ResolveEventArgs args)
			//{
			//	Console.WriteLine(args.Name);
			//	return null;
			//}; 

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

	public abstract class ApplicationHost<ApplicationHostControllerType, FactoryType, ContainerType> : ApplicationHost
		where ApplicationHostControllerType : ApplicationHostController<FactoryType, ContainerType>
		where FactoryType : IContainerFactory<ContainerType>
	{
		private ApplicationHostControllerType applicationHostController;

		private AppDomain privateAppDomain;

		private System.Timers.Timer heartBeatTimer;

		private System.Timers.Timer monitoringTimer;

		public override void Start(NDepend.Path.IAbsoluteDirectoryPath baseDirectory)
		{
			this.heartBeatTimer = new System.Timers.Timer(new TimeSpan(0, 0, 10).TotalMilliseconds);
			this.heartBeatTimer.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
			{
				this.applicationHostController.HeartBeat();
			};

			this.monitoringTimer = new System.Timers.Timer(new TimeSpan(0, 0, 30).TotalMilliseconds);
			this.monitoringTimer.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
			{
				this.AppDomainStatisticHistory.Add(this.applicationHostController.GetAppDomainStatistics());
			};


			Contract.Requires(baseDirectory != null && baseDirectory.Exists);

			IAbsoluteDirectoryPath absoluteApplicationBaseDirectory = this.GetAbsoluteDirectoryPath(baseDirectory);
			IAbsoluteFilePath absoluteApplicationConfigurationFile = this.GetAbsoluteFilePath(baseDirectory);
			this.privateAppDomain = this.CreateDomain(this.Name, absoluteApplicationBaseDirectory, absoluteApplicationConfigurationFile);

			Type typeOfApplicationController = typeof(ApplicationHostControllerType);
			this.applicationHostController = (ApplicationHostControllerType)this.privateAppDomain.CreateInstanceAndUnwrap(typeOfApplicationController.Assembly.FullName, typeOfApplicationController.FullName);
			this.applicationHostController.SetFactoryType(this.FactoryType);
			this.applicationHostController.InitializeContainer();
			this.applicationHostController.Start();
			this.heartBeatTimer.Start();
			this.monitoringTimer.Start();
		}

		public override void Stop()
		{
			this.monitoringTimer.Stop();
			this.heartBeatTimer.Stop();
			this.heartBeatTimer.Dispose();
			this.heartBeatTimer = null;

			this.applicationHostController.Stop();
			AppDomain.Unload(this.privateAppDomain);
			this.applicationHostController = null;
			this.privateAppDomain = null;
		}

	}
}
