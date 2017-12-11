using System;
using System.Web;

namespace WebApp1
{
    public class SampleAsyncHandler : IHttpAsyncHandler
    {
        public bool IsReusable
        {
            get { return true; }
        }

        public void ProcessRequest(HttpContext context)
        {
            context.Response.Write(string.Format("<p>{0:HH:mm:ss.fff}: {1}</p>", DateTime.Now, "Begin"));

            // 実際の処理を記述します。
            System.Threading.Thread.Sleep(3000);

            context.Response.Write(string.Format("<p>{0:HH:mm:ss.fff}: {1}</p>", DateTime.Now, "End"));
        }

        public IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData)
        {
            return new Action<HttpContext>(ProcessRequest).BeginInvoke(context, cb, extraData);
        }

        // ProcessRequest メソッドと同じスレッドで実行されます。
        public void EndProcessRequest(IAsyncResult result)
        {
        }
    }
}
