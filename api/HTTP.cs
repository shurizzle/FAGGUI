using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace api
{
  public class HTTP
  {
    private CookieContainer _cookies = null;
    private string _csrf = null;
    private static string BASE = "http://10.0.2.2:3000/1/";

    public HTTP()
    {
      _cookies = new CookieContainer();
      _csrf = JsonConvert.DeserializeObject<string>(GET("csrf"));
    }

    public string Request(string meth,
      string url,
      string payload)
    {
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(BASE + url);
      request.CookieContainer = _cookies;
      request.Method = meth;
      request.ContentType = "application/x-www-form-urlencoded";

      if (payload != null)
      {
        if (payload.Length != 0)
          payload += "&";
        payload += "_csrf=" + HttpUtility.UrlEncode(_csrf);
        request.ContentLength = payload.Length;

        using (var requestStream = request.GetRequestStream())
        using (var writer = new StreamWriter(requestStream))
        {
          writer.Write(payload);
          writer.Close();
        }
      }

      string result;
      HttpWebResponse response = (HttpWebResponse)request.GetResponse();

      using (var responseStream = response.GetResponseStream())
      using (var reader = new StreamReader(responseStream))
      {
        result = reader.ReadToEnd();
      }

      return result;
    }

    public string Request(string meth,
      string url)
    {
      return Request(meth, url, null);
    }

    public string GET(string url)
    {
      return Request("GET", url);
    }

    public string POST(string url, string payload)
    {
      return Request("POST", url, payload);
    }

    public string PUT(string url, string payload)
    {
      return Request("PUT", url, payload);
    }

    public string DELETE(string url)
    {
      return Request("DELETE", url);
    }

    public void csrf_renew()
    {
      _csrf = JsonConvert.DeserializeObject<string>(GET("csrf/renew"));
    }

    public bool isAuth()
    {
      return JsonConvert.DeserializeObject<bool>(GET("auth"));
    }
  }
}
