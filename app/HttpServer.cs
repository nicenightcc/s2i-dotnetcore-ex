using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Linq;

namespace app
{
  public class HttpServer : IDisposable
  {
    private int port;
    private HttpListener httpListener = null;
    public HttpServer(int port)
    {
      this.port = port;
    }
    /// <summary>
    /// 启动本地网页服务器
    /// </summary>
    /// <returns></returns>
    public void Start()
    {
      try
      {
        //监听端口
        httpListener = new HttpListener();
        var host = "http://+:" + port.ToString() + "/";
        httpListener.Prefixes.Add(host);
        httpListener.Start();
        Console.WriteLine("Server Start On: " + host);
        httpListener.BeginGetContext(new AsyncCallback(onWebResponse), httpListener);  //开始异步接收request请求
      }
      catch (Exception ex)
      {
      }
    }
    /// <summary>
    /// 网页服务器相应处理
    /// </summary>
    /// <param name="ar"></param>
    private void onWebResponse(IAsyncResult ar)
    {
      try
      {

        HttpListener httpListener = ar.AsyncState as HttpListener;
        HttpListenerContext context = httpListener.EndGetContext(ar);  //接收到的请求context（一个环境封装体）            

        httpListener.BeginGetContext(new AsyncCallback(onWebResponse), httpListener);  //开始 第二次 异步接收request请求

        HttpListenerRequest lisRequest = context.Request;  //接收的request数据
        HttpListenerResponse lisResponse = context.Response;  //用来向客户端发送回复

        var query = lisRequest.Url.Query.Length > 2 ? lisRequest.Url.Query.Substring(1) : "";

        if (!Uri.TryCreate(query, UriKind.Absolute, out Uri uri)) return;

        HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(uri);

        //webRequest.Headers = (WebHeaderCollection)lisRequest.Headers;
        //webRequest.UserAgent = lisRequest.UserAgent;
        webRequest.Method = lisRequest.HttpMethod;
        //webRequest.CookieContainer = new CookieContainer();
        //webRequest.CookieContainer.Add(lisRequest.Cookies);



        using (HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse())
        {
          lisResponse.StatusCode = (int)webResponse.StatusCode;
          lisResponse.ContentType = webResponse.ContentType;
          lisResponse.ContentEncoding = Encoding.UTF8;
          //lisResponse.Cookies = webResponse.Cookies; //处理Cookies
          //lisResponse.Headers = webResponse.Headers;

          using (var sr = new StreamReader(webResponse.GetResponseStream()))
          using (var sw = lisResponse.OutputStream)
          {
            var res = sr.ReadToEnd();
            var byts = Encoding.UTF8.GetBytes(res);
            sw.Write(byts);
          }

          //using (var sr = new StreamReader(webResponse.GetResponseStream()))
          //{
          //  sr.Read(chars);
          //}
          //using (var sw = new StreamWriter(lisResponse.OutputStream))
          //{
          //  sw.Write(chars);
          //}

        }
      }
      catch (Exception e)
      {
      }
    }

    public void Stop()
    {
      if (httpListener != null)
      {
        httpListener.Stop();
      }
    }

    public void Dispose(object sender, System.EventArgs args)
    {
      this.Dispose();
    }

    public void Dispose()
    {
      Stop();
    }
  }
}
