using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.CodeGen.MetaData.Plugin;

namespace Oragon.CodeGen.MetaData.Domain
{
	public class DomainModel
	{
		public List<Entity> Entities { get; private set; }

		public DomainModel()
		{
			this.Entities = new List<Entity>();
		}

		public bool CanInsert { get; set; }
		public bool CanUpdate { get; set; }
		public bool CanDelete { get; set; }

		public INameStretegy NameStretegy { get; set; }

	}
}
