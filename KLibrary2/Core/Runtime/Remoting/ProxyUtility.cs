using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keiho.Runtime.Remoting
{
    /// <summary>
    /// オブジェクトに対するプロキシに関するメソッドを提供します。
    /// </summary>
    public static class ProxyUtility
    {
        /// <summary>
        /// 元となるオブジェクトを作成し、それに対する透過プロキシを作成します。
        /// </summary>
        /// <typeparam name="TTarget">プロキシの対象となるオブジェクトの型。インターフェイスまたは <see cref="MarshalByRefObject"/> から派生した型に限定されます。</typeparam>
        /// <typeparam name="TProxy">プロキシの型。</typeparam>
        /// <returns>透過プロキシ。</returns>
        public static TTarget Create<TTarget, TProxy>()
            where TTarget : class, new()
            where TProxy : ProxyBase<TTarget>, new()
        {
            var proxy = new TProxy();
            proxy.Target = new TTarget();
            return proxy.GetTransparentProxy();
        }

        /// <summary>
        /// 元となるオブジェクトを作成せずに透過プロキシを作成します。
        /// </summary>
        /// <typeparam name="TTarget">プロキシの対象となるオブジェクトの型。インターフェイスまたは <see cref="MarshalByRefObject"/> から派生した型に限定されます。</typeparam>
        /// <typeparam name="TProxy">プロキシの型。</typeparam>
        /// <returns>透過プロキシ。</returns>
        public static TTarget CreateFake<TTarget, TProxy>()
            where TTarget : class
            where TProxy : ProxyBase<TTarget>, new()
        {
            var proxy = new TProxy();
            return proxy.GetTransparentProxy();
        }

        /// <summary>
        /// 指定されたオブジェクトに対する透過プロキシを作成します。
        /// </summary>
        /// <typeparam name="TTarget">プロキシの対象となるオブジェクトの型。インターフェイスまたは <see cref="MarshalByRefObject"/> から派生した型に限定されます。</typeparam>
        /// <typeparam name="TProxy">プロキシの型。</typeparam>
        /// <param name="target">プロキシの対象となるオブジェクト。</param>
        /// <returns>透過プロキシ。</returns>
        public static TTarget ToProxy<TTarget, TProxy>(TTarget target)
            where TTarget : class
            where TProxy : ProxyBase<TTarget>, new()
        {
            if (target == null) throw new ArgumentNullException("target");

            var proxy = new TProxy();
            proxy.Target = target;
            return proxy.GetTransparentProxy();
        }
    }
}
