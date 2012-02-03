using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web;

namespace api.types
{
  public class User
  {
    private HTTP _http = null;
    private int _id = 0;
    private string _name = null;

    public User(HTTP http, int id, string name)
      : this(http, id)
    {
      _name = name;
    }

    public User(HTTP http, int id)
    {
      _http = http;
      _id = id;
    }

    private void populate()
    {
      JObject o = JObject.Parse(_http.GET("users/" + _id.ToString()));
      _name = o.Value<string>("name");
    }

    public int ID
    {
      get
      {
        return _id;
      }
    }

    public string Name
    {
      get
      {
        if (_name == null)
          populate();
        return _name;
      }
    }

    public void ChangePassword(string password)
    {
      string payload = "password=" + HttpUtility.UrlEncode(password);
      _http.PUT("users/" + _id.ToString() + "/password", payload);
    }

    public Dictionary<string, bool> Powers
    {
      get
      {
        return JsonConvert.DeserializeObject<Dictionary<string, bool>>(_http.GET("users/" + _id.ToString() + "/powers"));
      }
    }

    public bool Can(string what)
    {
      return JsonConvert.DeserializeObject<bool>(_http.GET("users/" + _id.ToString() + "/can/" + what));
    }

    public void setCan(string what)
    {
      string payload = HttpUtility.UrlEncode(what) + "=";
      _http.PUT("users/" + _id.ToString() + "/can", payload);
    }

    public void setCan(string[] what)
    {
      string payload = "";
      for (int i = 0; i < what.Length; i++)
      {
        payload += HttpUtility.UrlEncode(what[i]) + "=";
      }

      _http.PUT("users/" + _id.ToString() + "/can", payload);
    }

    public bool Cannot(string what)
    {
      return JsonConvert.DeserializeObject<bool>(_http.GET("users/" + _id.ToString() + "/cannot/" + what));
    }

    public void setCannot(string what)
    {
      string payload = HttpUtility.UrlEncode(what) + "=";
      _http.PUT("users/" + _id.ToString() + "/cannot", payload);
    }

    public void setCannot(string[] what)
    {
      string payload = "";
      for (int i = 0; i < what.Length; i++)
      {
        payload += HttpUtility.UrlEncode(what[i]) + "=";
      }

      _http.PUT("users/" + _id.ToString() + "/cannot", payload);
    }

    static public User from_json(HTTP http, JObject o)
    {
      return new User(http, o.Value<int>("id"), o.Value<string>("name"));
    }

    static public User from_json(HTTP http, string json)
    {
      return from_json(http, JObject.Parse(json));
    }
  }

  public class Drop
  {
    private HTTP _http = null;
    private int _id = 0;
    private User _author = null;
    private string _content = null;
    private string _created_at = null;
    private string _updated_at = null;

    public Drop(HTTP http, int id)
    {
      _http = http;
      _id = id;
    }

    public Drop(HTTP http, int id, User author, string content, string created_at, string updated_at)
      : this(http, id)
    {
      _author = author;
      _content = content;
      _created_at = created_at;
      _updated_at = updated_at;
    }

    private void populate()
    {
      JObject o = JObject.Parse(_http.GET("drops/" + _id.ToString()));
      _author = User.from_json(_http, (JObject)o["author"]);
      _content = (string)o["content"];
      _created_at = (string)o["created_at"];
      _created_at = (string)o["updated_at"];
    }

    public int ID
    {
      get
      {
        return _id;
      }
    }

    public User Author
    {
      get
      {
        if (_author == null)
          populate();
        return _author;
      }
    }

    public string Content
    {
      get
      {
        if (_content == null)
          populate();
        return _content;
      }
    }

    public string CreatedAt
    {
      get
      {
        if (_created_at == null)
          populate();
        return _created_at;
      }
    }

    public string UpdatedAt
    {
      get
      {
        if (_updated_at == null)
          populate();
        return _updated_at;
      }
    }

    public bool Update(string title = null, string author_name = null, int author_id = -1, string content = null)
    {
      if (title == null && author_name == null && author_id < 0 && content == null)
        return false;

      string payload = "";

      if (title != null)
        payload += "title=" + HttpUtility.UrlEncode(title);

      if (author_name != null)
      {
        if (payload.Length != 0)
          payload += "&";
        payload += "author_name=" + HttpUtility.UrlEncode(author_name);
      }

      if (author_id > -1)
      {
        if (payload.Length != 0)
          payload += "&";
        payload += "author_id=" + author_id.ToString();
      }

      if (content != null)
      {
        if (payload.Length != 0)
          payload += "&";
        payload += "content=" + HttpUtility.UrlEncode(content);
      }

      return JsonConvert.DeserializeObject<bool>(_http.PUT("drops/" + _id.ToString(), payload));
    }

    static public Drop from_json(HTTP http, JObject o)
    {
      return new Drop(http,
        (int)o["id"],
        User.from_json(http, (JObject)o["author"]),
        (string)o["content"],
        (string)o["created_at"],
        (string)o["updated_at"]);
    }

    static public Drop from_json(HTTP http, string json)
    {
      return from_json(http, JObject.Parse(json));
    }
  }

