using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Keiho.Linq;

namespace Keiho.Text.RegularExpressions
{
    /// <summary>
    /// 正規表現に関するメソッドを提供します。
    /// </summary>
    /// <remarks>
    /// .NET Framework の正規表現
    /// http://msdn.microsoft.com/ja-jp/library/hs600312.aspx
    /// </remarks>
    public static class RegexHelper
    {
        /// <summary>
        /// 文字列内で、指定した正規表現に最初に一致する箇所を検索します。
        /// </summary>
        /// <param name="input">一致する箇所を検索する文字列。</param>
        /// <param name="pattern">一致させる正規表現パターン。</param>
        /// <param name="groupIndex">キャプチャされたグループの 0 から始まるインデックス。</param>
        /// <returns>一致した部分文字列。</returns>
        public static string Match(this string input, string pattern, int groupIndex)
        {
            return Regex.Match(input, pattern, RegexOptions.Singleline)
                .ToEnumerable()
                .Where(m => m.Success)
                .ForEach(m =>
                {
                    if (m.Groups.Count <= groupIndex)
                    {
                        throw new ArgumentOutOfRangeException("groupIndex");
                    }
                })
                .Select(m => m.Groups[groupIndex])
                .Where(g => g.Success)
                .Select(g => g.Value)
                .SingleOrDefault();
        }

        /// <summary>
        /// 文字列内で、指定した正規表現に最初に一致する箇所を検索します。
        /// </summary>
        /// <param name="input">一致する箇所を検索する文字列。</param>
        /// <param name="pattern">一致させる正規表現パターン。</param>
        /// <param name="groupIndexes">キャプチャされたグループの 0 から始まるインデックスの配列。</param>
        /// <returns>一致した部分文字列の配列。</returns>
        public static string[] Match(this string input, string pattern, params int[] groupIndexes)
        {
            Match match = Regex.Match(input, pattern, RegexOptions.Singleline);
            if (!match.Success)
            {
                return null;
            }

            return groupIndexes
                .ForEach(i =>
                {
                    if (match.Groups.Count <= i)
                    {
                        throw new ArgumentOutOfRangeException("groupIndexes");
                    }
                })
                .Select(i => match.Groups[i])
                .Where(g => g.Success)
                .Select(g => g.Value)
                .ToArray();
        }

        /// <summary>
        /// 文字列内で、指定した正規表現に一致する箇所をすべて検索します。
        /// </summary>
        /// <param name="input">一致する箇所を検索する文字列。</param>
        /// <param name="pattern">一致させる正規表現パターン。</param>
        /// <param name="groupIndex">キャプチャされたグループの 0 から始まるインデックス。</param>
        /// <returns>一致した部分文字列。</returns>
        public static string[] Matches(this string input, string pattern, int groupIndex)
        {
            return Regex.Matches(input, pattern, RegexOptions.Singleline)
                .Cast<Match>()
                .Where(m => m.Success)
                .ForEach(m =>
                {
                    if (m.Groups.Count <= groupIndex)
                    {
                        throw new ArgumentOutOfRangeException("groupIndex");
                    }
                })
                .Select(m => m.Groups[groupIndex])
                .Where(g => g.Success)
                .Select(g => g.Value)
                .ToArray();
        }

        /// <summary>
        /// 文字列内で指定した正規表現に一致するすべての文字列を、指定した文字列に置換します。
        /// </summary>
        /// <param name="input">一致する箇所を検索する文字列。</param>
        /// <param name="pattern">一致させる正規表現パターン。</param>
        /// <param name="replacement">置換文字列。</param>
        /// <returns>置換された新しい文字列。</returns>
        public static string ReplaceMatch(this string input, string pattern, string replacement)
        {
            return Regex.Replace(input, pattern, replacement, RegexOptions.Singleline);
        }
    }
}
