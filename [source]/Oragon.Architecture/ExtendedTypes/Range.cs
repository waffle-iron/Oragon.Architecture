using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oragon.Architecture.ExtendedTypes
{

	public class Range<T> : IEquatable<Range<T>>
		 where T : struct, IComparable<T>, IEquatable<T>
	{
		protected Nullable<T> startValue, endValue;
		public Range() : this(new Nullable<T>(), new Nullable<T>()) { }

		[DebuggerStepThrough]
		public Range(Nullable<T> start, Nullable<T> end)
		{
			AssertstartFollowsend(start, end);
			this.startValue = start;
			this.endValue = end;
		}

		public Nullable<T> Start
		{
			get { return startValue; }
			set
			{
				AssertstartFollowsend(value, this.endValue);
				startValue = value;
			}
		}
		public Nullable<T> End
		{
			get { return endValue; }
			set
			{
				AssertstartFollowsend(this.startValue, value);
				endValue = value;
			}
		}

		[DebuggerStepThrough]
		private void AssertstartFollowsend(Nullable<T> start, Nullable<T> end)
		{
			if (
				(start.HasValue && end.HasValue) && (end.Value.CompareTo(start.Value) < 0))
				throw new InvalidOperationException("Start must be less than or equal to End");
		}

		[DebuggerStepThrough]
		public Range<T> GetIntersection(Range<T> other)
		{
			if (!Intersects(other)) throw new InvalidOperationException("Ranges do not intersect");
			return new Range<T>(GetLaterstart(other.Start), GetEarlierend(other.End));
		}
		private Nullable<T> GetLaterstart(Nullable<T> other)
		{
			return Nullable.Compare<T>(startValue, other) >= 0 ? startValue : other;
		}
		private Nullable<T> GetEarlierend(Nullable<T> other)
		{
			//!end.HasValue == +infinity, not negative infinity
			//as is the case with !start.HasValue
			if (Nullable.Compare<T>(endValue, other) == 0) return other;
			if (endValue.HasValue && !other.HasValue) return endValue;
			if (!endValue.HasValue && other.HasValue) return other;
			return (Nullable.Compare<T>(endValue, other) >= 0) ? other : endValue;
		}
		public bool Intersects(Range<T> other)
		{
			if (
				(
					this.startValue.HasValue
					&&
					other.End.HasValue
					&&
					other.End.Value.CompareTo(this.startValue.Value) < 0
				)
				||
				(
					this.endValue.HasValue
					&&
					other.Start.HasValue
					&&
					other.Start.Value.CompareTo(this.endValue.Value) > 0
				)
				||
				(
					other.Start.HasValue
					&&
					this.endValue.HasValue
					&&
					this.endValue.Value.CompareTo(other.Start.Value) < 0
				)
				||
				(
					other.End.HasValue
					&&
					this.startValue.HasValue
					&&
					this.startValue.Value.CompareTo(other.End.Value) > 0
				)
			)
			{
				return false;
			}
			return true;
		}
		public bool Equals(Range<T> other)
		{
			if (object.ReferenceEquals(other, null)) return false;

			return
				(Nullable.Compare<T>(this.startValue, other.startValue) == 0)
				&&
				(Nullable.Compare<T>(this.endValue, other.endValue) == 0);
		}


	}
}
