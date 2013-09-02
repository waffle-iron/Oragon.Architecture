using NLog;
using Spring.Context;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
namespace Oragon.Architecture.Services
{
	public class ServiceContainer : IService
	{
		private Logger logger = LogManager.GetCurrentClassLogger();

		private IApplicationContext ApplicationContext { get; set; }
		
		public List<IService> Services { get; private set; }
	
		public string Name { get { return "Oragon Service Container"; } }

		public void Start()
		{
			if (this.Services != null && this.Services.Count > 0)
			{
				this.logger.Debug("Iniciando serviços...");
				foreach (IService current in this.Services)
				{
					this.logger.Debug("Iniciando serviço {0}.", current.Name);
					current.Start();
					this.logger.Debug("Serviço {0} iniciado com sucesso", current.Name);
				}
				this.logger.Debug("Todos os serviços foram iniciados.");
				return;
			}
			this.logger.Debug("Não há serviços a serem iniciados");
		}

		public void Stop()
		{
			if (this.Services != null && this.Services.Count > 0)
			{
				List<IService> list = new List<IService>(this.Services);
				list.Reverse();
				this.logger.Debug("Parando serviços...");
				using (List<IService>.Enumerator enumerator = list.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						IService current = enumerator.Current;
						this.logger.Debug("Parando serviço {0}.", current.Name);
						current.Stop();
						this.logger.Debug("O serviço {0} foi parado com sucesso.", current.Name);
					}
					return;
				}
			}
			this.logger.Debug("Não há serviços a serem parados.");
		}

	}
}
