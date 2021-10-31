using System;
using System.Net;

namespace EZBlocker
{
    class Listener
    {
        HttpListener listener;
        private bool enabled;
        private const string endpoint = "http://localhost:19691/";

        public string Message { get; private set; } = "";

        public Listener()
        {
            listener = new HttpListener();
            listener.Prefixes.Add(endpoint);
            listener.Start();

            enabled = true;
        }

        public void Listen()
        {
            while (enabled)
            {
                try
                {
                    HttpListenerContext context = listener.GetContext();
                    Handle(context.Request, context.Response);
                } catch (Exception) { }
            }
        }

        public void Stop()
        {
            enabled = false;
            listener.Stop();
        }

        private void Handle(HttpListenerRequest request, HttpListenerResponse response)
        {
            string path = request.Url.LocalPath.Substring(1);
            if (!path.Equals(Message))
            {
                Message = path;
            }

            byte[] buffer = System.Text.Encoding.UTF8.GetBytes("1");
            response.ContentLength64 = buffer.Length;
            System.IO.Stream output = response.OutputStream;
            output.Write(buffer, 0, buffer.Length);
            output.Close();
        }
    }
}
