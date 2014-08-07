using System;

namespace Oragon.Architecture.ExtendedTypes
{
	[Serializable]
	public class DateRange : Range<DateTime>
	{
		#region Public Properties

		public Nullable<TimeSpan> TimeSpan
		{
			get { return endValue - startValue; }
		}

		#endregion Public Properties
	}
}