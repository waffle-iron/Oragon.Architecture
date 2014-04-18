using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.CodeGen.MetaData.Domain
{
	public class ManyToOneProperty : Property
	{
		public Oragon.CodeGen.MetaData.DataBase.IForeignKey ForeignKey { get; private set; }

		public ManyToOneProperty(Entity owner, Oragon.CodeGen.MetaData.DataBase.IForeignKey foreignKey)
			: base(owner)
		{
			this.ForeignKey = foreignKey;
		}

		public override string Name
		{
			get
			{
				string returnValue = this.Owner.Model.NameStretegy.GetName(this);
				return returnValue;
			}
		}

		public override string Type { get { return this.Owner.Model.NameStretegy.GetName(new Entity(this.Owner.Model, this.ForeignKey.PrimaryTable)); } }

		public override string ToString()
		{
			return string.Format("Relação NX1 {0} <- {1} | {2} {3}", this.ForeignKey.ForeignTable.Name, this.ForeignKey.PrimaryTable.Name, this.Type, this.Name);
		}

	}
}
