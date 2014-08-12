using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using Oragon.Architecture.IO.Path;
using System;
using System.Configuration;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;

namespace Oragon.Architecture.ApplicationHosting
{
	public abstract class ApplicationHost
	{
		#region Public Constructors

		public ApplicationHost()
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
			return new ApplicationDescriptor()
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

	public abstract class ApplicationHost<ApplicationHostControllerType, FactoryType, ContainerType> : ApplicationHost
		where ApplicationHostControllerType : ApplicationHostController<FactoryType, ContainerType>
		where FactoryType : IContainerFactory<ContainerType>
	{
		#region Private Fields

		private volatile ApplicationHostControllerType applicationHostController;

		private System.Timers.Timer heartBeatTimer;
		private AppDomain privateAppDomain;

		#endregion Private Fields

		#region Public Methods

		public override AppDomainStatistic GetAppDomainStatistics()
		{
			return this.applicationHostController.GetAppDomainStatistics();
		}

		public override void Start(IAbsoluteDirectoryPath baseDirectory)
		{
			this.heartBeatTimer = new System.Timers.Timer(new TimeSpan(0, 0, 10).TotalMilliseconds);
			this.heartBeatTimer.Elapsed += delegate(object sender, System.Timers.ElapsedEventArgs e)
			{
				this.applicationHostController.HeartBeat();
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
		}

		public override void Stop()
		{
			this.heartBeatTimer.Stop();
			this.heartBeatTimer.Dispose();
			this.heartBeatTimer = null;

			this.applicationHostController.Stop();
			AppDomain.Unload(this.privateAppDomain);
			this.applicationHostController = null;
			this.privateAppDomain = null;
		}

		#endregion Public Methods
	}
}