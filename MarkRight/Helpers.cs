using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Documents;
using System.Windows.Controls;

namespace MarkRight.Helpers
{
  public static class StringHelpers
  {
    public static List<string> SplitWithStringSeparator(this string line, string separator)
    {
      return line.Split(new string[] { separator }, StringSplitOptions.None).ToList();
    }
  }

  public static class XamlHelpers
  {
    public static void AddMarkedUpText(this StackPanel sp, string markedUpText)
    {
      FlowDocumentScrollViewer fdsv = new FlowDocumentScrollViewer();
      fdsv.Document = FlowDocumentParser.Parse(markedUpText);
      fdsv.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
      fdsv.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
      sp.Children.Add(fdsv);
    }
  }
}
