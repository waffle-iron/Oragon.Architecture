using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Text
{
	public class CamelCaseFormatStrategy : FormatStrategy
	{
		public override string Format(string original)
		{
			if (string.IsNullOrWhiteSpace(original))
				return original;

			if (this.splitter.Any(currentChar => original.Contains(currentChar)))
			{
				string[] parts = original.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
				return string.Join(string.Empty, parts.Select(part => this.Format(part)).ToArray());
			}
			else if (original.Length == 1)
			{
				return original.ToUpper();
			}
			else
			{
				string returnValue = original.Substring(0, 1).ToUpper() + original.Substring(1, original.Length - 1);
				return returnValue;
			}
		}
	}
}
