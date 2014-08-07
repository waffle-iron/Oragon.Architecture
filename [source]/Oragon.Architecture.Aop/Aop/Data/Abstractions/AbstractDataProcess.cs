using Spring.Objects.Factory.Attributes;
using System;

namespace Oragon.Architecture.Aop.Data.Abstractions
{
	public abstract class AbstractDataProcess<ContextType, AttributeType>
		where ContextType : AbstractContext<AttributeType>
		where AttributeType : AbstractContextAttribute
	{
		#region Protected Properties

		protected virtual ContextType ObjectContext
		{
			get
			{
				ContextType returnValue = Spring.Threading.LogicalThreadContext.GetData(this.ObjectContextKey) as ContextType;
				if (returnValue == null)
					throw new InvalidOperationException(string.Format("AbstractDataProcess cannot find context with key '{0}'", this.ObjectContextKey));
				return returnValue;
			}
		}

		[Required]
		protected string ObjectContextKey { get; set; }

		#endregion Protected Properties
	}
}