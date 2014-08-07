using Ninject;

namespace Oragon.Architecture.ApplicationHosting.Ninject
{
	public interface INinjectFactory : IContainerFactory<IKernel> { }

	public class NinjectApplicationHost : ApplicationHost<NinjectApplicationHostController, INinjectFactory, IKernel> { }

	public class NinjectApplicationHostController : ApplicationHostController<INinjectFactory, IKernel>
	{
		#region Public Methods

		public override void Stop()
		{
			this.Container.Dispose();
			this.Container = null;
		}

		#endregion Public Methods
	}
}