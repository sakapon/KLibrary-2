using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace Keiho.Web
{
    public static class HttpResponseHelper
    {
        // contentType, downloadFileName は null 可能。
        public static void RenderFile(this HttpResponse response, string sourceFilePath, string contentType, string downloadFileName)
        {
            response.Clear();
            response.SetHeader(null, contentType, downloadFileName);
            response.TransmitFile(sourceFilePath);
            response.End();
        }

        public static void RenderFile(this HttpResponse response, IEnumerable<string> lines, Encoding contentEncoding, string contentType, string downloadFileName)
        {
            response.Clear();
            response.SetHeader(contentEncoding, contentType, downloadFileName);
            response.WriteLines(lines);
            response.End();
        }

        public static void RenderFile(this HttpResponse response, byte[] binary, string contentType, string downloadFileName)
        {
            response.Clear();
            response.SetHeader(null, contentType, downloadFileName);
            response.OutputStream.Write(binary, 0, binary.Length);
            response.End();
        }

        public static void RenderFile(this HttpResponse response, Stream input, string contentType, string downloadFileName)
        {
            response.Clear();
            response.SetHeader(null, contentType, downloadFileName);
            input.CopyTo(response.OutputStream);
            response.End();
        }

        public static void WriteLine(this HttpResponse response, string s)
        {
            response.Output.WriteLine(s);
        }

        public static void WriteLine(this HttpResponse response, string format, params object[] arg)
        {
            response.Output.WriteLine(format, arg);
        }

        public static void WriteLines(this HttpResponse response, IEnumerable<string> lines)
        {
            foreach (string line in lines)
            {
                response.WriteLine(line);
            }
        }

        private static void SetHeader(this HttpResponse response, Encoding contentEncoding, string contentType, string downloadFileName)
        {
            if (contentEncoding != null)
            {
                response.ContentEncoding = contentEncoding;
            }

            response.ContentType = string.IsNullOrEmpty(contentType) ? SharedObjects.ContentTypes.OctetStream : contentType;

            if (!string.IsNullOrEmpty(downloadFileName))
            {
                response.AppendHeader("Content-Disposition", GetDispositionValue(downloadFileName));
            }
        }

        private static string GetDispositionValue(string fileName)
        {
            var disposition = new ContentDisposition
            {
                FileName = Uri.EscapeUriString(fileName),
            };
            return disposition.ToString();
        }

        private static string GetRfcDispositionValue(string fileName)
        {
            return string.Format("attachment; filename*=utf-8''{0}", Uri.EscapeUriString(fileName));
        }
    }
}
