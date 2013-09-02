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
		private Logger logger;

		public IApplicationContext ApplicationContext { set; get; }

		public ServiceManager()
		{
			this.logger = LogManager.GetCurrentClassLogger();
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
						this.logger.Debug("Criando Contexto Spring");
						this.ApplicationContext = ContextRegistry.GetContext();
						this.logger.Debug("Contexto Spring e inicializado com sucesso!");
					},
					delegate(Exception ex)
					{
						this.logger.Debug("Erro na inicialização do contexto spring: " + ex.ToString());
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
					this.logger.Debug("Finalizando contexto spring...");
					if (this.ApplicationContext != null)
					{
						this.ApplicationContext.Dispose();
						System.Configuration.ConfigurationManager.RefreshSection("spring/context");
						this.logger.Debug("Contexto spring finalizado com sucesso.");
					}
					else
						this.logger.Debug("Contexto não foi finalizado pois não existia no ServiceManager.");
					this.ApplicationContext = null;
				},
				delegate(Exception ex)
				{
					this.logger.Debug("Erro na finalização do contexto spring: " + ex.ToString());
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
					this.logger.Debug("Iniciando MainService...");
					this.MainService = this.ApplicationContext.GetObject<IService>("MainService");
					this.MainService.Start();
					this.logger.Debug("MainService foi iniciado com sucesso.");
				},
				delegate(Exception ex)
				{
					this.logger.Debug("Erro na iniciação do MainService: " + ex.ToString());
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
						this.logger.Debug("Parando MainService...");
						this.MainService.Stop();
						this.MainService = null;
						this.logger.Debug("MainService foi parado com sucesso.");
					},
					delegate(Exception ex)
					{
						this.logger.Debug("Erro na paralização do MainService: " + ex.ToString());
						this.MainService = null;
						throw new ServiceStopException("Erro na paralização do MainService.", ex);
					}
				);
			}
		}

		#endregion

	}
}
