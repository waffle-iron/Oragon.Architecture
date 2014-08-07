﻿using AopAlliance.Intercept;
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNet.SignalR.Client.Hubs;
using Oragon.Architecture.Extensions;
using Oragon.Architecture.Text;
using Spring.Aop.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;

namespace Oragon.Architecture.Services.SignalRServices
{
	public class SignalRMediator<TServerInterfaceType, TClientInterfaceType>
	{
		#region Members

		private TClientInterfaceType clientProxy;
		private HubConnection connection;

		private FormatStrategy hubMethodFormatStrategy;
		private FormatStrategy hubNameFormatStrategy;
		private IHubProxy hubProxy;
		private TServerInterfaceType serverProxy;

		#endregion Members

		#region Properties

		public TServerInterfaceType Server { get { return this.serverProxy; } }

		#endregion Properties

		#region Interceptors

		private class ClientMethodInterceptor : IMethodInterceptor
		{
			#region Public Methods

			public object Invoke(IMethodInvocation invocation)
			{
				object returnFromExecution = invocation.Proceed();
				return returnFromExecution;
			}

			#endregion Public Methods
		}

		private class ServerMethodInterceptor : IMethodInterceptor
		{
			#region Private Fields

			private FormatStrategy hubMethodFormatStrategy;
			private Microsoft.AspNet.SignalR.Client.IHubProxy hubProxy;

			#endregion Private Fields

			#region Public Constructors

			public ServerMethodInterceptor(Microsoft.AspNet.SignalR.Client.IHubProxy hubProxy, FormatStrategy hubMethodFormatStrategy)
			{
				this.hubProxy = hubProxy;
				this.hubMethodFormatStrategy = hubMethodFormatStrategy;
			}

			#endregion Public Constructors

			#region Public Methods

			public object Invoke(IMethodInvocation invocation)
			{
				string methodName = this.hubMethodFormatStrategy.Format(invocation.Method.Name);

				Task<object> task = this.hubProxy.Invoke<object>(methodName, invocation.Arguments);
				task.Wait();
				object returnValue = task.Result;
				if (returnValue != null && !returnValue.GetType().IsAssignableFrom(invocation.Method.ReturnType))
				{
					returnValue = Convert.ChangeType(returnValue, invocation.Method.ReturnType);
				}
				return returnValue;
			}

			#endregion Public Methods
		}

		#endregion Interceptors

		#region Constructor

		public SignalRMediator(HubConnection connection, TClientInterfaceType clientObject, FormatStrategy hubNameFormatStrategy, FormatStrategy hubMethodFormatStrategy)
		{
			Type serviceInterfaceType = typeof(TServerInterfaceType);
			Type clientInterfaceType = typeof(TClientInterfaceType);

			if (serviceInterfaceType.IsInterface == false)
				throw new InvalidCastException("ServiceInterface must be an interface");
			if (clientInterfaceType.IsInterface == false)
				throw new InvalidCastException("clientInterface must be an interface");

			if (clientObject.IsNull())
				throw new ArgumentNullException("clientObject cannot be null");
			if (connection.IsNull())
				throw new ArgumentNullException("connection cannot be null");
			if (hubNameFormatStrategy.IsNull())
				throw new ArgumentNullException("hubNameFormatStrategy cannot be null");
			if (hubMethodFormatStrategy.IsNull())
				throw new ArgumentNullException("hubMethodFormatStrategy cannot be null");

			this.connection = connection;
			this.hubNameFormatStrategy = hubNameFormatStrategy;
			this.hubMethodFormatStrategy = hubMethodFormatStrategy;

			string hubName = hubNameFormatStrategy.Format(serviceInterfaceType.Name.Substring(1));
			this.hubProxy = this.connection.CreateHubProxy(hubName);
			{
				var serverProxyFactory = new ProxyFactory(serviceInterfaceType, new ServerMethodInterceptor(this.hubProxy, this.hubMethodFormatStrategy));
				this.serverProxy = (TServerInterfaceType)serverProxyFactory.GetProxy();
			}
			{
				var clientProxyFactory = new ProxyFactory(clientInterfaceType, new ClientMethodInterceptor())
				{
					Target = clientObject
				};
				this.clientProxy = (TClientInterfaceType)clientProxyFactory.GetProxy();
			}
			this.BindClientMethodsAsServerEvents(clientInterfaceType);
		}

		public void Bind(Delegate method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			this.Bind(methodInfo.Name, method, methodInfo);
		}

		public void Bind(string name, Delegate method)
		{
			MethodInfo methodInfo = method.GetMethodInfo();
			this.Bind(name, method, methodInfo);
		}

		private void Bind(string name, Delegate method, MethodInfo methodInfo)
		{
			string methodName = this.hubMethodFormatStrategy.Format(name);
			Subscription subscription = this.hubProxy.Subscribe(methodName);
			subscription.Received += delegate(IList<Newtonsoft.Json.Linq.JToken> args)
			{
				List<object> argsToSend = new List<object>();
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (args.Count != parameters.Length)
					throw new InvalidCastException("The method {0} cannot be called with this arguments.".FormatWith(methodName));

				for (int i = 0; i < parameters.Length; i++)
				{
					object objectToSend = args[i].ToObject(parameters[i].ParameterType);
					argsToSend.Add(objectToSend);
				}
				method.DynamicInvoke(argsToSend.ToArray());
			};
		}

		private void BindClientMethodsAsServerEvents(Type clientInterface)
		{
			foreach (var method in clientInterface.GetMethods())
			{
				Delegate delegateOfMethod = this.CreateDelegate(method);
				this.Bind(delegateOfMethod);
			}
		}

		private Delegate CreateDelegate(MethodInfo method)
		{
			List<Type> args = method.GetParameters().Select(p => p.ParameterType).ToList();
			Type delegateType;
			if (method.ReturnType == typeof(void))
			{
				delegateType = Expression.GetActionType(args.ToArray());
			}
			else
			{
				args.Add(method.ReturnType);
				delegateType = Expression.GetFuncType(args.ToArray());
			}
			Delegate delegateToReturn = Delegate.CreateDelegate(delegateType, null, method);
			return delegateToReturn;
		}

		#endregion Constructor
	}
}