using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using AopAlliance.Intercept;
using Oragon.Architecture.Aop;
using Oragon.Architecture.Business;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.Aop.ExceptionHandling
{
	public class ExceptionHandlerAroundAdvice : IMethodInterceptor
	{
		private Type BusinessExceptionType { get; set; }

		private Oragon.Architecture.Logging.ILogger Logger { get; set; }

		private string GenericErrorMessage { get; set; }

		private bool EnableDebug { get; set; }

		public object Invoke(IMethodInvocation invocation)
		{
			ExceptionHandlingAttribute currentAttribute = this.GetAttribute(invocation);
			string targetTypeFullName = string.Concat(invocation.TargetType.Namespace, ".", invocation.TargetType.Name);
			string targetMethod = string.Concat(targetTypeFullName, ".", invocation.Method);

			object returnValue = null;
			using (LogContext logContext = new LogContext())
			{
				logContext.SetValue("Type", targetTypeFullName);
				logContext.SetValue("Method", targetMethod);

				try
				{
					if (this.EnableDebug)
						this.Logger.Log(targetTypeFullName, string.Concat("Begin ", targetMethod), Oragon.Architecture.Logging.LogLevel.Debug, logContext.GetDictionary());

					returnValue = invocation.Proceed();

					if (this.EnableDebug)
						this.Logger.Log(targetTypeFullName, string.Concat("End ", targetMethod), Oragon.Architecture.Logging.LogLevel.Debug, logContext.GetDictionary());

				}
				catch (UndefinedException)
				{
					throw;
				}
				catch (FaultException)
				{
					throw;
				}
				catch (Exception ex)
				{
					Type exceptionType = ex.GetType();

					bool isBusinessException = (this.BusinessExceptionType.IsAssignableFrom(exceptionType));

					string exceptionTypeKey = isBusinessException ? "BusinessException" : "ApplicationException";

					logContext.SetValue(exceptionTypeKey, string.Concat(exceptionType.Namespace, ".", exceptionType.Name));

					this.Logger.Log(targetTypeFullName, ex.ToString(), Oragon.Architecture.Logging.LogLevel.Warn, logContext.GetDictionary());

					if (currentAttribute.Strategy.HasFlag(ExceptionHandlingStrategy.ContinueRunning))
					{
						//Do Nothing -> Suppress Exception
					}
					else if (currentAttribute.Strategy.HasFlag(ExceptionHandlingStrategy.BreackOnException))
					{
						if (isBusinessException)
						{
							if (System.ServiceModel.OperationContext.Current == null)
								throw;
							else
								throw new FaultException(ex.Message);
						}
						else
						{
							if (System.ServiceModel.OperationContext.Current == null)
								throw new UndefinedException(this.GenericErrorMessage);
							else
								throw new FaultException(this.GenericErrorMessage);
						}
					}
				}
			}
			return returnValue;
		}

		private ExceptionHandlingAttribute GetAttribute(IMethodInvocation invocation)
		{
			ExceptionHandlingAttribute attribute = invocation.GetAttibutes<ExceptionHandlingAttribute>().FirstOrDefault();
			if (attribute == null)
			{
				attribute = new ExceptionHandlingAttribute(ExceptionHandlingStrategy.BreackOnException);
			}

			if (attribute.Strategy.HasFlag(ExceptionHandlingStrategy.ContinueRunning) && (invocation.Method.ReturnType != typeof(void)))
			{
				throw new InvalidOperationException("To use ExceptionHandlingStrategy.ContinueRunning, the method must return void");
			}
			return attribute;
		}

	}
}
