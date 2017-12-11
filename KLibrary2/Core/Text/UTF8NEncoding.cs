using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Keiho.Text
{
    public class UTF8NEncoding : UTF8Encoding
    {
        public override string EncodingName
        {
            get { return "Unicode (UTF-8 BOM なし)"; }
        }
    }
}
