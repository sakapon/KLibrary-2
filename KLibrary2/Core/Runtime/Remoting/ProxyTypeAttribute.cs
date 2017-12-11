using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace Keiho.Runtime.Remoting
{
    /// <summary>
    /// オブジェクトに対するプロキシの型を指定します。
    /// </summary>
    /// <remarks>
    /// この属性を適用するクラスは、<see cref="ContextBoundObject"/> クラスを継承していなければなりません。
    /// </remarks>
    public sealed class ProxyTypeAttribute : ProxyAttribute
    {
        /// <summary>
        /// プロキシの型を取得します。
        /// </summary>
        /// <value>プロキシの型。</value>
        public Type ProxyType { get; private set; }

        /// <summary>
        /// <see cref="ProxyTypeAttribute"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        /// <param name="proxyType">プロキシの型。</param>
        public ProxyTypeAttribute(Type proxyType)
        {
            ProxyType = proxyType;
        }

        /// <summary>
        /// 初期化されていない透過プロキシを作成します。
        /// </summary>
        /// <param name="serverType">プロキシの対象となるオブジェクトの型。</param>
        /// <returns>初期化されていない透過プロキシ。</returns>
        public override MarshalByRefObject CreateInstance(Type serverType)
        {
            var proxy = (RealProxy)Activator.CreateInstance(ProxyType);
            return (MarshalByRefObject)proxy.GetTransparentProxy();
        }
    }
}
