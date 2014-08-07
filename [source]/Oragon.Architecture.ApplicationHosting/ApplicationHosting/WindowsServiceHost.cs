using Oragon.Architecture.IO.Path;
using Topshelf;

namespace Oragon.Architecture.ApplicationHosting
{
	public class WindowsServiceHost : ConsoleServiceHost, ServiceControl
	{
		#region Public Properties

		public WindowsServiceConfiguration WindowsServiceConfiguration { get; set; }

		#endregion Public Properties

		#region Public Methods

		public void Configure(Topshelf.HostConfigurators.HostConfigurator hostConfig, string configurationFileName)
		{
			hostConfig.Service<WindowsServiceHost>(serviceConfigurator =>
			{
				serviceConfigurator.ConstructUsing(() => this);
				serviceConfigurator.WhenStarted((serviceManagerInstance, hostControl) => serviceManagerInstance.Start(hostControl));
				serviceConfigurator.WhenStopped((serviceManagerInstance, hostControl) => serviceManagerInstance.Stop(hostControl));
				serviceConfigurator.WhenShutdown((serviceManagerInstance, hostControl) => serviceManagerInstance.Shutdown(hostControl));
			});

			hostConfig.EnableShutdown();

			hostConfig.SetServiceName(this.Name);
			hostConfig.SetDisplayName(this.FriendlyName);
			hostConfig.SetDescription(this.Description);

			switch (this.WindowsServiceConfiguration.IdentityType)
			{
				case AccountType.LocalService: hostConfig.RunAsLocalService(); break;
				case AccountType.LocalSystem: hostConfig.RunAsLocalSystem(); break;
				case AccountType.NetworkService: hostConfig.RunAsNetworkService(); break;
				case AccountType.Prompt: hostConfig.RunAsPrompt(); break;
				case AccountType.Custom: hostConfig.RunAs(this.WindowsServiceConfiguration.CustomIdentityCredential.Username, this.WindowsServiceConfiguration.CustomIdentityCredential.Password); break;
			}

			switch (this.WindowsServiceConfiguration.StartMode)
			{
				case StartMode.Automatically: hostConfig.StartAutomatically(); break;
				case StartMode.AutomaticallyDelayed: hostConfig.StartAutomaticallyDelayed(); break;
				case StartMode.Disabled: hostConfig.Disabled(); break;
				case StartMode.Manually: hostConfig.StartManually(); break;
			}
			if (this.WindowsServiceConfiguration.Dependences != null)
			{
				foreach (string dependency in this.WindowsServiceConfiguration.Dependences)
				{
					hostConfig.AddDependency(dependency);
				}
			}

			this.ConfigurationFilePath = configurationFileName.ToAbsoluteFilePath();
		}

		public void Shutdown(HostControl hostControl)
		{
			this.Stop();
			return;
		}

		public bool Start(HostControl hostControl)
		{
			this.Start();
			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			this.Stop();
			return true;
		}

		#endregion Public Methods
	}
}