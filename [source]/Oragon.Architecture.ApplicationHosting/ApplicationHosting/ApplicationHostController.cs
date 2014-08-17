using Oragon.Architecture.ApplicationHosting.Services.Contracts;
using System;
using System.Globalization;
using System.Linq;

namespace Oragon.Architecture.ApplicationHosting
{
	public abstract class ApplicationHostController<TFactoryType, TContainerType> : MarshalByRefObject
		where TFactoryType : IContainerFactory<TContainerType>
	{
		#region Public Constructors

		protected ApplicationHostController()
		{
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
		}

		#endregion Public Constructors

		#region Protected Properties

		protected TContainerType Container { get; set; }

		#endregion Protected Properties

		#region Private Properties

		private TFactoryType Factory { get; set; }

		#endregion Private Properties

		#region Public Methods

		[LoaderOptimization(LoaderOptimization.MultiDomainHost)]
		public virtual AppDomainStatistic GetAppDomainStatistics()
		{
			return new AppDomainStatistic()
			{
				MonitoringTotalAllocatedMemorySize = AppDomain.CurrentDomain.MonitoringTotalAllocatedMemorySize,
				MonitoringSurvivedMemorySize = AppDomain.CurrentDomain.MonitoringSurvivedMemorySize,
				Date = DateTime.Now,
				Assemblies = AppDomain.CurrentDomain.GetAssemblies().Select(it => new AssemblyDescriptor() { Name = it.FullName }).ToList()
			};
		}

		[LoaderOptimization(LoaderOptimization.MultiDomainHost)]
		public virtual void HeartBeat() { }

		public void InitializeContainer()
		{
			this.Container = this.Factory.CreateContainer();
		}

		public override object InitializeLifetimeService()
		{
			// This ensures the object lasts for as long as the client wants it
			return null;
		}

		public void SetFactoryType(string factoryType)
		{
			Type type = System.Type.GetType(factoryType, true, false);
			if (type == null)
				throw new System.InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Type '{0}' could not be found", factoryType));

			if (typeof(TFactoryType).IsAssignableFrom(type))
				this.Factory = (TFactoryType)Activator.CreateInstance(type);
			else
				throw new System.InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Bootstrap '{0}' could not be found", factoryType));
		}

		[LoaderOptimization(LoaderOptimization.MultiDomainHost)]
		public virtual void Start() { }

		[LoaderOptimization(LoaderOptimization.MultiDomainHost)]
		public virtual void Stop() { }

		#endregion Public Methods
	}
}