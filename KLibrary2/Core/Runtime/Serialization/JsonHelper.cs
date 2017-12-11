using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Keiho.Runtime.Serialization
{
    /// <summary>
    /// オブジェクトを JSON 形式にシリアル化するためのメソッドを提供します。
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// オブジェクトを JSON 形式にシリアル化します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="obj">オブジェクト。</param>
        /// <returns>JSON 形式のデータ。</returns>
        public static string ToJson<T>(this T obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            byte[] binary;
            using (var output = new MemoryStream())
            {
                obj.ToJson(output);
                binary = output.ToArray();
            }

            // Silverlight の場合、GetString(Byte[]) が存在しません。
            return Encoding.UTF8.GetString(binary, 0, binary.Length);
        }

        /// <summary>
        /// オブジェクトを JSON 形式にシリアル化します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="obj">オブジェクト。</param>
        /// <param name="output">出力用のストリーム。</param>
        public static void ToJson<T>(this T obj, Stream output)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (output == null) throw new ArgumentNullException("output");

            var serializer = new DataContractJsonSerializer(typeof(T));
            serializer.WriteObject(output, obj);
        }

        /// <summary>
        /// JSON 形式のデータをオブジェクトに逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="jsonText">JSON 形式のデータ。</param>
        /// <returns>オブジェクト。</returns>
        public static T FromJson<T>(this string jsonText)
        {
            if (jsonText == null) throw new ArgumentNullException("jsonText");

            var binary = Encoding.UTF8.GetBytes(jsonText);
            using (var input = new MemoryStream(binary))
            {
                return input.FromJson<T>();
            }
        }

        /// <summary>
        /// JSON 形式のデータをオブジェクトに逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="input">入力用のストリーム。</param>
        /// <returns>オブジェクト。</returns>
        public static T FromJson<T>(this Stream input)
        {
            if (input == null) throw new ArgumentNullException("input");

            var serializer = new DataContractJsonSerializer(typeof(T));
            return (T)serializer.ReadObject(input);
        }
    }
}
