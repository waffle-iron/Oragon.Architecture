﻿using AopAlliance.Intercept;
using Oragon.Architecture.Aop.Data.Abstractions;
using Oragon.Architecture.Data.ConnectionStrings;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Oragon.Architecture.Aop.Data.Redis
{
	[CLSCompliant(false)]
	public class RedisContextAroundAdvice : AbstractContextAroundAdvice<RedisContextAttribute, RedisContext>
	{
		#region Public Properties

		public List<RedisConnectionString> ConnectionStrings { get; set; }

		#endregion Public Properties

		#region Protected Properties

		protected override Func<RedisContextAttribute, bool> AttributeQueryFilter
		{
			get
			{
				return (it =>
				{
					it.RedisConnectionString = this.ConnectionStrings.Where(localConnecions => localConnecions.Key == it.ContextKey).FirstOrDefault();
					return (it.RedisConnectionString != null);
				});
			}
		}

		protected override string ContextStackListKey
		{
			get { return "Oragon.Architecture.AOP.Data.RedisContextAroundAdvice.Contexts"; }
		}

		#endregion Protected Properties

		#region Protected Methods

		protected override object Invoke(AopAlliance.Intercept.IMethodInvocation invocation, IEnumerable<RedisContextAttribute> contextAttributes)
		{
			object returnValue = null;
			returnValue = this.InvokeUsingContext(invocation, new Stack<RedisContextAttribute>(contextAttributes));
			return returnValue;
		}

		#endregion Protected Methods

		#region Private Methods

		private object InvokeUsingContext(IMethodInvocation invocation, Stack<RedisContextAttribute> contextAttributesStack)
		{
			//Este método é chamado recursivamente, removendo o item do Stack sempre que houver um. Até que não haja nenhum. Quando não houver nenhum item mais, ele efetivamente
			//manda executar a chamada ao método de destino.
			//Esse controle é necessário pois as os "Usings" de Contexto, Sessão e Transação precisam ser encadeados
			object returnValue = null;
			if (contextAttributesStack.Count == 0)
			{
				returnValue = invocation.Proceed();
			}
			else
			{
				//Obtendo o primeiro primeiro último RequiredPersistenceContextAttribute da stack, removendo-o.
				RedisContextAttribute currentContextAttribute = contextAttributesStack.Pop();
				//Criando o contexto
				using (RedisContext currentContext = new RedisContext(currentContextAttribute, this.ContextStack))
				{
					returnValue = this.InvokeUsingContext(invocation, contextAttributesStack);
				}
			}
			return returnValue;
		}

		#endregion Private Methods
	}
}