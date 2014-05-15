using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.Text
{
	public abstract class FormatStrategy
	{
		public static FormatStrategy None;

		static FormatStrategy()
		{
			FormatStrategy.None = new NoneFormatStrategy();
		}

		protected char[] splitter = new char[] { ' ', '\t' };

		public abstract string Format(string originalFormat);
	}


	public class NoneFormatStrategy : FormatStrategy
	{
		public override string Format(string original)
		{
			return original;
		}
	}
}
