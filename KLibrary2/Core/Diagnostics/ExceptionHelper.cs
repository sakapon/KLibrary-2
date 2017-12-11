using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keiho.Diagnostics
{
    public static class ExceptionHelper
    {
        /// <summary>
        /// 内部例外を、現在の例外から順に取得します。
        /// </summary>
        /// <param name="ex">例外オブジェクト。</param>
        /// <returns>内部例外のコレクション。</returns>
        public static IEnumerable<Exception> GetInnerExceptions(this Exception ex)
        {
            if (ex == null)
            {
                throw new ArgumentNullException("ex");
            }

            Exception current = ex;
            do
            {
                yield return current;
            }
            while ((current = current.InnerException) != null);
        }
    }
}
