using System;
using System.Collections.Generic;
using System.Linq;

namespace Oragon.Architecture.Aop.Data.Abstractions
{
	public abstract class AbstractContext<AttributeType> : IDisposable
		where AttributeType : AbstractContextAttribute
	{
		protected Stack<AbstractContext<AttributeType>> ContextStack { get; private set; }

		protected AttributeType ContextAttribute { get; private set; }

		public AbstractContext(AttributeType contextAttribute, Stack<AbstractContext<AttributeType>> contextStack)
		{
			this.ContextAttribute = contextAttribute;
			this.ContextStack = contextStack;
			Spring.Threading.LogicalThreadContext.SetData(this.ContextKey, this);
			this.ContextStack.Push(this);
		}

		protected string ContextKey
		{
			get { return this.ContextAttribute.ContextKey; }
		}

		#region Dispose Methods

		private bool disposed = false;

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		private void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					this.DisposeContext();
				}
				this.DisposeFields();
				disposed = true;
			}
		}

		protected virtual void DisposeFields()
		{
			this.ContextStack = null;
			this.ContextAttribute = null;
		}

		protected virtual void DisposeContext()
		{
			var topInstance = this.ContextStack.Pop();
			if (topInstance != this)
				throw new InvalidOperationException("The TOP item of Stack, must be me");

			AbstractContext<AttributeType> firstParentWithSameKey = this.ContextStack.FirstOrDefault(it => it.ContextKey == this.ContextKey);

			if (this.ContextStack.Count == 0 || firstParentWithSameKey == null)
			{
				Spring.Threading.LogicalThreadContext.SetData(this.ContextKey, null);
				Spring.Threading.LogicalThreadContext.FreeNamedDataSlot(this.ContextKey);
			}
			else
			{
				Spring.Threading.LogicalThreadContext.SetData(firstParentWithSameKey.ContextKey, firstParentWithSameKey);
			}
		}

		~AbstractContext()
		{
			// Simply call Dispose(false).
			Dispose(false);
		}

		#endregion Dispose Methods
	}
}