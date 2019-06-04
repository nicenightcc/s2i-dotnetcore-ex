using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace app
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");

      new HttpServer(8080).Start();

      //const int port = 8080;
      ////定义端口号
      //TcpListener tcplistener = new TcpListener(IPAddress.Any, port);
      //Console.WriteLine("侦听端口号： " + port.ToString());
      //tcplistener.Start();
      ////侦听端口号
      //while (true)
      //{
      //  Socket socket = tcplistener.AcceptSocket();
      //  //并获取传送和接收数据的Scoket实例
      //  Proxy proxy = new Proxy(socket);
      //  //Proxy类实例化
      //  Thread thread = new Thread(new ThreadStart(proxy.Run));
      //  //创建线程
      //  thread.Start();
      //  //启动线程
      //}


      //new SocketConnection(8080).setConnection();
      while (true)
        Console.Read();
    }
  }
}
