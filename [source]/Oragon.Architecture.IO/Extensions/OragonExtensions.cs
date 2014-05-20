using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Linq;

namespace Oragon.Architecture.Extensions
{
	///<summary>
	///Provides a set of extension methods dedicated to enumerables.
	///</summary>
	public static partial class OragonExtensions
	{
		private class CollectionReadOnlyWrapper<T> : IReadOnlyCollection<T>
		{
			internal CollectionReadOnlyWrapper(ICollection<T> collection)
			{
				Debug.Assert(collection != null);
				m_Collection = collection;
			}

			private readonly ICollection<T> m_Collection;

			public int Count
			{
				get { return m_Collection.Count; }
			}

			public IEnumerator<T> GetEnumerator()
			{
				return m_Collection.GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return ((IEnumerable)m_Collection).GetEnumerator();
			}
		}

		private sealed class ListReadOnlyWrapper<T> : CollectionReadOnlyWrapper<T>, IReadOnlyList<T>
		{
			internal ListReadOnlyWrapper(IList<T> list)
				: base(list)
			{
				Debug.Assert(list != null);
				m_List = list;
			}

			private readonly IList<T> m_List;

			public T this[int index]
			{
				get { return m_List[index]; }
			}
		}

		private sealed class DicoLookup<TKey, T> : Dictionary<TKey, IEnumerable<T>>, ILookup<TKey, T>
		{
			public new IEnumerator<IGrouping<TKey, T>> GetEnumerator()
			{
				var dico = this as Dictionary<TKey, IEnumerable<T>>;
				foreach (var pair in dico)
				{
					yield return new Grouping(pair.Key, pair.Value);
				}
			}

			bool ILookup<TKey, T>.Contains(TKey key)
			{
				return this.ContainsKey(key);
			}

			// HACK: 17Aug2012: these two are needed to avoid complex Code Contract warning!
			public new int Count { get { return base.Count; } }

			public new IEnumerable<T> this[TKey key] { get { return base[key]; } set { base[key] = value; } }

			private sealed class Grouping : IGrouping<TKey, T>
			{
				internal Grouping(TKey key, IEnumerable<T> seq)
				{
					Debug.Assert(seq != null);
					m_Key = key;
					m_Seq = seq;
				}

				private readonly TKey m_Key;
				private readonly IEnumerable<T> m_Seq;

				public IEnumerator<T> GetEnumerator()
				{
					return m_Seq.GetEnumerator();
				}

				IEnumerator IEnumerable.GetEnumerator()
				{
					return GetEnumerator();
				}

				public TKey Key { get { return m_Key; } }
			}
		}


		private static IEnumerable<T> UnionIterator<T>(HashSet<T> hashset, IEnumerable<T> seq)
		{
			Debug.Assert(hashset != null);
			Debug.Assert(seq != null);

			foreach (T t in hashset)
			{
				yield return t;
			}
			foreach (T t in seq)
			{
				if (hashset.Contains(t))
				{
					continue;
				}
				yield return t;
			}
		}

	}
}