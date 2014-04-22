﻿using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ApplicationHosting.SimpleInjector
{
	public class SimpleInjectorHostController : ApplicationHostController
	{
		private ISimpleInjectorBootstrap Bootstrap { get; set; }

		public Container Container { get; set; }

		public void Setup(string bootstrapType)
		{
			Type type = System.Type.GetType(bootstrapType, true, false);
			if (type == null)
				throw new System.InvalidOperationException(string.Format("Type '{0}' could not be found", bootstrapType));

			if (typeof(ISimpleInjectorBootstrap).IsAssignableFrom(type))
				this.Bootstrap = Activator.CreateInstance(type) as ISimpleInjectorBootstrap;

			if (this.Bootstrap == null)
				throw new System.InvalidOperationException(string.Format("Bootstrap '{0}' could not be found", bootstrapType));
		}

		public override void Start()
		{
			Container container = new Container();
			this.Bootstrap.Bootstrap(container);
			this.Container = container;

			//this.context = Spring.Context.Support.ContextRegistry.GetContext();
			//foreach (var item in this.context.GetObjects<ILifecycle>().Select(it => it.Value))
			//{
			//	item.Start();
			//}
		}

		

		public override void Stop()
		{
			if (this.Container != null)
			{
				//foreach (var item in this.context.GetObjects<ILifecycle>().Select(it => it.Value).Reverse())
				//{
				//	item.Stop();
				//}
				this.Container = null;
			}
		}
	}
}
