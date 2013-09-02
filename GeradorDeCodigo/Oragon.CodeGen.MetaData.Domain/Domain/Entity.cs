using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.CodeGen.MetaData.DataBase;
using Oragon.CodeGen.MetaData.Plugin;

namespace Oragon.CodeGen.MetaData.Domain
{
	public class Entity
	{
		public DomainModel Model { get; private set; }
		public ITable Table { get; private set; }
		public List<Property> Properties { get; private set; }

		public Entity(DomainModel model, ITable currentTable)
		{
			this.Model = model;
			this.Table = currentTable;
			this.Properties = new List<Property>();
		}

		public string Name { get { return  this.Model.NameStretegy.GetName(this); } }
		public string PluralName { get { return this.Model.NameStretegy.GetPluralName(this); } }

	}
}
