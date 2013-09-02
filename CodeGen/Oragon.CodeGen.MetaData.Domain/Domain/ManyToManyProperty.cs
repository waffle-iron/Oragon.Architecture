using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.CodeGen.MetaData.Domain
{
	public class ManyToManyProperty : Property
	{
		public Oragon.CodeGen.MetaData.DataBase.IForeignKey LeftForeignKey { get; private set; }
		public Oragon.CodeGen.MetaData.DataBase.IForeignKey RightForeignKey { get; private set; }
		
		public Oragon.CodeGen.MetaData.DataBase.ITable Table { 
			get
			{
				return this.LeftForeignKey.ForeignTable;
			}
		}

		public ManyToManyProperty(Entity owner, Oragon.CodeGen.MetaData.DataBase.IForeignKey leftForeignKey, Oragon.CodeGen.MetaData.DataBase.IForeignKey rightForeignKey)
			: base(owner)
		{
			this.LeftForeignKey = leftForeignKey;
			this.RightForeignKey = rightForeignKey;
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
				return this.Owner.Model.NameStretegy.GetName(new Entity(this.Owner.Model, this.RightForeignKey.PrimaryTable));
			}
		}








		public override string ToString()
		{
			return string.Format("Relação NXN {0} <-> {1} | {2} {3}", this.LeftForeignKey.PrimaryTable.Name, this.RightForeignKey.PrimaryTable.Name, this.Type, this.Name);
		}
	}
}
