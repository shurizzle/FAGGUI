using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using Newtonsoft.Json;
using api.types;

namespace api
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
      string payload = "name=" + HttpUtility.UrlEncode(name) +
        "&password=" + HttpUtility.UrlEncode(password);
      int uid = JsonConvert.DeserializeObject<int>(_http.POST("auth", payload));
      _user = new User(_http, uid);
    }

    public void Login(int id, string password)
    {
      string payload = "id=" + id.ToString() +
        "&password=" + HttpUtility.UrlEncode(password);
      int uid = JsonConvert.DeserializeObject<int>(_http.POST("auth", payload));
      _user = new User(_http, uid);
    }

    public void Register(string name, string password)
    {
      string payload = "name=" + HttpUtility.UrlEncode(name) +
        "&password=" + HttpUtility.UrlEncode(password);
      _http.POST("users", payload);
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
      string _tags = JsonConvert.SerializeObject(tags);
      string payload = "name=" + HttpUtility.UrlEncode(name) +
        "&title=" + HttpUtility.UrlEncode(title) +
        "&tags=" + HttpUtility.UrlEncode(_tags) +
        "&content=" + HttpUtility.UrlEncode(content);
      return Flow.from_json(_http, _http.POST("flows", payload));
    }

    public Flow StartFlow(string title, string[] tags, string content)
    {
      string _tags = JsonConvert.SerializeObject(tags);
      string payload = "title=" + HttpUtility.UrlEncode(title) +
        "&tags=" + HttpUtility.UrlEncode(_tags) +
        "&content=" + HttpUtility.UrlEncode(content);
      return Flow.from_json(_http, _http.POST("flows", payload));
    }

    public void DeleteFlow(int id)
    {
      _http.DELETE("flows/" + id.ToString());
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
