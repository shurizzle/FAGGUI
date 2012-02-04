using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using fag.api.types;

namespace fag
{
  class Helpers
  {
    static public ListViewItem[] Convert(Flow[] flows)
    {
      ListViewItem[] items = new ListViewItem [flows.Length];

      for (int i = 0; i < flows.Length; i++)
      {
        items[i] = new ListViewItem(new string[] {
          flows[i].ID.ToString(),
          String.Join(", ", flows[i].Tags),
          flows[i].Title,
          flows[i].Author.Name,
          System.Xml.XmlConvert.ToString(flows[i].UpdatedAt, System.Xml.XmlDateTimeSerializationMode.Local)
        }, -1);
      }

      return items;
    }
  }
}
