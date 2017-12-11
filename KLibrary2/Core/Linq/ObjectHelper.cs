using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keiho.Linq
{
    /// <summary>
    /// オブジェクトに対する拡張メソッドを提供します。
    /// </summary>
    public static class ObjectHelper
    {
        /// <summary>
        /// オブジェクトに対して、指定された処理を実行します。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="element"/> の型。</typeparam>
        /// <param name="element">任意のオブジェクト。</param>
        /// <param name="action">オブジェクトに対して実行される処理。</param>
        /// <returns>対象のオブジェクト。</returns>
        public static TSource Act<TSource>(this TSource element, Action<TSource> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            action(element);
            return element;
        }

        public static TSource ChangeIf<TSource>(this TSource element, TSource condition, TSource newValue)
        {
            return object.Equals(element, condition) ? newValue : element;
        }

        /// <summary>
        /// オブジェクトを変換します。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="element"/> の型。</typeparam>
        /// <typeparam name="TResult"><paramref name="selector"/> によって返される値の型。</typeparam>
        /// <param name="element">任意のオブジェクト。</param>
        /// <param name="selector">オブジェクトに適用する変換関数。</param>
        /// <returns>変換された値。</returns>
        public static TResult Select<TSource, TResult>(this TSource element, Func<TSource, TResult> selector)
        {
            if (selector == null)
            {
                throw new ArgumentNullException("selector");
            }

            return selector(element);
        }

        /// <summary>
        /// オブジェクトを、それを唯一の要素とする列挙子に変換します。
        /// </summary>
        /// <typeparam name="TSource"><paramref name="element"/> の型。</typeparam>
        /// <param name="element">任意のオブジェクト。</param>
        /// <returns>指定されたオブジェクトを唯一の要素とする <see cref="System.Collections.Generic.IEnumerable&lt;T&gt;"/>。</returns>
        public static IEnumerable<TSource> ToEnumerable<TSource>(this TSource element)
        {
            yield return element;
        }
    }
}
