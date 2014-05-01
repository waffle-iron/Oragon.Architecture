using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting
{
	public abstract class ApplicationHostController<FactoryType, ContainerType> : MarshalByRefObject
		where FactoryType : IContainerFactory<ContainerType>
	{
		public virtual void Start() { }

		public virtual void Stop() { }

		public virtual void HeartBeat() { }

		public virtual AppDomainStatistic GetAppDomainStatistics()
		{
			return new AppDomainStatistic()
			{
				MonitoringTotalAllocatedMemorySize = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize,
				MonitoringSurvivedMemorySize = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize,
				Date = DateTime.Now,
			};
		}

		private FactoryType Factory { get; set; }

		protected ContainerType Container { get; set; }

		public void SetFactoryType(string factoryType)
		{
			Type type = System.Type.GetType(factoryType, true, false);
			if (type == null)
				throw new System.InvalidOperationException(string.Format("Type '{0}' could not be found", factoryType));

			if (typeof(FactoryType).IsAssignableFrom(type))
				this.Factory = (FactoryType)Activator.CreateInstance(type);

			if (this.Factory == null)
				throw new System.InvalidOperationException(string.Format("Bootstrap '{0}' could not be found", factoryType));
		}

		public void InitializeContainer()
		{
			this.Container = this.Factory.CreateContainer();
		}
	}
}
