using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Keiho
{
    public static class UriHelper
    {
        public static string GetName(this Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            string localPath = uri.LocalPath;
            if (localPath.EndsWith("/"))
            {
                localPath = localPath.Remove(localPath.Length - 1);
            }
            return localPath.Substring(localPath.LastIndexOf('/') + 1);
        }

        public static string GetExtension(this Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return Path.GetExtension(uri.GetName());
        }

        public static Uri GetParent(this Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            string uriString = uri.OriginalString;
            if (uriString.EndsWith("/"))
            {
                uriString = uriString.Remove(uriString.Length - 1);
            }
            return new Uri(uriString.Substring(0, uriString.LastIndexOf('/') + 1));
        }

        public static bool IsDirectory(this Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return uri.OriginalString.EndsWith("/");
        }

        public static string UrlDecode(this Uri uri)
        {
            if (uri == null)
            {
                throw new ArgumentNullException("uri");
            }

            return uri.ToString();
        }
    }
}
