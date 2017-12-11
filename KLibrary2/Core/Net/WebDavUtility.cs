using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Xml.Linq;

namespace Keiho.Net
{
    public static class WebDavUtility
    {
        private static readonly XNamespace Dav = "DAV:";

        public static byte[] Read(Uri address, ICredentials credentials = null)
        {
            if (address == null) throw new ArgumentNullException("address");

            using (var client = HttpWebUtility.CreateWebClient(credentials))
            {
                return client.DownloadData(address);
            }
        }

        public static Stream OpenRead(Uri address, ICredentials credentials = null)
        {
            if (address == null) throw new ArgumentNullException("address");

            using (var client = HttpWebUtility.CreateWebClient(credentials))
            {
                return client.OpenRead(address);
            }
        }

        public static void Create(Uri address, byte[] content, ICredentials credentials = null)
        {
            if (address == null) throw new ArgumentNullException("address");
            if (content == null) throw new ArgumentNullException("content");

            EnsureServicePoint(address);

            using (var client = HttpWebUtility.CreateWebClient(credentials, new { ContentType = SharedObjects.ContentTypes.OctetStream }))
            {
                client.UploadData(address, "PUT", content);
            }
        }

        public static Stream OpenCreate(Uri address, ICredentials credentials = null)
        {
            if (address == null) throw new ArgumentNullException("address");

            EnsureServicePoint(address);

            using (var client = HttpWebUtility.CreateWebClient(credentials, new { ContentType = SharedObjects.ContentTypes.OctetStream }))
            {
                return client.OpenWrite(address, "PUT");
            }
        }

        public static void Update(Uri address, byte[] content, ICredentials credentials = null)
        {
            Create(address, content, credentials);
        }

        public static Stream OpenUpdate(Uri address, ICredentials credentials = null)
        {
            return OpenCreate(address, credentials);
        }

        public static void Delete(Uri address, ICredentials credentials = null)
        {
            if (address == null) throw new ArgumentNullException("address");

            using (var client = HttpWebUtility.CreateWebClient(credentials))
            {
                client.UploadData(address, "DELETE", new byte[0]);
            }
        }

        public static WebDavEntry[] GetChildren(Uri address, ICredentials credentials = null)
        {
            return GetProperties(address, 1, credentials)
                .Where(e => e.Uri != address)
                .ToArray();
        }

        public static WebDavEntry[] GetProperties(Uri address, int depth, ICredentials credentials = null)
        {
            if (address == null) throw new ArgumentNullException("address");
            if (depth < 0)
            {
                throw new ArgumentOutOfRangeException("depth", depth, "0 以上の整数でなければなりません。");
            }

            EnsureServicePoint(address);

            string propXml;
            using (var client = HttpWebUtility.CreateWebClient(credentials, new { ContentType = SharedObjects.ContentTypes.Xml, Translate = "f", Depth = depth }))
            {
                // MEMO: 空文字列を送信すると、すべてのプロパティを取得できます。
                propXml = client.UploadString(address, "PROPFIND", NetResource.PropFindRequestBody);
            }

            var hostUri = new Uri(address.GetLeftPart(UriPartial.Authority));

            return XElement.Parse(propXml)
                .Descendants(Dav + "response")
                .Select(res => res.ToEntry(hostUri))
                .OrderBy(e => !e.IsDirectory)
                .ThenBy(e => e.Uri.UrlDecode())
                .ToArray();
        }

        private static void EnsureServicePoint(Uri address)
        {
            // MEMO: POST、PUT、PROPFIND で送信する場合、既定ではヘッダーに「Expect: 100-continue」が付加されてしまいます。
            var hostUri = new Uri(address.GetLeftPart(UriPartial.Authority));
            var servicePoint = ServicePointManager.FindServicePoint(hostUri);
            servicePoint.Expect100Continue = false;
        }

        private static WebDavEntry ToEntry(this XElement response, Uri hostUri)
        {
            var propElements = response.Elements(Dav + "propstat")
                .Where(ps => ps.Element(Dav + "status").Value.Contains("200"))
                .Select(ps => ps.Element(Dav + "prop"))
                .SelectMany(p => p.Elements())
                .ToArray();

            return new WebDavEntry
            {
                Uri = new Uri(hostUri, response.Element(Dav + "href").Value),
                IsDirectory = propElements.Where(pe => pe.Name == Dav + "resourcetype").Select(pe => pe.Elements(Dav + "collection").Any()).FirstOrDefault(),
                ContentLength = propElements.Where(pe => pe.Name == Dav + "getcontentlength").Select(pe => (int?)pe.Value.To<int>()).FirstOrDefault(),
                Created = propElements.Where(pe => pe.Name == Dav + "creationdate").Select(pe => (DateTime?)pe.Value.To<DateTime>()).FirstOrDefault(),
                LastModified = propElements.Where(pe => pe.Name == Dav + "getlastmodified").Select(pe => (DateTime?)pe.Value.To<DateTime>()).FirstOrDefault(),
            };
        }
    }

    [DataContract]
    [DebuggerDisplay(@"\{{Uri}\}")]
    public class WebDavEntry
    {
        [DataMember]
        public Uri Uri { get; set; }

        [DataMember]
        public bool IsDirectory { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public int? ContentLength { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? Created { get; set; }

        [DataMember(EmitDefaultValue = false)]
        public DateTime? LastModified { get; set; }
    }
}
