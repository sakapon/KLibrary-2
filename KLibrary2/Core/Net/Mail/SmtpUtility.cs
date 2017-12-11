using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Keiho.Linq;

namespace Keiho.Net.Mail
{
    public static class SmtpUtility
    {
        public static void Send(string from, string[] to, string[] cc, string subject, string body)
        {
            if (from == null)
            {
                throw new ArgumentNullException("from");
            }
            if (subject == null)
            {
                throw new ArgumentNullException("subject");
            }
            if (body == null)
            {
                throw new ArgumentNullException("body");
            }

            using (var client = new SmtpClient())
            using (var message = new MailMessage())
            {
                message.From = new MailAddress(from);

                if (to != null)
                {
                    to.ForEachExecute(a => message.To.Add(a));
                }
                if (cc != null)
                {
                    cc.ForEachExecute(a => message.CC.Add(a));
                }

                message.Subject = ConvertByBEncoding(subject, SharedObjects.Encodings.Iso2022Jp);
                message.AlternateViews.Add(ToAlternateView(body, SharedObjects.Encodings.Iso2022Jp));

                client.Send(message);
            }
        }

        /// <summary>
        /// テキストを B エンコーディングにより変換します。
        /// </summary>
        /// <param name="text">テキスト。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        /// <returns>B エンコーディングされたテキスト。</returns>
        private static string ConvertByBEncoding(string text, Encoding encoding)
        {
            return string.Format("=?{0}?B?{1}?=", encoding.WebName, Convert.ToBase64String(encoding.GetBytes(text)));
        }

        /// <summary>
        /// テキストを <see cref="System.Net.Mail.AlternateView"/> オブジェクトに変換します。
        /// </summary>
        /// <param name="text">テキスト。</param>
        /// <param name="encoding">文字エンコーディング。</param>
        /// <returns><see cref="System.Net.Mail.AlternateView"/> オブジェクト。</returns>
        private static AlternateView ToAlternateView(string text, Encoding encoding)
        {
            var alternateView = AlternateView.CreateAlternateViewFromString(text, encoding, MediaTypeNames.Text.Plain);
            alternateView.TransferEncoding = TransferEncoding.SevenBit;

            return alternateView;
        }
    }
}
