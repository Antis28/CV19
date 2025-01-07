using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoreWebServer;

namespace TestServer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Run();
            
            Console.ReadLine();
        }

        static void Run()
        {
            var server = new WebServer(8080);
            server.RequestReceiver += OnRequestReceiver;
            server.Start();
            Console.WriteLine("Server started!");
        }

        private static void OnRequestReceiver(object sender, RequestReceiverEventArgs e)
        {
            var context = e.Context;
            Console.WriteLine("Connection {0}", context.Request.UserHostAddress);

            using var writer = new StreamWriter(context.Response.OutputStream);
            writer.WriteLine("Hello from server");
        }
    }
}
