using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.CodeGen.MetaData.Plugin
{
	public interface ILanguageConvention
	{
		string GetPlural(string text);
	}
}
