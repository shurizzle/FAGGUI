using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Test
{
  class Program
  {
    static void Main(string[] args)
    {
      //api.Base b = new api.Base();
      //Console.WriteLine("ID: {0}\nNAME: {1}", b.GetUser(1).ID, b.GetUser(1).Name);
      api.Base b = new api.Base();
      //b.Register("shura", "polipo");
      b.Login("root", "cocco");
      //Console.WriteLine(b.GetUser(2).Name);
      Console.WriteLine(b.UpdateDrop(1, null, null, 2));
      //api.Base b = new api.Base("root", "cocco");
      //Console.WriteLine("ID: {0}\nNAME: {1}", b.Me.ID, b.Me.Name);
      api.types.Flow[] flows = b.GetFlows();
      for (int i = 0; i < flows.Length; i++)
      {
        Console.WriteLine("ID: {0}\tTITLE: {1}", flows[i].ID, flows[i].Title);
        api.types.Drop[] drops = flows[i].Drops;
        for (int j = 0; j < drops.Length; j++)
          Console.WriteLine("  ID: {0}\tCONTENT: {1}", drops[j].ID, drops[j].Content);
      }
      //Console.WriteLine("Can change drops? {0}.", b.Me.Can("change drops"));
      //Console.WriteLine(b.Me.Powers);
      Console.ReadKey();
    }
  }
}