  public class Drops
  {
    static public Drop[] from_json(HTTP http, string json)
    {
      return from_json(http, JArray.Parse(json).Values<JObject>().ToArray<JObject>());
    }

    static public Drop[] from_json(HTTP http, JArray a)
    {
      return from_json(http, a.Values<JObject>().ToArray<JObject>());
    }

    static public Drop[] from_json(HTTP http, JObject[] os)
    {
      Drop[] drops = new Drop[os.Length];

      for (int i = 0; i < os.Length; i++)
      {
        drops[i] = Drop.from_json(http, os[i]);
      }

      return drops;
    }

    static public Drop[] from_id_array(HTTP http, string json)
    {
      return from_id_array(http, JArray.Parse(json).Values<int>().ToArray<int>());
    }

    static public Drop[] from_id_array(HTTP http, JArray a)
    {
      return from_id_array(http, a.Values<int>().ToArray<int>());
    }

    static public Drop[] from_id_array(HTTP http, int[] ids)
    {
      Drop[] drops = new Drop[ids.Length];

      for (int i = 0; i < ids.Length; i++)
      {
        drops[i] = new Drop(http, ids[i]);
      }

      return drops;
    }
  }

  public class Flow
  {
    private HTTP _http = null;
    private int _id = 0;
    private string _title = null;
    private string[] _tags = null;
    private User _author = null;
    private Drop[] _drops = null;
    private string _created_at = null;
    private string _updated_at = null;

    static public Flow from_json(HTTP http, JObject o)
    {
      return new Flow(http,
        (int)o["id"],
        o.Value<string>("title"),
        o["tags"].Values<string>().ToArray<string>(),
        User.from_json(http, (JObject)o["author"]),
        api.types.Drops.from_id_array(http, (JArray)o["drops"]),
        o.Value<string>("created_at"),
        o.Value<string>("updated_at"));
    }

    static public Flow from_json(HTTP http, string json)
    {
      return from_json(http, JObject.Parse(json));
    }

    public Flow(HTTP http, int id)
    {
      _http = http;
      _id = id;
    }

    public Flow(HTTP http, int id, string title, string[] tags, User author, Drop[] drops, string created_at, string updated_at)
      : this(http, id)
    {
      _title = title;
      _tags = tags;
      _author = author;
      _drops = drops;
      _created_at = created_at;
      _updated_at = updated_at;
    }

    private void populate()
    {
      JObject o = JObject.Parse(_http.GET("flows/" + _id.ToString()));
      _title = (string)o["title"];
      _tags = ((JArray)o["tags"]).Values<string>().ToArray<string>();
      _author = User.from_json(_http, (JObject)o["author"]);
      _drops = api.types.Drops.from_id_array(_http, (JArray)o["drops"]);
      _created_at = (string)o["created_at"];
      _created_at = (string)o["updated_at"];
    }

    public int ID
    {
      get
      {
        return _id;
      }
    }

    public string Title
    {
      get
      {
        if (_title == null)
          populate();
        return _title;
      }
    }

    public string[] Tags
    {
      get
      {
        if (_tags == null)
          populate();
        return _tags;
      }
    }

    public User Author
    {
      get
      {
        if (_author == null)
          populate();
        return _author;
      }
    }

    public Drop[] Drops
    {
      get
      {
        if (_drops == null)
          populate();
        return _drops;
      }
    }

    public string CreatedAt
    {
      get
      {
        if (_created_at == null)
          populate();
        return _created_at;
      }
    }

    public string UpdatedAt
    {
      get
      {
        if (_updated_at == null)
          populate();
        return _updated_at;
      }
    }

    public Drop[] GetDrops(int limit)
    {
      return types.Drops.from_json(_http, _http.GET("flows/" + _id.ToString() + "/drops?limit=" + limit.ToString()));
    }

    public Drop[] GetDrops(int limit, int offset)
    {
      return types.Drops.from_json(_http, _http.GET("flows/" + _id.ToString() + "/drops?limit=" + limit.ToString() + "&offset=" + offset.ToString()));
    }

    public void AddDrop(string name, string title, string content)
    {
      string payload = "name=" + HttpUtility.UrlEncode(name) +
        "&title=" + HttpUtility.UrlEncode(title) +
        "&content=" + HttpUtility.UrlEncode(content);

      _http.POST("flows/" + _id.ToString() + "/drops", payload);
    }

    public void AddDrop(string title, string content)
    {
      string payload = "title=" + HttpUtility.UrlEncode(title) +
        "&content=" + HttpUtility.UrlEncode(content);

      _http.POST("flows/" + _id.ToString() + "/drops", payload);
    }
  }

  public class Flows
  {
    static public Flow[] from_json(HTTP http, string json)
    {
      return from_json(http, JArray.Parse(json).Values<JObject>().ToArray<JObject>());
    }
    
    static public Flow[] from_json(HTTP http, JArray a)
    {
      return from_json(http, a.Values<JObject>().ToArray<JObject>());
    }

    static public Flow[] from_json(HTTP http, JObject[] os)
    {
      Flow[] flows = new Flow[os.Length];

      for (int i = 0; i < os.Length; i++) {
        flows[i] = Flow.from_json(http, os[i]);
      }

      return flows;
    }
  }
}
