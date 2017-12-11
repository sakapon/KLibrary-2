using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Keiho.IO
{
    public static class StreamHelper
    {
        private const int BufferSize = 0x1000;

        public static void Copy(this Stream input, Stream output)
        {
            if (input == null)
            {
                throw new ArgumentNullException("input");
            }
            if (output == null)
            {
                throw new ArgumentNullException("output");
            }

            var buffer = new byte[BufferSize];
            int readSize;

            while ((readSize = input.Read(buffer, 0, BufferSize)) > 0)
            {
                output.Write(buffer, 0, readSize);
            }
        }
    }
}
