using AopAlliance.Intercept;
using Oragon.Architecture.Aop.Data.Abstractions;
using Spring.Objects.Factory.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using NH = NHibernate;

namespace Oragon.Architecture.Aop.Data.NHibernate
{
	public class NHContextAroundAdvice : AbstractContextAroundAdvice<NHContextAttribute, NHContext>
	{
		#region Dependence Injection

		private bool ElevateToSystemTransactionsIfRequired { get; set; }

		private NH.IInterceptor Interceptor { get; set; }

		[Required]
		private List<ISessionFactoryBuilder> SessionFactoryBuilders { get; set; }

		#endregion Dependence Injection

		#region Protected Properties

		protected override Func<NHContextAttribute, bool> AttributeQueryFilter
		{
			get
			{
				return (it =>
				{
					it.SessionFactoryBuilder = this.SessionFactoryBuilders.FirstOrDefault(sfb => sfb.ObjectContextKey == it.ContextKey);
					return (it.SessionFactoryBuilder != null);
				});
			}
		}

		protected override string ContextStackListKey
		{
			get { return "Oragon.Architecture.Aop.Data.NHContextAroundAdvice.Contexts"; }
		}

		#endregion Protected Properties

		#region Protected Methods

		protected override object Invoke(IMethodInvocation invocation, IEnumerable<NHContextAttribute> contextAttributes)
		{
			object returnValue = null;

			if (this.ElevateToSystemTransactionsIfRequired && contextAttributes.Count(it => it.IsTransactional.HasValue && it.IsTransactional.Value) > 1)
			{
				using (var scope = new System.Transactions.TransactionScope(System.Transactions.TransactionScopeOption.Required))
				{
					returnValue = this.InvokeUsingContext(invocation, new Stack<NHContextAttribute>(contextAttributes));
					scope.Complete();
				}
			}
			else
			{
				returnValue = this.InvokeUsingContext(invocation, new Stack<NHContextAttribute>(contextAttributes));
			}
			return returnValue;
		}

		#endregion Protected Methods

		#region Private Methods

		private object InvokeUsingContext(IMethodInvocation invocation, Stack<NHContextAttribute> contextAttributesStack)
		{
			//Este m�todo � chamado recursivamente, removendo o item do Stack sempre que houver um. At� que n�o haja nenhum. Quando n�o houver nenhum item mais, ele efetivamente
			//manda executar a chamada ao m�todo de destino.
			//Esse controle � necess�rio pois as os "Usings" de Contexto, Sess�o e Transa��o precisam ser encadeados
			object returnValue = null;
			if (contextAttributesStack.Count == 0)
			{
				returnValue = invocation.Proceed();
			}
			else
			{
				//Obtendo o primeiro primeiro �ltimo RequiredPersistenceContextAttribute da stack, removendo-o.
				NHContextAttribute currentContextAttribute = contextAttributesStack.Pop();
				//Criando o contexto
				using (NHContext currentContext = new NHContext(currentContextAttribute, this.Interceptor, this.ContextStack))
				{
					returnValue = this.InvokeUsingContext(invocation, contextAttributesStack);
					currentContext.Complete();
				}
			}
			return returnValue;
		}

		#endregion Private Methods
	}
}