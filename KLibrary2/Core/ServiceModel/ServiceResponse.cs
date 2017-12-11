using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace Keiho.ServiceModel
{
    /// <summary>
    /// サービスの応答を表す抽象クラスです。
    /// </summary>
    [DataContract]
    public abstract class ServiceResponseBase
    {
        [DataMember(Order = 0, EmitDefaultValue = false)]
        public MessageType MessageType { get; set; }

        [DataMember(Order = 1, EmitDefaultValue = false)]
        public string Message { get; set; }
    }

    /// <summary>
    /// 戻り値を持たないサービスの応答を表します。
    /// </summary>
    [DataContract]
    public class ServiceResponse : ServiceResponseBase
    {
    }

    /// <summary>
    /// 戻り値を持つサービスの応答を表します。
    /// </summary>
    /// <typeparam name="TResult">サービスの戻り値の型。</typeparam>
    [DataContract]
    public class ServiceResponse<TResult> : ServiceResponseBase
    {
        /// <summary>
        /// サービスの戻り値を取得または設定します。
        /// </summary>
        /// <value>サービスの戻り値。</value>
        [DataMember(Order = 2)]
        public TResult Result { get; set; }
    }

    /// <summary>
    /// メッセージの種類を表します。
    /// </summary>
    public enum MessageType
    {
        None,
        Information,
        Warning,
        Error,
    }
}
