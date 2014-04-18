using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Oragon.Architecture.Extensions;

namespace Oragon.Architecture.ExtendedTypes
{
	[Serializable]
	public class DateRange : Range<DateTime>
	{
		public Nullable<TimeSpan> TimeSpan
		{
			get { return endValue - startValue; }
		}
	}
}
