using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Keiho
{
    /// <summary>
    /// オブジェクトを別のオブジェクトに変換するためのメソッドを提供します。
    /// </summary>
    public static class ConvertHelper
    {
        #region IConvertible

        /// <summary>
        /// 指定されたオブジェクトと等しい値を持つ、指定された型のオブジェクトを返します。
        /// </summary>
        /// <typeparam name="T">返すオブジェクトの型。</typeparam>
        /// <param name="value"><see cref="System.IConvertible"/> インターフェイスを実装するオブジェクト。</param>
        /// <returns><paramref name="value"/> と等しい値を持つ、型が <typeparamref name="T"/> のオブジェクト。</returns>
        /// <exception cref="System.InvalidCastException">この変換はサポートされていません。</exception>
        /// <exception cref="System.InvalidCastException"><paramref name="value"/> が <see langword="null"/> であり、<typeparamref name="T"/> が値型または <see cref="System.TimeSpan"/> です。</exception>
        /// <exception cref="System.FormatException"><paramref name="value"/> は、<typeparamref name="T"/> によって認識されている形式ではありません。</exception>
        /// <exception cref="System.OverflowException"><paramref name="value"/> は、<typeparamref name="T"/> の範囲外の数値を表します。</exception>
        public static T To<T>(this object value)
        {
            if (typeof(T).IsEnum)
            {
                if (value == null)
                {
                    throw new InvalidCastException("Null オブジェクトを値型に変換することはできません。");
                }
                try
                {
                    return (T)Enum.Parse(typeof(T), value.ToString());
                }
                catch (ArgumentException ex)
                {
                    throw new FormatException("入力文字列の形式が正しくありません。", ex);
                }
            }
            else if (typeof(T) == typeof(TimeSpan))
            {
                if (value == null)
                {
                    throw new InvalidCastException("Null オブジェクトを System.TimeSpan に変換することはできません。");
                }
                return (T)(object)TimeSpan.Parse(value.ToString());
            }
            else
            {
                return (T)Convert.ChangeType(value, typeof(T));
            }
        }

        #endregion

        #region Hexadecimal

        /// <summary>
        /// バイト配列を 16 進数表記の文字列に変換します。
        /// </summary>
        /// <param name="binary">バイト配列。</param>
        /// <param name="lowercase">アルファベットを小文字で表記する場合は <see langword="true"/>。</param>
        /// <returns>16 進数表記の文字列。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="binary"/> が <see langword="null"/> です。</exception>
        public static string ToHexString(this byte[] binary, bool lowercase = false)
        {
            if (binary == null)
            {
                throw new ArgumentNullException("binary");
            }

            return new string(binary.SelectMany(b => b.ToString(lowercase ? "x2" : "X2")).ToArray());
        }

        /// <summary>
        /// 16 進数表記の文字列をバイト配列に変換します。
        /// </summary>
        /// <param name="hexString">16 進数表記の文字列。</param>
        /// <returns>バイト配列。</returns>
        /// <exception cref="System.ArgumentNullException"><paramref name="hexString"/> が <see langword="null"/> です。</exception>
        /// <exception cref="System.FormatException">入力文字列の形式が正しくありません。</exception>
        public static byte[] FromHexString(this string hexString)
        {
            if (hexString == null)
            {
                throw new ArgumentNullException("hexString");
            }
            if (hexString.Length % 2 != 0)
            {
                throw new FormatException("入力文字列の長さが偶数ではありません。");
            }

            return Enumerable.Range(0, hexString.Length / 2)
                .Select(i => hexString.Substring(2 * i, 2))
                .Select(s => byte.Parse(s, NumberStyles.HexNumber))
                .ToArray();
        }

        #endregion

        #region Properties

        /// <summary>
        /// 指定されたオブジェクトのプロパティをディクショナリに変換します。
        /// </summary>
        /// <param name="obj">任意のオブジェクト。</param>
        /// <returns>プロパティの名前と値をペアとするコレクション。</returns>
        public static Dictionary<string, object> ToProperties(this object obj)
        {
            if (obj == null)
            {
                throw new ArgumentNullException("obj");
            }

            return TypeDescriptor.GetProperties(obj)
                .Cast<PropertyDescriptor>()
                .ToDictionary(p => p.Name, p => p.GetValue(obj));
        }

        #endregion

        #region SecureString

        public static SecureString ToSecureString(this string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            var secureText = new SecureString();

            foreach (char c in text)
            {
                secureText.AppendChar(c);
            }
            secureText.MakeReadOnly();

            return secureText;
        }

        public static string FromSecureString(this SecureString secureText)
        {
            if (secureText == null)
            {
                throw new ArgumentNullException("secureText");
            }

            IntPtr pointer = IntPtr.Zero;
            try
            {
                pointer = Marshal.SecureStringToCoTaskMemUnicode(secureText);

                var buffer = new char[secureText.Length];
                Marshal.Copy(pointer, buffer, 0, buffer.Length);

                return new string(buffer);
            }
            finally
            {
                if (pointer != IntPtr.Zero)
                {
                    Marshal.ZeroFreeCoTaskMemUnicode(pointer);
                }
            }
        }

        #endregion
    }
}
