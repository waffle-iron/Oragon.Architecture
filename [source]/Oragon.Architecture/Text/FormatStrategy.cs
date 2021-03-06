﻿namespace Oragon.Architecture.Text
{
	public abstract class FormatStrategy
	{
		#region Public Fields

		public static FormatStrategy None = new NoneFormatStrategy();

		#endregion Public Fields

		#region Protected Fields

		protected char[] splitter = new char[] { ' ', '\t' };

		#endregion Protected Fields



		#region Public Methods

		public abstract string Format(string original);

		#endregion Public Methods
	}

	public class NoneFormatStrategy : FormatStrategy
	{
		#region Public Methods

		public override string Format(string original)
		{
			return original;
		}

		#endregion Public Methods
	}
}