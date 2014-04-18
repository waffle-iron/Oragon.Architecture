using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Oragon.CodeGen.MetaData.Plugin
{
	public class EnglishLanguageConvention : ILanguageConvention
	{
		public string GetPlural(string text)
		{
			string returnValue = text;
			if (returnValue.EndsWith("y"))
				returnValue = string.Concat(returnValue.Substring(0, returnValue.Length - 1), "ies");
			else
				returnValue = string.Concat(returnValue, "s");
			return returnValue;
		}
	}
}
