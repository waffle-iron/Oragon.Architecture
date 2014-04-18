using System;
using System.Configuration;
using System.ServiceProcess;
using NLog;
using Spring.Context;
using Spring.Context.Support;
using Topshelf;

namespace Oragon.Architecture.Services
{
	public class ServiceManager : ServiceControl
	{
		public TimeSpan? StartTimeOut { get; set; }
		public TimeSpan? StopTimeOut { get; set; }

		private IService MainService { get; set; }
		public IApplicationContext ApplicationContext { set; get; }

		public ServiceManager()
		{
		}

		public string Name
		{
			get { return "ServiceManager"; }
		}

		public bool Start(HostControl hostControl)
		{
			if (this.StartTimeOut != null && hostControl != null)
				hostControl.RequestAdditionalTime(this.StartTimeOut.Value);
			this.InitializeSpringContext();
			this.StartMainService();
			return true;
		}

		public bool Stop(HostControl hostControl)
		{
			if (this.StopTimeOut != null && hostControl != null)
				hostControl.RequestAdditionalTime(this.StopTimeOut.Value);

			this.StopMainService();
			this.FinalizeSpringContext();
			return true;
		}

		#region Spring Context Operations

		private void InitializeSpringContext()
		{
			if (this.ApplicationContext == null)
			{
				RetryManager.Try(
					delegate
					{
						this.ApplicationContext = ContextRegistry.GetContext();
					},
					delegate(Exception ex)
					{
						throw new ServiceStartException("Erro na inicialização do contexto spring.", ex);
					}
				);
			}
		}

		private void FinalizeSpringContext()
		{
			RetryManager.Try(
				delegate
				{
					if (this.ApplicationContext != null)
					{
						this.ApplicationContext.Dispose();
						System.Configuration.ConfigurationManager.RefreshSection("spring/context");
					}
					this.ApplicationContext = null;
				},
				delegate(Exception ex)
				{
					throw new ServiceStopException("Erro na finalização do contexto spring.", ex);
				}
			);

		}

		#endregion


		#region LifeCycle Management using MainService Operations (Get / Start / Stop)

		private void StartMainService()
		{
			RetryManager.Try(
				delegate
				{
					this.MainService = this.ApplicationContext.GetObject<IService>("MainService");
					this.MainService.Start();
				},
				delegate(Exception ex)
				{
					this.MainService = null;
					throw new ServiceStartException("Erro na iniciação do MainService.", ex);
				}
			);
		}

		private void StopMainService()
		{
			if (this.MainService != null)
			{
				RetryManager.Try(
					delegate
					{
						this.MainService.Stop();
						this.MainService = null;
					},
					delegate(Exception ex)
					{
						this.MainService = null;
						throw new ServiceStopException("Erro na paralização do MainService.", ex);
					}
				);
			}
		}

		#endregion

	}
}
