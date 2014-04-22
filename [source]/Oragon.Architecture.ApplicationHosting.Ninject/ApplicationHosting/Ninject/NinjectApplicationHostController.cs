using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject;

namespace Oragon.Architecture.ApplicationHosting.Ninject
{
	public class NinjectApplicationHostController : ApplicationHostController
	{

		private INinjectFactory ninjectFactory;

		private IKernel kernel;

		public override void Start()
		{
			this.kernel = ninjectFactory.BuildKernel();
		}

		public override void Stop()
		{
			
		}

		public void Setup(string bootstrapType)
		{
			Type type = System.Type.GetType(bootstrapType, true, false);
			if (type == null)
				throw new System.InvalidOperationException(string.Format("Type '{0}' could not be found", bootstrapType));

			if (typeof(INinjectFactory).IsAssignableFrom(type))
				this.ninjectFactory = Activator.CreateInstance(type) as INinjectFactory;

			if (this.ninjectFactory == null)
				throw new System.InvalidOperationException(string.Format("Bootstrap '{0}' could not be found", bootstrapType));
		}
	}
}
