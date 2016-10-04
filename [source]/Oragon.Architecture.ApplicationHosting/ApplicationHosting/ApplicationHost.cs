using System.Timers;
using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using Oragon.Architecture.IO.Path;
using System;
using System.Configuration;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using FluentAssertions;

namespace Oragon.Architecture.ApplicationHosting
{
	public abstract class ApplicationHost
	{
		#region Public Constructors

		protected ApplicationHost()
		{
		}

		#endregion Public Constructors

		#region Public Properties

		public string ApplicationBaseDirectory { get; set; }

		public string ApplicationConfigurationFile { get; set; }

		public string Description { get; set; }

		public bool EnableShadowCopy { get; set; }

		public string FactoryType { get; set; }

		public string FriendlyName { get; set; }

		public string Name { get; set; }

		#endregion Public Properties

		#region Public Methods

		public abstract AppDomainStatistic GetAppDomainStatistics();

		public abstract void Start(IAbsoluteDirectoryPath baseDirectory);

		public abstract void Stop();

		public virtual ApplicationDescriptor ToDescriptor()
		{
			return new ApplicationDescriptor
			{
				Name = this.Name,
				FriendlyName = this.FriendlyName,
				Description = this.Description,
				FactoryType = this.FactoryType,
				ApplicationConfigurationFile = this.ApplicationConfigurationFile,
				ApplicationBaseDirectory = this.ApplicationBaseDirectory,
				TypeName = this.GetType().FullName
			};
		}

		#endregion Public Methods

		#region Protected Methods

		protected AppDomain CreateDomain(string appDomainName, IAbsoluteDirectoryPath absoluteApplicationBaseDirectory, IAbsoluteFilePath absoluteApplicationConfigurationFile)
		{
			var domainEvidence = new Evidence();
			domainEvidence.AddHostEvidence(new Zone(SecurityZone.Trusted));

			//PermissionSet permissions = SecurityManager.GetStandardSandbox(domainEvidence);
			var permissions = new PermissionSet(PermissionState.Unrestricted);
			permissions.AddPermission(new FileIOPermission(PermissionState.Unrestricted));
			permissions.AddPermission(new ConfigurationPermission(PermissionState.Unrestricted));
			permissions.AddPermission(new SecurityPermission(PermissionState.Unrestricted));

			//[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]

			// all assemblies need Execution permission to run at all
			permissions.AddPermission(new SecurityPermission(SecurityPermissionFlag.Execution));
			// grant permission to perform DNS lookups
			permissions.AddPermission(new System.Net.DnsPermission(PermissionState.Unrestricted));

			var domainSetup = new AppDomainSetup()
			{
				ApplicationBase = absoluteApplicationBaseDirectory.DirectoryInfo.FullName,
				ConfigurationFile = absoluteApplicationConfigurationFile.FileInfo.FullName,
				ShadowCopyFiles = this.EnableShadowCopy.ToString()
			};

			AppDomain appDomain = AppDomain.CreateDomain(appDomainName, domainEvidence, domainSetup, permissions);
			return appDomain;
		}

		protected IAbsoluteDirectoryPath GetAbsoluteDirectoryPath(IAbsoluteDirectoryPath baseDirectory)
		{
			IRelativeDirectoryPath relativeApplicationBaseDirectory = this.ApplicationBaseDirectory.ToRelativeDirectoryPath();
			IAbsoluteDirectoryPath absoluteApplicationBaseDirectory = relativeApplicationBaseDirectory.GetAbsolutePathFrom(baseDirectory);
			if (absoluteApplicationBaseDirectory.Exists == false)
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "ApplicationBaseDirectory could not be resolved '{0}'", this.ApplicationBaseDirectory));

			return absoluteApplicationBaseDirectory;
		}

		protected IAbsoluteFilePath GetAbsoluteFilePath(IAbsoluteDirectoryPath baseDirectory)
		{
			IRelativeFilePath relativeApplicationConfigurationFile = this.ApplicationConfigurationFile.ToRelativeFilePath();
			IAbsoluteFilePath absoluteApplicationConfigurationFile = relativeApplicationConfigurationFile.GetAbsolutePathFrom(baseDirectory);
			if (absoluteApplicationConfigurationFile.Exists == false)
				throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "ApplicationConfigurationFile could not be resolved using '{0}'", this.ApplicationConfigurationFile));
			return absoluteApplicationConfigurationFile;
		}

		#endregion Protected Methods
	}

	public abstract class ApplicationHost<TApplicationHostControllerType, TFactoryType, TContainerType> : ApplicationHost
		where TApplicationHostControllerType : ApplicationHostController<TFactoryType, TContainerType>
		where TFactoryType : IContainerFactory<TContainerType>
	{
		#region Private Fields

		private volatile TApplicationHostControllerType _applicationHostController;

		private System.Timers.Timer _heartBeatTimer;
		private AppDomain _privateAppDomain;

		#endregion Private Fields

		#region Public Methods

		public override AppDomainStatistic GetAppDomainStatistics()
		{
			return this._applicationHostController.GetAppDomainStatistics();
		}

		public override void Start(IAbsoluteDirectoryPath baseDirectory)
		{
			this._heartBeatTimer = new System.Timers.Timer(new TimeSpan(0, 0, 10).TotalMilliseconds);
			this._heartBeatTimer.Elapsed += (sender, e) => this._applicationHostController.HeartBeat();

			baseDirectory.Should().NotBeNull();
			baseDirectory.Exists.Should().BeTrue();

			IAbsoluteDirectoryPath absoluteApplicationBaseDirectory = this.GetAbsoluteDirectoryPath(baseDirectory);
			IAbsoluteFilePath absoluteApplicationConfigurationFile = this.GetAbsoluteFilePath(baseDirectory);
			this._privateAppDomain = this.CreateDomain(this.Name, absoluteApplicationBaseDirectory, absoluteApplicationConfigurationFile);

			Type typeOfApplicationController = typeof(TApplicationHostControllerType);
			this._applicationHostController = (TApplicationHostControllerType)this._privateAppDomain.CreateInstanceAndUnwrap(typeOfApplicationController.Assembly.FullName, typeOfApplicationController.FullName);
			this._applicationHostController.SetFactoryType(this.FactoryType);
			this._applicationHostController.InitializeContainer();
			this._applicationHostController.Start();
			this._heartBeatTimer.Start();
		}

		public override void Stop()
		{
			this._heartBeatTimer.Stop();
			this._heartBeatTimer.Dispose();
			this._heartBeatTimer = null;

			this._applicationHostController.Stop();
			AppDomain.Unload(this._privateAppDomain);
			this._applicationHostController = null;
			this._privateAppDomain = null;
		}

		#endregion Public Methods
	}
}