using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.Architecture.AOP.Data.Abstractions
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
				throw new InvalidOperationException("Era experado como primeiro item do Stack o próprio elemento");

			if (this.ContextStack.Count == 0)
			{
				Spring.Threading.LogicalThreadContext.SetData(this.ContextKey, null);
				Spring.Threading.LogicalThreadContext.FreeNamedDataSlot(this.ContextKey);
			}
			else
			{
				var parentContext = this.ContextStack.Peek();
				Spring.Threading.LogicalThreadContext.SetData(parentContext.ContextKey, parentContext);
			}
		}
		

		~AbstractContext()
		{
			// Simply call Dispose(false).
			Dispose(false);
		}

		#endregion

	}
}
