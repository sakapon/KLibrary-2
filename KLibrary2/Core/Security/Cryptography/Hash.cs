using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Keiho.Security.Cryptography
{
    /// <summary>
    /// データをハッシュするためのメソッドを提供します。
    /// </summary>
    public static class Hash
    {
        /// <summary>既定のハッシュ アルゴリズムです。</summary>
        private const string DefaultAlgorithmName = "HMACSHA256";

        /// <summary>テキストとバイナリの変換のために使用する文字エンコーディングです。</summary>
        private static readonly Encoding TextEncoding = Encoding.Unicode;

        /// <summary>
        /// データをハッシュします。
        /// </summary>
        /// <param name="data">データ。</param>
        /// <param name="salt">ソルト。</param>
        /// <param name="algorithmName">ハッシュ アルゴリズムの名前。</param>
        /// <returns>ハッシュされたデータ。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="data"/> が <see langword="null"/> です。</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="salt"/> が <see langword="null"/> です。</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="algorithmName"/> が <see langword="null"/> です。</exception>
        public static byte[] Compute(byte[] data, byte[] salt, string algorithmName = DefaultAlgorithmName)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (salt == null)
            {
                throw new ArgumentNullException("salt");
            }
            if (algorithmName == null)
            {
                throw new ArgumentNullException("algorithmName");
            }

            using (var hashAlg = HashAlgorithm.Create(algorithmName))
            {
                if (hashAlg is KeyedHashAlgorithm)
                {
                    var keyedHashAlg = (KeyedHashAlgorithm)hashAlg;
                    keyedHashAlg.Key = Enumerable.Range(0, keyedHashAlg.Key.Length)
                        .Select(i => salt[i % salt.Length]).ToArray();

                    return keyedHashAlg.ComputeHash(data);
                }
                else
                {
                    var saltedData = salt.Concat(data).ToArray();

                    return hashAlg.ComputeHash(saltedData);
                }
            }
        }

        /// <summary>
        /// 平文のテキストをハッシュします。
        /// </summary>
        /// <param name="text">平文のテキスト。</param>
        /// <param name="salt">ソルト。</param>
        /// <param name="algorithmName">ハッシュ アルゴリズムの名前。</param>
        /// <returns>ハッシュされた Base64 形式のテキスト。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="text"/> が <see langword="null"/> です。</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="salt"/> が <see langword="null"/> です。</exception>
        /// <exception cref="System.ArgumentNullException"><paramref name="algorithmName"/> が <see langword="null"/> です。</exception>
        public static string Compute(string text, string salt, string algorithmName = DefaultAlgorithmName)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            if (salt == null)
            {
                throw new ArgumentNullException("salt");
            }

            return Convert.ToBase64String(Compute(TextEncoding.GetBytes(text), Convert.FromBase64String(salt)));
        }
    }
}
