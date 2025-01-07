using System;
using System.Net;

namespace CoreWebServer
{
    public class WebServer
    {
        public event EventHandler<RequestReceiverEventArgs> RequestReceiver; // для получения данных от сервера
        private HttpListener _listener;
        private readonly int _port;
        private bool _enabled;
        private readonly object _syncRoot = new object(); // для потокобезопасности(не использовать this)

        public int Port => _port;

        public bool Enabled
        {
            get => _enabled; set
            {
                if (value) Start();
                else Stop();
            }
        }

        public WebServer(int port) => _port = port;

        public void Start()
        {
            if (_enabled) return; //Поторяется чтобы меньше нагружать переходом в lock (Оптимизация)
            lock (_syncRoot)
            {
                if (_enabled) return;

                _listener = new HttpListener();
                _listener.Prefixes.Add($"http://*:{_port}/"); //в cmd ввести: netsh http add urlacl url-http://*:8080/ user=user_name
                _listener.Prefixes.Add($"http://+:{_port}/");
                _enabled = true;
                ListenAsync();
            }
        }

        public void Stop()
        {
            if (!_enabled) return; //Поторяется чтобы меньше нагружать переходом в lock (Оптимизация)
            lock (_syncRoot)
            {
                if (!_enabled) return;
                _listener = null;
                _enabled = false;
            }
        }

        private async void ListenAsync()
        {
            var listen = _listener; // захватываем чтобы после остановки сервера не потерять ссылку и корректно завершиться.
            listen.Start();
            while (_enabled)
            {
                var context = await listen.GetContextAsync().ConfigureAwait(false); //false - не захватывать котекст выполнения
                ProcessRequest(context);
            }

            listen.Stop();
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            RequestReceiver?.Invoke(this, new RequestReceiverEventArgs(context));
        }
    }


    public class RequestReceiverEventArgs : EventArgs
    {
        public HttpListenerContext Context { get; }

        public RequestReceiverEventArgs(HttpListenerContext context) => Context = context;
    }
}
