using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Collections.Concurrent;
using System.Web.Mvc;
using System.Text;
using ServiceStack.Text;
using System.IO;
using System.Net.Http.Headers;

namespace inTimePayServer.Controllers
{
    public class ServerSentController : ApiController
    {
        
        private static readonly ConcurrentQueue<StreamWriter> _streammessage = new ConcurrentQueue<StreamWriter>();

        public HttpResponseMessage Get(HttpRequestMessage request)
        {
            HttpResponseMessage response = request.CreateResponse();
            response.Content = new PushStreamContent(OnStreamAvailable, "text/event-stream");
            return response;
        }

        public void Post(string result)
        {
            MessageCallback(result);
        }

        public static void OnStreamAvailable(Stream stream, HttpContent headers, TransportContext context)
        {
            StreamWriter streamwriter = new StreamWriter(stream);
            _streammessage.Enqueue(streamwriter);
        }

        private static void MessageCallback(string m)
        {
            foreach (var subscriber in _streammessage)
            {
                subscriber.WriteLine("data:" + m + "\n");
                subscriber.Flush();
            }
        }
    }
}
