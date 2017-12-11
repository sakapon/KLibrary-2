using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Keiho.Security.Cryptography
{
    /// <summary>
    /// 共通鍵暗号化アルゴリズムにより、データを暗号化または復号するためのメソッドを提供します。
    /// </summary>
    public static class SymmetricEncryption
    {
        /// <summary>既定の共通鍵暗号化アルゴリズムです。</summary>
        private const string DefaultAlgorithmName = "Rijndael";

        /// <summary>テキストとバイナリの変換のために使用する文字エンコーディングです。</summary>
        private static readonly Encoding TextEncoding = Encoding.Unicode;

        /// <summary>
        /// データを暗号化します。
        /// </summary>
        /// <param name="data">データ。</param>
        /// <param name="decryptionKey">復号キー。</param>
        /// <param name="algorithmName">共通鍵暗号化アルゴリズムの名前。</param>
        /// <returns>暗号化されたデータ。</returns>
        public static byte[] Encrypt(byte[] data, byte[] decryptionKey, string algorithmName = DefaultAlgorithmName)
        {
            return Transform(data, decryptionKey, true, algorithmName);
        }

        /// <summary>
        /// 平文のテキストを暗号化します。
        /// </summary>
        /// <param name="text">平文のテキスト。</param>
        /// <param name="decryptionKey">復号キー。</param>
        /// <param name="algorithmName">共通鍵暗号化アルゴリズムの名前。</param>
        /// <returns>暗号化された Base64 形式のテキスト。</returns>
        public static string Encrypt(string text, string decryptionKey, string algorithmName = DefaultAlgorithmName)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            if (decryptionKey == null)
            {
                throw new ArgumentNullException("decryptionKey");
            }

            return Convert.ToBase64String(Encrypt(TextEncoding.GetBytes(text), decryptionKey.FromHexString(), algorithmName));
        }

        /// <summary>
        /// データを一方のストリームから読み取って暗号化し、他方のストリームに書き込みます。
        /// </summary>
        /// <param name="input">入力用のストリーム。</param>
        /// <param name="output">出力用のストリーム。</param>
        /// <param name="decryptionKey">復号キー。</param>
        /// <param name="algorithmName">共通鍵暗号化アルゴリズムの名前。</param>
        public static void Encrypt(Stream input, Stream output, string decryptionKey, string algorithmName = DefaultAlgorithmName)
        {
            if (decryptionKey == null)
            {
                throw new ArgumentNullException("decryptionKey");
            }

            Transform(input, output, decryptionKey.FromHexString(), true, algorithmName);
        }

        /// <summary>
        /// 暗号化されたデータを復号します。
        /// </summary>
        /// <param name="data">暗号化されたデータ。</param>
        /// <param name="decryptionKey">復号キー。</param>
        /// <param name="algorithmName">共通鍵暗号化アルゴリズムの名前。</param>
        /// <returns>復号されたデータ。</returns>
        public static byte[] Decrypt(byte[] data, byte[] decryptionKey, string algorithmName = DefaultAlgorithmName)
        {
            return Transform(data, decryptionKey, false, algorithmName);
        }

        /// <summary>
        /// 暗号化されたテキストを復号します。
        /// </summary>
        /// <param name="text">暗号化された Base64 形式のテキスト。</param>
        /// <param name="decryptionKey">復号キー。</param>
        /// <param name="algorithmName">共通鍵暗号化アルゴリズムの名前。</param>
        /// <returns>平文のテキスト。</returns>
        public static string Decrypt(string text, string decryptionKey, string algorithmName = DefaultAlgorithmName)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }
            if (decryptionKey == null)
            {
                throw new ArgumentNullException("decryptionKey");
            }

            return TextEncoding.GetString(Decrypt(Convert.FromBase64String(text), decryptionKey.FromHexString(), algorithmName));
        }

        /// <summary>
        /// 暗号化されたデータを一方のストリームから読み取って復号し、他方のストリームに書き込みます。
        /// </summary>
        /// <param name="input">入力用のストリーム。</param>
        /// <param name="output">出力用のストリーム。</param>
        /// <param name="decryptionKey">復号キー。</param>
        /// <param name="algorithmName">共通鍵暗号化アルゴリズムの名前。</param>
        public static void Decrypt(Stream input, Stream output, string decryptionKey, string algorithmName = DefaultAlgorithmName)
        {
            if (decryptionKey == null)
            {
                throw new ArgumentNullException("decryptionKey");
            }

            Transform(input, output, decryptionKey.FromHexString(), false, algorithmName);
        }

        private static byte[] Transform(byte[] data, byte[] decryptionKey, bool encrypt, string algorithmName)
        {
            if (data == null)
            {
                throw new ArgumentNullException("data");
            }
            if (decryptionKey == null)
            {
                throw new ArgumentNullException("decryptionKey");
            }
            if (algorithmName == null)
            {
                throw new ArgumentNullException("algorithmName");
            }

            using (SymmetricAlgorithm symAlg = CreateSymmetricAlgorithm(algorithmName, decryptionKey))
            using (ICryptoTransform transform = encrypt ? symAlg.CreateEncryptor() : symAlg.CreateDecryptor())
            using (MemoryStream stream = new MemoryStream())
            using (CryptoStream cryptoStream = new CryptoStream(stream, transform, CryptoStreamMode.Write))
            {
                cryptoStream.Write(data, 0, data.Length);
                cryptoStream.FlushFinalBlock();

                return stream.ToArray();
            }
        }

        private static void Transform(Stream input, Stream output, byte[] decryptionKey, bool encrypt, string algorithmName)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }
            if (decryptionKey == null)
            {
                throw new ArgumentNullException("decryptionKey");
            }
            if (algorithmName == null)
            {
                throw new ArgumentNullException("algorithmName");
            }

            using (SymmetricAlgorithm symAlg = CreateSymmetricAlgorithm(algorithmName, decryptionKey))
            using (ICryptoTransform transform = encrypt ? symAlg.CreateEncryptor() : symAlg.CreateDecryptor())
            using (CryptoStream cryptoStream = new CryptoStream(output, transform, CryptoStreamMode.Write))
            {
                input.CopyTo(cryptoStream);
                cryptoStream.FlushFinalBlock();
            }
        }

        private static SymmetricAlgorithm CreateSymmetricAlgorithm(string algorithmName, byte[] decryptionKey)
        {
            SymmetricAlgorithm symAlg = SymmetricAlgorithm.Create(algorithmName);

            symAlg.Key = decryptionKey;
            symAlg.IV = new byte[symAlg.BlockSize / 8];

            return symAlg;
        }
    }
}
