using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml;

namespace fag.api.types
{
  public abstract class Metadatable
  {
    private string _model_name;
    protected int _id = 0;
    protected JObject _metadata;
    protected HTTP _http;

    public JObject Metadata
    {
      get
      {
        return _metadata;
      }
    }

    public virtual string ModelName
    {
      get
      {
        if (_model_name == null)
          _model_name = this.GetType().Name.ToLower();
        return _model_name;
      }
    }

    private string MetaUrl
    {
      get
      {
        return "metadata/" + ModelName + "/" + _id.ToString();
      }
    }

    protected virtual void LoadMetadata()
    {
      _metadata = JObject.Parse(_http.GET(MetaUrl));
    }

    public virtual bool DeleteMetadata()
    {
      return JsonConvert.DeserializeObject<bool>(_http.DELETE(MetaUrl));
    }

    public virtual bool UpdateMetadata(Parameters pars)
    {
      return JsonConvert.DeserializeObject<bool>(_http.PUT(MetaUrl, pars));
    }

    public virtual T Value<T>(string name)
    {
      if (_metadata == null)
        LoadMetadata();
      return _metadata.Value<T>(name);
    }
  }

  public class User : Metadatable
  {
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
      _http.PUT("users/" + _id.ToString() + "/password", new Parameters() { {"password", password} });
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
      _http.PUT("users/" + _id.ToString() + "/can", new Parameters() { {what, ""} });
    }

    public void setCan(string[] what)
    {
      Parameters pars = new Parameters();

      for (int i = 0; i < what.Length; i++)
      {
        pars[what[i]] = "";
      }

      _http.PUT("users/" + _id.ToString() + "/can", pars);
    }

    public bool Cannot(string what)
    {
      return JsonConvert.DeserializeObject<bool>(_http.GET("users/" + _id.ToString() + "/cannot/" + what));
    }

    public void setCannot(string what)
    {
      _http.PUT("users/" + _id.ToString() + "/cannot", new Parameters() { {what, ""} });
    }

    public void setCannot(string[] what)
    {
      Parameters pars = new Parameters();
      for (int i = 0; i < what.Length; i++)
      {
        pars[what[i]] = "";
      }

      _http.PUT("users/" + _id.ToString() + "/cannot", pars);
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

  public class Drop : Metadatable
  {
    private User _author = null;
    private string _content = null;
    private DateTime _created_at;
    private DateTime _updated_at;

    public Drop(HTTP http, int id)
    {
      _http = http;
      _id = id;
    }

    public Drop(HTTP http, int id, User author, string content, string created_at, string updated_at)
      : this(http, id, author, content, XmlConvert.ToDateTime(created_at, XmlDateTimeSerializationMode.RoundtripKind), XmlConvert.ToDateTime(updated_at, XmlDateTimeSerializationMode.RoundtripKind)) { }

    public Drop(HTTP http, int id, User author, string content, DateTime created_at, DateTime updated_at)
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
      _content = o.Value<string>("content");
      _created_at = XmlConvert.ToDateTime(o.Value<string>("created_at"), XmlDateTimeSerializationMode.RoundtripKind);
      _created_at = XmlConvert.ToDateTime(o.Value<string>("updated_at"), XmlDateTimeSerializationMode.RoundtripKind);
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

    public DateTime CreatedAt
    {
      get
      {
        if (_created_at == null)
          populate();
        return _created_at;
      }
    }

    public DateTime UpdatedAt
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

      Parameters pars = new Parameters();

      if (title != null)
        pars["title"] = title;

      if (author_name != null)
        pars["author_name"] = author_name;

      if (author_id > -1)
        pars["author_id"] = author_id;

      if (content != null)
        pars["content"] = content;

      return JsonConvert.DeserializeObject<bool>(_http.PUT("drops/" + _id.ToString(), pars));
    }

    static public Drop from_json(HTTP http, JObject o)
    {
      return new Drop(http,
        o.Value<int>("id"),
        User.from_json(http, o.Value<JObject>("author")),
        o.Value<string>("content"),
        o.Value<string>("created_at"),
        o.Value<string>("updated_at"));
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

  public class Flow : Metadatable
  {
    private string _title = null;
    private string[] _tags = null;
    private User _author = null;
    private Drop[] _drops = null;
    private DateTime _created_at;
    private DateTime _updated_at;

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
      : this(http, id, title, tags, author, drops, XmlConvert.ToDateTime(created_at, XmlDateTimeSerializationMode.RoundtripKind), XmlConvert.ToDateTime(updated_at, XmlDateTimeSerializationMode.RoundtripKind)) { }

    public Flow(HTTP http, int id, string title, string[] tags, User author, Drop[] drops, DateTime created_at, DateTime update_at)
      : this(http, id)
    {
      _title = title;
      _tags = tags;
      _author = author;
      _drops = drops;
      _created_at = created_at;
      _updated_at = update_at;
    }

    private void populate()
    {
      JObject o = JObject.Parse(_http.GET("flows/" + _id.ToString()));
      _title = o.Value<string>("title");
      _tags = ((JArray)o["tags"]).Values<string>().ToArray<string>();
      _author = User.from_json(_http, o.Value<JObject>("author"));
      _drops = api.types.Drops.from_id_array(_http, o.Value<JArray>("drops"));
      _created_at = XmlConvert.ToDateTime(o.Value<string>("created_at"), XmlDateTimeSerializationMode.RoundtripKind);
      _created_at = XmlConvert.ToDateTime(o.Value<string>("updated_at"), XmlDateTimeSerializationMode.RoundtripKind);
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

    public DateTime CreatedAt
    {
      get
      {
        if (_created_at == null)
          populate();
        return _created_at;
      }
    }

    public DateTime UpdatedAt
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
      _http.POST("flows/" + _id.ToString() + "/drops", new Parameters() {
        {"name", name},
        {"title", title},
        {"content", content}
      });
    }

    public void AddDrop(string title, string content)
    {
      _http.POST("flows/" + _id.ToString() + "/drops", new Parameters() {
        {"title", title},
        {"content", content}
      });
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
