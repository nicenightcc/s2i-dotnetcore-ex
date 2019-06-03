using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace app
{
  public class RequestHandler
  {
    public RequestHandler(Socket sock)
    {
      byte[] result = new byte[1024];
      while (true)
      {
        try
        {
          //通过clientSocket接收数据  
          int receiveNumber = sock.Receive(result);//获取接收数据的长度
          if (receiveNumber == 0)
          {
            sock.Close();
            return;
          }
          Console.WriteLine("接收客户端{0}消息{1}", sock.RemoteEndPoint.ToString(), Encoding.ASCII.GetString(result, 0, receiveNumber));

          Console.WriteLine("信息获取成功");
        }
        catch (Exception ex)
        {
          sock.Close();
          return;
        }
      }
    }
  }
}
