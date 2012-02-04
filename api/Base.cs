using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Web;
using fag.api.types;

namespace fag.api
{
  public class Base
  {
    private HTTP _http = null;
    private User _user = null;

    public Base()
    {
      _http = new HTTP();
    }

    public Base(string name, string password)
      : this()
    {
      Login(name, password);
    }

    public Base(int id, string password)
      : this()
    {
      Login(id, password);
    }

    public void Login(string name, string password)
    {
      int uid = JsonConvert.DeserializeObject<int>(_http.POST("auth", new Parameters() {
        {"name", name},
        {"password", password}
      }));
      _user = new User(_http, uid);
    }

    public void Login(int id, string password)
    {
      int uid = JsonConvert.DeserializeObject<int>(_http.POST("auth", new Parameters() {
        {"id", id},
        {"password", password}
      }));
      _user = new User(_http, uid);
    }

    public void Register(string name, string password)
    {
      _http.POST("users", new Parameters() {
        {"name", name},
        {"password", password}
      });
    }

    public User GetUser(int id)
    {
      return User.from_json(_http, _http.GET("users/" + id.ToString()));
    }

    public User Me
    {
      get
      {
        return _user;
      }
    }

    public Flow GetFlow(int id)
    {
      return new Flow(_http, id);
    }

    public Flow[] GetFlows(bool sort = true)
    {
      string url = "flows?expression=*";
      if (!sort)
        url += "&no_sort=true";
      return Flows.from_json(_http, _http.GET(url));
    }

    public Flow[] GetFlows(string expression, bool sort = true)
    {
      string url = "flows?expresion=" + HttpUtility.UrlEncode(expression);
      if (!sort)
        url += "&no_sort=true";
      return Flows.from_json(_http, _http.GET(url));
    }

    public Flow[] GetFlows(string expression, int offset, bool sort = true)
    {
      string url = "flows?expression=" + HttpUtility.UrlEncode(expression) +
        "&offset=" + offset.ToString();
      if (!sort)
        url += "&no_sort=true";
      return Flows.from_json(_http, _http.GET(url));
    }

    public Flow[] GetFlows(int offset, bool sort = true)
    {
      return GetFlows("*", offset, sort);
    }

    public Flow[] GetFlows(string expression, int offset, int limit, bool sort = true)
    {
      string url = "flows?expression=" + HttpUtility.UrlEncode(expression) +
        "&offset=" + offset.ToString() + "&limit=" + limit.ToString();
      if (!sort)
        url += "&no_sort=true";
      return Flows.from_json(_http, _http.GET(url));
    }

    public Flow[] GetFlows(int offset, int limit, bool sort = true)
    {
      return GetFlows("*", offset, limit, sort);
    }

    public Flow StartFlow(string name, string title, string[] tags, string content)
    {
      return Flow.from_json(_http, _http.POST("flows", new Parameters() {
        {"name", name},
        {"title", title},
        {"tags", tags},
        {"content", content}
      }));
    }

    public Flow StartFlow(string title, string[] tags, string content)
    {
      return Flow.from_json(_http, _http.POST("flows", new Parameters() {
        {"title", title},
        {"tags", tags},
        {"content", content}
      }));
    }

    public void DeleteFlow(int id)
    {
      _http.DELETE("flows/" + id.ToString());
    }

    public Drop GetDrop(int id)
    {
      return new Drop(_http, id);
    }

    public void DeleteDrop(int id)
    {
      _http.DELETE("drops/" + id.ToString());
    }

    public bool UpdateDrop(int id, string title = null, string author_name = null, int author_id = -1, string content = null)
    {
      return (new Drop(_http, id)).Update(title, author_name, author_id, content);
    }

  }
}
