using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Activation;
using System.Runtime.Remoting.Messaging;
using System.Runtime.Remoting.Proxies;
using System.Text;

namespace Keiho.Runtime.Remoting
{
    /// <summary>
    /// オブジェクトに対するプロキシを表します。
    /// </summary>
    /// <typeparam name="T">プロキシの対象となるオブジェクトの型。インターフェイスまたは <see cref="MarshalByRefObject"/> から派生した型に限定されます。</typeparam>
    [DebuggerNonUserCode]
    public class ProxyBase<T> : RealProxy where T : class
    {
        /// <summary>
        /// プロキシの対象となるオブジェクトを取得または設定します。
        /// </summary>
        /// <value>プロキシの対象となるオブジェクト。</value>
        public T Target { get; set; }

        /// <summary>
        /// <see cref="ProxyBase&lt;T&gt;"/> クラスの新しいインスタンスを初期化します。
        /// </summary>
        public ProxyBase()
            : base(typeof(T))
        {
        }

        /// <summary>
        /// 現在のインスタンスの透過プロキシを取得します。
        /// </summary>
        /// <returns>現在のインスタンスの透過プロキシ。</returns>
        public new T GetTransparentProxy()
        {
            return (T)base.GetTransparentProxy();
        }

        /// <summary>
        /// 指定されたコンストラクターまたはメソッドを呼び出します。
        /// </summary>
        /// <param name="msg">メソッドの呼び出しに関する情報を格納している <see cref="IMessage"/>。</param>
        /// <returns>呼び出されたメソッドが返す <see cref="IMessage"/>。</returns>
        public override IMessage Invoke(IMessage msg)
        {
            var ctorCall = msg as IConstructionCallMessage;
            if (ctorCall != null)
            {
                return InvokeConstructor(ctorCall);
            }

            var methodCall = msg as IMethodCallMessage;
            if (methodCall != null)
            {
                return InvokeMethod(methodCall);
            }

            throw new InvalidOperationException();
        }

        /// <summary>
        /// 指定されたコンストラクターを呼び出します。
        /// </summary>
        /// <param name="ctorCall">メソッドの呼び出しに関する情報を格納している <see cref="IConstructionCallMessage"/>。</param>
        /// <returns>呼び出されたメソッドが返す <see cref="IConstructionReturnMessage"/>。</returns>
        private IConstructionReturnMessage InvokeConstructor(IConstructionCallMessage ctorCall)
        {
            var ctorReturn = InitializeServerObject(ctorCall);
            Target = GetUnwrappedServer() as T;
            SetStubData(this, Target);
            return ctorReturn;
        }

        /// <summary>
        /// 指定されたメソッドを呼び出します。
        /// </summary>
        /// <param name="methodCall">メソッドの呼び出しに関する情報を格納している <see cref="IMethodCallMessage"/>。</param>
        /// <returns>呼び出されたメソッドが返す <see cref="IMethodReturnMessage"/>。</returns>
        protected virtual IMethodReturnMessage InvokeMethod(IMethodCallMessage methodCall)
        {
            var marshal = Target as MarshalByRefObject;

            if (marshal != null)
            {
                return RemotingServices.ExecuteMessage(marshal, methodCall);
            }
            else if (Target != null)
            {
                var returnValue = methodCall.MethodBase.Invoke(Target, methodCall.Args);
                return CreateNormalMethodReturn(returnValue, methodCall);
            }
            else
            {
                return CreateErrorMethodReturn(new InvalidOperationException("Target プロパティが設定されていないため、InvokeMethod メソッドをオーバーライドする必要があります。"), methodCall);
            }
        }

        public static IMethodReturnMessage CreateNormalMethodReturn(object returnValue, IMethodCallMessage methodCall)
        {
            return new ReturnMessage(returnValue, null, 0, methodCall.LogicalCallContext, methodCall);
        }

        public static IMethodReturnMessage CreateErrorMethodReturn(Exception ex, IMethodCallMessage methodCall)
        {
            return new ReturnMessage(ex, methodCall);
        }
    }
}
