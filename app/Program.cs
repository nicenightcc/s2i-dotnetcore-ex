using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

      new SocketConnection(8080).setConnection();


      //CreateWebHostBuilder(args).Build().Run();
    }

    public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
        WebHost.CreateDefaultBuilder(args)
        .UseOpenShiftIntegration(_ => _.CertificateMountPoint = "/var/run/secrets/service-cert")
        .UseStartup<Startup>();
  }
  class SocketConnection
  {
    private int port;//监听端口号
    private static byte[] result = new byte[1024];
    private static Socket server;//服务器Socket
    private IPAddress ip;//Ip地址
    private static Socket client;//客户端Socket
    private static Thread myThread;//启动监听线程
    private static Thread receiveThread;//接收数据线程
    public SocketConnection(int port)
    {
      this.port = port;//初始化端口
      ip = IPAddress.Any;//初始化ip地址

    }
    public void setConnection()
    {
      server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);//实例化socket对象（采用网络流传输方式，TCP协议传输）
      server.Bind(new IPEndPoint(ip, port));//绑定ip及端口
      Console.WriteLine("绑定端口ip" + this.ip + ":" + this.port);
      server.Listen(10);//监听端口
      Console.WriteLine("正在监听IP" + this.ip + "  端口：" + this.port + "......");
      myThread = new Thread(ListenClientConnect);
      myThread.Start();
    }

    private static void ListenClientConnect()
    {
      while (true)
      {
        client = server.Accept();
        receiveThread = new Thread(ReceiveMessage);
        receiveThread.Start(client);
      }
    }

    /// <summary>  
    /// 接收消息  
    /// </summary>  
    /// <param name="clientSocket"></param>  
    private static void ReceiveMessage(object clientSocket)
    {

      client = (Socket)clientSocket;
      while (true)
      {
        try
        {
          //通过clientSocket接收数据  
          int receiveNumber = client.Receive(result);//获取接收数据的长度
          Console.WriteLine("接收客户端{0}消息{1}", client.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));
          Console.WriteLine("信息获取成功");
        }
        catch (Exception ex)
        {
          Console.WriteLine("从服务器获取数据错误" + "错误信息" + ex.Message);
          client.Shutdown(SocketShutdown.Both);
          client.Close();
          break;
        }
      }
    }

  }
}
