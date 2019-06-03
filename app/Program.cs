using System;

namespace app
{
  class Program
  {
    static void Main(string[] args)
    {
      Console.WriteLine("Hello World!");
      new SocketConnection(8080).setConnection();
      Console.ReadKey();
    }
  }
}
