using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.CodeGen.MetaData.Domain
{
	public abstract class Property
	{
		public Entity Owner { get; private set; }

		public Property(Entity owner)
		{
			this.Owner = owner;
		}

		public abstract string Name { get; }

		public abstract string Type { get; }

		public virtual string PropertyGetAndSet { get { return "get; set;"; } }

		public virtual string DeclarativeType { get { return this.Type; } }

		public virtual string SingleItemType { get { return this.Type; } }


	}
}
