using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Keiho
{
    /// <summary>
    /// 乱数に関するメソッドを提供します。
    /// </summary>
    public static class RandomUtility
    {
        /// <summary>
        /// 指定されたサイズのバイト配列をランダムに生成します。
        /// </summary>
        /// <param name="size">サイズ (バイト単位)。</param>
        /// <returns>ランダムに生成されたバイト配列。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="size"/> が 0 未満です。</exception>
        public static byte[] GenerateBytes(int size)
        {
            if (size < 0)
            {
                throw new ArgumentOutOfRangeException("size", size, "値を 0 未満にすることはできません。");
            }

            var data = new byte[size];

            using (var generator = RandomNumberGenerator.Create())
            {
                generator.GetBytes(data);
            }
            return data;
        }

        /// <summary>
        /// 指定されたサイズの Base64 文字列をランダムに生成します。
        /// </summary>
        /// <param name="size">サイズ (バイト単位)。</param>
        /// <returns>ランダムに生成された Base64 文字列。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="size"/> が 0 未満です。</exception>
        /// <remarks>
        /// ランダムに生成された Base64 文字列は、例えばソルトとして使用されます。
        /// SqlMembershipProvider では、16 バイトのソルトを使用します。 
        /// </remarks>
        public static string GenerateBase64String(int size)
        {
            return Convert.ToBase64String(GenerateBytes(size));
        }

        /// <summary>
        /// 指定されたサイズの 16 進数文字列をランダムに生成します。
        /// </summary>
        /// <param name="size">サイズ (バイト単位)。</param>
        /// <param name="lowercase">アルファベットを小文字で表記する場合は <see langword="true"/>。</param>
        /// <returns>ランダムに生成された 16 進数文字列。</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="size"/> が 0 未満です。</exception>
        /// <remarks>
        /// ランダムに生成された 16 進数文字列は、例えば復号キーとして使用されます。
        /// 復号キーのサイズは暗号化アルゴリズムごとに決められており、<see cref="SymmetricAlgorithm.LegalKeySizes"/> プロパティで取得することができます。
        /// <ul>
        /// <li>Rijndael または AES の場合: 128, 192, 256 ビットのいずれか</li>
        /// <li>TripleDES の場合: 128, 192 ビットのいずれか</li>
        /// </ul>
        /// </remarks>
        public static string GenerateHexString(int size, bool lowercase = false)
        {
            return GenerateBytes(size).ToHexString(lowercase);
        }
    }
}
