using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace Keiho.Linq
{
    /// <summary>
    /// <see cref="System.Linq.Enumerable"/> クラスをさらに拡張したメソッドを提供します。
    /// </summary>
    public static class EnumerableHelper
    {
        public static IEnumerable<TSource> GetRecursively<TSource>(TSource seed, Func<TSource, IEnumerable<TSource>> getChildren)
        {
            if (getChildren == null) throw new ArgumentNullException("getChildren");

            var children = getChildren(seed).ToArray();
            return children.Concat(children.SelectMany(s => GetRecursively(s, getChildren)));
        }

        public static IEnumerable<TResult> GetRecursively<TSource, TResult>(TSource seed, Func<TSource, IEnumerable<TSource>> getChildren, Func<TSource, IEnumerable<TResult>> selector)
        {
            if (getChildren == null) throw new ArgumentNullException("getChildren");
            if (selector == null) throw new ArgumentNullException("selector");

            return selector(seed).Concat(getChildren(seed).SelectMany(s => GetRecursively(s, getChildren, selector)));
        }

        public static IEnumerable<TSource> ForEach<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");

            return source.Select(e =>
            {
                action(e);
                return e;
            });
        }

        public static void ForEachExecute<TSource>(this IEnumerable<TSource> source, Action<TSource> action)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (action == null) throw new ArgumentNullException("action");

            foreach (var element in source)
            {
                action(element);
            }
        }

        [Obsolete]
        public static void Execute<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            foreach (var element in source)
            {
            }
        }

        public static bool Contains<TSource>(this IEnumerable<TSource> source, TSource value, Func<TSource, TSource, bool> comparer)
        {
            if (source == null) throw new ArgumentNullException("source");
            if (comparer == null) return source.Contains(value);

            return source.Any(e => comparer(e, value));
        }

        public static bool ContainsIgnoreCase(this IEnumerable<string> source, string value)
        {
#if !SILVERLIGHT
            return source.Contains(value, (s1, s2) => string.Equals(s1, s2, StringComparison.InvariantCultureIgnoreCase));
#else
            return source.Contains(value, (s1, s2) => string.Equals(s1, s2, StringComparison.OrdinalIgnoreCase));
#endif
        }

        public static bool SequenceEqual<TSource1, TSource2>(this IEnumerable<TSource1> first, IEnumerable<TSource2> second, Func<TSource1, TSource2, bool> comparer)
        {
            if (first == null) throw new ArgumentNullException("first");
            if (second == null) throw new ArgumentNullException("second");
            if (comparer == null) throw new ArgumentNullException("comparer");

            using (var e1 = first.GetEnumerator())
            using (var e2 = second.GetEnumerator())
            {
                while (true)
                {
                    bool hasNext1 = e1.MoveNext();
                    bool hasNext2 = e2.MoveNext();

                    if (hasNext1 && hasNext2)
                    {
                        if (!comparer(e1.Current, e2.Current))
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return !hasNext1 && !hasNext2;
                    }
                }
            }
        }

        /// <summary>
        /// <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/> から <see cref="System.Collections.ObjectModel.Collection&lt;T&gt;"/> を作成します。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> の要素の型。</typeparam>
        /// <param name="source"><see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>。</param>
        /// <returns>入力シーケンスの要素を含む <see cref="System.Collections.ObjectModel.Collection&lt;T&gt;"/>。</returns>
        public static Collection<TSource> ToCollection<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

            return new Collection<TSource>((source as IList<TSource>) ?? source.ToList());
        }

        /// <summary>
        /// <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/> から <see cref="System.Collections.ObjectModel.ObservableCollection&lt;T&gt;"/> を作成します。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="source"/> の要素の型。</typeparam>
        /// <param name="source"><see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>。</param>
        /// <returns>入力シーケンスの要素を含む <see cref="System.Collections.ObjectModel.ObservableCollection&lt;T&gt;"/>。</returns>
        public static ObservableCollection<TSource> ToObservableCollection<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null) throw new ArgumentNullException("source");

#if !SILVERLIGHT
            return new ObservableCollection<TSource>(source);
#else
            var collection = new ObservableCollection<TSource>();
            source.ForEachExecute(collection.Add);
            return collection;
#endif
        }

        private static int? CountAsCollection<TSource>(this IEnumerable<TSource> source)
        {
            var c1 = source as ICollection<TSource>;
            if (c1 != null)
            {
                return c1.Count;
            }

            var c2 = source as ICollection;
            if (c2 != null)
            {
                return c2.Count;
            }

            return null;
        }
    }
}
