using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace app
{
    public class Program
    {
        public static void Main(string[] args)
        {
            const int DEFPORTNUM = 8081;
            int port2use = DEFPORTNUM;
            const int BACKLOG = 10; // maximum length of pending connections queue

            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Establish the local endpoint for the socket
            IPAddress ipAddress = IPAddress.Loopback;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, port2use);

            listener.Bind(localEndPoint);
            listener.Listen(BACKLOG);
            System.Console.WriteLine("Listening ... " + localEndPoint.ToString());

            while (true)
            {
              Socket sock = listener.Accept();
              //if(sock.Connected)
              //    System.Console.WriteLine("Connection established");
              RequestHandler rh = new RequestHandler(sock);
              Thread rhThread = new Thread(new ThreadStart(rh.DoRequest));
              rhThread.Start();
            }



      //CreateWebHostBuilder(args).Build().Run();
    }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
