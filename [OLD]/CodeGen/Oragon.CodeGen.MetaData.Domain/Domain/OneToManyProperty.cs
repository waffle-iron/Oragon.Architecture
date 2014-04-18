using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.CodeGen.MetaData.Domain
{
	public class OneToManyProperty : Property
	{
		public Oragon.CodeGen.MetaData.DataBase.IForeignKey ForeignKey { get; private set; }

		public OneToManyProperty(Entity owner, Oragon.CodeGen.MetaData.DataBase.IForeignKey foreignKey)
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

		public override string Type
		{
			get
			{
				return string.Format("List<{0}>", this.SingleItemType);
			}
		}

		public override string DeclarativeType
		{
			get
			{
				return string.Format("IList<{0}>", this.SingleItemType);
			}
		}

		public override string SingleItemType
		{
			get
			{
				return this.Owner.Model.NameStretegy.GetName(new Entity(this.Owner.Model, this.ForeignKey.ForeignTable));
			}
		}

		public bool DeleteCascade
		{
			get
			{
				return (this.ForeignKey.DeleteRule == "CASCADE");
			}
		}

		public override string ToString()
		{
			return string.Format("Relação 1XN {0} -> {1} | {2} {3}", this.ForeignKey.PrimaryTable.Name, this.ForeignKey.ForeignTable.Name, this.Type, this.Name);
		}
	}
}
