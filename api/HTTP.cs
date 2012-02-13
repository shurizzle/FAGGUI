using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using System.Web;
using Newtonsoft.Json;

namespace fag.api
{
  public class Parameters
    : Dictionary<string, object> { }

  public class HTTP
  {
    private CookieContainer _cookies = null;
    private string _csrf = null;
    private string BASE;

    public HTTP(string _base = "http://10.0.2.2:3000/1/")
    {
      BASE = _base;
      _cookies = new CookieContainer();
    }

    public string Csrf
    {
      get
      {
        if (_csrf == null)
          _csrf = JsonConvert.DeserializeObject<string>(GET("csrf"));
        return _csrf;
      }
    }

    private string jsonize(Parameters dict)
    {
      return JsonConvert.SerializeObject(dict);
    }

    public string Request(string meth, string url, Parameters pars = null)
    {
      url = BASE + url;
      if (!meth.Equals("GET"))
        url += "?csrf=" + HttpUtility.UrlEncode(Csrf);
      HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
      request.CookieContainer = _cookies;
      request.Method = meth;
      request.ContentType = "application/json";

      if (pars != null)
      {
        string payload = jsonize(pars);
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

    public string Request(string meth, string url)
    {
      return Request(meth, url, null);
    }

    public string GET(string url)
    {
      return Request("GET", url);
    }

    public string POST(string url, Parameters pars)
    {
      return Request("POST", url, pars);
    }

    public string PUT(string url, Parameters pars)
    {
      return Request("PUT", url, pars);
    }

    public string DELETE(string url)
    {
      return Request("DELETE", url);
    }

    public void csrf_renew()
    {
      _csrf = JsonConvert.DeserializeObject<string>(GET("csrf/renew"));
    }

    public bool isAuth
    {
      get
      {
        return JsonConvert.DeserializeObject<bool>(GET("auth"));
      }
    }
  }
}
