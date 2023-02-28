using System;
using WebSocketSharp.Net;

namespace WebSocketSharp.Server {
    public class HttpRequestEventArgs : EventArgs {
        internal HttpRequestEventArgs(HttpListenerContext context) {
            Request = context.Request;
            Response = context.Response;
        }

        public HttpListenerRequest Request { get; }

        public HttpListenerResponse Response { get; }
    }
}