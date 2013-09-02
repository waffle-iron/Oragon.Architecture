using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.CodeGen.MetaData.Plugin;

namespace Oragon.CodeGen.MetaData.Domain
{
	public class ValueTypeProperty : Property
	{
		public Oragon.CodeGen.MetaData.DataBase.IColumn Column { get; private set; }

		public ValueTypeProperty(Entity owner, Oragon.CodeGen.MetaData.DataBase.IColumn column)
			: base(owner)
		{
			this.Column = column;
		}

		public override string Name
		{
			get
			{
				return this.Owner.Model.NameStretegy.GetName(this);
			}

		}

		public override string Type
		{
			get
			{
				string returnValue = null;
				if (this.Column.LanguageType.Contains("[]") || this.Column.LanguageType == "string")
					returnValue = this.Column.LanguageType;
				else
				{
					//Exceção, quando uma PK AutoKey é gerada com Seek Numeric
					if (this.Column.IsInPrimaryKey && this.Column.IsAutoKey && this.Column.DataTypeName == "numeric" && this.Column.NumericScale == 0)
						returnValue = "long" + (this.Column.IsNullable ? "?" : string.Empty);
					else
						returnValue = this.Column.LanguageType + (this.Column.IsNullable ? "?" : string.Empty);
				}
				return returnValue;
			}
		}

		public override string PropertyGetAndSet { get { return "get; set;"; } }


		public override string ToString()
		{
			return string.Format("{0} {1}", this.Type, this.Name);
		}
	}
}
