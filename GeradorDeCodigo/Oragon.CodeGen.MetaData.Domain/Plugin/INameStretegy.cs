using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Oragon.CodeGen.MetaData.Domain;

namespace Oragon.CodeGen.MetaData.Plugin
{
	public interface INameStretegy
	{
		string GetName(Entity entity);
		string GetName(Property property);
		string GetPluralName(Entity entity);
	}
}
