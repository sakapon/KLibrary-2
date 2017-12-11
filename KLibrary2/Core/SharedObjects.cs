using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Keiho.Text;

namespace Keiho
{
    public static class SharedObjects
    {
        public static class Encodings
        {
            public static readonly Encoding ASCII = Encoding.ASCII;
            public static readonly Encoding UTF7 = Encoding.UTF7;
            public static readonly Encoding UTF8 = Encoding.UTF8;
            public static readonly Encoding UTF8N = new UTF8NEncoding();
            public static readonly Encoding UTF16 = Encoding.Unicode;
            public static readonly Encoding UTF16BE = Encoding.BigEndianUnicode;
            public static readonly Encoding UTF32 = Encoding.UTF32;
            public static readonly Encoding UTF32BE = Encoding.GetEncoding("utf-32BE");
            public static readonly Encoding ShiftJis = Encoding.GetEncoding("shift_jis");
            public static readonly Encoding EucJp = Encoding.GetEncoding("euc-jp");
            public static readonly Encoding Iso2022Jp = Encoding.GetEncoding("iso-2022-jp");
            public static readonly Encoding Iso8859_1 = Encoding.GetEncoding("iso-8859-1");
        }

        public static class ContentTypes
        {
            public const string Any = "*/*";
            public const string Text = "text/plain";
            public const string Xml = "text/xml";
            public const string ApplicationXml = "application/xml";
            public const string Html = "text/html";
            public const string Css = "text/css";
            public const string JavaScript = "text/javascript";
            public const string Gif = "image/gif";
            public const string Jpeg = "image/jpeg";
            public const string Png = "image/png";
            public const string Json = "application/json";
            public const string OctetStream = "application/octet-stream";
        }

        public static class UserAgents
        {
            public const string Windows7_IE9 = "Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0)";
            public const string IOS5_Safari51 = "Mozilla/5.0 (iPhone; CPU iPhone OS 5_0 like Mac OS X) AppleWebKit/534.46 (KHTML, like Gecko) Version/5.1 Mobile/9A334 Safari/7534.48.3";
            public const string WP75_IE9 = "Mozilla/5.0 (compatible; MSIE 9.0; Windows Phone OS 7.5; Trident/5.0; IEMobile/9.0; FujitsuToshibaMobileCommun; IS12T; KDDI)";
        }
    }
}
