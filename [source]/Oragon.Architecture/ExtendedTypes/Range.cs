using System;
using System.Diagnostics;

namespace Oragon.Architecture.ExtendedTypes
{
	public class Range<T> : IEquatable<Range<T>>
		 where T : struct, IComparable<T>, IEquatable<T>
	{
		#region Protected Fields

		protected Nullable<T> startValue, endValue;

		#endregion Protected Fields

		#region Public Constructors

		public Range()
			: this(new Nullable<T>(), new Nullable<T>())
		{
		}

		[DebuggerStepThrough]
		public Range(Nullable<T> start, Nullable<T> end)
		{
			AssertstartFollowsend(start, end);
			this.startValue = start;
			this.endValue = end;
		}

		#endregion Public Constructors

		#region Public Properties

		public Nullable<T> End
		{
			get { return endValue; }
			set
			{
				AssertstartFollowsend(this.startValue, value);
				endValue = value;
			}
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

		#endregion Public Properties

		#region Public Methods

		public bool Equals(Range<T> other)
		{
			if (object.ReferenceEquals(other, null)) return false;

			return
				(Nullable.Compare<T>(this.startValue, other.startValue) == 0)
				&&
				(Nullable.Compare<T>(this.endValue, other.endValue) == 0);
		}

		[DebuggerStepThrough]
		public Range<T> GetIntersection(Range<T> other)
		{
			if (!Intersects(other)) throw new InvalidOperationException("Ranges do not intersect");
			return new Range<T>(GetLaterstart(other.Start), GetEarlierend(other.End));
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

		#endregion Public Methods

		#region Private Methods

		[DebuggerStepThrough]
		private void AssertstartFollowsend(Nullable<T> start, Nullable<T> end)
		{
			if (
				(start.HasValue && end.HasValue) && (end.Value.CompareTo(start.Value) < 0))
				throw new InvalidOperationException("Start must be less than or equal to End");
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

		private Nullable<T> GetLaterstart(Nullable<T> other)
		{
			return Nullable.Compare<T>(startValue, other) >= 0 ? startValue : other;
		}

		#endregion Private Methods
	}
}