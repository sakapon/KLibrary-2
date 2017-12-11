using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;

namespace Keiho.Runtime.Serialization
{
    /// <summary>
    /// オブジェクトをバイナリ形式にシリアル化するためのメソッドを提供します。
    /// </summary>
    public static class BinaryHelper
    {
        /// <summary>
        /// オブジェクトをバイナリ形式にシリアル化します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="obj">オブジェクト。</param>
        /// <returns>バイナリ形式のデータ。</returns>
        public static byte[] ToBinary<T>(this T obj)
        {
            if (obj == null) throw new ArgumentNullException("obj");

            using (var output = new MemoryStream())
            {
                obj.ToBinary(output);
                return output.ToArray();
            }
        }

        /// <summary>
        /// オブジェクトをバイナリ形式にシリアル化します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="obj">オブジェクト。</param>
        /// <param name="output">出力用のストリーム。</param>
        public static void ToBinary<T>(this T obj, Stream output)
        {
            if (obj == null) throw new ArgumentNullException("obj");
            if (output == null) throw new ArgumentNullException("output");

            var formatter = new BinaryFormatter();
            formatter.Serialize(output, obj);
        }

        /// <summary>
        /// バイナリ形式のデータをオブジェクトに逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="binary">バイナリ形式のデータ。</param>
        /// <returns>オブジェクト。</returns>
        public static T FromBinary<T>(this byte[] binary)
        {
            if (binary == null) throw new ArgumentNullException("binary");

            using (var input = new MemoryStream(binary))
            {
                return input.FromBinary<T>();
            }
        }

        /// <summary>
        /// バイナリ形式のデータをオブジェクトに逆シリアル化します。
        /// </summary>
        /// <typeparam name="T">オブジェクトの型。</typeparam>
        /// <param name="input">入力用のストリーム。</param>
        /// <returns>オブジェクト。</returns>
        public static T FromBinary<T>(this Stream input)
        {
            if (input == null) throw new ArgumentNullException("input");

            var formatter = new BinaryFormatter();
            return (T)formatter.Deserialize(input);
        }
    }
}
