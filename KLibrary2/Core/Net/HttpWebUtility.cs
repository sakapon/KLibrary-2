using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Text;
using Keiho.Linq;

namespace Keiho.Net
{
    public static class HttpWebUtility
    {
        public static WebClient CreateWebClient(ICredentials credentials = null, object headers = null)
        {
            var client = new WebClient
            {
                Encoding = Encoding.UTF8,
                Credentials = credentials,
            };
            client.Headers[HttpRequestHeader.Accept] = SharedObjects.ContentTypes.Any;
            client.Headers[HttpRequestHeader.AcceptLanguage] = CultureInfo.CurrentUICulture.Name;
            client.Headers[HttpRequestHeader.UserAgent] = SharedObjects.UserAgents.Windows7_IE9;

            if (headers != null)
            {
                headers.ToProperties()
                    .ForEachExecute(p =>
                    {
                        if (Enum.IsDefined(typeof(HttpRequestHeader), p.Key))
                        {
                            client.Headers[p.Key.To<HttpRequestHeader>()] = p.Value.To<string>();
                        }
                        else
                        {
                            client.Headers[p.Key] = p.Value.To<string>();
                        }
                    });
            }

            return client;
        }

        public static string DownloadString(string targetUri, object headers = null)
        {
            using (var client = CreateWebClient(headers: headers))
            {
                return client.DownloadString(new Uri(targetUri));
            }
        }

        public static void DownloadStringAsync(string targetUri, Action<string> onCompleted = null, Action<Exception> onError = null)
        {
            var client = new WebClient { Encoding = Encoding.UTF8 };

            client.DownloadStringCompleted += (o, e) =>
            {
                if (e.Error != null)
                {
                    if (onError != null)
                    {
                        onError(e.Error);
                    }
                    return;
                }

                if (onCompleted != null)
                {
                    onCompleted(e.Result);
                }
            };

            client.DownloadStringAsync(new Uri(targetUri));
        }
    }
}
